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
    public partial class MainPage : PhoneApplicationPage
    {
        public Popup popup;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Add.png", UriKind.Relative);
            button1.Text = AppResources.AddCategory;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddCategoryButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/Search.png", UriKind.Relative);
            button2.Text = AppResources.Search;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(SearchButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/Settings.png", UriKind.Relative);
            button3.Text = AppResources.Settings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(SettingsButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/Statistics.png", UriKind.Relative);
            button4.Text = AppResources.Statistics;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(StatisticsButton_Click);

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = AppResources.About;
            ApplicationBar.MenuItems.Add(menuItem1);
            menuItem1.Click += new EventHandler(AboutMenuItem_Click);

            lblCategories.Text = AppResources.Categories;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

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

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (MessageBox.Show(AppResources.ExitAppQuestion,
                AppResources.ExitApp, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Terminate();
            }
        }

        private void lstCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var category = (Category)lstCategories.SelectedItem;
            int categoryId = category.CategoryId;
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.CurrentCategoryNumber = categoryId;
                }
                context.SubmitChanges();
            }
            NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + categoryId, UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First() as AppSettings;
                string orderStyle = appSettings.CategoryOrderStyle;
                //lstCategories.ItemsSource = null;
                lstCategories.Items.Clear();
                if (context.Categories.Count() != 0)
                {
                    switch (appSettings.CategoryOrderBy)
                    {
                        case "NAME":
                            if (orderStyle == "A")
                            {
                                lstCategories.ItemsSource = context.Categories.OrderBy(j => j.CategoryName).ToList();
                            }
                            else
                            {
                                lstCategories.ItemsSource = context.Categories.OrderByDescending(j => j.CategoryName).ToList();
                            }
                            break;
                        case "CDATE":
                            if (orderStyle == "A")
                            {
                                lstCategories.ItemsSource = context.Categories.OrderBy(j => j.CreationDate).ToList();
                            }
                            else
                            {
                                lstCategories.ItemsSource = context.Categories.OrderByDescending(j => j.CreationDate).ToList();
                            }
                            break;
                        case "MDATE":
                            if (orderStyle == "A")
                            {
                                lstCategories.ItemsSource = context.Categories.OrderBy(j => j.ModificationDate).ToList();
                            }
                            else
                            {
                                lstCategories.ItemsSource = context.Categories.OrderByDescending(j => j.ModificationDate).ToList();
                            }
                            break;
                        case "BOOKCOUNT":
                            if (orderStyle == "A")
                            {
                                lstCategories.ItemsSource = context.Categories.OrderBy(j => j.CategoryBookCount).ToList();
                            }
                            else
                            {
                                lstCategories.ItemsSource = context.Categories.OrderByDescending(j => j.CategoryBookCount).ToList();
                            }
                            break;
                        default:
                            if (orderStyle == "A")
                            {
                                lstCategories.ItemsSource = context.Categories.OrderBy(j => j.CategoryName).ToList();
                            }
                            else
                            {
                                lstCategories.ItemsSource = context.Categories.OrderByDescending(j => j.CategoryName).ToList();
                            }
                            break;
                    }
                    lstCategories.DisplayMemberPath = "CategoryNameCount";
                    SetBackgroundColor();
                }
            }

            // Sample code for building a localized ApplicationBar
            //private void BuildLocalizedApplicationBar()
            //{
            //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
            //    ApplicationBar = new ApplicationBar();

            //    // Create a new button and set the text value to the localized string from AppResources.
            //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            //    appBarButton.Text = AppResources.AppBarButtonText;
            //    ApplicationBar.Buttons.Add(appBarButton);

            //    // Create a new menu item with the localized string from AppResources.
            //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //    ApplicationBar.MenuItems.Add(appBarMenuItem);
            //}
        }

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            int categoryId;

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
            control.txtName.Focus();

            control.btnOK.Click += (s, args) =>
            {
                bool folder = false;
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

                // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                // kontrol eden bir kod bölümü
                using (var contextFolder = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    folder =
                        contextFolder.Categories.Any(j => j.CategoryName.Equals(categoryName));
                }
                if (folder == true)
                {
                    MessageBox.Show(AppResources.CategoryExists);
                }
                // eğer bu isimde bir klasör oluşturulmamışsa
                // oluşturulması için gerekli kodlar aşağıdadır
                else
                {
                    using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                    {
                        //noteFolderId = context.NoteFolders.Count() + 1;
                        Category category = new Category();
                        category.CategoryName = categoryName;
                        //noteFolder.NoteFolderId = noteFolderId;
                        category.CategoryBookCount = 0;
                        category.CreationDate = DateTime.Now;
                        category.ModificationDate = DateTime.Now;
                        category.AuthorOrderBy = "NAME";
                        category.AuthorOrderStyle = "A";
                        category.CategoryNameCount = category.CategoryName + " (" + category.CategoryBookCount.ToString() + ")";

                        context.Categories.InsertOnSubmit(category);
                        context.SubmitChanges();

                        //lstCategories.ItemsSource = context.Categories;
                        MessageBox.Show(AppResources.CategoryAddSuccess);
                        popup.IsOpen = false;
                        Category category2 = context.Categories.Where(j => j.CategoryName.Equals(categoryName)).Single() as Category;
                        NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + category2.CategoryId, UriKind.Relative));
                    }

                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };

            //PhoneApplicationPage_Loaded(this, new RoutedEventArgs());
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GeneralSettingsPage.xaml", UriKind.Relative));
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
        private void StatisticsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/StatisticsPage.xaml", UriKind.Relative));
        }

    }
}