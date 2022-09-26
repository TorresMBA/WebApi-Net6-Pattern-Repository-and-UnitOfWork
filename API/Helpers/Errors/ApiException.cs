namespace API.Helpers.Errors
{
    public class ApiException : ApiResponse
    {
        public string Details { get; set; }

        public ApiException(int statudCode, string message = null, string details = null) : base(statudCode, message)
        {
            Details = details;
        }
    }
}
