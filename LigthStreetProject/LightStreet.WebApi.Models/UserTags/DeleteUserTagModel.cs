using System.Collections.Generic;

namespace LightStreet.WebApi.Models.UserTags
{
    public class DeleteUserTagModel
    {
        public DeleteUserTagModel(List<int> tagIds, int userId)
        {
            TagIds = tagIds;
            UserId = userId;
        }

        public List<int> TagIds { get; }
        public int UserId { get; }
    }
}