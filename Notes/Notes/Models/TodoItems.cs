using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Models
{
    public class TodoItems
    {
        [PrimaryKey ,AutoIncrement]
        public int Id { get; set; }
        public string Details { get; set; }
        public bool IsChecked { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
