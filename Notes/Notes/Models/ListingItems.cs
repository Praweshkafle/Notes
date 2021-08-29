using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Notes.Models
{
   public class ListingItems
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Details { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
