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
		private Stopwatch watch = null;

		CancellationTokenSource cancellationToken;

		public TimerPage()
		{
			InitializeComponent();

			var tapImage = new TapGestureRecognizer(); 
			tapImage.Tapped += OnButtonClicked;
			timeControl.GestureRecognizers.Add(tapImage);
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			if (cancellationToken != null)
			{
				cancellationToken.Cancel();
				timeControl.Source = "play.png";
				cancellationToken = null;
			}
			else
			{
				cancellationToken = new CancellationTokenSource();
				timeControl.Source = "pause.png";
				TimerRunning(this.cancellationToken.Token);
			}
		}

		async void TimerRunning(CancellationToken token)
		{
			if (watch == null)
				watch = Stopwatch.StartNew();
			while (!token.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(10, token);
				}
				catch (TaskCanceledException)
				{
				}
				//string.Format("Time: {0}h {1}m {2}s {3}ms", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);
				Device.BeginInvokeOnMainThread(() => timeLabel.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));
			}
		}
	}
}
