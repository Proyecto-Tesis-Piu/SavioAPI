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
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionContext _context;

        public TransactionsController(TransactionContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetTransactions([FromBody] String Password,
            [FromBody] DateTime FromDate, [FromBody] DateTime ToDate)
        {
            string userId = User.Claims.First(c => c.Type == "id").Value;
            CategoryDto[] categories = _context.Categories2.FromSql();
            ApplicationUser usertemp = await _context.Users.FindAsync(userId);
            if(usertemp != null && usertemp.Password == Password)
            {
                if (FromDate == null && ToDate == null)
                {
                    return Ok(_context.Transactions.Where(t => t.UserId == userId));
                }
                else
                {

                }

            }
            else
            {
                var msg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { ReasonPhrase = "ApplicationUser Not Found or Incorrect Password" };
                throw new System.Web.Http.HttpResponseException(msg);
            }

        }

        //// GET: api/Transactions/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        //{
        //    var transaction = await _context.Transactions.FindAsync(id);

        //    if (transaction == null)
        //    {
        //        return NotFound();
        //    }

        //    return transaction;
        //}

        // PUT: api/Transactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(Guid id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(Guid id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
