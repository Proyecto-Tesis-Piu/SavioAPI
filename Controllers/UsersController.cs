using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using MonetaAPI.Models;
using MonetaAPI.Data;
using Newtonsoft.Json;
using System.Text.Json;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace MonetaAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly UserContext _context;

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, //UserContext context, 
            IOptions<ApplicationSettings> appSettings)
        {
            //_context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<ActionResult<ApplicationUser>> Login([FromBody] ApplicationUser user)
        {
            ApplicationUser result = await _userManager.FindByNameAsync(user.Email);
            
            if (result == null)
            {
                return BadRequest(new { ReasonPhrase = "Email incorrecto" });
            }
            else {
                if (await _userManager.CheckPasswordAsync(result, user.Password)) {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                            new Claim("id", result.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(30),
                        //Issuer = JwtIdentityOptions.Issuer,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok(new { token });
                }
            }
            return BadRequest(new { ReasonPhrase = "Password incorrecto" });

        }

        // PUT: api/User/e91e94b8-d50f-4015-862a-1806c9fbe20c
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<ActionResult<ApplicationUser>> PutUser(String id, UserDto user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    ApplicationUser usertemp = await _context.Users.FindAsync(id);
        //    //_context.Entry(usertemp).State = EntityState.Modified;
        //    if (usertemp.PasswordHash == user.PasswordHash)
        //    {
        //        if(user.NewPassword != null && user.PasswordHash != user.PasswordHash)
        //        {
        //            user.PasswordHash = user.NewPassword;
        //        }
        //        _context.Users.Update(user);
        //    }
        //    else {
        //        var msg = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Incorrect Password" };
        //        throw new System.Web.Http.HttpResponseException(msg);
        //    }
            

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok(user);
        //}

        // POST: api/User/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Register")]
        public async Task<ActionResult<ApplicationUser>> Register([FromBody] ApplicationUser user)
        {
            user.CivilStatebyte = user.CivilStateString switch
            {
                "Single" => Convert.ToByte(CivilState.Single),
                "Married" => Convert.ToByte(CivilState.Married),
                "Divorced" => Convert.ToByte(CivilState.Divorced),
                "Widowed" => Convert.ToByte(CivilState.Widowed),
                _ => Convert.ToByte(CivilState.FreeUnion),
            };
            user.SexBit = user.Sex == "Male";
            user.UserName = user.Email;
            try
            {
                var result = await _userManager.CreateAsync(user, user.Password);
                return Ok(result);
            }
            catch (Exception ex) {
                throw ex;
            }


            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUser", new { id = user.Id }, user);

        }

        //private bool UserExists(String id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
