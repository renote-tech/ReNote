using Server.ReNote.Data;

namespace Server.ReNote.Utilities
{
    public class DatabaseUtil
    {
        /// <summary>
        /// Sets data to the <see cref="Database"/>.
        /// </summary>
        /// <param name="root">The name of the <see cref="Container"/>.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The value of the item.</param>
        public static void Set(string root, string key, string value)
        {
            if (Server.Database[root] == null)
                Server.Database.AddContainer(root);

            bool itemExists = ItemExists(root, key);
            if (itemExists)
                Server.Database[root][key] = value;
            else
                Server.Database[root].AddItem(key, value);
        }

        /// <summary>
        /// Removes a key from the <see cref="Database"/>. Returns whether the key has actually been removed.
        /// </summary>
        /// <param name="root">The name of the <see cref="Container"/>.</param>
        /// <param name="key">The key of the item.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool Remove(string root, string key)
        {
            if (Server.Database[root] == null)
                return false;

            return Server.Database[root].RemoveItem(key);
        }

        /// <summary>
        /// Gets data from the <see cref="Database"/>.
        /// </summary>
        /// <param name="root">The name of the <see cref="Container"/>.</param>
        /// <param name="key">The key of the item.</param>
        /// <returns><see cref="string"/></returns>
        public static string Get(string root, string key)
        {
            if (Server.Database[root] == null)
                return null;

            if (Server.Database[root][key] == null)
                return null;

            return (string)Server.Database[root][key];
        }

        /// <summary>
        /// Gets a dictionary with all the items.
        /// </summary>
        /// <param name="root">The name of the <see cref="Container"/>.</param>
        /// <returns>Dictionary[<see cref="string"/>, <see cref="string"/>]</returns>
        public static Dictionary<string, string> GetItems(string root)
        {
            if (Server.Database[root] == null)
                return new Dictionary<string, string>();

            return Server.Database[root].GetItems();
        }

        /// <summary>
        /// Gets all the values of the items within a <see cref="Container"/>.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static string[] GetValues(string root)
        {
            if (Server.Database[root] == null)
                return Array.Empty<string>();

            return Server.Database[root].GetItemsValues();
        }

        /// <summary>
        /// Returns whether the item exists.
        /// </summary>
        /// <param name="root">The name of the <see cref="Container"/>.</param>
        /// <param name="key">The key of the item.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ItemExists(string root, string key)
        {
            if (Server.Database[root] == null)
                return false;

            if (Server.Database[root][key] == null)
                return false;

            return true;
        }
    }
}