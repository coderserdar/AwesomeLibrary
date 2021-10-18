using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeLibrary.Resources;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Marketplace;

namespace AwesomeLibrary
{
    public partial class AuthorSettingsPage : PhoneApplicationPage
    {
        public int authorId;
        public int categoryId;

        public AuthorSettingsPage()
        {
            InitializeComponent();
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First();
                lblFontFamily.Text = AppResources.FontFamily + " (" + AppResources.Selected + ": " + appSettings.FontFamily + ")";
                lblFontSize.Text = AppResources.FontSize + " (" + AppResources.Selected + ": " + appSettings.FontSize + ")";
            }

            pvAuthorSettings.Title = AppResources.AuthorSettings;
            piFont.Header = AppResources.Font;
            piOtherSettings.Header = AppResources.OtherSettings;

            btnFontFamily.Content = AppResources.Select;
            btnFontSize.Content = AppResources.Select;
            btnBookOrder.Content = AppResources.Select;
            btnBookOrderStyle.Content = AppResources.Select;
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
            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var author = context.Authors.Where(j => j.AuthorId.Equals(e.Fragment)).Single() as Author;
                authorId = author.AuthorId;
                var appSettings = context.AppSettings.First();
                categoryId = appSettings.CurrentCategoryNumber;
                string orderStyle = author.BookOrderStyle;

                if (author.BookOrderBy == "NAME")
                {
                    lblBookOrder.Text = AppResources.BookOrderBy + " (" + AppResources.Selected + ": " + AppResources.Name + ")";
                }
                if (author.BookOrderBy == "CDATE")
                {
                    lblBookOrder.Text = AppResources.BookOrderBy + " (" + AppResources.Selected + ": " + AppResources.CreationDate + ")";
                }
                if (author.BookOrderBy == "MDATE")
                {
                    lblBookOrder.Text = AppResources.BookOrderBy + " (" + AppResources.Selected + ": " + AppResources.ModificationDate + ")";
                }
                if (author.BookOrderBy == "SDATE")
                {
                    lblBookOrder.Text = AppResources.BookOrderBy + " (" + AppResources.Selected + ": " + AppResources.StartDate + ")";
                }
                if (author.BookOrderBy == "FDATE")
                {
                    lblBookOrder.Text = AppResources.BookOrderBy + " (" + AppResources.Selected + ": " + AppResources.FinishDate + ")";
                }
                if (author.BookOrderBy == "RATING")
                {
                    lblBookOrder.Text = AppResources.BookOrderBy + " (" + AppResources.Selected + ": " + AppResources.BookRating + ")";
                }
                if (author.BookOrderStyle == "A")
                {
                    lblBookOrderStyle.Text = AppResources.BookOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Ascending + ")";
                }
                if (author.BookOrderStyle == "D")
                {
                    lblBookOrderStyle.Text = AppResources.BookOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Descending + ")";
                }
                //lstNoteList.DisplayMemberPath = "NameCreation";
                SetBackgroundColor();
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

        private void btnBookOrder_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderSettingsPage.xaml#" + authorId, UriKind.Relative));
        }

        private void btnBookOrderStyle_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderStyleSettingsPage.xaml#" + authorId, UriKind.Relative));
        }

        private void btnFontSize_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FontSizeSettingsPage.xaml#" + authorId, UriKind.Relative));
        }

        private void btnFontFamily_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FontFamilySettingsPage.xaml#" + authorId, UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //pvAuthorSettings.Title = AppResources.AuthorSettings;
            //piFont.Header = AppResources.Font;
            //piOtherSettings.Header = AppResources.OtherSettings;

            //btnFontFamily.Content = AppResources.Select;
            //btnFontSize.Content = AppResources.Select;
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + authorId, UriKind.Relative));
            }
        }
    }
}