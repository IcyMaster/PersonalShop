﻿using System.Security.Claims;

namespace PersonalShop.Extension
{
    public static class ContextExtension
    {
        public static string? GetUserId(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
    }
}
