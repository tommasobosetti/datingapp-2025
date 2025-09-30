using System;
using System.Text.RegularExpressions;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    public void AddGroup(Entities.Group group)
    {
        throw new NotImplementedException();
    }

    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public Task<Connection?> GetConnection(string connectionId)
    {
        throw new NotImplementedException();
    }

    public Task<Entities.Group?> GetGroupForConnection(string connectionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Message?> GetMessage(string messageId)
    {
        return await context.Messages.FindAsync(messageId);
    }

    public Task<Entities.Group?> GetMessageGroup(string groupName)
    {
        throw new NotImplementedException();
    }

    // public async Task<Group?> GetMessageGroup(string groupName)
    // {
    //     return await context.Groups
    //                         .Include(x => x.Connections)
    //                         .FirstOrdDefaultAsync(x => x.Name == groupName);
    // }

    public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams)
    {
        var query = context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

        query = messageParams.Container switch
        {
            "Outbox" => query.Where(x => x.SenderId == messageParams.MemberId && x.SenderDeleted == false),
            _ => query.Where(x => x.RecipientId == messageParams.MemberId && x.RecipientDeleted == false)
        };

        var messageQuery = query.Select(MessageExtensions.ToDtoProjection());

        return await PaginationHelper.CreateAsync(messageQuery, messageParams.pageNumber, messageParams.PageSize);
    }

    public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
    {
        await context.Messages.Where(x => x.RecipientId == currentMemberId && x.SenderId == recipientId && x.DateRead == null)
                              .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.DateRead, DateTime.UtcNow));

        return await context.Messages.Where(x => (x.RecipientId == currentMemberId && x.RecipientDeleted == false && x.SenderId == recipientId) || (x.SenderId == currentMemberId && x.SenderDeleted == false && x.RecipientId == recipientId))
                                     .OrderBy(x => x.MessageSent)
                                     .Select(MessageExtensions.ToDtoProjection()).ToListAsync();
    }

    public Task RemoveConnection(string connectionId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
