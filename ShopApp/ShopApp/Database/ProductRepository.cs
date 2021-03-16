using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using ShopApp.Models;

namespace ShopApp.Database
{
    public interface IProductRepository
    {
        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
        void DeleteProduct(uint id);
        List<ProductDTO> GetProducts(uint shopId);
        Product Get(uint id);
        bool isPasswordRight(ShopAdminProductRequest request);
    }
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(string conn) : base(conn)
        { 
        }

        public Product AddProduct(Product product)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Products (ShopId, Name, Category, Cost, Count) VALUES(@ShopId, @Name, @Category, @Cost, @Count); SELECT LAST_INSERT_ID()";
                uint id = db.Query<uint>(sqlQuery, product).FirstOrDefault();

                return Get(id);
            }
        }

        public void DeleteProduct(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Products WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public Product UpdateProduct(Product product)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Products SET ShopId=@ShopId, Name=@Name, Category=@Category, Cost=@Cost, Count=@Count WHERE Id = @Id";
                db.Execute(sqlQuery, product);

                return Get(product.Id);
            }
        }

        public Product Get(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "SELECT * FROM Products WHERE Id=@id";
                return db.Query<Product>(sqlQuery, new { id }).FirstOrDefault();
            }
        }

        public List<ProductDTO> GetProducts(uint shopId)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "SELECT * FROM Products WHERE shopId=@shopId";
                return db.Query<ProductDTO>(sqlQuery, new { shopId }).ToList();
            }
        }

        public bool isPasswordRight(ShopAdminProductRequest request)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "SELECT Password FROM Shops WHERE Id=@shopId";
                string password = db.Query<string>(sqlQuery, request.Product).FirstOrDefault();

                return request.Password == password;
            }
        }

    }
}
