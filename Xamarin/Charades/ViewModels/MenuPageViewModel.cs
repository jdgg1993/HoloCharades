﻿using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Charades
{
	public class MenuPageViewModel
	{
		public ICommand GoHomeCommand { get; set; }
		public ICommand GoSecondCommand { get; set; }

		public MenuPageViewModel()
		{
			GoHomeCommand = new Command(GoHome);
			GoSecondCommand = new Command(GoSecond);
		}

		void GoHome(object obj)
		{
			App.NavigationPage.Navigation.PopToRootAsync();
			App.MenuIsPresented = false;
		}

		void GoSecond(object obj)
		{
			App.NavigationPage.Navigation.PushAsync(new LeaderboardPage());
			App.MenuIsPresented = false;
		}
	}
}
