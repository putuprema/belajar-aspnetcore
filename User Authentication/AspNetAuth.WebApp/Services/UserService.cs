using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;
using AspNetAuth.WebApp.Constants;
using AspNetAuth.WebApp.Extensions;
using AspNetAuth.WebApp.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AspNetAuth.WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient(Defaults.DefaultHttpClientName);
        }
        
        public async Task<LoginResponse> Login(LoginUserRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, Defaults.MediaTypes.ApplicationJson);

            var response = await _httpClient.PostAsync("/api/User/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LoginResponse>(responseBody);
            }

            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<BaseResponse>(responseBody);
                throw new HttpRequestException(responseDto.Message);
            }

            throw new HttpRequestException(
                "An error occured when processing your request. Please try again in a few minutes");
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/User");
            await requestMessage.PutCurrentUserAccessToken(_httpContextAccessor.HttpContext);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserDto>>(responseBody);
            }

            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<BaseResponse>(responseBody);
                throw new HttpRequestException(responseDto.Message);
            }
            
            throw new HttpRequestException(
                "An error occured when processing your request. Please try again in a few minutes");
        }

        public async Task<UserDto> GetCurrentUserProfile()
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/User/Me");
            await requestMessage.PutCurrentUserAccessToken(_httpContextAccessor.HttpContext);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDto>(responseBody);
            }
            
            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<BaseResponse>(responseBody);
                throw new HttpRequestException(responseDto.Message);
            }
            
            throw new HttpRequestException("An error occured when processing your request. Please try again in a few minutes");
        }

        public async Task ChangeUserActiveStatus(string userId, bool active)
        {
            using var requestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"/api/User/{userId}/Status?active={active}");

            await requestMessage.PutCurrentUserAccessToken(_httpContextAccessor.HttpContext);

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    throw new HttpRequestException(
                        "An error occured when processing your request. Please try again in a few minutes");
                
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<BaseResponse>(responseBody);
                throw new HttpRequestException(responseDto.Message);
            }
        }
    }
}