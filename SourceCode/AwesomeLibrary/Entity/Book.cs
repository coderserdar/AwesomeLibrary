using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary
{
    [Table]
    public class Book
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int BookId { get; set; }

        [Column]
        public string BookGuid { get; set; }

        [Column]
        public int BookCategoryId { get; set; }

        [Column]
        public string BookCategoryName { get; set; }

        [Column]
        public string BookAuthorName { get; set; }

        [Column]
        public string BookName { get; set; }

        [Column]
        public int BookPageNumber { get; set; }

        [Column]
        public string BookPublisherName { get; set; }

        [Column]
        public DateTime ReadStartDate { get; set; }

        [Column]
        public DateTime ReadFinishDate { get; set; }

        [Column]
        public int BookRating { get; set; }

        [Column]
        public string BookComment { get; set; }

        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }

        //[Column]
        //public string BookInformation { get; set; }

        [Column]
        public string BookNameRating { get; set; }
    }
}
