using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BMCGMobile
{
	public partial class MainPage : TabbedPage
    {
		public MainPage()
		{
			InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                // Hide Title for Android only to conserve space
                foreach (var item in this.Children)
                {
                    if (item is NavigationPage)
                    {
                        item.Title = string.Empty;
                    }
                }
               
            }
        }
	}
}
