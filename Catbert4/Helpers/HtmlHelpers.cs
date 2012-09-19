using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Catbert4.Helpers
{
    /// <summary>
    /// Used to keep the html script includes in one place in case they change
    /// </summary>
    public static class HtmlHelpers
    {
        public static HtmlString IncludeJqueryTemplate()
        {
            var jqueryTemplateUrl = WebConfigurationManager.AppSettings["jqueryTemplateUrl"];

            var template = string.Format(@"<script type='text/javascript' src='{0}'></script>", jqueryTemplateUrl);
            //var t = new TagBuilder("script");
            //t.Attributes.Add("type", "text/javascript");
            //t.Attributes.Add("src", jqueryTemplateUrl);

            return new HtmlString(template);
        }
    }
}