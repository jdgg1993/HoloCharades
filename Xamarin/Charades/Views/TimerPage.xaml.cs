using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Charades
{
	public partial class TimerPage : ContentPage
	{

		public TimeSpan tsCumulative;
		public TimeSpan tsSecond = new TimeSpan(0, 0, 1);
		public bool bRunning = false;

		CancellationTokenSource cancellationToken;

		public TimerPage()
		{
			InitializeComponent();
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			if (cancellationToken != null)
			{
				cancellationToken.Cancel();
				stopChrono.Text = "Start";
				cancellationToken = null;
			}
			else
			{
				cancellationToken = new CancellationTokenSource();
				stopChrono.Text = "Stop";
				TimerRunning(this.cancellationToken.Token);
			}
		}

		async void TimerRunning(CancellationToken token)
		{
			var watch = Stopwatch.StartNew();
			while (!token.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(10, token);
				}
				catch (TaskCanceledException)
				{
				}
				Device.BeginInvokeOnMainThread(() => timeLabel.Text = watch.Elapsed.ToString());
			}
		}
	}
}
