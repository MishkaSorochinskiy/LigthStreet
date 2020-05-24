using LightStreet.WebApi.Models.User;
using System.Collections.Generic;
using LightStreet.WebApi.Models.Point;

namespace LightStreet.WebApi.Models.Review
{
    public class PointsWithUsersModel
    {
        public IEnumerable<ViewUserModel> Users { get; set; }
        public IEnumerable<ViewPointModel> Points { get; set; }
    }
}
