namespace serverAPI.Database
{
    public abstract class Repository
    {
        public string connectionString;
        public Repository(string conn)
        {
            connectionString = conn;
        }
    }
}
