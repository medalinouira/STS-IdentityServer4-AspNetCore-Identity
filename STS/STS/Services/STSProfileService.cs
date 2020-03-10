/// Mohamed Ali NOUIRA
/// http://www.mohamedalinouira.com
/// https://github.com/medalinouira
/// Copyright © Mohamed Ali NOUIRA. All rights reserved.

using STS.Models;
using System.Linq;
using IdentityModel;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;

namespace STS.Services
{
    public class STSProfileService : IProfileService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        #endregion

        #region Constructor
        public STSProfileService(
            UserManager<ApplicationUser> _userManager, 
            IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory)
        {
            this._userManager = _userManager;
            this._claimsFactory = _claimsFactory;
        }
        #endregion

        #region Methods
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();

            if (!string.IsNullOrEmpty(user.Email) && user.Email.StartsWith("admin"))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "Admin"));
            }
            context.IssuedClaims = claims;
        }
        #endregion
    }
}
