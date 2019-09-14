using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DevParks.Backend.Model
{
    public class Ride
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parkId")]
        public string ParkId { get; set; }

        [JsonProperty("fastpassbooth")]
        public string FastpassBooth { get; set; }

        [JsonProperty("shortname")]
        public string ShortName { get; set; }

        [JsonProperty("openemhmorning")]
        public string OpenEMHMorning { get; set; }

        [JsonProperty("openemhevening")]
        public string OpenEMHEvening { get; set; }

        [JsonProperty("seasonal")]
        public string Seasonal { get; set; }

        [JsonProperty("loadingspeed")]
        public string LoadingSpeed { get; set; }

        [JsonProperty("specialneeds")]
        public string SpecialNeeds { get; set; }

        [JsonProperty("intense")]
        public bool Intense { get; set; }

        [JsonProperty("heightrestrictions")]
        public string HeightRestrictions { get; set; }

        [JsonProperty("riderswap")]
        public bool RiderSwap { get; set; }

        [JsonProperty("fastpassonly")]
        public bool FastPassOnly { get; set; }

        [JsonProperty("waitTime")]
        public string WaitTime { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }
}
