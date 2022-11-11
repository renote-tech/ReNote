using System.Dynamic;
using Newtonsoft.Json;
using ProtoBuf;
using Server.Common.Utilities;

namespace Server.ReNote.Data
{
    [ProtoContract]
    public class Database
    {
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
        public string SaveLocation { get; set; }

        private static Database instance;

        [ProtoMember(1)]
        private List<RootDocument> dbRootDocuments = new List<RootDocument>();

        public RootDocument this[string name]
        {
            get
            {
                for (int i = 0; i < dbRootDocuments.Count; i++)
                {
                    if (dbRootDocuments[i].Name == name)
                        return dbRootDocuments[i];
                }
                return null;
            }
            set
            {
                for (int i = 0; i < dbRootDocuments.Count; i++)
                {
                    if (dbRootDocuments[i].Name != name)
                        continue;

                    dbRootDocuments[i] = value;
                }
            }
        }

        public RootDocument this[int index]
        {
            get
            {
                if (dbRootDocuments.Count - 1 <= index)
                    return dbRootDocuments[index];

                return null;
            }
        }

        public void AddDocument(RootDocument document)
        {
            if (document != null)
                dbRootDocuments.Add(document);
        }

        public void AddDocument(string documentName)
        {
            if (!string.IsNullOrWhiteSpace(documentName))
                dbRootDocuments.Add(new RootDocument(documentName));
        }

        public void RemoveDocument(RootDocument document)
        {
            if (document != null && dbRootDocuments.Contains(document))
                dbRootDocuments.Remove(document);
        }

        public void RemoveDocument(string name)
        {
            for (int i = 0; i < dbRootDocuments.Count; i++)
            {
                if (dbRootDocuments[i].Name == name)
                    dbRootDocuments.RemoveAt(i);
            }
        }

        public bool IsEmpty()
        {
            return dbRootDocuments.Count == 0;
        }

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


            dbRootDocuments = db.dbRootDocuments;
        }

        public void Clear()
        {
            dbRootDocuments.Clear();
        }

        public async void Save()
        {
            if (IsEmpty())
                return;

            if (string.IsNullOrWhiteSpace(SaveLocation))
                return;

            if (FileUtil.IsFileBeingUsed(SaveLocation))
                return;

            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, this);

            await File.WriteAllBytesAsync(SaveLocation, stream.ToArray());
        }
    }

    [ProtoContract]
    public class RootDocument
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        private Dictionary<string, Document> documents = new Dictionary<string, Document>();

        public RootDocument()
        {

        }

        public RootDocument(string name)
        {
            Name = name;
        }

        public object this[string key]
        {
            get
            {
                if (documents.ContainsKey(key))
                    return documents[key];
               
                return null;
            }
            set
            {
                if (value is not string)
                    return;

                if (documents.ContainsKey(key))
                    documents[key] = new Document((string)value);
            }
        }

        public object this[int index]
        {
            get
            {
                if (documents.Count - 1 <= index)
                    return documents.ElementAt(index).Value.Data;

                return null;
            }
        }

        public void AddKey(string key, string value)
        {
            if (!documents.ContainsKey(key))
                documents.Add(key, new Document(value));
        }

        public void AddKey(string key, Document value)
        {
            if (!documents.ContainsKey(key))
                documents.Add(key, value);
        }

        public void RemoveKey(string key)
        {
            if (documents.ContainsKey(key))
                documents.Remove(key);
        }

        public void RemoveKey(Document document)
        {
            for (int i = 0; i < documents.Count; i++)
            {
                KeyValuePair<string, Document> keyValue = documents.ElementAt(i);
                if (keyValue.Value == document)
                    documents.Remove(keyValue.Key);
            }
        }

        public Document[] GetValues()
        {
            Document[] docs = new Document[documents.Count];
            for(int i = 0; i < docs.Length; i++)
                docs[i] = documents.ElementAt(i).Value;

            return docs;
        }

        public bool IsEmpty()
        {
            return documents.Count == 0;
        }
    }

    [ProtoContract]
    public class Document
    {
        public dynamic Data
        {
            get
            {
                if (!JsonUtil.ValiditateJson(jsonData))
                    return null;

                return JsonConvert.DeserializeObject<ExpandoObject>(jsonData);
            }
        }

        [ProtoMember(1)]
        private string jsonData;

        public Document()
        {

        }

        public Document(string data)
        {
            Set(data);
        }

        public void Set(string data)
        {
            jsonData = data;
        }

        public string GetRaw()
        {
            return jsonData;
        }
    }
}