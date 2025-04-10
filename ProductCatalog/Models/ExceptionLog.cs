namespace ProductCatalog.Models
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string Path { get; set; }
    }
}
