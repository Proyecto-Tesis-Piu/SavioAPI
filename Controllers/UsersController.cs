using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SavioAPI.Models;
using SavioAPI.Data;
using Newtonsoft.Json;
using System.Text.Json;
using System.Net;

namespace SavioAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        //// GET: api/User
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        // GET: api/User/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> GetUser([FromBody]User user)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (result == null)
            {
                return NotFound();
            }

            if (user.Password != result.Password)
            {
                var msg = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Incorrect Password" };
                throw new System.Web.Http.HttpResponseException(msg);
            }
            return result;
        }

        // PUT: api/User/e91e94b8-d50f-4015-862a-1806c9fbe20c
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(Guid id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            User usertemp = await _context.Users.FindAsync(id);
            //_context.Entry(usertemp).State = EntityState.Modified;
            if (usertemp.Password == user.Password)
            {
                if(user.NewPassword != null && user.Password != user.NewPassword)
                {
                    user.Password = user.NewPassword;
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
        }

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            user.CountryCode = user.Country.CountryCode;
            user.StateCode = user.State.StateCode;
            switch (user.CivilStateString)
            {
                case "Single":
                    user.CivilStatebyte = Convert.ToByte(CivilState.Single);
                    break;
                default:
                    user.CivilStatebyte = Convert.ToByte(CivilState.Married);
                    break;
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
