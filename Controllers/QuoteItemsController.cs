using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteApi.Models;

namespace QuoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteItemsController : ControllerBase
    {
        private readonly QuoteContext _context;
        private readonly System.Timers.Timer timer=null;

        public QuoteItemsController(QuoteContext context)
        {
            // if(timer == null)
            // {
            //     timer = new System.Timers.Timer();
            //     timer.Elapsed += Dowork;
            //     timer.Interval = 500000;
            //     timer.Enabled = true;
            // }
            _context = context;
        }

        // public void Dowork(object source, ElapsedEventArgs e){
        //     var quotes= _context.QuoteItems.ToList();
        //     foreach(var quote in quotes){
        //         if(quote.created.AddHours(1)>DateTime.Now){
        //             _context.QuoteItems.Remove(quote);
        //         }
        //     }
        // }
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
            if (quoteItem.author == "")
            {
                ModelState.AddModelError("Author", "Не добавлен автор");
            }
            if (quoteItem.quote == "")
            {
                ModelState.AddModelError("Quote", "Не добавлена цитата");
            }
            // если есть ошибки - возвращаем ошибку 400
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
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

        // POST: api/QuoteItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuoteItem>> PostQuoteItem(QuoteItem quoteItem)
        {
            _context.QuoteItems.Add(quoteItem);
            if (quoteItem.author == "")
            {
                ModelState.AddModelError("Author", "Не добавлен автор");
            }
            // если есть ошибки - возвращаем ошибку 400
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuoteItem", new { id = quoteItem.Id }, quoteItem);
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

        [HttpGet("random")]
        public async Task<ActionResult<QuoteItem>> GetRandomQuoteItem()
        {
            var quotes = await _context.QuoteItems.ToListAsync();
            var quote = quotes[new Random().Next(0,quotes.Count)];
            return quote;
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<QuoteItem>>> GetQuotesByCategory(string category)
        {
            var quotes = await _context.QuoteItems.Where(x=>x.category == category).ToListAsync();
            return quotes;
        }
        private bool QuoteItemExists(long id)
        {
            return _context.QuoteItems.Any(e => e.Id == id);
        }           
    }
}