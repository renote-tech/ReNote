using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    public class TeamProfileResponse
    {
        /// <summary>
        /// The team name of the <see cref="TeamProfileResponse"/>.
        /// </summary>
        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        /// <summary>
        /// The team id of the <see cref="TeamProfileResponse"/>.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// The team grade of the <see cref="TeamProfileResponse"/>.
        /// </summary>
        [JsonProperty("teamGrade")]
        public string TeamGrade { get; set; }

        /// <summary>
        /// The team leader id of the <see cref="TeamProfileResponse"/>.
        /// </summary>
        [JsonProperty("teamLeaderId")]
        public long TeamLeaderId { get; set; }

        /// <summary>
        /// The delegates of the <see cref="TeamProfileResponse"/>.
        /// </summary>
        [JsonProperty("delegates")]
        public long[] Delegates { get; set; }

        /// <summary>
        /// The alternates of the <see cref="TeamProfileResponse"/>.
        /// </summary>
        [JsonProperty("alternates")]
        public long[] Alternates { get; set; }
    }
}