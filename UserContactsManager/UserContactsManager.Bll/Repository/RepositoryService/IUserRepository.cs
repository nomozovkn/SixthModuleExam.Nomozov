using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContactsManager.Dal.Entities;

namespace UserContactsManager.Bll.Repository.RepositoryService;

public interface IUserRepository
{
    Task<long> InsertUserAync(User user);
    Task<User> SelectUserByIdAync(long id);
    Task<User> SelectUserByUserNameAync(string userName);
    Task UpdateUserRoleAsync(long userId, string userRole);
    Task DeleteUserByIdAsync(long userId);
    //Task<bool> CheckUserById(long userId);
    //Task<bool> CheckUsernameExists(string username);
    //Task<bool> CheckEmailExists(string email);
    //Task<bool> CheckPhoneNumberExists(string phoneNum);
}