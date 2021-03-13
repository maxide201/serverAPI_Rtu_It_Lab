using System.Collections.Generic;
using System.Linq;
using Dapper;
using serverAPI.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace serverAPI.Database
{
    public interface IUserRepository
    {
        User Add(User user);
        void Delete(uint id);
        User Get(uint id);
        List<User> GetUsers();
        User Update(User user);
    }
    public class UserRepository : IUserRepository
    {
        string connectionString = null;
        public UserRepository(string conn)
        {
            connectionString = conn;
        }
        public List<User> GetUsers()
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                List<User> users = db.Query<User>("SELECT * FROM Users").ToList();
                foreach (var user in users)
                {
                    user.Purchases = db.Query<Purchase>("SELECT * FROM Purchases WHERE UserId = @id", new { user.Id }).ToList();
                }
                return users;
            }
        }

        public User Get(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                User user = db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
                if (user == null)
                    return null;

                user.Purchases = db.Query<Purchase>("SELECT * FROM Purchases WHERE UserId = @id", new { id }).ToList();
                return user;
            }
        }

        public User Add(User user)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryAddUser = "INSERT INTO Users () VALUES(); SELECT LAST_INSERT_ID()";
                uint id = db.Query<uint>(sqlQueryAddUser).FirstOrDefault();

                var sqlQueryAddPurchase = "INSERT INTO Purchases (userid, name, purchaseDate, cost) VALUES(" + id + ", @name, @purchaseDate, @cost)";

                foreach (var purchase in user.Purchases)
                {
                    db.Execute(sqlQueryAddPurchase, purchase);
                }

                return Get(id);
            }
        }

        public User Update(User user)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryGetPurchases = "SELECT * from Purchases WHERE userId=@userId";

                var PurchasesInDb = db.Query<Purchase>(sqlQueryGetPurchases, new { userId = user.Id }).ToList();

                //Проходимся по покупкам, которые есть на текущий момент есть в бд
                foreach (var purchase in PurchasesInDb)
                {
                    //match будет не null, если пользователь не удалял покупку из своего списка
                    Purchase match = user.Purchases.FirstOrDefault(x => x.Id == purchase.Id);
                    if (match != null)
                    {
                        //удаляем из списка user'a покупку, чтобы в этом списке остались
                        //только новые записи, которые мы затем добавим в бд
                        user.Purchases.Remove(match);
                        continue;
                    }
                    //если пользователь удалил из списка покупку, то удаляем покупку из бд
                    db.Execute("DELETE FROM Purchases WHERE Id = @id", new { id = purchase.Id });
                }

                var sqlQueryAddPurchase = "INSERT INTO Purchases (userid, name, purchaseDate, cost) VALUES(" + user.Id + ", @name, @purchaseDate, @cost)";
                //оставшийся покупки у user'a являются новыми. Добавляем их в бд
                foreach (var purchase in user.Purchases)
                {
                    db.Execute(sqlQueryAddPurchase, purchase);
                }

                return Get(user.Id);
            }
        }

        public void Delete(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
