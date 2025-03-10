using API.Dtos.Response;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository(DataContext context, IMapper mapper) : IMessageRepository
{
    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessageById(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    public async Task<List<MessageDto>> GetMessagesForUser(string username, string? container)
    {
        IQueryable<Message>? query = context.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = container switch
        {
            "Inbox" => query.Where(m => 
                    m.ReceiverUsername == username
                    && !m.ReceiverDeleted
                ),
            "Outbox" => query.Where(m => 
                    m.SenderUsername == username
                    && !m.SenderDeleted
                ),
            _ => query.Where(m => 
                    m.ReceiverUsername == username && m.DateRead == null
                    && !m.ReceiverDeleted
                )
        };

        IQueryable<MessageDto>? messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return await messages.ToListAsync();
    }

    public async Task<List<MessageDto>> GetMessageThread(string currentUsername, string receiverUsername)
    {
        IQueryable<Message>? query  = context.Messages
            .Where(m => 
                m.ReceiverUsername == currentUsername && !m.ReceiverDeleted && m.SenderUsername == receiverUsername ||
                m.SenderUsername == currentUsername && !m.SenderDeleted && m.ReceiverUsername == receiverUsername
            )
            .OrderBy(m => m.MessageSent)
            .AsQueryable();

        List<Message>? unreadMessages = query
            .Where(m => m.DateRead == null && m.ReceiverUsername == currentUsername)
            .ToList();

        if (unreadMessages.Count != 0)
        {
            unreadMessages.ForEach(m => m.DateRead = DateTime.UtcNow);
            await context.SaveChangesAsync();
        }

        return await query.ProjectTo<MessageDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
