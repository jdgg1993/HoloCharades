using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace Charades
{
	public partial class LeaderboardPage : ContentPage
	{

		IMobileServiceTable<leaderboard> table;

		public LeaderboardPage()
		{
			InitializeComponent();
			Title = "Leaderboard";

			table = App.MobileService.GetTable<leaderboard>();

			loadLeaderboard();
		}

		public async void loadLeaderboard()
		{
			LeaderBoardView.ItemsSource = await GetLeaderboardItem();
			LeaderBoardView.IsVisible = true;
			loading.IsVisible = false;
		}

		public async Task<ObservableCollection<leaderboard>> GetLeaderboardItem()
		{

			try
			{
				IEnumerable<leaderboard> items = await table.OrderBy(g => g.TotalTime).ToEnumerableAsync();
				return new ObservableCollection<leaderboard>(items);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				System.Diagnostics.Debug.WriteLine(msioe.Message);
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
			}

			return null;
		}
	}
}
