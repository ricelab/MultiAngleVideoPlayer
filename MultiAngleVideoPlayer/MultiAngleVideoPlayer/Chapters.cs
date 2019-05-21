using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiAngleVideoPlayer
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Chapters
    {
        [JsonProperty(PropertyName = "Items")]
        public ChapAttributes[] Attributes { get; set; }

        //[JsonProperty(PropertyName = "ChapterName")]
        //public String name;

        //[JsonProperty(PropertyName = "Time")]
        //public double startTime;

    }
}

