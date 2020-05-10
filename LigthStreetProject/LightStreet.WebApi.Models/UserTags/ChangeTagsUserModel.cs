using System.Collections.Generic;

namespace LightStreet.WebApi.Models.UserTags
{
    public class ChangeTagsUserModel
    {
        public ChangeTagsUserModel(List<UserTagBindingModel> addedTagList,
            List<DeleteUserTagModel> deletedTagList)
        {
            AddedTagList = addedTagList;
            DeletedTagList = deletedTagList;
        }

        public List<UserTagBindingModel> AddedTagList { get; }
        public List<DeleteUserTagModel> DeletedTagList { get; }
    }
}