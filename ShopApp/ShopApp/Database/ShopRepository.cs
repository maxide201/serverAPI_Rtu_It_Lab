using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using ShopApp.Models;

namespace ShopApp.Database
{
    public interface IShopRepository
    {
        public Shop Add(Shop shop);
        public void Delete(uint id);
        public List<ShopDTO> Get();
        public Shop Get(uint id);
    }
    public class ShopRepository : Repository, IShopRepository
    {
        public ShopRepository(string conn) : base(conn)
        {
        }

        public Shop Add(Shop shop)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Shops (Name, Address, PhoneNumber, Password) VALUES(@Name, @Address, @PhoneNumber, @Password); SELECT LAST_INSERT_ID()";
                uint id = db.Query<uint>(sqlQuery, shop).FirstOrDefault();

                return Get(id);
            }
        }

        public void Delete(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryDeleteShop = "DELETE FROM Shops WHERE Id = @id";
                var sqlQueryCheckIdForDelete = "SELECT Id FROM Checks where ShopId = @id and NOT EXISTS(SELECT * FROM Users WHERE Id IN(select UserId from Checks where ShopId = @id));";
                var sqlQueryDeleteChecks = "DELETE FROM Checks WHERE Id IN @ids";

                db.Execute(sqlQueryDeleteShop, new { id });
                List<uint> CheckIds = db.Query<uint>(sqlQueryCheckIdForDelete, new { id }).ToList();
                db.Execute(sqlQueryDeleteChecks, new { ids = CheckIds.ToArray() });
            }
        }

        public List<ShopDTO> Get()
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "SELECT * FROM Shops";
                return db.Query<ShopDTO>(sqlQuery).ToList();
            }
        }

        public Shop Get(uint id)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQueryAddUser = "SELECT * FROM Shops WHERE Id=@id";
                return db.Query<Shop>(sqlQueryAddUser, new { id }).FirstOrDefault();
            }
        }
    }
}
