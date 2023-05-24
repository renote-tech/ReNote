namespace Client.ReNote.Data;

using Newtonsoft.Json;

internal class School
{
    public static School Instance;

    [JsonProperty("schoolName")]
    public string SchoolName { get; set; }

    [JsonProperty("schoolType")]
    public SchoolType SchoolType { get; set; }

    [JsonProperty("schoolLocation")]
    public string SchoolLocation { get; set; }
}

internal enum SchoolType
{
    HIGH_SCHOOL = 0,
    MIDDLE_SCHOOL = 1,
    PRIMARY_SCHOOL = 2
}