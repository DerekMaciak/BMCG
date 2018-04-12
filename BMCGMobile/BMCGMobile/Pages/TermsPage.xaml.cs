using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TermsPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("BMCGMobile.html.Terms.html");
            string termsHtml = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                termsHtml = reader.ReadToEnd();
            };

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = termsHtml;
            termsWebView.Source = htmlSource;
           
    }

        private async void OnAcceptButtonClicked(object sender, EventArgs e)
        {
         
            await Navigation.PopModalAsync();
        }
    }
}