namespace Samples.WinForms {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal sealed class TickerManager : IDisposable
    {
        private readonly Timer _timer;

        public Ticker[] Tickers { get; }

        public TickerManager()
        {
            Tickers = GetTickers().ToArray();
            _timer = InitTimer();
        }

        private static IEnumerable<Ticker> GetTickers()
        {
            return Enumerable.Range(1, 10).Select(n => new Ticker
            {
                Id = "Tick-" + n.ToString(),
                Price = n * 1.2m
            });
        }

        private Timer InitTimer()
        {
            return new Timer(state =>
            {
                UpdateTickers();
            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        private void UpdateTickers()
        {
            lock (_timer)
            {
                foreach (var item in Tickers)
                {
                    item.Price += 1.3m;
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}