using System;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IMemberRepository? _memberRepository;
    public IMemberRepository MemberRepository => _memberRepository ??= new MemberRepository(context);

    private IMessageRepository? _messageRepository;
    public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(context);

    private ILikesRepository? _likesRepository;
    public ILikesRepository LikesRepository => _likesRepository ??= new LikesRepository(context);

    public async Task<bool> Complete()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occured while saving changes", ex);
        }
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
