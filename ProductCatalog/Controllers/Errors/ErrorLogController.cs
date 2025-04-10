using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Data;

namespace ProductCatalog.Controllers.Errors
{
    public class ErrorLogController : Controller
    {
        private readonly ApplicationDbContext context;

        public ErrorLogController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var Errors=context.ExceptionLogs.ToList();
            return View(Errors);
        }
    }
}
