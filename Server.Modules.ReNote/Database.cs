using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

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
                bool isFound = false;
                for(int i = 0; i < dbDocuments.Count; i++)
                {
                    if (dbDocuments[i].Name == name)
                        dbDocuments[i] = value; 
                }

                if (!isFound)
                    dbDocuments.Add(value);
            }
        }

        public async void Save(string location)
        {
            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, this);
            await File.WriteAllBytesAsync(location, stream.ToArray());
        }

        public void Load(string location)
        {
            byte[] data = File.ReadAllBytes(location);
            using MemoryStream stream = new MemoryStream(data);
            Database db = Serializer.Deserialize<Database>(stream);
            
            dbDocuments = db.dbDocuments;
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
                if (dbKeysValues.ContainsKey(key))
                    dbKeysValues[key] = new DocumentData((string)Convert.ChangeType(value, TypeCode.String), value.GetType());
                else
                    dbKeysValues.Add(key, new DocumentData((string)Convert.ChangeType(value, TypeCode.String), value.GetType()));
            }
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