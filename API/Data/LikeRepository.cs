using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikeRepository(AppDbContext context) : ILikesRepository
{
    public void AddLike(MemberLike like)
    {
        context.Entry(like).State = EntityState.Added;
    }

    public void DeleteLike(MemberLike like)
    {
        context.Entry(like).State = EntityState.Deleted;
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await context.Likes
                            .Where(x => x.SourceMemberId == memberId)
                            .SelectMany(x => x.TargetMemberId)
                            .ToListAsync();
    }

    public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        return await context.Likes.SingleOrDefaultAsync(x => x.SourceMemberId == sourceMemberId && x.TargetMemberId == targetMemberId);
    }

    public Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllChanges()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
