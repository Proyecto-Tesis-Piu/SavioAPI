using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavioAPI.Models;

namespace SavioAPI.Controllers
{
    [Route("api/UserProfile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile() {
            string userId = User.Claims.First(c => c.Type == "id").Value;
            var user = await _userManager.FindByIdAsync(userId);

            user.Sex = user.SexBit ? "Male" : "Female";

            switch (user.CivilStatebyte) {
                case (byte)CivilState.Single:
                    user.CivilStateString = "Single";
                    break;
                case (byte)CivilState.Married:
                    user.CivilStateString = "Married";
                    break;
                case (byte)CivilState.Divorced:
                    user.CivilStateString = "Divorced";
                    break;
                case (byte)CivilState.Widowed:
                    user.CivilStateString = "Widowed";
                    break;
                default:
                    user.CivilStateString = "FreeUnion";
                    break;
            }

            return new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.CountryCode,
                user.StateCode,
                user.Sex,
                user.BirthDate,
                user.Job,
                user.CivilStateString
            };
        }
    }
}
