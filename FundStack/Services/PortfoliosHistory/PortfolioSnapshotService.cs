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
            TimeSpan interval = TimeSpan.FromHours(24);
            //calculate time to run the first time & delay to set the timer
            //DateTime.Today gives time of midnight 00.00
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
            var curTime = DateTime.Now;
            var firstInterval = nextRunTime.Subtract(curTime);

            Action action = () =>
            {
                var t1 = Task.Delay(firstInterval);
                t1.Wait();
                //remove inactive accounts at expected time
                SnapshotPortfolioValue(null);
                //now schedule it to be called every 24 hours for future
                // timer repeates call to RemoveScheduledAccounts every 24 hours.
                _timer = new Timer(
                    SnapshotPortfolioValue,
                    null,
                    TimeSpan.Zero,
                    interval
                );
            };

            Task.Run(action);
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
                var portfolioHistoryService = scope.ServiceProvider.GetRequiredService<PortfolioHistoryService>();
                portfolioHistoryService.RecordPorfoltioValue();
            }
        }
    }
}
