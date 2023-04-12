using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class Team
    {
        /// <summary>
        /// The name of the team.
        /// Example: 10G
        /// </summary>
        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        /// <summary>
        /// The internal Id of the team.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// In the case of a class, this field is for the class' grade.
        /// Example: 10th Grade
        /// </summary>
        [JsonProperty("teamGrade")]
        public string TeamGrade { get; set; }

        /// <summary>
        /// In the case of a class, this field is for the students' uids.
        /// </summary>
        [JsonProperty("students")]
        public long[] Students { get; set; }

        /// <summary>
        /// In the case of a class, this field is for the form teacher's uid.
        /// </summary>
        [JsonProperty("teamLeaderId")]
        public long TeamLeaderId { get; set; }

        /// <summary>
        /// In the case of a class, this field is for the delegates' uids.
        /// </summary>
        [JsonProperty("delegates")]
        public long[] Delegates { get; set; }

        /// <summary>
        /// In the case of a class, this field is for delegates-alternates' uids.
        /// </summary>
        [JsonProperty("alternates")]
        public long[] Alternates { get; set; }

        /// <summary>
        /// Returns the number of students in the team.
        /// </summary>
        public int StudentsCount
        {
            get
            {
                if (Students == null)
                    return 0;

                return Students.Length;
            }
        }
    }
}