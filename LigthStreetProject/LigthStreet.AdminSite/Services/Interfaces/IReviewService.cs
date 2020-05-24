using LightStreet.WebApi.Models.Review;
using LigthStreet.AdminSite.Models;
using System.Threading.Tasks;

namespace LigthStreet.AdminSite.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PointsWithUsersModel> GetAllPointsAndUsers();
        Task<ActionResult> AddReviewAsync(TaskForUser task);
    }
}
