using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RNDatabase = Server.ReNote.Data.Database;

namespace Server.Database.Management
{
    internal class DatabaseManager
    {
        public static readonly char[] PATH_ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_".ToCharArray();

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

        public static Dictionary<string, string> GetItems(string dbName, string containerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return null;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return null;

            return s_Databases[dbName][containerName].GetItems();
        }

        public static int AddContainer(string dbName, string containerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (s_Databases[dbName].IsContainerExists(containerName))
                return 1;

            s_Databases[dbName].AddContainer(containerName);
            s_Databases[dbName].NeedSave = true;

            return 0;
        }

        public static int RenameContainer(string dbName, string containerName, string newContainerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return -1;

            s_Databases[dbName][containerName].Name = newContainerName;

            return 0;
        }

        public static void DeleteContainer(string dbName, string containerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return;

            s_Databases[dbName].RemoveContainer(containerName);
            s_Databases[dbName].NeedSave = true;
        }

        public static int AddItem(string dbName, string containerName, string itemName, string itemValue)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return -1;

            bool itemExists = s_Databases[dbName][containerName].IsItemExists(itemName);
            if (itemExists)
                return 1;

            s_Databases[dbName][containerName].AddItem(itemName, itemValue);
            s_Databases[dbName].NeedSave = true;

            return 0;
        }

        public static int EditItem(string dbName, string containerName, string oldItemName, string newItemName, string newItemValue)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return -1;

            bool itemExists = s_Databases[dbName][containerName].IsItemExists(oldItemName);
            if (itemExists)
                s_Databases[dbName][containerName].RemoveItem(oldItemName);

            s_Databases[dbName][containerName].AddItem(newItemName, newItemValue);
            s_Databases[dbName].NeedSave = true;

            return 0;
        }

        public static void DeleteItem(string dbName, string containerName, string itemName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return;

            s_Databases[dbName][containerName].RemoveItem(itemName);
            s_Databases[dbName].NeedSave = true;
        }

        public static bool NeedSave(string dbName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return false;

            return s_Databases[dbName].NeedSave;
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
            s_Databases[dbName].NeedSave = false;

            return 0;
        }

        public static bool IsIllegalChar(char character)
        {
            if (char.IsControl(character) || character == '\r')
                return false;

            if (!PATH_ALLOWED_CHARS.Contains(character))
                return true;

            return false;
        }
    }
}