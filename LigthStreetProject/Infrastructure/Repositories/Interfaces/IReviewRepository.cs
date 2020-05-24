using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IReviewRepository: IRepository<ReviewEntity, int>
    {
        Task<IEnumerable<Review>> GetPagesAsync(int count, int page, string searchQuery);
    }
}
