using System.Dynamic;
using Newtonsoft.Json;
using ProtoBuf;
using Server.Common.Utilities;

namespace Server.ReNote.Data
{
    [ProtoContract]
    public class Database
    {
        /// <summary>
        /// The current existing instance of the <see cref="Database"/> class; creates one if <see cref="instance"/> is null.
        /// </summary>
        public static Database Instance
        {
            get
            {
                return instance ??= new Database();
            }
            private set
            {
                instance = value;
            }
        }
        /// <summary>
        /// The location of the database file.
        /// </summary>
        public string SaveLocation { get; set; }

        /// <summary>
        /// The private instance of the <see cref="Instance"/> field.
        /// </summary>
        private static Database instance;

        /// <summary>
        /// The <see cref="Database"/>'s instance <see cref="RootDocument"/> list.
        /// </summary>
        [ProtoMember(1)]
        private List<RootDocument> rootDocuments = new List<RootDocument>();

        /// <summary>
        /// Returns a <see cref="RootDocument"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="RootDocument"/> class.</param>
        /// <returns><see cref="RootDocument"/></returns>
        public RootDocument this[string name]
        {
            get
            {
                for (int i = 0; i < rootDocuments.Count; i++)
                {
                    if (rootDocuments[i].Name == name)
                        return rootDocuments[i];
                }
                return null;
            }
            set
            {
                for (int i = 0; i < rootDocuments.Count; i++)
                {
                    if (rootDocuments[i].Name != name)
                        continue;

                    rootDocuments[i] = value;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="RootDocument"/> class.
        /// </summary>
        /// <param name="index">The index of the <see cref="RootDocument"/> within the <see cref="rootDocuments"/> list.</param>
        /// <returns><see cref="RootDocument"/></returns>
        public RootDocument this[int index]
        {
            get
            {
                if (rootDocuments.Count - 1 <= index)
                    return rootDocuments[index];

                return null;
            }
        }

        /// <summary>
        /// Adds a <see cref="RootDocument"/> to the <see cref="rootDocuments"/> list.
        /// </summary>
        /// <param name="document">The <see cref="RootDocument"/> to be added.</param>
        public void AddDocument(RootDocument document)
        {
            if (document != null)
                rootDocuments.Add(document);
        }

        /// <summary>
        /// Adds a <see cref="RootDocument"/> to the <see cref="rootDocuments"/> list.
        /// </summary>
        /// <param name="documentName">The <see cref="RootDocument"/> name.</param>
        public void AddDocument(string documentName)
        {
            if (!string.IsNullOrWhiteSpace(documentName))
                rootDocuments.Add(new RootDocument(documentName));
        }

        /// <summary>
        /// Removes a <see cref="RootDocument"/> from the <see cref="rootDocuments"/> class.
        /// </summary>
        /// <param name="document">The <see cref="RootDocument"/> to be removed.</param>
        public void RemoveDocument(RootDocument document)
        {
            if (document != null && rootDocuments.Contains(document))
                rootDocuments.Remove(document);
        }

        /// <summary>
        /// Removes a <see cref="RootDocument"/> from the <see cref="rootDocuments"/> class.
        /// </summary>
        /// <param name="name">The <see cref="RootDocument"/> name.</param>
        public void RemoveDocument(string name)
        {
            for (int i = 0; i < rootDocuments.Count; i++)
            {
                if (rootDocuments[i].Name == name)
                    rootDocuments.RemoveAt(i);
            }
        }
        
        /// <summary>
        /// Returns whether the <see cref="rootDocuments"/> list is empty;
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsEmpty()
        {
            return rootDocuments.Count == 0;
        }

        /// <summary>
        /// Loads the <see cref="rootDocuments"/> list from a database file.
        /// </summary>
        public void Load()
        {
            if (string.IsNullOrWhiteSpace(SaveLocation))
                return;

            if (FileUtil.IsFileBeingUsed(SaveLocation))
                return;

            if (!File.Exists(SaveLocation))
                return;

            byte[] data = File.ReadAllBytes(SaveLocation);
            using MemoryStream stream = new MemoryStream(data);
            Database db = Serializer.Deserialize<Database>(stream);

            rootDocuments = db.rootDocuments;
        }

        /// <summary>
        /// Clears the <see cref="rootDocuments"/> list.
        /// </summary>
        public void Clear()
        {
            rootDocuments.Clear();
        }

        /// <summary>
        /// Saves the <see cref="Database"/> to a database file.
        /// </summary>
        public void Save()
        {
            Backup();
            Save(SaveLocation);
        }

        /// <summary>
        /// Back up the <see cref="Database"/>'s instance to a different file (backups/db_school_XXXX_XX_XX_XX_XX_XX_XXX.dat)
        /// </summary>
        public void Backup()
        {
            DateTime curTime = DateTime.Now;
            string saveLocation = $"backups/db_school_{curTime.Year}_{curTime.Month}_{curTime.Day}_{curTime.Hour}_{curTime.Minute}_{curTime.Second}_{curTime.Millisecond}.dat";

            Save(saveLocation);
        }

        /// <summary>
        /// Saves the <see cref="Database"/> to a database file.
        /// </summary>
        /// <param name="saveLocation">The file location to save the <see cref="Database"/>.</param>
        private async void Save(string saveLocation)
        {
            if (IsEmpty())
                return;

            if (FileUtil.IsFileBeingUsed(saveLocation))
                return;

            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, this);

            await File.WriteAllBytesAsync(saveLocation, stream.ToArray());
        }
    }

    [ProtoContract]
    public class RootDocument
    {
        /// <summary>
        /// The name of the <see cref="RootDocument"/>'s instance.
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="RootDocument"/>'s instance <see cref="Document"/> list.
        /// </summary>
        [ProtoMember(2)]
        private Dictionary<string, Document> documents = new Dictionary<string, Document>();

        public RootDocument()
        {

        }

        public RootDocument(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Returns a <see cref="Document"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="Document"/>.</param>
        /// <returns><see cref="object"/></returns>
        public object this[string name]
        {
            get
            {
                if (documents.ContainsKey(name))
                    return documents[name];
               
                return null;
            }
            set
            {
                if (value is not string)
                    return;

                if (documents.ContainsKey(name))
                    documents[name] = new Document((string)value);
            }
        }

        /// <summary>
        /// Returns a <see cref="Document"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="Document"/> within the <see cref="documents"/> list.</param>
        /// <returns><see cref="object"/></returns>
        public object this[int index]
        {
            get
            {
                if (documents.Count - 1 <= index)
                    return documents.ElementAt(index).Value.Data;

                return null;
            }
        }

        /// <summary>
        /// Adds a <see cref="Document"/> to the <see cref="documents"/> list.
        /// </summary>
        /// <param name="key">The key of the <see cref="Document"/> to be added.</param>
        /// <param name="value">The <see cref="Document"/>.</param>
        public void AddKey(string key, string value)
        {
            if (!documents.ContainsKey(key))
                documents.Add(key, new Document(value));
        }

        /// <summary>
        /// Adds a <see cref="Document"/> to the <see cref="documents"/> list.
        /// </summary>
        /// <param name="key">The key of the <see cref="Document"/> to be added.</param>
        /// <param name="value">The <see cref="Document"/>.</param>
        public void AddKey(string key, Document value)
        {
            if (!documents.ContainsKey(key))
                documents.Add(key, value);
        }

        /// <summary>
        /// Removes a <see cref="Document"/> from the <see cref="documents"/> list.
        /// </summary>
        /// <param name="key">The key of the <see cref="Document"/> to be removed.</param>
        public void RemoveKey(string key)
        {
            if (documents.ContainsKey(key))
                documents.Remove(key);
        }

        /// <summary>
        /// Removes a <see cref="Document"/> from the <see cref="documents"/> list.
        /// </summary>
        /// <param name="document">The <see cref="Document"/> to be removed.</param>
        public void RemoveKey(Document document)
        {
            for (int i = 0; i < documents.Count; i++)
            {
                KeyValuePair<string, Document> keyValue = documents.ElementAt(i);
                if (keyValue.Value == document)
                    documents.Remove(keyValue.Key);
            }
        }

        /// <summary>
        /// Gets all the <see cref="Document"/>s from the <see cref="documents"/> list.
        /// </summary>
        /// <returns><see cref="Document"/>[]</returns>
        public Document[] GetValues()
        {
            Document[] docs = new Document[documents.Count];
            for(int i = 0; i < docs.Length; i++)
                docs[i] = documents.ElementAt(i).Value;

            return docs;
        }

        /// <summary>
        /// Returns whether the <see cref="documents"/> list is empty.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsEmpty()
        {
            return documents.Count == 0;
        }
    }

    [ProtoContract]
    public class Document
    {
        /// <summary>
        /// The deserialized data of <see cref="jsonData"/> as an <see cref="ExpandoObject"/>.
        /// </summary>
        public dynamic Data
        {
            get
            {
                if (!JsonUtil.ValiditateJson(jsonData))
                    return null;

                return JsonConvert.DeserializeObject<ExpandoObject>(jsonData);
            }
        }

        /// <summary>
        /// The JSON data of the <see cref="Document"/> as a <see cref="string"/>.
        /// </summary>
        [ProtoMember(1)]
        private string jsonData;

        public Document()
        {

        }

        public Document(string data)
        {
            Set(data);
        }

        /// <summary>
        /// Sets the <see cref="Document"/>'s JSON data.
        /// </summary>
        /// <param name="data">The data to be setted.</param>
        public void Set(string data)
        {
            jsonData = data;
        }

        /// <summary>
        /// Returns the JSON data of the <see cref="Document"/>.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public string GetRaw()
        {
            return jsonData;
        }
    }
}