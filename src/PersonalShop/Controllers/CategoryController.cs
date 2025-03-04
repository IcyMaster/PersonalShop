using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Categories.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Extension;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Controllers;

[Route("Categories")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("GetCategoriesPartial")]
    [Authorize(Roles = RolesContract.Admin)]
    public async Task<ActionResult> GetCategoryPartial()
    {
        var serviceResult = await _categoryService.GetAllCategoriesWithUserAsync();

        if (serviceResult.IsSuccess)
        {
            return PartialView("GetCategoriesPartial", serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddCategory")]
    public ActionResult AddCategory()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddCategory")]
    public async Task<ActionResult> AddCategory(CreateCategoryDto createCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createCategoryDto);
        }

        var serviceResult = await _categoryService.CreateCategoryAsync(User.Identity!.GetUserId(), createCategoryDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserCategories), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("AddSubCategory/{categoryId:int}", Name = "AddSubCategory")]
    public ActionResult AddSubCategory(int categoryId)
    {
        var subCategory = new CreateCategoryDto() { ParentId = categoryId };
        return View(subCategory);
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("UpdateCategory/{categoryId:int}", Name = "UpdateCategory")]
    public async Task<ActionResult> UpdateCategory(int categoryId)
    {
        var serviceResult = await _categoryService.GetCategoryDetailsWithUserAndValidateOwnerAsync(categoryId, User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("UpdateCategory/{categoryId:int}", Name = "UpdateCategory")]
    public async Task<ActionResult> UpdateProduct(int categoryId, UpdateCategoryDto updateCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateCategoryDto);
        }

        var serviceResult = await _categoryService.UpdateCategoryAndValidateOwnerAsync(User.Identity!.GetUserId(), categoryId, updateCategoryDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserCategories), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpPost]
    [Authorize(Roles = RolesContract.Admin)]
    [Route("DeleteCategory/{categoryId:int}", Name = "DeleteCategory")]
    public async Task<ActionResult> DeleteCategory(int categoryId)
    {
        var serviceResult = await _categoryService.DeleteCategoryAndValidateOwnerAsync(User.Identity!.GetUserId(), categoryId);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserCategories), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

}
