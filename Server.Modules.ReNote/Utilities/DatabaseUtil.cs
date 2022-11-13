using Server.ReNote.Data;

namespace Server.ReNote.Utilities
{
    public class DatabaseUtil
    {
        /// <summary>
        /// Allowed characters for [root->key] database path style.
        /// </summary>
        public static readonly char[] PATH_ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToLower()
                                                                                                 .ToCharArray();

        /// <summary>
        /// Sets data to the <see cref="Database"/>.
        /// </summary>
        /// <param name="root">The name of the <see cref="RootDocument"/>.</param>
        /// <param name="key">The key of the <see cref="Document"/>.</param>
        /// <param name="value">The value of the <see cref="Document"/>.</param>
        public static void Set(string root, string key, string value)
        {
            if (Database.Instance[root] == null)
                Database.Instance.AddDocument(root);

            bool isDocumentExists = DocumentExists(root, key);
            if (isDocumentExists)
                Database.Instance[root][key] = value;
            else
                Database.Instance[root].AddKey(key, new Document(value));
        }

        /// <summary>
        /// Removes a key from the <see cref="Database"/>. Returns whether the key has actually been removed.
        /// </summary>
        /// <param name="root">The name of the <see cref="RootDocument"/>.</param>
        /// <param name="key">The key of the <see cref="Document"/>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool Remove(string root, string key)
        {
            if (Database.Instance[root] == null)
                return false;

            return Database.Instance[root].RemoveKey(key);
        }

        /// <summary>
        /// Gets data from the <see cref="Database"/>.
        /// </summary>
        /// <param name="root">The name of the <see cref="RootDocument"/>.</param>
        /// <param name="key">The key of the <see cref="Document"/>.</param>
        /// <returns><see cref="Document"/></returns>
        public static Document Get(string root, string key)
        {
            if (Database.Instance[root] == null)
                return null;

            if (Database.Instance[root][key] == null)
                return null;

            return (Document)Database.Instance[root][key];
        }

        /// <summary>
        /// Gets a dictionary with all the <see cref="Document"/>s.
        /// </summary>
        /// <param name="root">The name of the <see cref="RootDocument"/>.</param>
        /// <returns>Dictionary[<see cref="string"/>, <see cref="Document"/>]</returns>
        public static Dictionary<string, Document> GetDictionary(string root)
        {
            if (Database.Instance[root] == null)
                return new Dictionary<string, Document>();

            return Database.Instance[root].GetDictionary();
        }

        /// <summary>
        /// Gets all the <see cref="Document"/>s within a <see cref="RootDocument"/>.
        /// </summary>
        /// <param name="root">The name of the <see cref="RootDocument"/>.</param>
        /// <returns><see cref="Document"/>[]</returns>
        public static Document[] GetDocuments(string root)
        {
            if (Database.Instance[root] == null)
                return Array.Empty<Document>();

            return Database.Instance[root].GetValues();
        }

        /// <summary>
        /// Returns whether the <see cref="Document"/> exists.
        /// </summary>
        /// <param name="root">The name of the <see cref="RootDocument"/>.</param>
        /// <param name="key">The key of the <see cref="Document"/>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool DocumentExists(string root, string key)
        {
            if (Database.Instance[root] == null)
                return false;

            if (Database.Instance[root][key] == null)
                return false;

            return true;
        }

        /// <summary>
        /// Returns whether a path contains illegal characters.
        /// </summary>
        /// <param name="path">The <see cref="string"/> to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ContainsIllegalCharacters(string path)
        {
            for(int i = 0; i < path.Length; i++)
            {
                if (!PATH_ALLOWED_CHARS.Contains(path[i]))
                    return true;
            }

            return false;
        }
    }
}