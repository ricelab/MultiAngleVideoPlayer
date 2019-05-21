using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiAngleVideoPlayer
{
    /// <summary>
    /// Class for json deserialization.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Chapters
    {
        //"Items" contains all chapters. ChapAttributes are deserialized and stored in Attributes list.
        [JsonProperty(PropertyName = "Items")]
        public ChapAttributes[] Attributes { get; set; }

    }
}

