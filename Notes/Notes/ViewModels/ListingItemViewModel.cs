using Notes.Database;
using Notes.Models;
using Notes.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Notes.ViewModels
{
   public class ListingItemViewModel:BaseViewModel
    {
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }

        private SelectionMode _selection;

        public SelectionMode Selection
        {
            get { return _selection; }
            set { _selection = value; OnPropertyChanged(nameof(Selection)); }
        }

        private bool _addVisible;

        public bool AddIsVisible
        {
            get { return _addVisible; }
            set { _addVisible = value; OnPropertyChanged(nameof(AddIsVisible)); }
        }

        private bool _actionMenuVisible;

        public bool ActionMenuVisible
        {
            get { return _actionMenuVisible; }
            set { _actionMenuVisible = value; OnPropertyChanged(nameof(ActionMenuVisible)); }
        }


        private ObservableRangeCollection<ListingItems> listingItem = new ObservableRangeCollection<ListingItems>();
        public ObservableRangeCollection<ListingItems> ListingItem
        {
            get {
                return listingItem;
                }
            set { listingItem = value; OnPropertyChanged(nameof(ListingItem)); }
        }

        public ObservableCollection<ListingItems> SelectedItem = new ObservableCollection<ListingItems>();

        public ListingItemViewModel()
        {
            MessagingCenter.Subscribe<object, string>(this, "RefreshHomePage", async (sender, message) =>
            {
                try
                {
                    IsRefreshing = true;
                    await LoadFromDb();
                }
                catch
                { }
                finally { IsRefreshing = false; }
            });
            Task.Run(async () =>
            {
                try{
                    IsRefreshing = true;
                    await Task.Delay(2000);
                    await LoadFromDb();}
                catch { }
                finally { IsRefreshing = false; }
            });
            AddIsVisible = true;
            SearchItemCommand = new Command( async()=> await RetriveSearchedData());
            RefreshingCommand = new Command(()=> RefreshData());
            LongPressCommand = new Command<ListingItems>((obj) => LongPress(obj));
            PressedCommand = new Command<ListingItems>((obj) => SelectData(obj));
            CancleCommand = new Command(() =>Cancle());
            DeleteCommand = new Command(() => Delete());
        }

        public ICommand SearchItemCommand { get; }
        public ICommand RefreshingCommand { get; }
        public ICommand LongPressCommand { get; }
        public ICommand PressedCommand { get; }
        public ICommand CancleCommand { get; }
        public ICommand DeleteCommand { get; }

        async void Delete()
        {
            try
            {
                IsRefreshing = true;
                var database = await NotesDb.Instance;
                foreach (var item in SelectedItem)
                {
                    await database.DeleteItemAsync(item);
                }
                await LoadFromDb();
            }
            catch (Exception)
            {
                throw;
            }
            finally { IsRefreshing = false; }
          
        }
        void Cancle()
        {
            SelectedItem.Clear();
            Selection = SelectionMode.None;
            ActionMenuVisible = false;
            AddIsVisible = true;
        }
        void SelectData(ListingItems obj)
        {
            if (Selection!=SelectionMode.None)
            {
                if (SelectedItem.Contains(obj))
                {
                    SelectedItem.Remove(obj);
                }
                else
                    SelectedItem.Add(obj);
            }
            else
            {
                MoveToDetailPage(obj);
            }
        }

        void LongPress(ListingItems obj)
        {
            if (IsBusy) return;
            IsBusy = true;
            ActionMenuVisible = true;
            AddIsVisible = false;
            if (Selection == SelectionMode.None)
            {
                Selection = SelectionMode.Multiple;
                AddIsVisible = false;
                SelectedItem.Add(obj);
            }
            IsBusy = false;
        }
        private async void RefreshData()
        {
            try
            {
                IsRefreshing = true;
                await LoadFromDb();
            }
            catch (Exception)
            {
                throw;
            }
            finally { IsRefreshing = false; }
        }
        private async Task<IEnumerable<ListingItems>> RetriveSearchedData()
        {
            try
            {
                IsBusy = true;
                var database = await NotesDb.Instance;
                if (string.IsNullOrEmpty(SearchText))
                {
                    return await LoadFromDb();
                }
                else
                {
                    var dataList = await database.GetSearchedItemsAsync(SearchText);
                    listingItem.Clear();
                    listingItem.AddRange(dataList);
                    return listingItem;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { IsBusy = false; }
        }

        private async void MoveToDetailPage(ListingItems obj)
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                var Id = obj.Id;
                var database = await NotesDb.Instance;
                var data = await database.GetItemAsync(Id);
                await App.Current.MainPage.Navigation.PushAsync(new DataAddPage(data));
            }
            catch (Exception)
            {
                throw;
            }
            finally { IsBusy = false; }
        }

        private async Task<ObservableCollection<ListingItems>> LoadFromDb()
        {
            try
            {
                if (IsBusy) return listingItem;
                IsBusy = true;
                ListingItem.Clear();
                var database = await NotesDb.Instance;
                // await database.DropTableAsync();
                var data = await database.GetItemsAsync();
                ListingItem.AddRange(data);
                return ListingItem;
            }
            catch (Exception)
            {
                throw;
            }
            finally {IsBusy = false;}

        }
    }
}
