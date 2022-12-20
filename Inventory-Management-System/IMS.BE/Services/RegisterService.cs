using IMS.BE.Models;
using IMS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.BE.Services
{
    public class RegisterService
    {
        private readonly IMSDBContext db;

        public RegisterService(IMSDBContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Get Roles Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetDropdownAsync()
        {
            var dropdowns = await this.db.UserRoleEnums
                 .Select(Q => new SelectListItem
                 {
                     Value = Q.UserRoleEnumId.ToString(),
                     Text = Q.Name
                 })
                 .ToListAsync();

            return dropdowns;
        }

        /// <summary>
        /// Validate User Code Exists
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public async Task<bool> IsUserCodeExists(string userCode)
        {
            var tolowerUserCode = userCode.ToLower();

            var isExists = await this.db.MasterUsers
                .AnyAsync(Q => Q.UserCode.ToLower() == tolowerUserCode);

            return isExists;
        }

        /// <summary>
        /// Validate Email Exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailExists(string email)
        {
            var tolowerEmail = email.ToLower();

            var isExists = await this.db.MasterUsers
                .AnyAsync(Q => Q.Email.ToLower() == tolowerEmail);

            return isExists;
        }

        /// <summary>
        /// Create New User
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        public async Task CreateNewUserAsync(Register registerUser)
        {
            var datetimeOffset = DateTime.UtcNow;

            var userRoleEnumId = await this.db.UserRoleEnums
                    .Where(Q => Q.UserRoleEnumId == int.Parse(registerUser.UserRole))
                    .Select(Q => Q.UserRoleEnumId)
                    .FirstOrDefaultAsync();

            this.db.MasterUsers.Add(new MasterUser
            {
                UserRoleEnumId = userRoleEnumId,
                Email = registerUser.Email,
                Name = registerUser.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password),
                UserCode = registerUser.UserCode.ToUpper(),
                CreatedBy = "SYSTEM",
                UpdatedBy = "SYSTEM",
                CreatedAt= datetimeOffset,
                UpdatedAt= datetimeOffset,
            });

            await this.db.SaveChangesAsync();
        }
    }
}
