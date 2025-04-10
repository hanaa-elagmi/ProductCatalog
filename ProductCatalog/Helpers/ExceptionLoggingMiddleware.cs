using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Helpers
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            try
            {
                await _next(context); // مرر الطلب
            }
            catch (Exception ex)
            {
                // سجل الاستثناء في قاعدة البيانات
                var log = new ExceptionLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Date = DateTime.Now,
                    Source = ex.Source,
                    Path = context.Request.Path
                };

                dbContext.ExceptionLogs.Add(log);
                await dbContext.SaveChangesAsync();

                // ممكن تعيدي توجيه المستخدم أو ترمي الاستثناء حسب الحاجة
                throw; // عشان يوصلك صفحة الخطأ أو يشتغل الـ DeveloperExceptionPage
            }
        }
    }

}
