using System.Collections.Generic;

namespace LightStreet.WebApi.Models.UserTags
{
    public class UserTagBindingModel
    {
        public UserTagBindingModel(string name, List<int> userIds)
        {
            Name = name;
            UserIds = userIds;
        }

        public string Name { get; }
        public List<int> UserIds { get; }
    }
}