using System;
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
    public class CategoryAuthor
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int CategoryAuthorId { get; set; }

        [Column]
        public int CategoryId { get; set; }

        [Column]
        public int AuthorId { get; set; }
    }
}
