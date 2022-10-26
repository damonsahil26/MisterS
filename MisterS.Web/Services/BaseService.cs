using MisterS.Web.Enums;
using MisterS.Web.Models;
using MisterS.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MisterS.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDto();
            this.httpClient = httpClient;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<T> SendAsync<T>(ApiRequestModel apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MisterSApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }

                var apiResponse = new HttpResponseMessage();
                message.Method = GetAPIType(apiRequest.ApiType);

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
              
                return apiResponseDto;
            }
            catch (Exception e)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    IsSuccess = false,
                    Errors = new List<string>
                    {
                        Convert.ToString(e.Message)
                    },
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }

        private HttpMethod GetAPIType(APIType apiType)
        {
            switch (apiType)
            {
                case APIType.GET:
                    return HttpMethod.Get;
                case APIType.POST:
                    return HttpMethod.Post;
                case APIType.PUT:
                    return HttpMethod.Put;
                case APIType.DELETE:
                    return HttpMethod.Delete;
                case APIType.PATCH:
                    return HttpMethod.Patch;
                default:
                    return HttpMethod.Get;
            }
        }
    }
}
