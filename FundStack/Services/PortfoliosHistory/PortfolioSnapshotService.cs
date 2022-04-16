using FundStack.Data;

namespace FundStack.Services.PortfoliosHistory
{
    public class PortfolioSnapshotService : IHostedService
    {
        private Timer _timer;

        private readonly IServiceScopeFactory scopeFactory;

        public PortfolioSnapshotService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // timer repeates call to SnapshotPortfolioValue every 24 hours.
            _timer = new Timer(
                SnapshotPortfolioValue,
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(24)
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void SnapshotPortfolioValue (object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var portfolioHistoryService = scope.ServiceProvider.GetRequiredService<IPortfolioHistoryService>();
                portfolioHistoryService.RecordPorfoltioValue();
            }
        }
    }
}
