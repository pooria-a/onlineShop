namespace API.Errors
{
    public class ApiException : ApiResponse

    {
        private string Details;

  

        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
    }
}