using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public string FileLocation { get; set; }

        /// <summary>
        /// External boolean property.
        /// </summary>
        public bool NeedSave { get; set; }

        /// <summary>
        /// Used with the lock() statement; ensure one action at the time.
        /// </summary>
        internal readonly object Locker = new object();

        /// <summary>
        /// The <see cref="Database"/>'s instance <see cref="Container"/> list.
        /// </summary>
        [ProtoMember(1)]
        private List<Container> m_Containers = new List<Container>();


        /// <summary>
        /// Returns a <see cref="Container"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="Container"/> class.</param>
        /// <returns><see cref="Container"/></returns>
        public Container this[string name]
        {
            get
            {
                lock (Locker)
                {
                    for (int i = 0; i < m_Containers.Count; i++)
                    {
                        if (m_Containers[i].Name == name)
                            return m_Containers[i];
                    }
                    return null;
                }
            }
            set
            {
                lock (Locker)
                {
                    for (int i = 0; i < m_Containers.Count; i++)
                    {
                        if (m_Containers[i].Name != name)
                            continue;

                        m_Containers[i] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the container list.
        /// </summary>
        /// <param name="containerList">The container list.</param>
        public void SetContainerList(Container[] containerList)
        {
            lock (Locker)
            {
                m_Containers = containerList.ToList();
            }
        }

        /// <summary>
        /// Creates and adds a <see cref="Container"/> to the <see cref="m_Containers"/> list.
        /// </summary>
        /// <param name="containerName">The <see cref="Container"/> name.</param>
        public void AddContainer(string containerName)
        {
            lock (Locker)
            {
                if (!string.IsNullOrWhiteSpace(containerName))
                    return;

                m_Containers.Add(new Container(this, containerName));
            }
        }

        /// <summary>
        /// Removes a <see cref="Container"/> from the <see cref="m_Containers"/> list.
        /// Returns whether the operation was successful or not.
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns><see cref="bool"/></returns>
        public bool RemoveContainer(string containerName)
        {
            lock (Locker)
            {
                if (string.IsNullOrWhiteSpace(containerName))
                    return false;

                for (int i = 0; i < m_Containers.Count; i++)
                {
                    if (m_Containers[i].Name == containerName)
                        return m_Containers.Remove(m_Containers[i]);
                }

                return false;
            }
        }

        /// <summary>
        /// Clears the content from a specified <see cref="Container"/>.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        public void ClearContainerContent(string containerName)
        {
            lock (Locker)
            {
                if (string.IsNullOrWhiteSpace(containerName))
                    return;

                for (int i = 0; i < m_Containers.Count; i++)
                {
                    if (m_Containers[i].Name == containerName)
                        m_Containers[i].Clear();
                }
            }
        }

        public bool IsContainerExists(string containerName)
        {
            lock (Locker) 
            {
                for (int i = 0; i < m_Containers.Count; i++)
                {
                    if (m_Containers[i].Name == containerName)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns all the containers within the <see cref="m_Containers"/> list.
        /// </summary>
        /// <returns><see cref="Container"/>[]</returns>
        public Container[] GetContainers()
        {
            lock (Locker)
            {
                return m_Containers.ToArray();
            }
        }

        /// <summary>
        /// Clears the <see cref="m_Containers"/> list.
        /// </summary>
        public void Clear()
        {
            lock (Locker)
            {
                m_Containers.Clear();
            }
        }

        /// <summary>
        /// Returns whether the <see cref="m_Containers"/> list is empty;
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsEmpty()
        {
            lock (Locker)
            {
                return m_Containers.Count == 0;
            }
        }

        /// <summary>
        /// Loads the <see cref="m_Containers"/> list from a database file.
        /// Returns whether the file was actually loaded.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool Load()
        {
            lock (Locker)
            {
                if (string.IsNullOrWhiteSpace(FileLocation))
                    return false;

                if (FileUtil.IsFileBeingUsed(FileLocation))
                    return false;

                if (!File.Exists(FileLocation))
                    return false;

                try
                {
#if DEBUG
                    byte[] data = File.ReadAllBytes(FileLocation);
                    using MemoryStream stream = new MemoryStream(data);
                    Database database = Serializer.Deserialize<Database>(stream);

                    m_Containers = database.m_Containers;

                    for (int i = 0; i < m_Containers.Count; i++)
                        m_Containers.ElementAt(i).SetParent(this);
#else

                    byte[] data = File.ReadAllBytes(FileLocation);
                    using MemoryStream stream = new MemoryStream(data);
                    Database database = Serializer.Deserialize<Database>(stream);

                    /*
                     * byte[] encryptedData = File.ReadAllBytes(SaveLocation);
                     * byte[] decryptedData = ByteShifting.Decrypt(encryptedData);
                     * using MemoryStream stream = new MemoryStream(decryptedData);
                     * Database database = Serializer.Deserialize<Database>(stream);
                    */

                    m_Containers = database.m_Containers;

                    for (int i = 0; i < m_Containers.Count; i++)
                        m_Containers.ElementAt(i).SetParent(this);
#endif
                }
                catch (ProtoException)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Saves the <see cref="Database"/> to a database file.
        /// Returns whether the file has actually been saved.
        /// </summary>
        /// <param name="doBackup">If true; do a backup of the <see cref="Database"/>.</param>
        public async Task<bool> SaveAsync(bool doBackup = false)
        {
            if(doBackup)
                await BackupAsync();

            return await SaveAsync(FileLocation);
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
        /// Saves the <see cref="Database"/> to a database file.
        /// </summary>
        /// <param name="saveLocation">The file location to save the <see cref="Database"/>.</param>
        public bool Save()
        {
            if (IsEmpty())
                return false;

            if (FileUtil.IsFileBeingUsed(FileLocation))
                return false;

            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, this);
#if DEBUG
            File.WriteAllBytes(FileLocation, stream.ToArray());
#else
            ByteShifting.Encrypt(stream.ToArray(), saveLocation);
#endif
            return true;
        }

        /// <summary>
        /// Saves the <see cref="Database"/> to a database file asynchronously.
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
#if DEBUG
            await File.WriteAllBytesAsync(saveLocation, stream.ToArray());
#else
                await ByteShifting.EncryptAsync(stream.ToArray(), saveLocation);
#endif
            return true;
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
        private readonly Dictionary<string, string> m_Items = new Dictionary<string, string>();

        /// <summary>
        /// Parent database of that container instance.
        /// </summary>
        private Database m_Parent;

        /// <summary>
        /// Parameterless constructor required by ProtoBuf.
        /// </summary>
        public Container()
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent database instance.</param>
        /// <param name="name">The name of the contaienr.</param>
        public Container(Database parent, string name)
        {
            m_Parent = parent;
            Name = name;
        }

        /// <summary>
        /// Returns an item.
        /// Sets the content of an existing item.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <returns><see cref="string"/></returns>
        public string this[string name]
        {
            get
            {
                lock (m_Parent.Locker)
                {
                    if (m_Items.ContainsKey(name))
                        return m_Items[name];

                    return null;
                }
            }
            set
            {
                lock (m_Parent.Locker)
                {
                    if (value is not string)
                        return;

                    if (m_Items.ContainsKey(name))
                        m_Items[name] = value;
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="m_Items"/> list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The item value.</param>
        public void AddItem(string key, string value)
        {
            lock (m_Parent.Locker)
            {
                if (string.IsNullOrWhiteSpace(key))
                    return;

                if (!m_Items.ContainsKey(key))
                    m_Items.Add(key, value);
            }
        }

        /// <summary>
        /// Removes an item from the <see cref="m_Items"/> list. Returns whether the key has actually been removed.
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <returns><see cref="bool"/></returns>
        public bool RemoveItem(string key)
        {
            lock (m_Parent.Locker)
            {
                if (string.IsNullOrWhiteSpace(key))
                    return false;

                if (!m_Items.ContainsKey(key))
                    return false;

                m_Items.Remove(key);
                return true;
            }
        }

        /// <summary>
        /// Returns whether the item already exists.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <returns><see cref="bool"/></returns>
        public bool ContainsItem(string key)
        {
            lock (m_Parent.Locker)
            {
                if (string.IsNullOrWhiteSpace(key))
                    return false;

                if (!m_Items.ContainsKey(key))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Returns a dictionary with all the items.
        /// </summary>
        /// <returns>Dictionary[<see cref="string"/>, <see cref="string"/>]</returns>
        public Dictionary<string, string> GetItems()
        {
            lock (m_Parent.Locker)
            {
                return m_Items;
            }
        }

        /// <summary>
        /// Returns all the values of the items.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public string[] GetItemsValues()
        {
            lock (m_Parent.Locker)
            {
                string[] docsValues = new string[m_Items.Count];
                for (int i = 0; i < docsValues.Length; i++)
                    docsValues[i] = m_Items.ElementAt(i).Value;

                return docsValues;
            }
        }

        /// <summary>
        /// Clears all the content from the <see cref="m_Items"/> dictionary.
        /// </summary>
        public void Clear()
        {
            lock (m_Parent.Locker)
            {
                m_Items.Clear();
            }
        }

        /// <summary>
        /// Returns whether the <see cref="m_Items"/> list is empty.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsEmpty()
        {
            lock (m_Parent.Locker)
            {
                return m_Items.Count == 0;
            }
        }

        /// <summary>
        /// Sets the parent database instance.
        /// </summary>
        /// <param name="parent">The parent database.</param>
        internal void SetParent(Database parent)
        {
            m_Parent = parent;
        }
    }
}