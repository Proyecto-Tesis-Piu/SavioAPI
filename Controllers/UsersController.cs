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
        private readonly UserContext _context;

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, UserContext context, 
            IOptions<ApplicationSettings> appSettings)
        {
            _context = context;
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

        /* PUT: api/User/e91e94b8-d50f-4015-862a-1806c9fbe20c
         To protect from overposting attacks, enable the specific properties you want to bind to, for
         more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<ApplicationUser>> PutUser(String id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            ApplicationUser usertemp = await _context.Users.FindAsync(id);
            //_context.Entry(usertemp).State = EntityState.Modified;
            if (usertemp.PasswordHash == user.PasswordHash)
            {
                if(user.NewPassword != null && user.PasswordHash != user.PasswordHash)
                {
                    user.PasswordHash = user.NewPassword;
                }
                _context.Users.Update(user);
            }
            else {
                var msg = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Incorrect Password" };
                throw new System.Web.Http.HttpResponseException(msg);
            }
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }*/

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
                if (result.Succeeded)
                {
                    ApplicationUser userResult = await _userManager.FindByNameAsync(user.Email);

                    var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(userResult);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                                new Claim("id", userResult.Id.ToString()),
                                new Claim("emailToken", emailToken)
                            }),
                        Expires = DateTime.UtcNow.AddDays(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    MailSender.ConfirmMail(user.Email, token);
                    return Ok(result);
                }
                else
                {
                    return Conflict(result);
                }
            }
            catch (Exception ex) {
                throw ex;
            }


            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUser", new { id = user.Id }, user);

        }

        // POST: api/User/Feedback
        [HttpPost("Feedback")]
        public async Task<ActionResult> Feedback([FromBody] Feedback feedback)
        {
            Guid userId;
            try
            {
                userId = new Guid(User.Claims.First(c => c.Type == "id").Value);
                var user = await _context.Users.FindAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return Unauthorized("Invalid username");
            }

            feedback.UserId = userId;
            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //private bool UserExists(String id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
        // GET: api/User/ConfirmEmail
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail()
        {
            //Guid userId;
            ApplicationUser user;
            String temp;
            String emailToken;
            try
            {
                temp = User.Claims.First(c => c.Type == "id").Value;
                emailToken = User.Claims.First(c => c.Type == "emailToken").Value;

                if (temp == null || emailToken == null)
                {
                    return BadRequest(new { ReasonPhrase = "Token inválido" });
                }
                
                user = await _userManager.FindByIdAsync(temp);

                if (user == null)
                {
                    return NotFound("Usuario no encontrado");
                }
                //userId = new Guid();
                //user = await _context.Users.FindAsync(userId.ToString());
                
                var result = await _userManager.ConfirmEmailAsync(user, emailToken);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                throw new Exception("Couldnt Confirm Email");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
