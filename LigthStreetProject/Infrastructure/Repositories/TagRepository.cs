using AutoMapper;
using Domain.Models;
using Infrastructure.Models;
using Infrastructure.Models.ManyToMany;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TagRepository : Repository<TagEntity, int>, ITagRepository
    {
        private readonly IMapper _mapper;
        public TagRepository(DbContext databaseContext,
            IMapper mapper)
            :base(databaseContext)
        {
            _mapper = mapper;
        }
        public async Task<Tag> AddUsersTagAsync(string tagName, List<int> userIds)
        {
            tagName = tagName.ToLower();
            var tagEntity = await databaseContext.Set<TagEntity>().Where(x => x.Name == tagName).SingleOrDefaultAsync();
            if (tagEntity == null)
            {
                tagEntity = new TagEntity(); ;
                tagEntity.Name = tagName;
                tagEntity.UserTags = new List<UserTagEntity>();
            }
            tagEntity.ModifiedAt = DateTimeOffset.Now;

            foreach (var userId in userIds)
            {
                if (!tagEntity.UserTags.Exists(s => s.UserId == userId))
                {
                    tagEntity.UserTags.Add(new UserTagEntity() { UserId = userId });
                }
            }
            if (tagEntity.Id == 0) { 
                await databaseContext.Set<TagEntity>().AddAsync(tagEntity);
            }

            await databaseContext.SaveChangesAsync();

            return _mapper.Map<Tag>(tagEntity);
        }

        public async Task DeleteUserTagsAsync(int userId, List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var userTagToRemove = await databaseContext.Set<UserTagEntity>().FirstOrDefaultAsync(x => x.UserId == userId
                                                                                         && x.TagId == tagId);
                if (userTagToRemove != null)
                {
                    databaseContext.Remove(userTagToRemove);
                }
            }
            await databaseContext.SaveChangesAsync();
        }
    }
}
