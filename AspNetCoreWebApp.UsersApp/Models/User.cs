using Newtonsoft.Json;

namespace AspNetCoreWebApp.UsersApp.Models
{
    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public override string? ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
