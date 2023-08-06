using AspNetCoreWebApp.UsersApp.Models;
using System.Text.Json;

namespace AspNetCoreWebApp.UsersApp.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7222/api/");
        }

        public async Task<IEnumerable<User>?> ReadAllAsync()
        {
            var response = await _httpClient.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync<IEnumerable<User>>(responseStream);
                return users;
            }
            return null;
        }

        public async Task<User?> ReadAsync(string id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync<User>(responseStream);
                return users;
            }
            return null;
        }

        public async Task<User?> CreateAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("users", user);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        public async Task<User?> UpdateAsync(int id , User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"users/{id}", user);
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

    }
}
