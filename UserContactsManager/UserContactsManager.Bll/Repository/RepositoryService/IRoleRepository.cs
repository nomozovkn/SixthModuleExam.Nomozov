using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContactsManager.Dal.Entities;

namespace UserContactsManager.Bll.Repository.RepositoryService;

public interface IRoleRepository
{
    Task<ICollection<User>> GetAllUsersByRoleAsync(string role);
    Task<List<UserRole>> GetAllRolesAsync();
    Task<long> GetRoleIdAsync(string role);
}
