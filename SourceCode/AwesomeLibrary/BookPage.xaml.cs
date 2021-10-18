using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using AwesomeLibrary.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeLibrary
{
    public partial class BookPage : PhoneApplicationPage
    {
        public int authorId;
        public string authorName;
        public string categoryName;
        public int categoryId;
        public int bookId;
        public string pageName;
        double InputHeight = 0.0;
        public bool flag;
        public bool isFilled;
        public double ratingValue = 0;

        public BookPage()
        {
            InitializeComponent();

            SetBackgroundColor();

            //pvAuthor.Title = authorName;
            piBookName.Header = AppResources.BookName;
            piComment.Header = AppResources.BookComment;
            piPublisherName.Header = AppResources.PublisherName;
            piRating.Header = AppResources.BookRating;
            piStartFinishDate.Header = AppResources.Date;
            lblStartDate.Text = AppResources.StartDate;
            lblFinishDate.Text = AppResources.FinishDate;
            piPageNumber.Header = AppResources.PageNumber;


            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Save.png", UriKind.Relative);
            button1.Text = AppResources.Save;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(SaveButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/SendWithMail.png", UriKind.Relative);
            button2.Text = AppResources.SendWithMail;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(SendMailButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/SendWithSMS.png", UriKind.Relative);
            button3.Text = AppResources.SendWithSMS;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(SendSMSButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/Share.png", UriKind.Relative);
            button4.Text = AppResources.ShareBook;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(ShareBookButton_Click);

            isFilled = false;

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = AppResources.DeleteBook;
            ApplicationBar.MenuItems.Add(menuItem1);
            menuItem1.Click += new EventHandler(DeleteBookMenuItem_Click);

        }

        private void SendSMSButton_Click(object sender, EventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = "";
            smsComposeTask.Body = CreateSendMaterial();

            smsComposeTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void ShareBookButton_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();

            shareStatusTask.Status = CreateSendMaterial();

            shareStatusTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void SendMailButton_Click(object sender, EventArgs e)
        {
            // burada birden fazla e-posta hesabı varsa birini seçmesi söyleniyor
            //EmailAddressChooserTask emailAddressChooserTask;
            //emailAddressChooserTask = new EmailAddressChooserTask();
            //emailAddressChooserTask.Completed += new EventHandler<EmailResult>(emailAddressChooserTask_Completed);
            //emailAddressChooserTask.Show();

            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = txtBookName.Text;
            emailComposeTask.Body = CreateSendMaterial();
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";

            emailComposeTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithMail);
        }

        private string CreateSendMaterial()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(AppResources.BookName + ": " + txtBookName.Text);
            sb.AppendLine(AppResources.CategoryName + ": " + categoryName);
            sb.AppendLine(AppResources.AuthorName + ": " + authorName);
            sb.AppendLine(AppResources.PageNumber + ": " + txtPageNumber.Text);
            sb.AppendLine(AppResources.PublisherName + ": " + txtPublisherName.Text);
            sb.AppendLine(AppResources.StartDate + ": " + DateTime.Parse(dtStart.Value.ToString()).ToShortDateString());
            sb.AppendLine(AppResources.FinishDate + ": " + DateTime.Parse(dtFinish.Value.ToString()).ToShortDateString());
            sb.AppendLine(AppResources.BookComment + ": " + txtBookComment.Text);
            sb.AppendLine(AppResources.BookRating + ": " + rtRating.Value.ToString() + "/10");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(AppResources.SendWithApp);
            return sb.ToString();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //SetBackgroundColor();

            // yazarın adı sayfanın en üstünde görünsün diye yapılıyor bu
            //pvAuthor.Title = authorName;

            //pvAuthor.Title = authorName;
            //piBookName.Header = AppResources.BookName;
            //piComment.Header = AppResources.BookComment;
            //piPublisherName.Header = AppResources.PublisherName;
            //piRating.Header = AppResources.BookRating;
            //piStartFinishDate.Header = AppResources.Date;
            //lblStartDate.Text = AppResources.StartDate;
            //lblFinishDate.Text = AppResources.FinishDate;
            //piPageNumber.Header = AppResources.PageNumber;
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
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First();
                categoryId = appSettings.CurrentCategoryNumber;
                authorId = appSettings.CurrentAuthorNumber;

                // sayfanın font ayarları için yapılan bir değişiklik
                FontFamily temp = new FontFamily(appSettings.FontFamily);
                double fontsize = double.Parse(appSettings.FontSize);
                txtBookComment.FontFamily = temp;
                txtBookComment.FontSize = fontsize;
                txtPageNumber.FontFamily = temp;
                txtPageNumber.FontSize = fontsize;
                txtBookName.FontFamily = temp;
                txtBookName.FontSize = fontsize;
                txtBookComment.FontFamily = temp;
                txtBookComment.FontSize = fontsize;
                txtPublisherName.FontFamily = temp;
                txtPublisherName.FontSize = fontsize;
                rtRating.Value = 5;

                var author = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Single() as Author;
                authorName = author.AuthorName;

                var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Single() as Category;
                categoryName = category.CategoryName;
            }

            var lastPage = NavigationService.BackStack.FirstOrDefault();
            pageName = lastPage.Source.ToString();
            pvAuthor.SelectedIndex = 0;
            txtBookName.Focus();
            // yazarın adı sayfanın en üstünde görünsün diye yapılıyor bu
            pvAuthor.Title = authorName;
            SetBackgroundColor();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            bookId = int.Parse(e.Fragment);
            if (pageName.Contains("/AuthorPage.xaml"))
            {
                isFilled = true;
            }
            else
            {
                //using (var context2 = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                //{
                //    var appSettings = context2.AppSettings; ;
                //    var book2 = context2.Books.Where(j => j.BookId.Equals(bookId)) as Book;
                //    var bookAuthor = context2.BookAuthors.Where(j => j.BookId.Equals(bookId)).ToList() as List<BookAuthor>;
                //    var bAuthor = bookAuthor.First();
                //    var author = context2.Authors.Where(j => j.AuthorId.Equals(bAuthor.AuthorId)) as Author;
                //    foreach (var item in appSettings)
                //    {
                //        item.CurrentAuthorNumber = author.AuthorId;
                //        item.CurrentCategoryNumber = book2.BookCategoryId;
                //    }
                //    context2.SubmitChanges();
                //    pvAuthor.Title = author.AuthorName;
                //}
            }
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var book = context.Books.Where(j => j.BookId.Equals(e.Fragment)).Single() as Book;

                txtBookName.Text = book.BookName == "" ? "" : book.BookName;
                txtPageNumber.Text = book.BookPageNumber.ToString() == "" ? "" : book.BookPageNumber.ToString();
                txtPublisherName.Text = book.BookPublisherName == "" ? "" : book.BookPublisherName;
                dtStart.Value = book.ReadStartDate == null ? DateTime.Now : book.ReadStartDate;
                dtFinish.Value = book.ReadFinishDate == null ? DateTime.Now : book.ReadFinishDate;
                rtRating.Value = book.BookRating == null ? 0 : book.BookRating;
                txtBookComment.Text = book.BookComment == "" ? "" : book.BookComment;
            }

            SetBackgroundColor();
            pvAuthor.SelectedIndex = 0;
            //pvAuthor.Name = authorName;
            txtBookName.Focus();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //SaveButton_Click(this, new EventArgs());
            if (pageName.Contains("/SearchPage.xaml"))
            {
                //this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + authorId, UriKind.Relative));
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            txtBookComment_LostFocus(this, new RoutedEventArgs());
            this.pnlKeyboardPlaceHolder.Visibility = Visibility.Collapsed;
            if (txtBookName.Text.Trim().Length < 1)
            {
                MessageBox.Show(AppResources.BookNameMustBe);
            }
            else
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    if (isFilled || pageName.Contains("/SearchPage.xaml"))
                    {
                        var book = context.Books.Where(j => j.BookId.Equals(bookId)).Select(j => j);
                        foreach (var item in book)
                        {
                            item.BookCategoryId = categoryId;
                            item.BookName = txtBookName.Text == "" ? "" : txtBookName.Text;
                            item.BookPageNumber = txtPageNumber.Text == "" ? 0 : int.Parse(txtPageNumber.Text);
                            item.BookPublisherName = txtPublisherName.Text == "" ? "" : txtPublisherName.Text;
                            item.ReadStartDate = DateTime.Parse(dtStart.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtStart.Value.ToString());
                            item.ReadFinishDate = DateTime.Parse(dtFinish.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtFinish.Value.ToString()); ;
                            //item.BookRating = rtRating.Value.ToString() == "" ? 0 : int.Parse(rtRating.Value.ToString());
                            item.BookRating = int.Parse(ratingValue.ToString()) == 0 ? 0 : int.Parse(ratingValue.ToString());
                            item.BookComment = txtBookComment.Text == "" ? "" : txtBookComment.Text;
                            item.ModificationDate = DateTime.Now;

                            item.BookCategoryName = categoryName;
                            item.BookAuthorName = authorName;

                            //item.BookInformation = categoryName + " " + authorName + " " + txtBookName.Text + " " + txtPublisherName.Text + " " + txtBookComment.Text;
                            item.BookNameRating = item.BookName + " (" + item.BookRating + "/10)";
                        }
                        context.SubmitChanges();
                    }
                    else
                    {
                        Book book = new Book();
                        book.BookCategoryId = categoryId;
                        book.BookGuid = Guid.NewGuid().ToString();
                        book.BookName = txtBookName.Text == "" ? "" : txtBookName.Text;
                        book.BookPageNumber = txtPageNumber.Text == "" ? 0 : int.Parse(txtPageNumber.Text);
                        book.BookPublisherName = txtPublisherName.Text == "" ? "" : txtPublisherName.Text;
                        book.ReadStartDate = DateTime.Parse(dtStart.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtStart.Value.ToString());
                        book.ReadFinishDate = DateTime.Parse(dtFinish.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtFinish.Value.ToString()); ;
                        //book.BookRating = rtRating.Value.ToString() == "" ? 0 : int.Parse(rtRating.Value.ToString());
                        book.BookRating = int.Parse(ratingValue.ToString()) == 0 ? 0 : int.Parse(ratingValue.ToString());
                        book.BookComment = txtBookComment.Text == "" ? "" : txtBookComment.Text;
                        book.ModificationDate = DateTime.Now;

                        book.BookCategoryName = categoryName;
                        book.BookAuthorName = authorName;

                        //book.BookInformation = categoryName + " " + authorName + " " + book.BookName + " " + book.BookPublisherName + " " + book.BookComment;
                        book.BookNameRating = book.BookName + " (" + book.BookRating + "/10)";
                        book.CreationDate = DateTime.Now;
                        context.Books.InsertOnSubmit(book);
                        context.SubmitChanges();

                        Book book2 = context.Books.Where(j => j.BookGuid.Equals(book.BookGuid)).Single() as Book;

                        BookAuthor bookAuthor = new BookAuthor();
                        bookAuthor.AuthorId = authorId;
                        bookAuthor.BookId = book2.BookId;
                        context.BookAuthors.InsertOnSubmit(bookAuthor);
                        context.SubmitChanges();

                        var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                        foreach (var item in category)
                        {
                            item.CategoryBookCount = item.CategoryBookCount + 1;
                            item.CategoryNameCount = item.CategoryName + " (" + item.CategoryBookCount + ")";
                            item.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();

                        var author = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Select(j => j);
                        foreach (var item in author)
                        {
                            item.AuthorBookCount = item.AuthorBookCount + 1;
                            item.AuthorNameCount = item.AuthorName + " (" + item.AuthorBookCount + ")";
                            item.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();
                    }
                }
                MessageBox.Show(AppResources.BookSaveSuccess);
            }
            isFilled = false;
        }

        private void txtBookComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                double CurrentInputHeight = txtBookComment.ActualHeight;

                if (CurrentInputHeight > InputHeight)
                {
                    svBookComment.ScrollToVerticalOffset(svBookComment.VerticalOffset + CurrentInputHeight - InputHeight);
                }

                InputHeight = CurrentInputHeight;
            });
        }

        private void txtBookComment_GotFocus(object sender, RoutedEventArgs e)
        {
            App.RootFrame.RenderTransform = new CompositeTransform();
            flag = true;
        }

        private void txtBookComment_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtBookComment.Focus();
            //txtBookComment.Select(txtBookComment.Text.Length, 1);
            svBookComment.ScrollToVerticalOffset(e.GetPosition(txtBookComment).Y - 80);
        }

        private void txtBookComment_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!flag) return;
            txtBookComment.Focus();
            flag = false;
            this.pnlKeyboardPlaceHolder.Visibility = Visibility.Collapsed;
        }

        private void txtBookComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                svBookComment.ScrollToVerticalOffset(txtBookComment.ActualHeight);
            }
        }

        private void svBookComment_GotFocus(object sender, RoutedEventArgs e)
        {
            this.svBookComment.ScrollToVerticalOffset(this.txtBookComment.ActualHeight);
            this.svBookComment.UpdateLayout();
        }

        private void txtBookName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvAuthor.SelectedIndex = 1;
                txtPageNumber.Focus();
            }
        }

        private void txtPageNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvAuthor.SelectedIndex = 2;
                txtPublisherName.Focus();
            }
        }

        private void txtPublisherName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvAuthor.SelectedIndex = 3;
                dtStart.Focus();
            }
        }

        private void dtStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvAuthor.SelectedIndex = 3;
                dtFinish.Focus();
            }
        }

        private void dtFinish_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvAuthor.SelectedIndex = 4;
                rtRating.Focus();
            }
        }

        private void rtRating_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    pvAuthor.SelectedIndex = 5;
            //    txtBookComment.Focus();
            //}
        }

        private void rtRating_ValueChanged(object sender, EventArgs e)
        {
            //pvAuthor.SelectedIndex = 5;
            ratingValue = rtRating.Value;
            //txtBookComment.Focus();
        }

        private void dtFinish_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (isFilled)
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    var book = context.Books.Where(j => j.BookId.Equals(bookId)).Select(j => j);
                    foreach (var item in book)
                    {
                        item.ReadFinishDate = DateTime.Parse(dtFinish.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtFinish.Value.ToString());
                        item.ModificationDate = DateTime.Now;
                    }
                    context.SubmitChanges();
                }
            }
            pvAuthor.SelectedIndex = 4;
            rtRating.Focus();
        }

        private void dtStart_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (isFilled)
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    var book = context.Books.Where(j => j.BookId.Equals(bookId)).Select(j => j);
                    foreach (var item in book)
                    {
                        item.ReadStartDate = DateTime.Parse(dtStart.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtStart.Value.ToString());
                        item.ModificationDate = DateTime.Now;
                    }
                    context.SubmitChanges();
                }
            }
            pvAuthor.SelectedIndex = 3;
            dtFinish.Focus();
        }

        private void rtRating_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //pvAuthor.SelectedIndex = 5;
            //ratingValue = rtRating.Value;
            //txtBookComment.Focus();
        }

        private void DeleteBookMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteBookQuestion,
                AppResources.DeleteBook, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    var book = context.Books.Where(j => j.BookId.Equals(bookId)).Single() as Book;
                    var bookAuthors = context.BookAuthors.Where(j => j.BookId.Equals(bookId)).ToList() as List<BookAuthor>;
                    context.BookAuthors.DeleteAllOnSubmit(bookAuthors);
                    context.Books.DeleteOnSubmit(book);

                    var authors = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Select(j => j);
                    foreach (var item in authors)
                    {
                        item.ModificationDate = DateTime.Now;
                        item.AuthorBookCount = item.AuthorBookCount - 1;
                        item.AuthorNameCount = item.AuthorName + " (" + item.AuthorBookCount + ")";
                    }
                    context.SubmitChanges();

                    var categories = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                    foreach (var item in categories)
                    {
                        item.ModificationDate = DateTime.Now;
                        item.CategoryBookCount = item.CategoryBookCount - 1;
                        item.CategoryNameCount = item.CategoryName + " (" + item.CategoryBookCount + ")";
                    }
                    context.SubmitChanges();
                }
                MessageBox.Show(AppResources.BookDeleteSuccess);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (rtRating.Value != 10.0)
            {
                rtRating.Value = rtRating.Value + 1.0;
            }
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (rtRating.Value != 0.0)
            {
                rtRating.Value = rtRating.Value - 1.0;
            }
        }
    }
}