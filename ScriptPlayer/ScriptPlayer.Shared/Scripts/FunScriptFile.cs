using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using ScriptPlayer.Shared.Converters;

namespace ScriptPlayer.Shared.Scripts
{
    /// <summary>
    /// https://godoc.org/github.com/funjack/launchcontrol/protocol/funscript
    /// 
    /// Example:
    /// 
    /// {
    ///	"version": "1.0",
    ///	"inverted": false,
    ///	"range": 90,
    ///	"actions": [
    ///		{"pos": 0, "at": 100},
    ///		{"pos": 100, "at": 500},
    ///		...
    ///	]
    ///}
    ///
    ///version: funscript version (optional, default="1.0")
    ///inverted: positions are inverted (0=100,100=0) (optional, default=false)
    ///range: range of moment to use in percent (0-100) (optional, default=90)
    ///actions: script for a Launch
    ///  pos: position in percent (0-100)
    ///  at : time to be at position in milliseconds
    /// </summary>
    public class FunScriptFile
    {
        [JsonProperty(PropertyName = "version")]
        [JsonConverter(typeof(StringToVersionConverter))]
        public Version Version { get; set; }

        [JsonProperty(PropertyName = "inverted")]
        public bool Inverted { get; set; }

        [JsonProperty(PropertyName = "range")]
        public int Range { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public List<FunScriptAction> Actions { get; set; }

        [JsonProperty(PropertyName = "metadata")]
        public FunScriptMetaData MetaData { get; set; }

        public FunScriptFile()
        {
            Inverted = false;
            Version = new Version(1,0);
            Range = 90;
            Actions = new List<FunScriptAction>();
        }

        public void Save(string filename)
        {
            string content = JsonConvert.SerializeObject(this);

            File.WriteAllText(filename, content, new UTF8Encoding(false));
        }
    }

    public class FunScriptMetaData
    {
        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "original_name")]
        public string OriginalName { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "url_video")]
        public string UrlVideo { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }

        [JsonProperty(PropertyName = "performers")]
        public List<string> Performers { get; set; }

        [JsonProperty(PropertyName = "paid")]
        public bool Paid { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        public FunScriptMetaData()
        {
            Tags = new List<string>();
            Performers = new List<string>();
        }

        public void CopyTo(FunScriptMetaData metaData)
        {
            metaData.Url = Url;
            metaData.UrlVideo = UrlVideo;
            metaData.Comment = Comment;
            metaData.Creator = Creator;
            metaData.OriginalName = OriginalName;
            metaData.Paid = Paid;

            metaData.Performers = Performers == null ? new List<string>() : new List<string>(Performers);
            metaData.Tags = Tags == null ? new List<string>() : new List<string>(Tags);
        }

        public FunScriptMetaData Duplicate()
        {
            FunScriptMetaData result = new FunScriptMetaData();
            CopyTo(result);
            return result;
        }
    }
}