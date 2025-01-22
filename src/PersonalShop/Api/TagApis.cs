using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Tags.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Extension;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Api;

public static class TagApis
{
    public static void RegisterTagApis(this WebApplication app)
    {
        app.MapGet("api/tags", [AllowAnonymous] async (PagedResultOffset resultOffset, ITagService tagService) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(resultOffset);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await tagService.GetAllTagsWithUserAsync(resultOffset);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<PagedResult<SingleTagDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleTagDto>>.Failed(serviceResult.Errors));
        });

        app.MapPost("api/tags", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] CreateTagDto createTagDto, ITagService tagService, HttpContext context) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(createTagDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await tagService.CreateTagAsync(userId!, createTagDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPut("api/tags/{tagId:int}", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] UpdateTagDto updateTagDto, ITagService tagService, HttpContext context, int tagId) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(updateTagDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await tagService.UpdateTagAndValidateOwnerAsync(userId!, tagId, updateTagDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapDelete("api/tags/{tagId:int}", [Authorize(Roles = RolesContract.Admin)] async (ITagService tagService, HttpContext context, int tagId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await tagService.DeleteTagAndValidateOwnerAsync(userId!, tagId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
