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
    public partial class CategoryPage : PhoneApplicationPage
    {
        public Popup popup;
        public int categoryId;
        public string oldCategoryName;
        public CategoryPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Add.png", UriKind.Relative);
            button1.Text = AppResources.AddAuthor;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddAuthorButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/Delete.png", UriKind.Relative);
            button2.Text = AppResources.DeleteCategory;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(DeleteCategoryButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/Settings.png", UriKind.Relative);
            button3.Text = AppResources.CategorySettings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(CategorySettingsButton_Click);

            SetBackgroundColor();
            popup = new Popup();
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
            List<Author> authors = new List<Author>();
            List<Author> authorsOrdered = new List<Author>();

            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var category = context.Categories.Where(j => j.CategoryId.Equals(e.Fragment)).Single() as Category;
                string orderStyle = category.AuthorOrderStyle;
                var categoryAuthor = context.CategoryAuthors.Where(j => j.CategoryId.Equals(e.Fragment)).ToList() as List<CategoryAuthor>;

                foreach (var item in categoryAuthor)
                {
                    try
                    {
                        authors.Add(context.Authors.Where(j => j.AuthorId.Equals(item.AuthorId)).Single());
                    }
                    catch (Exception)
                    {
                    }

                }

                switch (category.AuthorOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            authorsOrdered = authors.OrderBy(j => j.AuthorName).ToList();
                        }
                        else
                        {
                            authorsOrdered = authors.OrderByDescending(j => j.AuthorName).ToList();
                        }
                        break;
                    case "BOOKCOUNT":
                        if (orderStyle == "A")
                        {
                            authorsOrdered = authors.OrderBy(j => j.AuthorBookCount).ToList();
                        }
                        else
                        {
                            authorsOrdered = authors.OrderByDescending(j => j.AuthorBookCount).ToList();
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            authorsOrdered = authors.OrderBy(j => j.CreationDate).ToList();
                        }
                        else
                        {
                            authorsOrdered = authors.OrderByDescending(j => j.CreationDate).ToList();
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            authorsOrdered = authors.OrderBy(j => j.ModificationDate).ToList();
                        }
                        else
                        {
                            authorsOrdered = authors.OrderByDescending(j => j.ModificationDate).ToList();
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            authorsOrdered = authors.OrderBy(j => j.AuthorName).ToList();
                        }
                        else
                        {
                            authorsOrdered = authors.OrderByDescending(j => j.AuthorName).ToList();
                        }
                        break;
                }

                lstAuthors.Items.Clear();
                categoryId = category.CategoryId;
                lblCategoryName.Text = category.CategoryName;
                lblAuthorList.Text = AppResources.AuthorList + " (" + category.CategoryName + ")";
                lstAuthors.ItemsSource = authorsOrdered;
                lstAuthors.DisplayMemberPath = "AuthorNameCount";
                SetBackgroundColor();
                //lstNoteList.DisplayMemberPath = "NameCreation";
            }
        }

        private void AddAuthorButton_Click(object sender, EventArgs e)
        {
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
            control.txtName.Focus();

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

                // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                // kontrol eden bir kod bölümü
                using (var contextAuthor = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    isCreated =
                        contextAuthor.Authors.Any(j => j.AuthorName.Equals(authorName));
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
                        Author author = new Author();
                        author.AuthorName = authorName;
                        author.CreationDate = DateTime.Now;
                        author.ModificationDate = DateTime.Now;
                        author.AuthorBookCount = 0;
                        // burada yazarın kitaplarını
                        // bitirme tarihine göre azalan bir şekilde ayarlamak için gerekli düzenleme yapılıyor
                        author.BookOrderBy = "FDATE";
                        author.BookOrderStyle = "D";
                        author.AuthorNameCount = author.AuthorName + " (" + author.AuthorBookCount + ")";
                        //note.NameDescriptionWithoutNewline = note.NameDescription.Replace(Environment.NewLine," ");
                        //note.IsPasswordProtected = false;

                        context.Authors.InsertOnSubmit(author);
                        context.SubmitChanges();

                        Author author3 = context.Authors.Where(j => j.AuthorName.Equals(authorName)).Single() as Author;

                        CategoryAuthor categoryAuthor = new CategoryAuthor();
                        categoryAuthor.CategoryId = categoryId;
                        categoryAuthor.AuthorId = author3.AuthorId;
                        context.CategoryAuthors.InsertOnSubmit(categoryAuthor);
                        context.SubmitChanges();

                        var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                        foreach (var item in category)
                        {
                            item.ModificationDate = DateTime.Now;
                            //item.CategoryNameCount = item.CategoryName + " (" + item.auth + ")";
                        }
                        context.SubmitChanges();

                        var appSettings = context.AppSettings;
                        foreach (var appSetting in appSettings)
                        {
                            appSetting.CurrentCategoryNumber = categoryId;
                        }
                        context.SubmitChanges();

                        List<Author> authors = new List<Author>();
                        var categoryAuthors = context.CategoryAuthors.Where(j => j.CategoryId.Equals(categoryId)).ToList() as List<CategoryAuthor>;
                        foreach (var item in categoryAuthors)
                        {
                            authors.Add(context.Authors.Where(j => j.AuthorId.Equals(item.AuthorId)).Single());
                        }
                        lstAuthors.ItemsSource = authors;
                        MessageBox.Show(AppResources.AuthorAddSuccess);
                        //Author author2 = context.Authors.Where(j => j.AuthorName.Equals(authorName)).Single() as Author;

                        var appSettings2 = context.AppSettings;
                        foreach (var item in appSettings2)
                        {
                            item.CurrentAuthorNumber = author3.AuthorId;
                        }
                        context.SubmitChanges();
                        NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + author3.AuthorId, UriKind.Relative));
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };

            //PhoneApplicationPage_Loaded(this, new RoutedEventArgs());
        }

        private void CategorySettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CategorySettingsPage.xaml#" + categoryId, UriKind.Relative));
        }

        private void lblCategoryName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            oldCategoryName = lblCategoryName.Text;
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupAddChange control = new PopupAddChange();
            control.txtLabel.Text = AppResources.EnterCategoryName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtName.Text = lblCategoryName.Text;
            control.txtName.Focus();
            control.txtName.Select(0, control.txtName.Text.Length);

            control.btnOK.Click += (s, args) =>
            {
                bool isCreated;
                string categoryName;
                popup.IsOpen = false;

                int length = control.txtName.Text.Length;
                string space = control.txtName.Text.Substring(length - Math.Min(1, length));
                if (space == " ")
                {
                    categoryName = control.txtName.Text.Remove(length - 1, 1);
                }
                else
                {
                    categoryName = control.txtName.Text;
                }

                if (categoryName != lblCategoryName.Text)
                {
                    // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                    // kontrol eden bir kod bölümü
                    using (var contextFolder = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                    {
                        isCreated =
                            contextFolder.Categories.Any(j => j.CategoryName.Equals(categoryName));
                    }
                    if (isCreated == true)
                    {
                        MessageBox.Show(AppResources.CategoryExists);
                    }
                    // eğer bu isimde bir klasör oluşturulmamışsa
                    // oluşturulması için gerekli kodlar aşağıdadır
                    else
                    {
                        using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                        {

                            // buraya kitapla ilgili bilginin güncelleneceği kod da eklenecek

                            var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                            foreach (var item in category)
                            {
                                item.CategoryName = categoryName;
                                item.ModificationDate = DateTime.Now;
                                item.CategoryNameCount = categoryName + " (" + item.CategoryBookCount.ToString() + ")";
                            }
                            context.SubmitChanges();

                            var book = context.Books.Where(j => j.BookCategoryId.Equals(categoryId)).Select(j => j);
                            foreach (var item in book)
                            {
                                item.BookCategoryName = item.BookCategoryName.Replace(oldCategoryName, categoryName);

                                //item.BookInformation = item.BookInformation.Replace(oldCategoryName, categoryName);
                                item.ModificationDate = DateTime.Now;
                            }
                            context.SubmitChanges();
                            //lstFolders.ItemsSource = context.NoteFolders;
                            //lstAuthors.ItemsSource = context.Categories;
                            MessageBox.Show(AppResources.CategoryNameChangeSuccess);
                            popup.IsOpen = false;
                            Category category2 = context.Categories.Where(j => j.CategoryName.Equals(categoryName)).Single() as Category;
                            NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + category2.CategoryId, UriKind.Relative));
                        }
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void DeleteCategoryButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteCategoryQuestion,
                AppResources.DeleteCategory, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    var books = context.Books.Where(j => j.BookCategoryId.Equals(categoryId)).ToList() as List<Book>;
                    foreach (var item in books)
                    {
                        var bookAuthors = context.BookAuthors.Where(j => j.BookId.Equals(item.BookId)).ToList() as List<BookAuthor>;
                        context.BookAuthors.DeleteAllOnSubmit(bookAuthors);
                    }
                    context.Books.DeleteAllOnSubmit(books);

                    var authorCategories = context.CategoryAuthors.Where(j => j.CategoryId.Equals(categoryId)).ToList() as List<CategoryAuthor>;
                    foreach (var item in authorCategories)
                    {
                        var author = context.Authors.Where(j => j.AuthorId.Equals(item.AuthorId)).ToList() as List<Author>;
                        context.Authors.DeleteAllOnSubmit(author);
                    }
                    context.CategoryAuthors.DeleteAllOnSubmit(authorCategories);

                    var categories = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Single() as Category;
                    context.Categories.DeleteOnSubmit(categories);

                    context.SubmitChanges();
                }
                MessageBox.Show(AppResources.CategoryDeleteSuccess);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void lstAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var author = (Author)lstAuthors.SelectedItem;
            int authorId = author.AuthorId;
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var item in appSettings)
                {
                    item.CurrentAuthorNumber = authorId;
                }
                context.SubmitChanges();
            }
            NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + authorId, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //SetBackgroundColor();
        }
    }
}