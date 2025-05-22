using Microsoft.EntityFrameworkCore;
using UserContacts.Core.Errors;
using UserContactsManager.Bll.Repository.RepositoryService;
using UserContactsManager.Dal;

namespace UserContacts.Bll.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly MainContext _context;
    public UserService(IUserRepository userRepository, MainContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }
    public async Task DeleteUserByIdAsync(long userId, string userRole)
    {
        if (userRole == "SuperAdmin")
        {
            await _userRepository.DeleteUserByIdAsync(userId);
        }
        else if (userRole == "Admin")
        {
            var user = await _userRepository.SelectUserByIdAync(userId);
            if (user.Role.Name == "User")
            {
                await _userRepository.DeleteUserByIdAsync(userId);
            }
            else
            {
                throw new NotAllowedException("Admin can not delete Admin or SuperAdmin");
            }
        }
    }

    public async Task UpdateUserRoleAsync(long userId, string userRole) => await _userRepository.UpdateUserRoleAsync(userId, userRole);
}
