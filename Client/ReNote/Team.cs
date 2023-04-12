using Newtonsoft.Json;

namespace Client.ReNote
{
    public class Team
    {
        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [JsonProperty("teamGrade")]
        public string TeamGrade { get; set; }

        [JsonProperty("teamLeaderId")]
        public long TeamLeaderId { get; set; }

        [JsonProperty("delegates")]
        public long[] Delegates { get; set; }

        [JsonProperty("alternates")]
        public long[] Alternates { get; set; }
    }
}