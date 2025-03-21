﻿using Microsoft.EntityFrameworkCore;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Services.Tags.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Domain.Entities.Tags;

namespace PersonalShop.DataAccessLayer.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository, ITagQueryRepository
{
    public TagRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Tag?> GetTagDetailsWithoutUserAsync(int tagId, bool track = true)
    {
        var data = await _dbSet
            .FirstOrDefaultAsync(x => x.Id == tagId);

        if (!track && data is not null)
        {
            _dbSet.Entry(data).State = EntityState.Detached;
        }

        return data;
    }

    public async Task<PagedResult<SingleTagDto>> GetAllTagsWithUserAsync(PagedResultOffset resultOffset)
    {
        var totalRecord = await _dbSet.CountAsync();

        var data = await _dbSet
            .Include(x => x.User)
            .AsNoTracking()
            .Select(x => new SingleTagDto
            {
                Id = x.Id,
                Name = x.Name,
                User = new TagUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                },
            })
            .ToListAsync();

        return PagedResult<SingleTagDto>.CreateNew(data, resultOffset, totalRecord);
    }
    public async Task<List<SingleTagDto>> GetAllTagsWithUserAsync()
    {
        return await _dbSet
            .Include(x => x.User)
            .AsNoTracking()
            .Select(x => new SingleTagDto
            {
                Id = x.Id,
                Name = x.Name,
                User = new TagUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                },
            })
            .ToListAsync();
    }
    public async Task<SingleTagDto?> FindTagWithNameAsync(string name)
    {
        return await _dbSet
            .Include(x => x.User)
            .AsNoTracking()
            .Where(x => x.Name.Contains(name) || x.Name == name)
            .Select(x => new SingleTagDto
            {
                Id = x.Id,
                Name = x.Name,
                User = new TagUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                },

            }).FirstOrDefaultAsync();
    }
}
