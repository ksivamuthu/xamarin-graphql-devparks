using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DevParks.Backend.Model
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Park
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parkId")]
        public string ParkId { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("openinghrs")]
        public string OpeningHours { get; set; }

        [JsonProperty("closinghrs")]
        public string ClosingHours { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }
}
