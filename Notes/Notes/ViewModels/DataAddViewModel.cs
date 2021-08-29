using Notes.Database;
using Notes.Models;
using Notes.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    public class DataAddViewModel : BaseViewModel
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }


        private string _details;

        public string Details
        {
            get { return _details; }
            set { _details = value; OnPropertyChanged(nameof(Details)); }
        }

        public ICommand Save => new Command(async () =>
       {
           try
           {
               if (IsBusy) return;
               IsBusy = true;
               var recentlyTypedData = Details;

               NotesDb database = await NotesDb.Instance;
               await database.SaveItemAsync(new ListingItems
               {
                   Id = Id,
                   Details = recentlyTypedData
               }) ;
               await App.Current.MainPage.Navigation.PopAsync();
           }
           catch (Exception)
           {
               throw;
           }
           finally { IsBusy = false; }

       });
        public DataAddViewModel( ListingItems listingItem)
        {
            if (listingItem==null)
            {
                return;
            }
            else
            {
                Id = listingItem.Id;
                Details = listingItem.Details;
            }
        }
    }
}
