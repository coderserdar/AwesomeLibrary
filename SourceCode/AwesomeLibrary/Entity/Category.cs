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
    public class Category
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int CategoryId { get; set; }

        [Column]
        public string CategoryName { get; set; }

        [Column]
        public int CategoryBookCount { get; set; }

        [Column]
        public string AuthorOrderBy { get; set; }

        [Column]
        public string AuthorOrderStyle { get; set; }

        [Column]
        public string CategoryNameCount { get; set; }

        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }
    }
}
