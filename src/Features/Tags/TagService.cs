﻿using EasyCaching.Core;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Domain.Tags;
using PersonalShop.Features.Tags.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Resources.Services.TagService;

namespace PersonalShop.Features.Tags;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly ITagQueryRepository _tagQueryRepository;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IUnitOfWork _unitOfWork;

    public TagService(ITagRepository tagRepository, ITagQueryRepository tagQueryRepository, IEasyCachingProvider cachingProvider, IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _tagQueryRepository = tagQueryRepository;
        _cachingProvider = cachingProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<string>> CreateTagAsync(string userId, CreateTagDto createTagDto)
    {
        var newTag = new Tag(userId, createTagDto.Name);

        await _tagRepository.AddAsync(newTag);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Tag);
            return ServiceResult<string>.Success(TagServiceSuccess.SuccessfulCreateTag);
        }

        return ServiceResult<string>.Failed(TagServiceErrors.CreateTagProblem);
    }
    public async Task<ServiceResult<string>> UpdateTagAndValidateOwnerAsync(string userId, int tagId, UpdateTagDto updateTagDto)
    {
        var tag = await _tagRepository.GetTagDetailsWithoutUserAsync(tagId);
        if (tag is null)
        {
            return ServiceResult<string>.Failed(TagServiceErrors.TagNotFound);
        }

        if (!tag.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(TagServiceErrors.TagOwnerMatchProblem);
        }

        tag.ChangeName(updateTagDto.Name);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Tag);
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);

            return ServiceResult<string>.Success(TagServiceSuccess.SuccessfulUpdateTag);
        }

        return ServiceResult<string>.Failed(TagServiceErrors.UpdateTagProblem);
    }
    public async Task<ServiceResult<string>> DeleteTagAndValidateOwnerAsync(string userId, int tagId)
    {
        var tag = await _tagRepository.GetTagDetailsWithoutUserAsync(tagId);
        if (tag is null)
        {
            return ServiceResult<string>.Failed(TagServiceErrors.TagNotFound);
        }

        if (!tag.UserId.Equals(userId))
        {
            return ServiceResult<string>.Failed(TagServiceErrors.TagOwnerMatchProblem);
        }

        _tagRepository.Delete(tag);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Tag);
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Product);

            return ServiceResult<string>.Success(TagServiceSuccess.SuccessfulDeleteTag);
        }

        return ServiceResult<string>.Failed(TagServiceErrors.DeleteTagProblem);
    }
    public async Task<ServiceResult<List<SingleTagDto>>> GetAllTagsWithUserAsync()
    {
        var cache = await _cachingProvider.GetAsync<List<SingleTagDto>>(CacheKeysContract.Tag);

        if (cache.HasValue)
        {
            return ServiceResult<List<SingleTagDto>>.Success(cache.Value);
        }

        var tags = await _tagQueryRepository.GetAllTagsWithUserAsync();

        await _cachingProvider.TrySetAsync(CacheKeysContract.Tag, tags, TimeSpan.FromHours(1));

        return ServiceResult<List<SingleTagDto>>.Success(tags);
    }
    public async Task<ServiceResult<List<SingleTagDto>>> GetAllTagsWithUserAndValidateOwnerAsync(string userId)
    {
        var cache = await _cachingProvider.GetAsync<List<SingleTagDto>>(CacheKeysContract.Tag);

        if (cache.HasValue)
        {
            return ServiceResult<List<SingleTagDto>>.Success(cache.Value);
        }

        var tags = await _tagQueryRepository.GetAllTagsWithUserAsync();

        foreach (var tag in tags)
        {
            if (tag.User.UserId.Equals(userId))
            {
                tag.User.IsOwner = true;
            }
        }

        await _cachingProvider.TrySetAsync(CacheKeysContract.Tag, tags, TimeSpan.FromHours(1));

        return ServiceResult<List<SingleTagDto>>.Success(tags);
    }
}