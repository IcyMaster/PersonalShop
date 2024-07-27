using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Personal_Shop.Domain.Errors;
using Personal_Shop.Domain.Products.DTO;
using Personal_Shop.Domain.Users.DTO;
using Personal_Shop.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Security.Claims;

namespace Personal_Shop.Api;

public static class CustomApis
{
    public static void RegisterProductApis(this WebApplication app)
    {
        app.MapGet("Api/Products",(IProductService productService) => productService.GetProducts()).RequireAuthorization();

        app.MapPost("Api/Products/AddProduct", async ([FromBody] ProductDTO productModel, IProductService productService,HttpContext context) =>
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;
            productModel.UserId = userId;

            await productService.AddProduct(productModel);

            return productModel;
        }).RequireAuthorization();

        app.MapPut("Api/Products/UpdateProduct/{productId:long}", async ([FromBody] ProductDTO productModel, IProductService productService, HttpContext context, long productId) =>
        {
            var product = await productService.GetProductById(productId);
            if (product is null)
            {
                return Results.BadRequest(new ApiErrorDTO() { Errors = new List<string>() { "محصول مورد نظر یافت نشد" } });
            }

            var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;
            if (!product.UserId.Equals(userId))
            {
                return Results.BadRequest(new ApiErrorDTO() { Errors = new List<string>() { "فقط سازنده محصول ، می تواند این محصول را ویرایش کند" } });
            }

            var validateResult = ValidateObjects(productModel);
            if (validateResult is not null)
            {
                return Results.BadRequest(validateResult);
            }

            productModel.UserId = userId;
            await productService.UpdateProductById(productId, productModel);

            return Results.Ok(productModel);
        }).RequireAuthorization();

        app.MapDelete("Api/Products/DeleteProduct/{productId:long}", async (IProductService productService, HttpContext context, long productId) => {

            var product = await productService.GetProductById(productId);
            if (product is null)
            {
                return Results.BadRequest(new ApiErrorDTO() { Errors = new List<string>() { "محصول مورد نظر یافت نشد" } });
            }

            var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;
            if (!product.UserId.Equals(userId))
            {
                return Results.BadRequest(new ApiErrorDTO() { Errors = new List<string>() { "فقط سازنده محصول ، می تواند این محصول را ویرایش کند" } });
            }

            await productService.DeleteProductById(productId);

            return Results.Ok("محصول مورد نظر با موفقیت حذف شد");
        }).RequireAuthorization();
    }

    public static void RegisterAccountApis(this WebApplication app)
    {
        app.MapPost("Api/Account/Register", async ([FromBody] RegisterDTO registerModel, IUserService userService) =>
        {
            var validateResult = ValidateObjects(registerModel);

            if (validateResult is not null)
            {
                return Results.BadRequest(validateResult);
            }

            await userService.CreateUserAsync(registerModel.UserName,
                                              registerModel.Password,
                                              registerModel.Email,
                                              registerModel.FirstName,
                                              registerModel.LastName,
                                              registerModel.PhoneNumber);

            return Results.Ok("User registered successfully");
        }).RequireAuthorization();

        app.MapPost("Api/Account/Login", async ([FromBody] LoginDTO loginModel,IAuthenticationService authenticationService, IUserService userService) =>
        {
            var user = await authenticationService.LoginAsync(loginModel.Email, loginModel.Password);
            if (user is not null)
            {
                var tokenString = userService.CreateTokenAsync(user);

                dynamic json = new ExpandoObject();
                json.Status = "Success";
                json.TokenString = tokenString;

                return Results.Ok(JsonConvert.SerializeObject(json));
            }

            return Results.BadRequest(new ApiErrorDTO() { Errors = new List<string>() { "شما با اطلاعات وارد شده نمی توانید وارد سایت شوید" } });
        });
    }

    private static ApiErrorDTO? ValidateObjects(object instance)
    {
        var validateRes = new List<ValidationResult>();

        if (!Validator.TryValidateObject(instance, new ValidationContext(instance), validateRes, true))
        {
            var result = new ApiErrorDTO();
            int counter = 0;
            foreach (var item in validateRes)
            {
                result.Errors.Add(validateRes[counter].ErrorMessage!);
                counter++;
            }

            return result;
        }

        return null;
    }
}
