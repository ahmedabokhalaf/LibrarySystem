using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ITI.LibSys.Presentation.Filteration
{
    public class ExceptionFilteration:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string msg = $"Date-Time: {DateTime.Now}\n Description: {context.Exception.Message}";
            File.WriteAllText("Audit.txt", msg);
            context.Result = new ViewResult()
            {
                ViewName = "Error"
            };
            base.OnException(context);
        }
    }
}
