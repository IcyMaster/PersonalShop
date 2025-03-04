using PersonalShop.Shared.Resources.Validations.Role;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.BusinessLayer.Services.Identitys.Roles.Dtos;

public class AssignRoleDto
{
    [Required(ErrorMessageResourceType = typeof(RoleMessages)
        , ErrorMessageResourceName = nameof(RoleMessages.UserEmailRequired))]
    public string UserEmail { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(RoleMessages)
        , ErrorMessageResourceName = nameof(RoleMessages.RoleNameRequired))]
    public string RoleName { get; set; } = string.Empty;
}
