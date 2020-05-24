using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models.Common
{
    public interface IEntity
    {
        string GetAction(EntityState state);
    }
}
