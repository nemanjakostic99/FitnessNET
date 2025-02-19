namespace FitnessNET.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }  
        public string Message { get; set; }  
        public DateTime Timestamp { get; set; }  

        public ErrorResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            Timestamp = DateTime.UtcNow; 
        }
    }
}
