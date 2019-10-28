using System.Web.Mvc;

namespace SportsLab.Services.Extensions
{
    public static class ShowHideHelper
    {
        /// <summary>
        /// Set Display Attribute based on Visibility
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public static MvcHtmlString DisplayShowHide(this HtmlHelper htmlHelper, bool visible)
        {
            var text = visible ? "" : "none";

            return new MvcHtmlString(text);
        }
    }
}