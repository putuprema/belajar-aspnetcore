using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;
using AspNetAuth.WebApp.Constants;
using AspNetAuth.WebApp.Extensions;
using AspNetAuth.WebApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AspNetAuth.WebApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public BlogService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient(Defaults.DefaultHttpClientName);
        }
        
        public async Task CreateBlogPost(CreateBlogPostRequest request)
        {
            var json = JsonConvert.SerializeObject(request);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Blogs");
            requestMessage.Content = new StringContent(json, Encoding.UTF8, Defaults.MediaTypes.ApplicationJson);
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

        public async Task DeleteBlog(string blogId)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"api/Blogs/{blogId}");
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

        public async Task<List<BlogPostDto>> GetAllBlogPosts()
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/Blogs");
            await requestMessage.PutCurrentUserAccessToken(_httpContextAccessor.HttpContext);
            
            var response = await _httpClient.SendAsync(requestMessage);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<BlogPostDto>>(responseBody);
            }
            
            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<BaseResponse>(responseBody);
                throw new HttpRequestException(responseDto.Message);
            }
            
            throw new HttpRequestException("An error occured when processing your request. Please try again in a few minutes");
        }

        public async Task<List<BlogPostDto>> GetCurrentUserBlogPosts()
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/Blogs/My");
            await requestMessage.PutCurrentUserAccessToken(_httpContextAccessor.HttpContext);
            
            var response = await _httpClient.SendAsync(requestMessage);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<BlogPostDto>>(responseBody);
            }

            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<BaseResponse>(responseBody);
                throw new HttpRequestException(responseDto.Message);
            }

            throw new HttpRequestException("An error occured when processing your request. Please try again in a few minutes");
        }
    }
}