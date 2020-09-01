using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavioAPI.Data;
using SavioAPI.Models;

namespace SavioAPI.Controllers
{
    [Route("api/Country")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly CountriesContext _context;

        public CountriesController(CountriesContext context)
        {
            _context = context;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(string id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // GET: api/Country/MEX/States
        [HttpGet("{countryCode}/States")]
        public async Task<ActionResult<IEnumerable<State>>> GetStates(string countryCode)
        {
            return await _context.States.Where(state => state.CountryCode == countryCode).ToListAsync();
        }

        // GET: api/Country/State/5
        //[HttpGet("State/{id}")]
        //public async Task<ActionResult<State>> GetState(string id)
        //{
        //    var state = await _context.States.FindAsync(id);

        //    if (state == null)
        //    {
        //        return NotFound();
        //    }

        //    return state;
        //}

        //private bool CountryExists(string id)
        //{
        //    return _context.Countries.Any(e => e.CountryCode == id);
        //}
    }
}
