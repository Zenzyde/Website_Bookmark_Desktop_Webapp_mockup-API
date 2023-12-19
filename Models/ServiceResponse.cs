namespace Website_Bookmark_Desktop_App_API.Models
{
    // Generic response class is useful for providing additional information to the frontend, like success state and any additional message
    public class ServiceResponse<T>
    {
        public T? Data { get; set; } // ?-mark makes variable nullable
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
