using Notes.Helpers;
using Notes.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Notes.Database
{
  public class NotesDb
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<NotesDb> Instance = new AsyncLazy<NotesDb>(async () =>
        {
            var instance = new NotesDb();
            CreateTableResult result = await Database.CreateTableAsync<ListingItems>();
            return instance;
        });

        public NotesDb()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }


        public async Task<IEnumerable<ListingItems>> GetItemsAsync()
        {
            var listItems = await Database.Table<ListingItems>().ToListAsync();
            return listItems;
        }
        public async Task<IEnumerable<ListingItems>> GetSearchedItemsAsync(string SerachText)
        {
            var listItems = await Database.Table<ListingItems>().Where(a=> a.Details.StartsWith(SerachText)).ToListAsync();
            return listItems;
        }

        //public Task<List<ListingItems>> GetItemsNotDoneAsync()
        //{
        //    // SQL queries are also possible
        //    return Database.QueryAsync<ListingItems>("SELECT * FROM [TodoItems] WHERE [IsChecked] = 0");
        //}

        public Task<ListingItems> GetItemAsync(int id)
        {
            return Database.Table<ListingItems>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(ListingItems item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(ListingItems item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> DropTableAsync()
        {
            return Database.DropTableAsync<ListingItems>();
        }
    }
}

