using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Extensions.Alerts
{
    public class AlertDecoratorResult : IActionResult
    {
        public IActionResult Result { get; }
        public string Type { get; }
        public string Title { get; }
        public string Body { get; }

        public AlertDecoratorResult(IActionResult result, string type, string title, string body)
        {
            Result = result;
            Type = type;
            Title = title;
            Body = body;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            //NOTE: Be sure you add a using statement for Microsoft.Extensions.DependencyInjection, otherwise
            //      this overload of GetService won't be available!
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);

            tempData["_alert.type"] = Type;
            tempData["_alert.title"] = Title;
            tempData["_alert.body"] = Body;
            await Result.ExecuteResultAsync(context);
        }
    }
}
