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
        bool isUserPasswordRight(User user);
        public List<PurchaseDTO> GetPurchases(uint UserId);
    }
    public class UserRepository : IUserRepository
    {
        string connectionString = null;
        public UserRepository(string conn)
        {
            connectionString = conn;
        }

        public User Add(User user)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryAddUser = "INSERT INTO Users (Name, Password) VALUES(@Name, @Password); SELECT * FROM Users WHERE Id=LAST_INSERT_ID()";
                user = db.Query<User>(sqlQueryAddUser, user).FirstOrDefault();

                return user;
            }
        }
        public void Delete(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryDeleteUser = "DELETE FROM Users WHERE Id = @id";
                var sqlQueryCheckIdForDelete = "SELECT Id FROM Checks where UserId = @id and NOT EXISTS(SELECT * FROM Shops WHERE Id IN(select ShopId from Checks where UserId = @id))";
                var sqlQueryDeleteChecks = "DELETE FROM Checks WHERE Id IN @ids";
                //CREATE TEMPORARY TABLE t AS select * from Checks where ShopId=1 and NOT EXISTS (SELECT * FROM Users WHERE Id IN ( select UserId from Checks where ShopId=1)); select * from t where Id In (SELECT Id FROM t); DROP TABLE t;

                db.Execute(sqlQueryDeleteUser, new { id });
                List<uint> CheckIds = db.Query<uint>(sqlQueryCheckIdForDelete, new { id }).ToList();
                db.Execute(sqlQueryDeleteChecks, new { ids = CheckIds.ToArray() });
            }
        }

        public List<PurchaseDTO> GetPurchases(uint UserId)
        {
            var sqlQuery = "SELECT * FROM Purchases WHERE Id IN (SELECT Id FROM Checks WHERE UserId=@UserId)";

            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                List<PurchaseDTO> purchases = db.Query<PurchaseDTO>(sqlQuery, new { UserId }).ToList();
                return purchases;
            }
        }

        public bool isUserPasswordRight(User user)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "SELECT Password FROM Users WHERE Id = @Id";
                string password = db.Query<string>(sqlQuery, user).FirstOrDefault();

                return user.Password == password;
            }
        }
    }
}
