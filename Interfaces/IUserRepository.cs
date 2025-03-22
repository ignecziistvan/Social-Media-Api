using API.Dtos.Response;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(int id);
    Task<PaginatedList<UserDto>> GetAllUsers(PaginationParams paginationParams);
    void UpdateUser(User user);
    void DeleteUser(User user);
    Task<bool> Complete();
}
