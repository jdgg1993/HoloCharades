﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Charades
{
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
		}

		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			App.SchoolName = name.Text;
			await Navigation.PushAsync(new TimerPage());
		}
	}
}
