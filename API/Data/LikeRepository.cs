using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikeRepository(AppDbContext context) : ILikesRepository
{
    public void AddLike(MemberLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await context.Likes
                            .Where(x => x.SourceMemberId == memberId)
                            .Select(x => x.TargetMemberId)
                            .ToListAsync();
    }

    public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        return await context.Likes.FindAsync(sourceMemberId, targetMemberId);
    }

    public async Task<PaginatedResult<Member>> GetMemberLikes(string predicate, string memberId, LikesParams likesParams)
    {
        var query = context.Likes.AsQueryable();
        IQueryable<Member> result;

        switch (predicate)
        {
            case "liked":
                result = query.Where(x => x.SourceMemberId == memberId).Select(x => x.TargetMember);
                break;
            case "liked by":
                result = query.Where(x => x.TargetMemberId == memberId).Select(x => x.SourceMember);
                break;
            default: // mutual
                var likeIds = await GetCurrentMemberLikeIds(memberId);
                result = query.Where(x => x.TargetMemberId == memberId && likeIds.Contains(x.SourceMemberId)).Select(x => x.SourceMember);
                break;
        }

        return await PaginationHelper.CreateAsync(result, likesParams.pageNumber, likesParams.PageSize);
    }

    public async Task<bool> SaveAllChanges()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
