using System;
using System.Collections.Generic;
using DevParks.Models;
using Newtonsoft.Json;

namespace DevParks.Services
{
    public class ParksResponse
    {
        [JsonProperty("parks")]
        public List<Park> Parks { get; set; }
    }
}
