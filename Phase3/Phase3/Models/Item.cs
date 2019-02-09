using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Phase3.Models
{
    public class Item
    {
        [JsonProperty(PropertyName = "Crime ID")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Completed")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "Month")]
        public string Month { get; set; }

        [JsonProperty(PropertyName = "Crime type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "LSOA code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

    }
}