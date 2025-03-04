using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Tags.Dtos;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Extension;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Controllers;

[Route("Tags")]
public class TagController : Controller
{
    public readonly ITagService _tagService;
    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("GetTagsPartial")]
    [Authorize(Roles = RolesContract.Admin)]
    public async Task<ActionResult> GetTagPartial()
    {
        var serviceResult = await _tagService.GetAllTagsWithUserAsync();

        if (serviceResult.IsSuccess)
        {
            return PartialView("GetTagsPartial", serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddTag")]
    public ActionResult AddTag()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddTag")]
    public async Task<ActionResult> AddTag(CreateTagDto createTagDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createTagDto);
        }

        var serviceResult = await _tagService.CreateTagAsync(User.Identity!.GetUserId(), createTagDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserTags), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("DeleteTag/{tagId:int}", Name = "DeleteTag")]
    public async Task<ActionResult> DeleteTag(int tagId)
    {
        var serviceResult = await _tagService.DeleteTagAndValidateOwnerAsync(User.Identity!.GetUserId(), tagId);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserTags), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}
