using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary
{
    public class AwesomeLibraryDataContext : DataContext
    {
        public const string ConnectionString = @"Data Source=isostore:/AwesomeLibrary.sdf; Max Database Size=256; Max Buffer Size=4096;";
        public AwesomeLibraryDataContext(string connectionString)
            : base(connectionString) { }
        public Table<Category> Categories;
        public Table<Author> Authors;
        public Table<Book> Books;
        public Table<AppSettings> AppSettings;
        public Table<BookAuthor> BookAuthors;
        public Table<CategoryAuthor> CategoryAuthors;
    }
}
