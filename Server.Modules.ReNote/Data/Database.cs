using ProtoBuf;
using Server.Common;
using Server.Common.Utilities;
#if !DEBUG
    using Server.Common.Proto;
#endif

namespace Server.ReNote.Data
{
    [ProtoContract]
    public class Database
    {
        /// <summary>
        /// The location of the database file.
        /// </summary>
        public string SaveLocation { get; set; }

        /// <summary>
        /// The <see cref="Database"/>'s instance <see cref="Container"/> list.
        /// </summary>
        [ProtoMember(1)]
        private List<Container> containers = new List<Container>();

        /// <summary>
        /// Returns a <see cref="Container"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="Container"/> class.</param>
        /// <returns><see cref="Container"/></returns>
        public Container this[string name]
        {
            get
            {
                for (int i = 0; i < containers.Count; i++)
                {
                    if (containers[i].Name == name)
                        return containers[i];
                }
                return null;
            }
            set
            {
                for (int i = 0; i < containers.Count; i++)
                {
                    if (containers[i].Name != name)
                        continue;

                    containers[i] = value;
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="Container"/> to the <see cref="containers"/> list.
        /// </summary>
        /// <param name="containerName">The <see cref="Container"/> name.</param>
        public void AddContainer(string containerName)
        {
            if (!string.IsNullOrWhiteSpace(containerName))
                containers.Add(new Container(containerName));
        }

        public Container[] GetContainers()
        {
            return containers.ToArray();
        }

        public bool IsContainerExists(string containerName)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].Name == containerName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the <see cref="containers"/> list from a database file. Returns whether the file was actually loaded.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool Load()
        {
            if (string.IsNullOrWhiteSpace(SaveLocation))
                return false;

            if (FileUtil.IsFileBeingUsed(SaveLocation))
                return false;

            if (!File.Exists(SaveLocation))
                return false;

            try
            {
#if !DEBUG
                byte[] encryptedData = File.ReadAllBytes(SaveLocation);
                byte[] decryptedData = ByteShifting.Decrypt(encryptedData);
                using MemoryStream stream = new MemoryStream(decryptedData);
                Database database = Serializer.Deserialize<Database>(stream);

                containers = database.containers;
#else
                byte[] data = File.ReadAllBytes(SaveLocation);
                using MemoryStream stream = new MemoryStream(data);
                Database database = Serializer.Deserialize<Database>(stream);

                containers = database.containers;
#endif
            }
            catch (ProtoException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves the <see cref="Database"/> to a database file.
        /// </summary>
        /// <param name="doBackup">If true; do a backup of the <see cref="Database"/>.</param>
        public async Task<bool> SaveAsync(bool doBackup = false)
        {
            if(doBackup)
                await BackupAsync();

            return await SaveAsync(SaveLocation);
        }

        /// <summary>
        /// Saves the <see cref="Database"/> to a database file.
        /// </summary>
        /// <param name="saveLocation">The file location to save the <see cref="Database"/>.</param>
        private async Task<bool> SaveAsync(string saveLocation)
        {
            if (IsEmpty())
                return false;

            if (FileUtil.IsFileBeingUsed(saveLocation))
                return false;

            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, this);
#if !DEBUG
            await ByteShifting.EncryptAsync(stream.ToArray(), saveLocation);
#else
            await File.WriteAllBytesAsync(saveLocation, stream.ToArray());
#endif
            return true;
        }

        /// <summary>
        /// Back up the <see cref="Database"/>'s instance to a different file (backups/db_school_YYYY_DD_MM_HH_mm_ss_MIM.dat)
        /// </summary>
        public async Task<bool> BackupAsync()
        {
            string dirName = Configuration.ReNoteConfig.DBBackupLocation ?? "backups";
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            string formatTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff");
            string fileName = $"db_school_{formatTime}.dat";
            string path = PathUtil.NormalizeToOS(Path.Combine(dirName, fileName));

            return await SaveAsync(path);
        }

        /// <summary>
        /// Clears the <see cref="containers"/> list.
        /// </summary>
        public void Clear()
        {
            containers.Clear();
        }

        /// <summary>
        /// Returns whether the <see cref="containers"/> list is empty;
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsEmpty()
        {
            return containers.Count == 0;
        }
    }

    [ProtoContract]
    public class Container
    {
        /// <summary>
        /// The name of the <see cref="Container"/>'s instance.
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// The dictionary that contains all of the items.
        /// </summary>
        [ProtoMember(2)]
        private readonly Dictionary<string, string> items = new Dictionary<string, string>();

        public Container()
        { }

        public Container(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Returns an item.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <returns><see cref="string"/></returns>
        public string this[string name]
        {
            get
            {
                if (items.ContainsKey(name))
                    return items[name];
               
                return null;
            }
            set
            {
                if (value is not string)
                    return;

                if (items.ContainsKey(name))
                    items[name] = value;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="items"/> list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The item value.</param>
        public void AddItem(string key, string value)
        {
            if (!items.ContainsKey(key))
                items.Add(key, value);
        }

        /// <summary>
        /// Removes an item from the <see cref="items"/> list. Returns whether the key has actually been removed.
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <returns><see cref="bool"/></returns>
        public bool RemoveItem(string key)
        {
            if (!items.ContainsKey(key))
                return false;

            items.Remove(key);
            return true;
        }

        /// <summary>
        /// Returns whether the item already exists.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <returns><see cref="bool"/></returns>
        public bool IsItemExists(string key)
        {
            if (!items.ContainsKey(key))
                return false;

            return true;
        }

        /// <summary>
        /// Returns a dictionary with all the items.
        /// </summary>
        /// <returns>Dictionary[<see cref="string"/>, <see cref="string"/>]</returns>
        public Dictionary<string, string> GetItems()
        {
            return items;
        }

        /// <summary>
        /// Returns all the values of the items.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public string[] GetItemsValues()
        {
            string[] docsValues = new string[items.Count];
            for (int i = 0; i < docsValues.Length; i++)
                docsValues[i] = items.ElementAt(i).Value;

            return docsValues;
        }

        /// <summary>
        /// Returns whether the <see cref="items"/> list is empty.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsEmpty()
        {
            return items.Count == 0;
        }
    }
}