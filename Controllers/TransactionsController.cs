using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetTransactions([FromBody] TransactionsRequest req)
        {
            string userId = String.Empty;
            try
            {
                userId = User.Claims.First(c => c.Type == "id").Value;
                var user = await _context.Users.FindAsync(userId);
                if (user == null) {
                    throw new Exception();
                }
            }
            catch (Exception) {
                return Unauthorized("Invalid username");
            }

            SqlParameter param = new SqlParameter("@userId", userId);
            SqlParameter param1 = new SqlParameter(), param2 = new SqlParameter();
            param1.ParameterName = "@dateStart";
            param2.ParameterName = "@dateEnd";
            if (req.FromDate != null) { param1.Value = req.FromDate; } else { param1.Value = DBNull.Value; }
            if (req.ToDate != null) { param2.Value = req.ToDate; } else { param2.Value = DBNull.Value; }
            String query = "GET_CATEGORIES_TOTALS @userId, @dateStart, @dateEnd";

            List<CategoryDto> categories =
                _context.Categories2.FromSqlRaw(query, new object[] { param, param1, param2 })
                .AsEnumerable()
                .ToList();

            query = "GET_TRANSACTIONS_FOR_USER @userId, @dateStart, @dateEnd";

            List<Transaction> transactions =
                _context.Transactions.FromSqlRaw(query, new object[] { param, param1, param2 })
                .AsEnumerable()
                .ToList();

            foreach (CategoryDto category in categories) {
                category.ChildrenTransactions = transactions.Where(t => t.Category == category.Id).ToArray();
            }

            return categories;
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
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTransaction(Guid id, Transaction transaction)
        //{
        //    if (id != transaction.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(transaction).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TransactionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Transactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        //{
        //    _context.Transactions.Add(transaction);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        //}

        // Post: api/Transactions/Delete
        [HttpPost("Delete")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> DeleteTransaction([FromBody] DeleteTransactionRequest body)
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

            var transaction = await _context.Transactions.FindAsync(body.TransactionID);
            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId == userId)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return await GetTransactions(body);
            }
            return Unauthorized("User does not have permission to delete transaction.");
        }

        //private bool TransactionExists(Guid id)
        //{
        //    return _context.Transactions.Any(e => e.Id == id);
        //}
    }
}
