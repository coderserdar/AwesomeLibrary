using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeLibrary.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeLibrary
{
    public partial class BackgroundColorSettingsPage : PhoneApplicationPage
    {
        public int authorId;

        public BackgroundColorSettingsPage()
        {
            InitializeComponent();

            lstBackgroundColor.Items.Clear();
            lstBackgroundColor.Items.Add(AppResources.Black);
            lstBackgroundColor.Items.Add(AppResources.Blue);
            lstBackgroundColor.Items.Add(AppResources.Brown);
            lstBackgroundColor.Items.Add(AppResources.Gray);
            lstBackgroundColor.Items.Add(AppResources.Green);
            lstBackgroundColor.Items.Add(AppResources.Orange);
            lstBackgroundColor.Items.Add(AppResources.Purple);
            lstBackgroundColor.Items.Add(AppResources.Red);
            lstBackgroundColor.Items.Add(AppResources.Yellow);
            lstBackgroundColor.SelectedIndex = -1;

            lblBackgroundColor.Text = AppResources.SelectBackgroundColor;
            lblGeneralSettings.Text = AppResources.GeneralSettings;

            SetBackgroundColor();
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
            //SetBackgroundColor();
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
            authorId = int.Parse(e.Fragment);
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var author = context.Authors.Where(j => j.AuthorId.Equals(authorId)).Single() as Author;
                lblGeneralSettings.Text = AppResources.GeneralSettings;
                lblBackgroundColor.Text = AppResources.SelectFontSize;
            }
        }

        private void lstBackgroundColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstBackgroundColor.SelectedIndex;
            string backgroundColor = "";
            if (index == 0)
            {
                backgroundColor = "BLA";
            }
            else if (index == 1)
            {
                backgroundColor = "BLU";
            }
            else if (index == 2)
            {
                backgroundColor = "BRO";
            }
            else if (index == 3)
            {
                backgroundColor = "GRA";
            }
            else if (index == 4)
            {
                backgroundColor = "GRE";
            }
            else if (index == 5)
            {
                backgroundColor = "ORA";
            }
            else if (index == 6)
            {
                backgroundColor = "PUR";
            }
            else if (index == 7)
            {
                backgroundColor = "RED";
            }
            else if (index == 8)
            {
                backgroundColor = "YEL";
            }
            else
            {
                backgroundColor = "BLA";
            }

            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppBackgroundColor = backgroundColor;
                }
                context.SubmitChanges();
                //CustomMessageBox messageBox = new CustomMessageBox()
                //{
                //    Caption = AppResources.BackgroundColor,
                //    Message = AppResources.SuccessfulBackgroundColorChanged,
                //    Background = messageBackGround
                //};
                //messageBox.Show();
                MessageBox.Show(AppResources.BackgroundColorChangeSuccess);
            }
            SetBackgroundColor();
            NavigationService.Navigate(new Uri("/GeneralSettingsPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/GeneralSettingsPage.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //SetBackgroundColor();
        }
    }
}