using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuoteApi.Models;
using System.Collections.Generic;

namespace QuoteApi
{    
    public  class  OverduesQuoteDeleteService: IHostedService, IDisposable
    {
        private readonly ILogger _logger;        
        private Timer _timer;
         private readonly QuoteContext _context;

        public OverduesQuoteDeleteService(QuoteContext context)
        {
            _context = context;
        }
        
        private  void DoWork(object state)
        {
            QuoteItem quoteItem=new QuoteItem();
            var quotes= _context.QuoteItems.ToList();
            foreach(var quote in quotes){
                if(quote.created.AddHours(1)>DateTime.Now){
                    // _context.Remove(_context.QuoteItems.Where(q=>q.created<DateTime.Now.AddHours(-1)));
                    _context.QuoteItems.Remove(quote);
                }
            }
           
            // _context.QuoteItems.Remove(quoteItem);
            _timer.Change(500000,Timeout.Infinite); 
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {     
            _timer = new Timer(DoWork, null, 0, Timeout.Infinite);
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {            
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}