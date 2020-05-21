using LigthStreet.AdminSite.Models;
using LigthStreet.AdminSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LigthStreet.AdminSite.Services
{
    public class AuthService : IAuthService
    {

        private const string URL = "https://localhost:5001/";

        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders
                       .Accept
                       .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Post, URL + "api/users/authorize/signup");
                request.Content =
                    new StringContent(JsonConvert.SerializeObject(registerModel),
                        Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {

                    return new ActionResult() { Successfull = false, Error = response.ReasonPhrase };
                }
            }
            return new ActionResult() { Successfull = true };
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            LoginResponse loginResponse;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                requestData.Add(new KeyValuePair<string, string>("password", loginModel.Password));
                requestData.Add(new KeyValuePair<string, string>("username", loginModel.Email));
                requestData.Add(new KeyValuePair<string, string>("client_id", "browser"));
                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                var request = await client.PostAsync($"{URL}connect/token", requestBody);
                if (!request.IsSuccessStatusCode)
                {
                    return new LoginResult() { Successful = request.IsSuccessStatusCode, Error = request.ReasonPhrase };
                }
                var response = await request.Content.ReadAsStringAsync();
                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
            }
            var claims = GetPermissions(loginResponse.access_token);
            return new LoginResult() { Successful = true, Permissions = claims, Response = loginResponse };
        }

        private IEnumerable<Claim> GetPermissions(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var claims = tokenS.Claims.Where(claim => claim.Type == "Permission").ToList();
            return claims;
        }
    }
}
