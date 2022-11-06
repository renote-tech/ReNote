using ProtoBuf;
using Server.Common.Utilities;

namespace Server.ReNote
{
    [ProtoContract]
    public class Database
    {
        [ProtoMember(1)]
        private List<Document> dbDocuments = new List<Document>();
 
        public Document this[string name]
        {
            get
            {
                for(int i = 0;  i < dbDocuments.Count; i++)
                {
                    if (dbDocuments[i].Name == name)
                        return dbDocuments[i];
                }
                return null;
            }
            set
            {
                for(int i = 0; i < dbDocuments.Count; i++)
                {
                    if (dbDocuments[i].Name != name)
                        continue;

                    if (value is Document)
                        dbDocuments[i] = value;
                }
            }
        }

        public void AddDocument(Document document)
        {
            if (document != null)
                dbDocuments.Add(document);
        }

        public void AddDocument(string documentName)
        {
            if (!string.IsNullOrWhiteSpace(documentName))
                dbDocuments.Add(new Document(documentName));
        }

        public void RemoveDocument(Document document)
        {
            if (document != null && dbDocuments.Contains(document))
                dbDocuments.Remove(document);
        }

        public void RemoveDocument(string name)
        {
            for(int i = 0; i < dbDocuments.Count; i++)
        }

        public bool IsEmpty()
        {
            return dbDocuments.Count == 0;
        }

        public void Load(string location)
        {
            if (FileUtil.IsFileBeingUsed(location))
                return;

            if (!File.Exists(location))
                return;

            byte[] data = File.ReadAllBytes(location);
            using MemoryStream stream = new MemoryStream(data);
            Database db = Serializer.Deserialize<Database>(stream);

            dbDocuments = db.dbDocuments;
        }

        public async void Save(string location)
        {
            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, this);
            await File.WriteAllBytesAsync(location, stream.ToArray());
        }
    }

    [ProtoContract]
    public class Document
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        private Dictionary<string, DocumentData> dbKeysValues = new Dictionary<string, DocumentData>();
        
        public Document()
        {

        }

        public Document(string name)
        {
            Name = name;
        }
        
        public object this[string key]
        {
            get
            {
                if (dbKeysValues.ContainsKey(key))
                    return  Convert.ChangeType(dbKeysValues[key].Value, dbKeysValues[key].Type);
                else
                    return null;
            }
            set
            {
                string dataValue = (string)Convert.ChangeType(value, TypeCode.String);
                if (dbKeysValues.ContainsKey(key))
                    dbKeysValues[key] = new DocumentData(dataValue, value.GetType());
            }
        }

        public void Add(string key, object value)
        {
            string dataValue = (string)Convert.ChangeType(value, TypeCode.String);
            if (!dbKeysValues.ContainsKey(key))
                dbKeysValues.Add(key, new DocumentData(dataValue, value.GetType()));
        }
    }

    [ProtoContract]
    public class DocumentData
    {
        [ProtoMember(1)]
        public string Value { get; set; }
        [ProtoMember(2)]
        public Type Type { get; set; }

        public DocumentData()
        {

        }

        public DocumentData(string value, Type type)
        {
            Value = value;
            Type = type;
        }
    }
}