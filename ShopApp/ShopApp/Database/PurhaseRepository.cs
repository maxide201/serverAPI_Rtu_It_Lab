using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using ShopApp.Models;
using Dapper;

namespace ShopApp.Database
{
    public interface IPurchaseRepository
    {
        bool isUserAndShopExists(PurchaseRequest request);
        public CheckDTO MakePurchase(PurchaseRequest request);
        bool isShopPasswordRight(ShopAdminRequest request);
        List<CheckDTO> GetChecks(uint ShopId);
    }
    public class PurchaseRepository : Repository, IPurchaseRepository
    {
        public PurchaseRepository(string conn) : base(conn)
        {
        }
        public bool isUserAndShopExists(PurchaseRequest request)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryFindUser = "SELECT COUNT(*) FROM Users WHERE Id=@UserId";
                var sqlQueryFindShop = "SELECT COUNT(*) FROM Shops WHERE Id=@ShopId";

                return db.ExecuteScalar<bool>(sqlQueryFindUser, request) &&
                       db.ExecuteScalar<bool>(sqlQueryFindShop, request);
            }
        }

        public CheckDTO MakePurchase(PurchaseRequest request)
        {
            //Запросы к бд
            var sqlQueryFindProduct = "SELECT * FROM Products WHERE Id=@Id AND ShopId=@ShopId";
            var sqlQuerySubtractProductCount = "UPDATE Products SET Count=@count WHERE Id=@id";
            Product productInShop;

            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                // проходимся по продуктам, которые собираемся купить
                foreach (ProductDTO purchasedProduct in request.Products)
                {
                    // проверяем - есть ли бд такой продукт(А ещё не хотим ли мы купить продукт из дуругого магазина)
                    productInShop = db.Query<Product>(sqlQueryFindProduct, new { Id = purchasedProduct.Id, ShopId = request.ShopId }).FirstOrDefault();

                    // если продукта нет или мы хотим купить больше, чем есть в наличии, то return 
                    if (productInShop == null ||
                        ((int)productInShop.Count - (int)purchasedProduct.Count) < 0 ||
                        (int)purchasedProduct.Count == 0)
                        return null;

                    // подтягиваем имена и стоимость (данные подменить не получится)
                    purchasedProduct.Name = productInShop.Name;
                    purchasedProduct.Cost = productInShop.Cost;
                    // вытягиваем категорию, если пользователь не задал
                    if (purchasedProduct.Category == null)
                        purchasedProduct.Category = productInShop.Category;

                    // уменьшаем в бд количество продукта
                    uint id = purchasedProduct.Id;
                    uint count = productInShop.Count - purchasedProduct.Count;
                    db.Execute(sqlQuerySubtractProductCount, new { id, count });
                }
                return AddCheck(request, db);
            }
        }

        private CheckDTO AddCheck(PurchaseRequest request, IDbConnection db)
        {
            var sqlQueryAddCheck = "INSERT INTO Checks (ShopId, UserId, ShopName, ShopAddress, UserName, PurchaseDate) VALUES(@ShopId, @UserId, @ShopName, @ShopAddress, @UserName, @PurchaseDate); SELECT * FROM Checks WHERE Id=LAST_INSERT_ID()";
            var sqlQueryFindUser = "SELECT * FROM Users WHERE Id=@UserId";
            var sqlQueryFindShop = "SELECT * FROM Shops WHERE Id=@ShopId";

            // подготавливаем данные, чтобы доавбить сформирвоать чек и добавить его в бд
            uint ShopId = request.ShopId;
            uint UserId = request.UserId;
            User user = db.Query<User>(sqlQueryFindUser, new { UserId }).FirstOrDefault();
            Shop shop = db.Query<Shop>(sqlQueryFindShop, new { ShopId }).FirstOrDefault();
            string UserName = user.Name;
            string ShopName = shop.Name;
            string ShopAddress = shop.Address;
            DateTime PurchaseDate = DateTime.Now;

            CheckDTO check = db.Query<CheckDTO>(sqlQueryAddCheck, new { ShopId, UserId, ShopName, ShopAddress, UserName, PurchaseDate }).FirstOrDefault();
            check.Purchases = AddPurchases(request.Products, check.Id, db, request.PaymentMethod);

            return check;
        }

        private List<PurchaseDTO> AddPurchases(List<ProductDTO> products, uint CheckId, IDbConnection db, string PaymentMethod)
        {
            var sqlQuery = "INSERT INTO Purchases (CheckId, Count, Cost, Name, Category, PaymentMethod) VALUES(@CheckId, @Count, @Cost, @Name, @Category, @PaymentMethod); SELECT * FROM Purchases WHERE Id=LAST_INSERT_ID()";
            List<PurchaseDTO> purchases = new List<PurchaseDTO>();

            // проходимся снова по продутам, чтобы сформировать покупки и добавить их в бд и чек
            foreach (ProductDTO purchasedProduct in products)
            {
                uint Count = purchasedProduct.Count;
                double Cost = purchasedProduct.Cost;
                string Name = purchasedProduct.Name;
                string Category = purchasedProduct.Category;
                PurchaseDTO purchase = db.Query<PurchaseDTO>(sqlQuery, new { CheckId, Count, Cost, Name, Category, PaymentMethod }).FirstOrDefault();
                purchases.Add(purchase);
            }

            return purchases;
        }

        public bool isShopPasswordRight(ShopAdminRequest request)
        {
            var sqlQuery = "SELECT Password FROM Shops WHERE Id=@ShopId";

            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                string password = db.Query<string>(sqlQuery, request).FirstOrDefault();
                return password == request.Password;
            }
        }

        public List<CheckDTO> GetChecks(uint ShopId)
        {
            var sqlQuerySelectChecks = "SELECT * FROM Checks WHERE ShopId = @ShopId";
            var sqlQuerySelectPurchases = "SELECT * FROM Purchases WHERE CheckId = @CheckId";

            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                List<CheckDTO> checks = db.Query<CheckDTO>(sqlQuerySelectChecks, new { ShopId }).ToList();

                foreach(CheckDTO check in checks)
                {
                    uint CheckId = check.Id;
                    check.Purchases = db.Query<PurchaseDTO>(sqlQuerySelectPurchases, new { CheckId }).ToList();
                }

                return checks;
            }
        }
    }
}
