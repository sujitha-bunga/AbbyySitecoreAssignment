using AbbyySitecoreAssignment.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace AbbyySitecoreAssignment.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        /// <summary>
        /// News Page action
        /// </summary>
        /// <param name="category"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult Index(string category, string date)
        {
            var model = new NewsPageModel();
            // Get the current rendering
            var rendering = RenderingContext.Current.Rendering;

            // Get the datasource item
            var datasourceItemId = rendering.DataSource;

            model.News = SearchNews(category, date, datasourceItemId);
            model.Categories = GetCategories();

            return View(model);
        }

        /// <summary>
        /// Getting the search results
        /// </summary>
        /// <param name="category"></param>
        /// <param name="datePublished"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<NewsPageResultItem> SearchNews(string category, string datePublished, string parentId)
        {

            // Get the search index
            var index = ContentSearchManager.GetIndex(Constants.index);

            using (var context = index.CreateSearchContext())
            {
                var results = context.GetQueryable<NewsPageResultItem>().ToList();
                //var predicate = PredicateBuilder.True<NewsPageResultItem>();//tried to implement it but faced issue with date filter

                results = results.Where(x => x.TemplateId == new ID(Constants.newstemplateid)).ToList();


                // Add category filter
                if (!string.IsNullOrEmpty(category) && category != "all")
                {
                    results = results.Where(item => item.Category.ToString() == category).ToList();
                }

                // Add date filter
                if (!string.IsNullOrEmpty(datePublished))
                {
                    string filterdate = DateTime.Parse(datePublished).ToShortDateString().ToString();
                    results = results.Where(item => item.Updated.ToShortDateString() == filterdate).ToList();
                }

                if (string.IsNullOrEmpty(parentId))
                {
                    var templateIdGuid = new Guid(parentId);
                    results = results.Where(item => item.Parent.Guid == templateIdGuid).ToList();
                }

                return results;

            }
        }

        private Item GetDatasourceItem(Rendering rendering)
        {
            // Get the datasource ID from the rendering
            var datasourceId = rendering.DataSource;

            // If the datasource ID is not empty, get the item
            if (!string.IsNullOrEmpty(datasourceId))
            {
                var datasourceItem = Sitecore.Context.Database.GetItem(datasourceId);
                return datasourceItem;
            }


            return null;
        }

        private List<string> GetCategories()
        {
            var categories = new List<string>();

            var categoryFolder = Sitecore.Context.Database.GetItem(new ID(Constants.categoryfolderid));
            if (categoryFolder != null)
            {
                categories.AddRange(categoryFolder.Children.Where(x => x.Fields["Name"] != null).Select(x => x.Fields["Name"].ToString()).ToList());
            }

            return categories;
        }



    }
}
