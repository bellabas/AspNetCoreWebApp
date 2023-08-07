using AspNetCoreWebApp.UsersApp.Models;
//using Newtonsoft.Json;

namespace AspNetCoreWebApp.UsersApp.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7222/api/");
            //_httpClient.DefaultRequestHeaders.Accept.Clear();
            //_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<User>?> ReadAllAsync()
        {
            var response = await _httpClient.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                //var responseString = await response.Content.ReadAsStringAsync();
                //var users = JsonConvert.DeserializeObject<IEnumerable<User>>(responseString);
                var users = await response.Content.ReadAsAsync<IEnumerable<User>>();
                return users;
            }
            return null;
        }

        public async Task<User?> ReadAsync(string username)
        {
            var response = await _httpClient.GetAsync($"users/{username}");
            if (response.IsSuccessStatusCode)
            {
                //var responseString = await response.Content.ReadAsStringAsync();
                //var user = JsonConvert.DeserializeObject<User>(responseString);
                var user = await response.Content.ReadAsAsync<User>();
                return user;
            }
            return null;
        }

        public async Task<bool> CreateAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("users", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"users/{user.Username}", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string username)
        {
            var response = await _httpClient.DeleteAsync($"users/{username}");
            return response.IsSuccessStatusCode;
        }

    }
}
