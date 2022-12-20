using IMS.BE.Models;
using IMS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IMS.BE.Services
{
    public class LoginService
    {
        private readonly IMSDBContext db;
        private readonly UserIdentityService userIdentityService;

        public LoginService(IMSDBContext db, UserIdentityService userIdentityService)
        {
            this.db = db;
            this.userIdentityService = userIdentityService;
        }

        public async Task<ClaimsPrincipal> Login(string authScheme, string userCode, string password)
        {
            var user = await this.GetUserAsync(userCode);

            if (user == null)
            {
                return null;
            }

            var isPasswordValid = this.VerifyPassword(user, password);

            if (!isPasswordValid)
            {
                return null;
            }

            var userRole = await (from mu in this.db.MasterUsers
                                  join ure in this.db.UserRoleEnums on mu.UserRoleEnumId equals ure.UserRoleEnumId
                                  where mu.UserCode.ToLower() == userCode.ToLower()
                                  select ure.Name)
                                  .FirstOrDefaultAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, userRole!),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.UserData, user.UserCode),
            };

            var claimsIdentity = new ClaimsIdentity(claims, authScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        public bool VerifyPassword(MasterUser user, string password)
        {
            var userPasswordHash = user.Password;
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, userPasswordHash);

            return isPasswordValid;
        }

        public async Task<MasterUser> GetUserAsync(string userCode)
        {
            var tolowerUserCode = userCode.ToLower();

            var user = await this.db.MasterUsers
                .Where(Q => Q.UserCode.ToLower() == tolowerUserCode)
                .FirstOrDefaultAsync();

            return user!;
        }

        public async Task<bool> ChangePasswordAsync(bool isForgotPassPage, ChangePassword change)
        {
            if (isForgotPassPage)
            {
                var user = await this.GetUserAsync(change.UserCode);

                if (user == null)
                {
                    return false;
                }

                user!.Password = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);
                user.UpdatedBy = change.UserCode.ToUpper();
                user.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var userLogin = userIdentityService.UserCode;

                var user = await this.GetUserAsync(userLogin);

                var isPasswordValid = this.VerifyPassword(user, change.OldPassword);

                if (!isPasswordValid)
                {
                    return false;
                }

                user!.Password = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);
                user.UpdatedBy = userLogin;
                user.UpdatedAt = DateTime.UtcNow;
            }

            await this.db.SaveChangesAsync();
            return true;
        }
    }
}
