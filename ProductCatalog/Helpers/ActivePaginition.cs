using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ProductCatalog.Helpers
{
    [HtmlTargetElement("li", Attributes = "Active-when")]
    public class ActivePagination : TagHelper
    {
        public int ActiveWhen { get; set; } // يجب أن يكون int لضمان مقارنة صحيحة

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContextData == null)
            {
                return;
            }

            int currentPageIndex = 1; // الافتراضي: الصفحة الأولى

            // **1️⃣ قراءة PageIndex من RouteData**
            if (ViewContextData.RouteData.Values.TryGetValue("PageIndex", out var pageIndexObj) &&
                pageIndexObj != null &&
                int.TryParse(pageIndexObj.ToString(), out int pageIndexFromRoute))
            {
                currentPageIndex = pageIndexFromRoute;
            }
            // **2️⃣ إذا لم يتم العثور عليه، نبحث في QueryString**
            else if (ViewContextData.HttpContext.Request.Query.TryGetValue("PageIndex", out var pageIndexQuery) &&
                     int.TryParse(pageIndexQuery, out int pageIndexFromQuery))
            {
                currentPageIndex = pageIndexFromQuery;
            }

            // **3️⃣ مقارنة `ActiveWhen` مع `currentPageIndex`**
            if (currentPageIndex == ActiveWhen)
            {
                if (output.Attributes.ContainsName("class"))
                {
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
        }
    }
}
