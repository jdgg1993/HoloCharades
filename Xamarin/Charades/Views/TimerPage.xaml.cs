using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;

namespace Charades
{
	public partial class TimerPage : ContentPage
	{

		public TimeSpan tsCumulative;
		public TimeSpan tsSecond = new TimeSpan(0, 0, 1);
		public bool bRunning = false;
		private Stopwatch watch = null;
		public MessagesViewModel ChatVM { get; set; } = new MessagesViewModel();
		public HubConnection conn { get; set; }
		public IHubProxy proxy { get; set; }
		private bool roundStarted = false;

		CancellationTokenSource cancellationToken;

		ObservableCollection<Times> times = new ObservableCollection<Times>();


		public TimerPage()
		{
			InitializeComponent();

			TimeView.ItemsSource = times;

			var tapImage = new TapGestureRecognizer(); 
			tapImage.Tapped += OnButtonClicked;
			timeControl.GestureRecognizers.Add(tapImage);

			var tapImageNext = new TapGestureRecognizer();
			tapImageNext.Tapped += OnNextButtonClicked;
			next.GestureRecognizers.Add(tapImageNext);

			SignalR();
		}

		public void SignalR() 
		{
			controls.IsVisible = false;

			timeLabel.Text = "Connecting";

			conn = new HubConnection("http://socketserverrelay.azurewebsites.net");
			proxy = conn.CreateHubProxy("ChatHub");

			proxy.On<Messages>("broadcastMessage", MessageReceived);

			conn.Start();
			conn.StateChanged += (change) => 
			{
				if (change.NewState.ToString().Equals("Connected"))
				{
					Device.BeginInvokeOnMainThread(() => { controls.IsVisible = true; timeLabel.Text = "00:00:00.000";});
				}
			};
		}


		public void MessageReceived(Messages msg)
		{
			if (!msg.Username.Equals("Xamarin"))
			{
				Device.BeginInvokeOnMainThread(() => bird.Text = msg.Message.Trim());
			}
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			if (cancellationToken != null)
			{
				cancellationToken.Cancel();
				timeControl.Source = "play.png";
				cancellationToken = null;
				watch.Stop();
			}
			else
			{
				cancellationToken = new CancellationTokenSource();
				timeControl.Source = "pause.png";
				TimerRunning(this.cancellationToken.Token);
				watch.Start();
			}

			if (!roundStarted)
			{
				proxy.Invoke("Send", new Messages { Username = "Xamarin", Message = "" });
				roundStarted = true;
			}
		}

		void OnNextButtonClicked(object sender, EventArgs e)
		{
			times.Add(new Times { BirdName = bird.Text.ToString(), TimeComplete = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds) });
			TimeView.ScrollTo(times.LastOrDefault(), ScrollToPosition.End, false);
			proxy.Invoke("Send", new Messages { Username = "Xamarin", Message = ""});
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
				Device.BeginInvokeOnMainThread(() => timeLabel.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));
			}
		}
	}
}
