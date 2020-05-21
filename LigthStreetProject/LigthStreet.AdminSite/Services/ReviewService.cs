using LightStreet.WebApi.Models.Review;
using LigthStreet.AdminSite.Models;
using LigthStreet.AdminSite.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LigthStreet.AdminSite.Services
{
    public class ReviewService: IReviewService
    {
        public async Task<ActionResult> AddReviewAsync(TaskForUser task)
        {
            using (var httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(task);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:5001/api/review", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    return new ActionResult() { Successfull = true };
                }
                return new ActionResult() { Successfull = false, Error = response.ReasonPhrase };
            }
        }

        public async Task<PointsWithUsersModel> GetAllPointsAndUsers()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:5001/api/review");
                if (response.IsSuccessStatusCode)
                {
                    var responseModel = JsonConvert.DeserializeObject<PointsWithUsersModel>(await response.Content.ReadAsStringAsync());
                    return responseModel;
                }
                throw new Exception("Problems with users and points");
            }
        }
    }
}
