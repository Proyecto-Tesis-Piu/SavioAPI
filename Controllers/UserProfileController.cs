using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonetaAPI.Data;
using MonetaAPI.Models;

namespace MonetaAPI.Controllers
{
    [Route("api/UserProfile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private CountriesContext _context;
        public UserProfileController(UserManager<ApplicationUser> userManager, CountriesContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile() {
            string userId = User.Claims.First(c => c.Type == "id").Value;
            var user = await _userManager.FindByIdAsync(userId);

            user.Sex = user.SexBit ? "Male" : "Female";

            user.CivilStateString = user.CivilStatebyte switch
            {
                (byte)CivilState.Single => "Single",
                (byte)CivilState.Married => "Married",
                (byte)CivilState.Divorced => "Divorced",
                (byte)CivilState.Widowed => "Widowed",
                _ => "FreeUnion",
            };
            user.StateName = _context.States.Where(state => state.CountryCode == user.CountryCode &&
                                state.StateCode == user.StateCode).FirstOrDefault().Name;

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
                user.StateName,
                user.CivilStateString
            };
        }
    }
}
