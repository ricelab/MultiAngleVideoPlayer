using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MultiAngleVideoPlayer
{
    /// <summary>
    /// Class for json deserialization.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ChapAttributes
    {
        //Deserialized chapter name.
        [JsonProperty(PropertyName = "ChapterName")]
        public string Name { get; set; }

        //Deserialized chapter start time.
        [JsonProperty(PropertyName = "Time")]
        public double StartTime { get; set; }
    }
}
