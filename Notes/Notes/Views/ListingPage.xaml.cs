using Notes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListingPage : ContentPage
    {
        public ListingPage()
        {
            InitializeComponent();
            BindingContext = new ListingItemViewModel();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DataAddPage(null));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<object, string>(this, "RefreshHomePage", string.Empty);
        }

    }
}