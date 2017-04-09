namespace Samples.WinForms
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            using (var form = new Form())
            using (var grid = new DataGridView())
            using (var tickerManager = new TickerManager())
            {
                grid.DataSource = new BindingList<Ticker>(tickerManager.Tickers);
                grid.Dock = DockStyle.Fill;

                form.Controls.Add(grid);
                Application.Run(form);
            }
        }
    }
}