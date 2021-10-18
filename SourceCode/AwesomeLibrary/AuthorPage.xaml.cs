using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Data.Common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using AwesomeLibrary.Resources;

namespace AwesomeLibrary
{
    public partial class AuthorPage : PhoneApplicationPage
    {

        public int authorId;
        public int categoryId;
        public int bookId;
        public Popup popup;
        public string oldAuthorName;

        public AuthorPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Add.png", UriKind.Relative);
            button1.Text = AppResources.AddBook;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddBookButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/Delete.png", UriKind.Relative);
            button2.Text = AppResources.DeleteAuthor;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(DeleteAuthorButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/Settings.png", UriKind.Relative);
            button3.Text = AppResources.AuthorSettings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(AuthorSettingsButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/AddCategory.png", UriKind.Relative);
            button4.Text = AppResources.AddCategory;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(AddCategoryButton_Click);

            popup = new Popup();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            List<Book> books = new List<Book>();
            List<Book> booksOrdered = new List<Book>();

            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);

            lstBooks.Items.Clear();

            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {

                var appSettings = context.AppSettings.First();
                categoryId = appSettings.CurrentCategoryNumber;

                var author = context.Authors.Where(j => j.AuthorId.Equals(e.Fragment)).Single() as Author;
                authorId = author.AuthorId;

                var appSettings2 = context.AppSettings;
                foreach (var item in appSettings2)
                {
                    item.CurrentAuthorNumber = authorId;
                }
                context.SubmitChanges();

                var authorBooks = context.BookAuthors.Where(j => j.AuthorId.Equals(e.Fragment)).ToList() as List<BookAuthor>;
                if (authorBooks.Count != 0)
                {
                    foreach (var item in authorBooks)
                    {
                        try
                        {
                            var book = context.Books.Where(j => j.BookCategoryId.Equals(categoryId) && j.BookId.Equals(item.BookId)).Single() as Book;
                            books.Add(book);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }


                string orderStyle = author.BookOrderStyle;

                switch (author.BookOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.BookName).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderByDescending(j => j.BookName).ToList();
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.CreationDate).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderByDescending(j => j.CreationDate).ToList();
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.ModificationDate).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderByDescending(j => j.ModificationDate).ToList();
                        }
                        break;
                    case "SDATE":
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.ReadStartDate).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderByDescending(j => j.ReadStartDate).ToList();
                        }
                        break;
                    case "FDATE":
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.ReadFinishDate).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderByDescending(j => j.ReadFinishDate).ToList();
                        }
                        break;
                    case "RATING":
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.BookRating).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderByDescending(j => j.BookRating).ToList();
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            booksOrdered = books.OrderBy(j => j.BookName).ToList();
                        }
                        else
                        {
                            booksOrdered = books.OrderBy(j => j.BookName).ToList();
                        }
                        break;
                }

                lblAuthorName.Text = author.AuthorName;
                lblBookList.Text = AppResources.BookList + " (" + author.AuthorName + ")";
                lstBooks.ItemsSource = booksOrdered;
                lstBooks.DisplayMemberPath = "BookNameRating";
                SetBackgroundColor();
                //lstNoteList.DisplayMemberPath = "NameCreation";
            }
        }

        private void SetBackgroundColor()
        {
            AppSettings appSettings = new AppSettings();
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                appSettings = context.AppSettings.First() as AppSettings;
            }

            if (appSettings.AppBackgroundImage != null)
            {
                MemoryStream stream = new MemoryStream(appSettings.AppBackgroundImage);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = image;
                this.LayoutRoot.Background = ib;
            }
            else
            {
                switch (appSettings.AppBackgroundColor)
                {
                    case "BLA":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                        break;
                    case "BLU":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case "BRO":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Brown);
                        break;
                    case "RED":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Red);
                        break;
                    case "GRE":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case "GRA":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case "YEL":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    case "ORA":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Orange);
                        break;
                    case "PUR":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Purple);
                        break;
                    default:
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                        break;
                }
            }
        }

        private void lstBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var book = (Book)lstBooks.SelectedItem;
            int bookId = book.BookId;
            NavigationService.Navigate(new Uri("/BookPage.xaml#" + bookId, UriKind.Relative));
        }

        private void AddBookButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/BookPage.xaml", UriKind.Relative));
        }

        private void DeleteAuthorButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteAuthorQuestion,
                AppResources.DeleteAuthor, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    var bookAuthors = context.BookAuthors.Where(j => j.AuthorId.Equals(authorId)).ToList() as List<BookAuthor>;
                    foreach (var item in bookAuthors)
                    {
                        var book = context.Books.Where(j => j.BookId.Equals(item.BookId)).Single() as Book;
                        var bookAuthors2 = context.BookAuthors.Where(j => j.BookId.Equals(bookId)).ToList() as List<BookAuthor>;
                        context.BookAuthors.DeleteAllOnSubmit(bookAuthors2);
                        context.Books.DeleteOnSubmit(book);
                    }

                    var categoryAuthors = context.CategoryAuthors.Where(j => j.AuthorId.Equals(authorId)).ToList() as List<CategoryAuthor>;
                    context.CategoryAuthors.DeleteAllOnSubmit(categoryAuthors);

                    var authors = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Single() as Author;
                    context.Authors.DeleteOnSubmit(authors);

                    context.SubmitChanges();

                    var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                    foreach (var item in category)
                    {
                        item.CategoryBookCount = context.Books.Where(j => j.BookCategoryId.Equals(item.CategoryId)).ToList().Count;
                        item.CategoryNameCount = item.CategoryName + " (" + item.CategoryBookCount + ")";
                        item.ModificationDate = DateTime.Now;
                        context.SubmitChanges();
                    }
                }
                MessageBox.Show(AppResources.AuthorDeleteSuccess);
                NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + categoryId, UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void lblAuthorName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            oldAuthorName = lblAuthorName.Text;
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupAddChange control = new PopupAddChange();
            control.txtLabel.Text = AppResources.EnterAuthorName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtName.Text = lblAuthorName.Text;
            control.txtName.Focus();
            control.txtName.Select(0, control.txtName.Text.Length);

            control.btnOK.Click += (s, args) =>
            {
                bool isCreated;
                string authorName;
                popup.IsOpen = false;
                int length = control.txtName.Text.Length;
                string space = control.txtName.Text.Substring(length - Math.Min(1, length));
                if (space == " ")
                {
                    authorName = control.txtName.Text.Remove(length - 1, 1);
                }
                else
                {
                    authorName = control.txtName.Text;
                }

                if (authorName != lblAuthorName.Text)
                {
                    // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                    // kontrol eden bir kod bölümü
                    using (var contextFolder = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                    {
                        isCreated =
                            contextFolder.Authors.Any(j => j.AuthorName.Equals(authorName));
                    }
                    if (isCreated == true)
                    {
                        MessageBox.Show(AppResources.AuthorExists);
                    }
                    // eğer bu isimde bir klasör oluşturulmamışsa
                    // oluşturulması için gerekli kodlar aşağıdadır
                    else
                    {
                        using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                        {

                            // buraya kitapla ilgili bilginin güncelleneceği kod da eklenecek

                            var author = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Select(j => j);
                            foreach (var item in author)
                            {
                                item.AuthorName = authorName;
                                item.ModificationDate = DateTime.Now;
                                item.AuthorNameCount = authorName + " (" + item.AuthorBookCount.ToString() + ")";
                            }
                            context.SubmitChanges();

                            var bookAuthors = context.BookAuthors.Where(j => j.AuthorId.Equals(authorId)).Select(j => j);
                            foreach (var item in bookAuthors)
                            {
                                var book = context.Books.Where(j => j.BookId.Equals(item.BookId)).Select(j => j);
                                foreach (var item2 in book)
                                {
                                    item2.BookAuthorName = item2.BookAuthorName.Replace(oldAuthorName, authorName);
                                    //item2.BookInformation = item2.BookInformation.Replace(oldAuthorName, authorName);
                                    item2.ModificationDate = DateTime.Now;
                                    context.SubmitChanges();
                                }
                            }

                            //lstFolders.ItemsSource = context.NoteFolders;
                            //lstAuthors.ItemsSource = context.Categories;
                            MessageBox.Show(AppResources.AuthorNameChangeSuccess);
                            popup.IsOpen = false;
                            CategoryAuthor categoryAuthor = context.CategoryAuthors.Where(j => j.AuthorId.Equals(authorId) && j.CategoryId.Equals(categoryId)).Single() as CategoryAuthor;
                            Author author2 = context.Authors.Where(j => j.AuthorName.Equals(authorName) && j.AuthorId.Equals(categoryAuthor.AuthorId)).Single() as Author;
                            NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + categoryAuthor.AuthorId, UriKind.Relative));
                        }
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + categoryId, UriKind.Relative));
            }
        }

        private void AuthorSettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AuthorSettingsPage.xaml#" + authorId, UriKind.Relative));
        }

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddCategoryPage.xaml#" + authorId, UriKind.Relative));
        }

    }
}