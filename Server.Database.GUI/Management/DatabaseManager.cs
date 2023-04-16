using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RNDatabase = Server.ReNote.Data.Database;

namespace Server.Database.Management
{
    internal class DatabaseManager
    {
        /// <summary>
        /// Allowed characters in a database path.
        /// </summary>
        public static readonly char[] PATH_ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_".ToCharArray();

        private readonly static Dictionary<string, RNDatabase> s_Databases = new Dictionary<string, RNDatabase>();

        /// <summary>
        /// Creates and adds a database to the <see cref="s_Databases"/> list.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <returns><see cref="RNDatabase"/></returns>
        public static RNDatabase CreateDatabase(string dbName)
        {
            if (s_Databases.ContainsKey(dbName))
                return null;

            RNDatabase database = new RNDatabase();
            s_Databases.Add(dbName, database);

            return database;
        }

        /// <summary>
        /// Adds an existing database to the <see cref="s_Databases"/> list.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="filePath">The path to the database file.</param>
        /// <returns><see cref="RNDatabase"/></returns>
        public static RNDatabase AddDatabase(string dbName, string filePath)
        {
            if (s_Databases.ContainsKey(dbName))
                return null;

            RNDatabase database = new RNDatabase();
            database.FileLocation = filePath;
            database.Load();

            s_Databases.Add(dbName, database);

            return database;
        }

        /// <summary>
        /// Removes a database from the <see cref="s_Databases"/> list.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        public static void RemoveDatabase(string dbName)
        {
            if (s_Databases.ContainsKey(dbName))
                s_Databases.Remove(dbName);
        }

        /// <summary>
        /// Gets the items from a specified container.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        /// <returns>Dictionary[<see cref="string"/>, <see cref="string"/>]</returns>
        public static Dictionary<string, string> GetItems(string dbName, string containerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return null;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return null;

            return s_Databases[dbName][containerName].GetItems();
        }

        /// <summary>
        /// Adds a container in the specified database.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        /// <returns><see cref="int"/></returns>
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

        /// <summary>
        /// Renames a container from a specified database.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        /// <param name="newContainerName">The container's new name.</param>
        /// <returns><see cref="int"/></returns>
        public static int RenameContainer(string dbName, string containerName, string newContainerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return -1;

            s_Databases[dbName][containerName].Name = newContainerName;

            return 0;
        }

        /// <summary>
        /// Deletes a container from a specified database.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        public static void DeleteContainer(string dbName, string containerName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return;

            s_Databases[dbName].RemoveContainer(containerName);
            s_Databases[dbName].NeedSave = true;
        }

        /// <summary>
        /// Adds an item from a specified container.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        /// <param name="itemName">The item's name.</param>
        /// <param name="itemValue">The item's value.</param>
        /// <returns></returns>
        public static int AddItem(string dbName, string containerName, string itemName, string itemValue)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return -1;

            bool itemExists = s_Databases[dbName][containerName].ContainsItem(itemName);
            if (itemExists)
                return 1;

            s_Databases[dbName][containerName].AddItem(itemName, itemValue);
            s_Databases[dbName].NeedSave = true;

            return 0;
        }

        /// <summary>
        /// Edits an item from a specified container.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        /// <param name="oldItemName">The item's old name.</param>
        /// <param name="newItemName">The item's new name.</param>
        /// <param name="newItemValue">The item's new value.</param>
        /// <returns><see cref="int"/></returns>
        public static int EditItem(string dbName, string containerName, string oldItemName, string newItemName, string newItemValue)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return -1;

            bool itemExists = s_Databases[dbName][containerName].ContainsItem(oldItemName);
            if (itemExists)
                s_Databases[dbName][containerName].RemoveItem(oldItemName);

            s_Databases[dbName][containerName].AddItem(newItemName, newItemValue);
            s_Databases[dbName].NeedSave = true;

            return 0;
        }

        /// <summary>
        /// Deletes an item from a specified container.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="containerName">The container's name.</param>
        /// <param name="itemName">The item's name.</param>
        public static void DeleteItem(string dbName, string containerName, string itemName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return;

            if (!s_Databases[dbName].IsContainerExists(containerName))
                return;

            s_Databases[dbName][containerName].RemoveItem(itemName);
            s_Databases[dbName].NeedSave = true;
        }

        /// <summary>
        /// Returns whether a database needs to be saved.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <returns></returns>
        public static bool NeedSave(string dbName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return false;

            return s_Databases[dbName].NeedSave;
        }

        /// <summary>
        /// Sets the save location for a specified database.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <param name="saveLocation">The database's new save location.</param>
        public static void SetSaveLocation(string dbName, string saveLocation)
        {
            if (!s_Databases.ContainsKey(dbName))
                return;

            s_Databases[dbName].FileLocation = saveLocation;
        }

        /// <summary>
        /// Saves the specified asynchronously.
        /// </summary>
        /// <param name="dbName">The database's name.</param>
        /// <returns><see cref="int"/></returns>
        public static async Task<int> SaveDatabaseAsync(string dbName)
        {
            if (!s_Databases.ContainsKey(dbName))
                return -1;

            if (string.IsNullOrWhiteSpace(s_Databases[dbName].FileLocation))
                return 1;

            if (s_Databases[dbName].IsEmpty())
                return 2;

            //TODO: Sort containers and items
            await s_Databases[dbName].SaveAsync();
            s_Databases[dbName].NeedSave = false;   

            return 0;
        }

        /// <summary>
        /// Returns whether the character can be used or not in a database path.
        /// </summary>
        /// <param name="character">The character to be checked.</param>
        /// <returns><see cref="bool"/></returns>
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