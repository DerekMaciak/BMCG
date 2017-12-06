﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FitnessPage : ContentPage
	{
		public FitnessPage ()
		{
			InitializeComponent ();
		}

        async void OnFitnessHistoryButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FitnessHistoryPage());
        }
    }
}