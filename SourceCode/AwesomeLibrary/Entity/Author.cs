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
    public class Author
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int AuthorId { get; set; }

        [Column]
        public string AuthorName { get; set; }

        [Column]
        public int AuthorBookCount { get; set; }

        [Column]
        public string BookOrderBy { get; set; }

        [Column]
        public string BookOrderStyle { get; set; }

        [Column]
        public string AuthorNameCount { get; set; }

        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }
    }
}
