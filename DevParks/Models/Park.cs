using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevParks.Models
{
    public class Park
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }

    public class ParkRides : Park
    {
        [JsonProperty("rides")]
        public List<Ride> Rides { get; set; } = new List<Ride>();
    }

    public class Ride
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("waitTime")]
        public string WaitTime { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }
}