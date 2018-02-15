// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-23-2018
// ***********************************************************************
// <copyright file="AboutPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Entities;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    /// <summary>
    /// Class AboutPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutPage"/> class.
        /// </summary>
        public AboutPage()
        {
            InitializeComponent();

            this.BindingContext = new AboutEntity();
        }
    }
}