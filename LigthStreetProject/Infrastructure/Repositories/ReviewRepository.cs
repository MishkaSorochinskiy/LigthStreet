using AutoMapper;
using Domain.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : Repository<ReviewEntity, int>, IReviewRepository
    {
        private readonly IMapper _mapper;

        public ReviewRepository(DbContext databaseContext,
            IMapper mapper)
            : base(databaseContext)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<Review>> GetPagesAsync(int count, int page, string searchQuery)
        {
            var query = databaseContext.Set<ReviewEntity>().AsQueryable();
            if (searchQuery != null)
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchQuery.ToUpper())
                || x.Subject.ToUpper().Contains(searchQuery.ToUpper()) || x.ApplyOn.UserName.ToUpper().Contains(searchQuery.ToUpper()));
            }
            query = query.Skip(page * count).Take(count);
            return _mapper.Map<List<Review>>(await query.ToListAsync());
        }
    }
}
