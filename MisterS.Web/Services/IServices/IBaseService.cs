using MisterS.Web.Models;

namespace MisterS.Web.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        public ResponseDto responseModel { get; set; }

        Task<T> SendAsync<T>(ApiRequestModel apiRequest);
    }
}
