using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MonetaAPI.Data;
using MonetaAPI.Models;

namespace MonetaAPI.Controllers
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

        #region Transactions
        // GET: api/Transactions
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralData>> GetTransactions([FromBody] TransactionsRequest req)
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
            catch (Exception x) {
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

            query = "GET_GENERAL_DATA @userId, @dateStart, @dateEnd";

            GeneralData data =
                _context.GeneralData.FromSqlRaw(query, new object[] { param, param1, param2 })
                .AsEnumerable()
                .Single();

            data.Transactions = categories;

            return data;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTransaction(Guid id, Transaction transaction)
        //{
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

        // POST: api/Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult<GeneralData>> PostTransaction([FromBody] CreateTransactionRequest body)
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

            body.Transaction.UserId = userId;

            _context.Transactions.Add(body.Transaction);
            await _context.SaveChangesAsync();

            return await GetTransactions(body);
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<ActionResult<GeneralData>> UpdateTransaction([FromBody] CreateTransactionRequest body)
        {
            //Check that user exists
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

            //check that transaction belongs to that user
            try
            {
                Transaction temp = TransactionExists(body.Transaction.Id.Value);
                if (temp != null)
                {
                    if (temp.UserId == userId)
                    {
                        body.Transaction.UserId = userId;
                        _context.Entry(body.Transaction).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else 
                    {
                        return Unauthorized();
                    }
                }
                else 
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return await GetTransactions(body);
        }

        // Post: api/Transactions/Delete
        [HttpPost("Delete")]
        [Authorize]
        public async Task<ActionResult<GeneralData>> DeleteTransaction([FromBody] DeleteTransactionRequest body)
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

        // GET: api/Transactions/Calendar
        [HttpGet("Calendar")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CalendarDates>>> GetTransactionsCalendar()
        {
            string userId = String.Empty;
            try
            {
                userId = User.Claims.First(c => c.Type == "id").Value;
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return Unauthorized("Invalid username");
            }

            SqlParameter param = new SqlParameter("@userId", userId);
            String query = "GET_CALENDAR_DATES @userId";

            List<CalendarDates> dates =
                _context.CalendarDates.FromSqlRaw(query, new object[] { param })
                .AsEnumerable()
                .ToList();

            return dates;
        }

        private Transaction TransactionExists(Guid id)
        {
            return _context.Transactions
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
        }
        #endregion

        #region Categories
        // GET: api/Transactions/Categories
        [HttpGet("Categories")]
        [Authorize]
        public async Task<ActionResult<List<Category>>> GetCategories()
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

            List<Category> categories = 
                await _context.Categories
                .Where(cat => (cat.UserId == userId || cat.UserId == null))
                .AsNoTracking()
                .ToListAsync();

            /*.Where(cat => (cat.UserId == userId || cat.UserId == null))*/

            if (categories == null || categories.Count == 0)
            {
                return NotFound();
            }

            return categories;
        }

        // Post: api/Transactions/CreateCategory
        [HttpPost("CreateCategory")]
        [Authorize]
        public async Task<ActionResult<List<Category>>> PostCategories([FromBody] Category body)
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

            body.UserId = userId;

            _context.Categories.Add(body);
            await _context.SaveChangesAsync();

            return await GetCategories();
        }

        // Post: api/Transactions/UpdateCategory
        [HttpPost("UpdateCategory")]
        [Authorize]
        public async Task<ActionResult<List<Category>>> UpdateCategory([FromBody] Category body)
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

            try
            {
                Category temp = CategoryExists(body.Id);
                if (temp != null)
                {
                    if (temp.UserId == userId)
                    {
                        body.UserId = userId;
                        _context.Entry(body).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return await GetCategories();
        }

        // Post: api/Transactions/DeleteCategory
        [HttpPost("DeleteCategory")]
        [Authorize]
        public async Task<ActionResult<List<Category>>> DeleteCategory([FromBody] DeleteCategoryRequest body)
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

            var category = await _context.Categories.FindAsync(body.deletedCategory);
            if (category == null)
            {
                return NotFound();
            }

            if (category.UserId == userId)
            {
                SqlParameter param = new SqlParameter("@userId", userId);
                SqlParameter param1 = new SqlParameter(), param2 = new SqlParameter();
                param1.ParameterName = "@deletedCategory";
                param1.Value = body.deletedCategory;
                param2.ParameterName = "@newCategory";
                if (body.newCategory != null) { param2.Value = body.newCategory; } else { param2.Value = DBNull.Value; }
                String query = "DELETE_CATEGORY @userId, @deletedCategory, @newCategory";
                var rowsAffected = _context.Database.ExecuteSqlRaw(query, new object[] { param, param1, param2 });
                
                return await GetCategories();
            }
            return Unauthorized("User does not have permission to delete category.");
        }

        // Get: api/Transactions/Category/HasTransactions/{id}
        [HttpGet("Category/HasTransactions/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> CategoryHasTransactions(string id)
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

            Guid idTemp = new Guid();
            bool result = Guid.TryParse(id, out idTemp);

            if (result)
                return await _context.Transactions
                    .AnyAsync(transaction => transaction.Category == idTemp && 
                            transaction.UserId == userId);

            return false;
        }

        private Category CategoryExists(Guid id)
        {
            return _context.Categories
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
        }
        #endregion

    }
}
