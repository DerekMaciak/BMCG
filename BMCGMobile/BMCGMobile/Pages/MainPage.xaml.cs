// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-06-2018
// ***********************************************************************
// <copyright file="MainPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Xamarin.Forms;

namespace BMCGMobile
{
    /// <summary>
    /// Class MainPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.TabbedPage" />
    public partial class MainPage : TabbedPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
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