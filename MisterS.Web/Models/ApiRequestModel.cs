using MisterS.Web.Enums;

namespace MisterS.Web.Models
{
    public class ApiRequestModel
    {
        public APIType ApiType { get; set; } = APIType.GET;
        public string Url { get; set; } = string.Empty;
        public object Data { get; set; } = new object();
        public string AccessToken { get; set; } = string.Empty;
    }
}
