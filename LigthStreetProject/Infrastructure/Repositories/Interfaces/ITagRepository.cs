using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ITagRepository: IRepository<TagEntity, int>
    {
        Task DeleteUserTagsAsync(int userId, List<int> tagIds);
        Task<Tag> AddUsersTagAsync(string tagName, List<int> userIds);
    }
}
