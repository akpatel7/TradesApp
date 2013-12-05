using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradesAppWebControls;

namespace TradesWebApplication.Helpers
{
    public static class BCATradesDataGrid
    {
        /// <summary>
        /// Generate Paged Data Grid
        /// </summary>
        /// <param name="html"></param>
        /// <param name="dataGrid"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderNorthwindDataGrid(this HtmlHelper html, TradesAppWebControls.TradesAppDataGrid dataGrid)
        {

            string control = dataGrid.CreateControl();

            return MvcHtmlString.Create(control);

        }
    }
}