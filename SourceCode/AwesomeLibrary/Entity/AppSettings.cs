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
    public class AppSettings
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int AppSettingsId { get; set; }

        [Column]
        public int CurrentCategoryNumber { get; set; }

        [Column]
        public int CurrentAuthorNumber { get; set; }

        [Column]
        public string AppLangName { get; set; }

        [Column]
        public string AppBackgroundColor { get; set; }

        [Column]
        public string CategoryOrderBy { get; set; }

        [Column]
        public string CategoryOrderStyle { get; set; }

        [Column]
        public string FontFamily { get; set; }
        [Column]
        public string FontSize { get; set; }

        [Column(DbType = "Image", UpdateCheck = UpdateCheck.Never)]
        public byte[] AppBackgroundImage { get; set; }
    }
}
