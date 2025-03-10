using API.Dtos.Response;
using API.Entities;
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

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        return await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<UserDto?> GetUser(string username)
    {
        return await context.Users
            .Where(user => user.UserName == username)
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByIdAsNonDto(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUserNameAsNonDto(string username)
    {
        return await context.Users
            .Where(user => user.UserName == username)
            .SingleOrDefaultAsync();
    }
}
