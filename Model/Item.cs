using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace OpenProjects.Model
{
    public class Item : ViewModelBase
    {
        [JsonProperty("filepath")] public string FilePath { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("minidescription")] public string MiniDescription { get; set; }

        [JsonProperty("author")] public string Author { get; set; }

        [JsonProperty("isedit")] public bool IsEdit { get; set; } = true;
    }
}