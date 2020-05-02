using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using Infrastructure.Models.ManyToMany;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PendingUserRepository: Repository<PendingUserEntity, int>, IPendingUserRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<PendingUserEntity> _userManager;
        private readonly IUserRepository _userRepository;

        public PendingUserRepository(DbContext databaseContext,
            IMapper mapper,
            UserManager<PendingUserEntity> userManager,
            IUserRepository userRepository)
            :base(databaseContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<User> ApproveUserAndSetTagsAsync(PendingUser pendingUser, UserStatusType userStatusType, List<string> tags, int currentUserId)
        {
            var userToRegistration = _mapper.Map<User>(pendingUser);
            userToRegistration.Status = userStatusType;
            userToRegistration.ModifiedById = currentUserId;
            var user = await _userRepository.RegisterByPasswordHashAsync(userToRegistration, pendingUser.PasswordHash);
            await DeleteAsync(user);
            if (tags != null)
            {
                foreach (var tagName in tags)
                {
                    var tagEntity = await databaseContext.Set<TagEntity>().FirstOrDefaultAsync(x => x.Name == tagName) ?? new TagEntity
                    {
                        Name = tagName.ToUpper(),
                        UserTags = new List<UserTagEntity>(),
                        ModifiedAt = DateTimeOffset.Now
                    };

                    if (!tagEntity.UserTags.Exists(s => s.UserId == user.Id))
                    {
                        tagEntity.UserTags.Add(new UserTagEntity() { UserId = user.Id });
                    }
                    if (tagEntity.Id == 0) { databaseContext.Set<TagEntity>().Add(tagEntity); }

                    await databaseContext.SaveChangesAsync();
                }
            }
            return user;
        }

        private async Task DeleteAsync(User user)
        {
            var userForDelete = await databaseContext.Set<PendingUser>().Where(x=>x.Id == user.Id).SingleOrDefaultAsync();
            databaseContext.Set<PendingUser>().Remove(userForDelete);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await databaseContext.Set<PendingUserEntity>().Where(x => x.NormalizedEmail == email.ToUpper()).FirstOrDefaultAsync() != null;
        }

        public async Task<IEnumerable<PendingUser>> GetPagesAsync(int count, int page, string searchQuery)
        {
            var query = databaseContext.Set<PendingUserEntity>().AsQueryable();
            if (searchQuery != null)
            {
                query = query.Where(x => x.UserName.ToUpper().Contains(searchQuery.ToUpper())
                || x.LastName.ToUpper().Contains(searchQuery.ToUpper()) || x.FirstName.ToUpper().Contains(searchQuery.ToUpper()));
            }
            query = query.Skip(page * count).Take(count);
            return _mapper.Map<List<PendingUser>>(await query.ToListAsync());

        }

        public async Task<PendingUser> RegisterAsync(PendingUser pendingUser, string password)
        {
            var entityUser = _mapper.Map<PendingUserEntity>(pendingUser);
            var result = await _userManager.CreateAsync(entityUser, password);
            return _mapper.Map<PendingUser>(entityUser);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await databaseContext.Set<PendingUserEntity>().Where(x => x.NormalizedUserName == username.ToUpper()).FirstOrDefaultAsync() != null;
        }
    }
}
