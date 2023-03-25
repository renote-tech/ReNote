using System.Collections.Generic;
using System.Threading.Tasks;
using RNDatabase = Server.ReNote.Data.Database;

namespace Server.Database.Management
{
    internal class DatabaseManager
    {
        private static Dictionary<string, RNDatabase> s_Databases = new Dictionary<string, RNDatabase>();

        public static RNDatabase CreateDatabase(string dbName)
        {
            if (s_Databases.ContainsKey(dbName))
                return null;

            RNDatabase database = new RNDatabase();
            s_Databases.Add(dbName, database);

            return database;
        }

        public static RNDatabase AddDatabase(string dbName, string filePath)
        {
            if (s_Databases.ContainsKey(dbName))
                return null;

            RNDatabase database = new RNDatabase();
            database.SaveLocation = filePath;
            database.Load();

            s_Databases.Add(dbName, database);

            return database;
        }

        public static void RemoveDatabase(string dbName)
        {
            if (s_Databases.ContainsKey(dbName))
                s_Databases.Remove(dbName);
        }

        public static Dictionary<string, string> GetItems(string database, string container)
        {
            if (!s_Databases.ContainsKey(database))
                return null;

            if (s_Databases[database][container] == null)
                return null;

            return s_Databases[database][container].GetItems();
        }

        public static int AddContainer(string database, string container)
        {
            if (!s_Databases.ContainsKey(database))
                return -1;

            if (s_Databases[database].IsContainerExists(container))
                return 1;

            s_Databases[database].AddContainer(container);

            return 0;
        }

        public static int AddItem(string database, string container, string itemName, string itemValue)
        {
            if (!s_Databases.ContainsKey(database))
                return -1;

            if (s_Databases[database][container] == null)
                return -1;

            if (s_Databases[database][container].IsItemExists(itemName))
                return 1;

            s_Databases[database][container].AddItem(itemName, itemValue);
            
            return 0;
        }

        public static void SetSaveLocation(string dbName, string saveLocation)
        {
            if (!s_Databases.ContainsKey(dbName))
                return;

            s_Databases[dbName].SaveLocation = saveLocation;
        }

        public static async Task<int> SaveDatabaseAsync(string dbName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (string.IsNullOrWhiteSpace(s_Databases[dbName].SaveLocation))
                return 1;

            if (s_Databases[dbName].IsEmpty())
                return 2;

            await s_Databases[dbName].SaveAsync();

            return 0;
        }
    }
}