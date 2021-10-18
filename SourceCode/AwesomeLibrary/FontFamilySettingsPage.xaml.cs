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
    public partial class FontFamilySettingsPage : PhoneApplicationPage
    {
        public int authorId;
        public FontFamilySettingsPage()
        {
            InitializeComponent();

            lstFontFamily.Items.Clear();
            lstFontFamily.Items.Add("Arial");
            lstFontFamily.Items.Add("Arial Black");
            lstFontFamily.Items.Add("Baskerville Old Face");
            lstFontFamily.Items.Add("Berlin Sans FB");
            lstFontFamily.Items.Add("Bookman Old Style");
            lstFontFamily.Items.Add("Calibri");
            lstFontFamily.Items.Add("Cambria");
            lstFontFamily.Items.Add("Candara");
            lstFontFamily.Items.Add("Comic Sans MS");
            lstFontFamily.Items.Add("Consolas");
            lstFontFamily.Items.Add("Constantia");
            lstFontFamily.Items.Add("Courier New");
            lstFontFamily.Items.Add("DokChampa");
            lstFontFamily.Items.Add("Ebrima");
            lstFontFamily.Items.Add("Georgia");
            lstFontFamily.Items.Add("Lucida Sans Unicode");
            lstFontFamily.Items.Add("Meiryo UI");
            lstFontFamily.Items.Add("Microsoft YaHei");
            lstFontFamily.Items.Add("Malgun Gothic");
            lstFontFamily.Items.Add("Segoe UI");
            lstFontFamily.Items.Add("Segoe WP");
            lstFontFamily.Items.Add("Tahoma");
            lstFontFamily.Items.Add("Trebuchet MS");
            lstFontFamily.Items.Add("Times New Roman");
            lstFontFamily.Items.Add("Verdana");
            lstFontFamily.SelectedIndex = -1;
        }

        private void lstFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFontFamily.SelectedIndex != -1)
            {
                using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                {
                    var appSettings = context.AppSettings;
                    foreach (var item in appSettings)
                    {
                        item.FontFamily = lstFontFamily.SelectedItem.ToString();
                    }
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.FontFamilyChangeSuccess);
                }
            }
            NavigationService.Navigate(new Uri("/AuthorSettingsPage.xaml#" + authorId, UriKind.Relative));
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
                lblAuthorName.Text = author.AuthorName;
                lblFontFamily.Text = AppResources.SelectFontFamily;
            }
            SetBackgroundColor();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/AuthorSettingsPage.xaml#" + authorId, UriKind.Relative));
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
    }
}