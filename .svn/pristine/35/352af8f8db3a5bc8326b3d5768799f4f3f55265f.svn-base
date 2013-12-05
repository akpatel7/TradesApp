using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesAppWebControls
{
    public class PageIDGeneration
    {
        public string PageID = string.Empty;
        public string JQueryPageID = string.Empty;

        public List<PageCollection> pageCollection;

        /// <summary>
        /// Page ID Generation
        /// </summary>
        public PageIDGeneration()
        {
            PageID = Guid.NewGuid().ToString();
            JQueryPageID = "#" + PageID;
            pageCollection = new List<PageCollection>();
        }

        /// <summary>
        /// Generate ID
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public string GenerateID(string objectName)
        {
            PageCollection pageItem = new PageCollection();

            pageItem.PageID = PageID;
            pageItem.JQueryPageID = "#" + PageID;
            pageItem.ID = objectName;
            pageItem.FullID = PageID + objectName;
            pageItem.JQueryID = "#" + PageID + objectName;
            pageCollection.Add(pageItem);

            return pageItem.FullID;
        }

        /// <summary>
        /// Generate JQuery
        /// </summary>
        /// <returns></returns>
        public List<PageCollection> GenerateJQuery()
        {
            return pageCollection;
        }

        public string RenderJavascriptVariables(string prefix)
        {
            StringBuilder js = new StringBuilder();

            js.Append("<script language=\"javascript\" type=\"text/javascript\">");
            js.Append(" var " + prefix + "PageID" + " = \"" + JQueryPageID + "\";");

            foreach (PageCollection item in pageCollection)
            {
                js.Append(" var " + prefix + item.ID + " = \"" + item.JQueryID + "\";");
            }

            js.Append("</script>");
            return js.ToString();

        }

    }

    public class PageCollection
    {
        public string JQueryPageID { get; set; }
        public string PageID { get; set; }
        public string ID { get; set; }
        public string FullID { get; set; }
        public string JQueryID { get; set; }
    }
}
