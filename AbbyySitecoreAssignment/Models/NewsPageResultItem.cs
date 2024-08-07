using Newtonsoft.Json;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System.Collections.Generic;

namespace AbbyySitecoreAssignment.Models
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class NewsPageResultItem : SearchResultItem
    {
        // Add custom fields here
        [IndexField("category")]
        public string Category { get; set; }

        [IndexField("description")]
        public string Description { get; set; }

        [IndexField("title")]
        public string Title { get; set; }
    }


    public class NewsPageModel
    {
        public List<NewsPageResultItem> News { get; set; }= new List<NewsPageResultItem>();
        public List<string> Categories { get; set; }=new List<string>();
    }
}