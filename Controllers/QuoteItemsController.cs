using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteApi.Models;

namespace QuoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QouteItemsController : ControllerBase
    {
        private readonly QuoteContext _context;

        public QouteItemsController(QuoteContext context)
        {
            _context = context;
        }

        // GET: api/QuoteItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuoteItem>>> GetQuoteItems()
        {
            return await _context.QuoteItems.ToListAsync();
        }

        // GET: api/QuoteItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteItem>> GetQuoteItem(long id)
        {
            var quoteItem = await _context.QuoteItems.FindAsync(id);

            if (quoteItem == null)
            {
                return NotFound();
            }

            return quoteItem;
        }

        // PUT: api/QuoteItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuoteItem(long id, QuoteItem quoteItem)
        {
            if (id != quoteItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(quoteItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuoteItem>> PostQuoteItem(QuoteItem todoItem)
        {
            _context.QuoteItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuoteItem", new { id = todoItem.Id }, todoItem);
        }


        // DELETE: api/QuoteItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuoteItem>> DeleteQuoteItem(long id)
        {
            var quoteItem = await _context.QuoteItems.FindAsync(id);
            if (quoteItem == null)
            {
                return NotFound();
            }

            _context.QuoteItems.Remove(quoteItem);
            await _context.SaveChangesAsync();

            return quoteItem;
        }

        private bool QuoteItemExists(long id)
        {
            return _context.QuoteItems.Any(e => e.Id == id);
        }

           
    }
}