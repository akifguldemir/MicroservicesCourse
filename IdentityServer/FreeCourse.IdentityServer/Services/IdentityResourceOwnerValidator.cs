using Duende.IdentityServer.Validation;
using FreeCourse.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = _userManager.FindByNameAsync(context.UserName).Result;

            if(existUser == null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Kullanıcı adı veya şifre hatalı" });

                context.Result.CustomResponse = errors;

                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

            if (passwordCheck == false)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Kullanıcı adı veya şifre hatalı" });

                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(existUser.Id.ToString(), Duende.IdentityModel.OidcConstants.AuthenticationMethods.Password );
        }
    }
}
