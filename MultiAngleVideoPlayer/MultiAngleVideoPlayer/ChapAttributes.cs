using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MultiAngleVideoPlayer
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ChapAttributes
    {
        [JsonProperty(PropertyName = "ChapterName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Time")]
        public double StartTime { get; set; }
    }
}
