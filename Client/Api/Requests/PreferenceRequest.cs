﻿using Newtonsoft.Json;

namespace Client.Api.Requests
{
    internal class PreferenceRequest : Request
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("theme")]
        public string Theme { get; set; }
    }
}