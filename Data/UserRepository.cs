using API.Dtos.Response;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<PaginatedList<UserDto>> GetAllUsers(PaginationParams paginationParams)
    {
        var users = context.Users
            .Include(u => u.Photos)
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PaginatedList<UserDto>.CreateAsync(
            users, paginationParams.PageNumber, paginationParams.PageSize
        );
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await context.Users
            .Where(u => u.NormalizedUserName == username.ToUpper())
            .Include(u => u.Photos)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users
            .Where(u => u.NormalizedEmail == email.ToUpper())
            .Include(u => u.Photos)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await context.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public void UpdateUser(User user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public void DeleteUser(User user)
    {
        context.Users.Remove(user);
    }
}
