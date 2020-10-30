using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuoteApi.Models;

namespace QuoteApi
{    
    public class OverduesQuoteDeleteService: IHostedService, IDisposable
    {
        private readonly ILogger _logger;        
        private Timer _timer;
         private readonly QuoteContext _context;

        public OverduesQuoteDeleteService(QuoteContext context)
        {
            _context = context;
        }

        private async void DoWork(object state)
        {
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