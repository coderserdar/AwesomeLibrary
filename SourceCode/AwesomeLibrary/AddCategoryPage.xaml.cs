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
    public partial class AddCategoryPage : PhoneApplicationPage
    {

        public int authorId;

        public AddCategoryPage()
        {
            InitializeComponent();
            SetBackgroundColor();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            authorId = int.Parse(e.Fragment);
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var author = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Single() as Author;
                lstCategories.Items.Clear();
                lblAuthorName.Text = author.AuthorName;
                lblCategories.Text = AppResources.Categories;
                var categories = context.Categories;
                lstCategories.ItemsSource = categories;
                lstCategories.DisplayMemberPath = "CategoryName";
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + authorId, UriKind.Relative));
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

        private void lstCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryAuthor categoryAuthor2 = null;
            Category category = lstCategories.SelectedItem as Category;
            CategoryAuthor categoryAuthor = new CategoryAuthor();
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                categoryAuthor.AuthorId = authorId;
                categoryAuthor.CategoryId = category.CategoryId;
                try
                {
                    categoryAuthor2 = context.CategoryAuthors.Where(j => j.CategoryId.Equals(categoryAuthor.CategoryId) && j.AuthorId.Equals(categoryAuthor.AuthorId)).Single() as CategoryAuthor;
                }
                catch (Exception)
                {
                    context.CategoryAuthors.InsertOnSubmit(categoryAuthor);
                    context.SubmitChanges();
                    var author = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Select(j => j);
                    foreach (var item in author)
                    {
                        item.ModificationDate = DateTime.Now;
                    }
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.AuthorCategoryAddSuccess);
                }
                if (categoryAuthor2 != null)
                {
                    MessageBox.Show(AppResources.AuthorAlreadySameCategory);
                }
                else
                {
                    
                }
            }
            this.NavigationService.Navigate(new Uri("/AuthorPage.xaml#" + authorId, UriKind.Relative));
        }
    }
}