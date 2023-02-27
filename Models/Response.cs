namespace EmploployeedB.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string errorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; }
    }
}
