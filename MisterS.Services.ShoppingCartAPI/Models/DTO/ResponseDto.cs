namespace MisterS.Services.ShoppingCartAPI.Models.DTO
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }

        public object? Result { get; set; }

        public string DisplayMessage { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = new List<string>();
    }
}
