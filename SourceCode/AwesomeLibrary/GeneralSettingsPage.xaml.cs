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

namespace AwesomeLibrary
{
    public partial class GeneralSettingsPage : PhoneApplicationPage
    {

        private static readonly string[] scopes = new string[] { "wl.signin", "wl.basic", "wl.offline_access", "wl.skydrive", "wl.skydrive_update" };

        /// <summary>
        ///     Stores the LiveAuthClient instance.
        /// </summary>
        private LiveAuthClient authClient;

        /// <summary>
        ///     Stores the LiveConnectClient instance.
        /// </summary>
        private LiveConnectClient liveClient;

        public int signIn;

        public GeneralSettingsPage()
        {
            InitializeComponent();
            InitializePage();

            pvGeneralSettings.Title = AppResources.GeneralSettings;

            piLanguage.Header = AppResources.Language;
            piSync.Header = AppResources.Sync;
            piOtherSettings.Header = AppResources.OtherSettings;
            piBackground.Header = AppResources.Background;

            //lblOneDrive.Text = AppResources.OneDrive;

            btnCategoryOrder.Content = AppResources.Select;
            btnCategoryOrderStyle.Content = AppResources.Select;
            btnLanguage.Content = AppResources.Select;
            btnBackgroundColor.Content = AppResources.Select;
            //btnOneDrive.Content = AppResources.Login;
            //btnOneDrive.SignInText = AppResources.SignIn;
            //btnOneDrive.SignOutText = AppResources.SignOut;
            btnOneDriveSync.Content = AppResources.Sync;
            lblOneDrive.Text = AppResources.OneDrive;
            txtSyncronizing.Text = AppResources.Synchronizing;

            pbSync.Visibility = Visibility.Collapsed;
            txtSyncronizing.Visibility = Visibility.Collapsed;
            txtSyncronizing.BorderBrush = this.LayoutRoot.Background;

            btnRemoveBackgroundImage.Content = AppResources.RemoveBackgroundImage;
            lblBackgroundImage.Text = AppResources.BackgroundImage;
            btnBackgroundImage.Content = AppResources.Select;
            btnResetSettings.Content = AppResources.ResetSettings;

            btnOneDriveSync.IsEnabled = false;
            cbSync.Content = AppResources.SyncOnOneFile;
            cbSync.IsEnabled = false;
            btnOneDrive.Content = "Sign In";

            SetBackgroundColor();

            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First() as AppSettings;
                if (appSettings.AppLangName == "EN")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.English + ")";
                }
                if (appSettings.AppLangName == "TR")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Turkish + ")";
                }
                if (appSettings.AppLangName == "DE")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.German + ")";
                }
                if (appSettings.AppLangName == "ES")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Spanish + ")";
                }

                if (appSettings.AppLangName == "PT")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Portuguese + ")";
                }
                if (appSettings.AppLangName == "AR")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Arabic + ")";
                }
                if (appSettings.AppLangName == "FA")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Persian + ")";
                }
                if (appSettings.AppLangName == "IT")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Italian + ")";
                }
                if (appSettings.AppLangName == "FR")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.French + ")";
                }
                if (appSettings.AppLangName == "RU")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Russian + ")";
                }
                if (appSettings.AppLangName == "ZH")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Chinese + ")";
                }
                if (appSettings.AppLangName == "JA")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Japanese + ")";
                }
                if (appSettings.AppLangName == "SA")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Sanskrit + ")";
                }
                if (appSettings.AppLangName == "TH")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Thai + ")";
                }


                if (appSettings.CategoryOrderBy == "NAME")
                {
                    lblCategoryOrder.Text = AppResources.CategoryOrderBy + " (" + AppResources.Selected + ": " + AppResources.Name + ")";
                }
                if (appSettings.CategoryOrderBy == "CDATE")
                {
                    lblCategoryOrder.Text = AppResources.CategoryOrderBy + " (" + AppResources.Selected + ": " + AppResources.CreationDate + ")";
                }
                if (appSettings.CategoryOrderBy == "MDATE")
                {
                    lblCategoryOrder.Text = AppResources.CategoryOrderBy + " (" + AppResources.Selected + ": " + AppResources.ModificationDate + ")";
                }
                if (appSettings.CategoryOrderBy == "BOOKCOUNT")
                {
                    lblCategoryOrder.Text = AppResources.CategoryOrderBy + " (" + AppResources.Selected + ": " + AppResources.BookCount + ")";
                }
                if (appSettings.CategoryOrderStyle == "A")
                {
                    lblCategoryOrderStyle.Text = AppResources.CategoryOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Ascending + ")";
                }
                if (appSettings.CategoryOrderStyle == "D")
                {
                    lblCategoryOrderStyle.Text = AppResources.CategoryOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Descending + ")";
                }
                if (appSettings.AppBackgroundColor == "BLA")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Black + ")";
                }
                if (appSettings.AppBackgroundColor == "BLU")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Blue + ")";
                }
                if (appSettings.AppBackgroundColor == "BRO")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Brown + ")";
                }
                if (appSettings.AppBackgroundColor == "RED")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Red + ")";
                }
                if (appSettings.AppBackgroundColor == "GRE")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Green + ")";
                }
                if (appSettings.AppBackgroundColor == "YEL")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Yellow + ")";
                }
                if (appSettings.AppBackgroundColor == "GRA")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Gray + ")";
                }
                if (appSettings.AppBackgroundColor == "ORA")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Orange + ")";
                }
                if (appSettings.AppBackgroundColor == "PUR")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Purple + ")";
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetBackgroundColor();
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        private async void btnOneDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.btnOneDrive.Content.ToString() == "Sign In" || this.btnOneDrive.Content.ToString() == "Sign in")
                {
                    LiveLoginResult loginResult = await this.authClient.LoginAsync(scopes);
                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        //this.btnOneDrive.Content = AppResources.SignOut;
                        this.btnOneDrive.Content = "Sign Out";

                        this.liveClient = new LiveConnectClient(loginResult.Session);
                        this.GetMe();
                        btnOneDriveSync.IsEnabled = true;
                        cbSync.IsEnabled = true;
                    }
                }
                else
                {
                    this.authClient.Logout();
                    //this.btnOneDrive.Content = AppResources.SignIn;
                    this.btnOneDrive.Content = "Sign Out";
                    btnOneDriveSync.IsEnabled = true;
                    cbSync.IsEnabled = true;
                    //this.tbResponse.Text = "";
                }
            }
            catch (LiveAuthException authExp)
            {
                //this.tbResponse.Text = authExp.ToString();
            }
        }

        private async void InitializePage()
        {
            try
            {
                // bu benim uygulamama ait bir client id
                this.authClient = new LiveAuthClient("0000000044125951");
                LiveLoginResult loginResult = await this.authClient.InitializeAsync(scopes);
                btnOneDrive.Content = "Sign In";
                if (loginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    //this.btnOneDrive.Content = AppResources.SignOut;
                    this.btnOneDrive.Content = "Sign Out";

                    this.liveClient = new LiveConnectClient(loginResult.Session);
                    //this.GetMe();
                }
            }
            catch (LiveAuthException authExp)
            {
                //this.tbResponse.Text = authExp.ToString();
            }
        }

        private async void GetMe()
        {
            try
            {
                LiveOperationResult operationResult = await this.liveClient.GetAsync("me");

                dynamic properties = operationResult.Result;
                //this.tbResponse.Text = properties.first_name + " " + properties.last_name;
            }
            catch (LiveConnectException e)
            {
                //this.tbResponse.Text = e.ToString();
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

        public async static Task<string> CreateDirectoryAsync(LiveConnectClient client,
string folderName, string parentFolder)
        {
            string folderId = null;

            // Retrieves all the directories.
            var queryFolder = parentFolder + "/files?filter=folders,albums";
            var opResult = await client.GetAsync(queryFolder);
            dynamic result = opResult.Result;

            foreach (dynamic folder in result.data)
            {
                // Checks if current folder has the passed name.
                if (folder.name.ToLowerInvariant() == folderName.ToLowerInvariant())
                {
                    folderId = folder.id;
                    break;
                }
            }

            if (folderId == null)
            {
                // Directory hasn't been found, so creates it using the PostAsync method.
                var folderData = new Dictionary<string, object>();
                folderData.Add("name", folderName);
                opResult = await client.PostAsync(parentFolder, folderData);
                result = opResult.Result;

                // Retrieves the id of the created folder.
                folderId = result.id;
            }

            return folderId;
        }

        private async void btnOneDriveSync_Click(object sender, RoutedEventArgs e)
        {

            IsolatedStorageFile myIsolatedStorage = null;
            StringBuilder sb = null;


            string folderName;
            try
            {
                //var folderData = new Dictionary<string, object>();
                folderName = "Awesome Library (" + DateTime.Now + ")";
                //folderName = folderName.Replace(":", ".");
                //folderName = folderName.Replace("/", ".");
                folderName = DesignFileName(folderName);

                string skyDriveFolder = await CreateDirectoryAsync(liveClient, folderName, "me/skydrive");

                if (cbSync.IsChecked == false)
                {

                    btnOneDrive.IsEnabled = false;
                    pbSync.Visibility = Visibility.Visible;
                    txtSyncronizing.Visibility = Visibility.Visible;

                    using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                    {
                        //var noteFolders = context.NoteFolders.ToList() as List<NoteFolder>;
                        var books = context.Books.ToList() as List<Book>;

                        for (int i = 0; i < books.Count; i++)
                        {
                            var bookAuthor =
                                context.BookAuthors.Where(j => j.BookId.Equals(books[i].BookId)).ToList() as
                                    List<BookAuthor>;

                            var category = context.Categories.Where(j => j.CategoryId.Equals(books[i].BookCategoryId)).Single() as Category;

                            List<Author> authors = new List<Author>();

                            for (int k = 0; k < bookAuthor.Count; k++)
                            {
                                authors.Add(context.Authors.Where(j => j.AuthorId.Equals(bookAuthor[k].AuthorId)).Single() as Author);
                            }


                            string fileName = Guid.NewGuid() + ". " + books[i].BookName + " (" + category.CategoryName +
                                              ").txt";

                            fileName = DesignFileName(fileName);
                            //fileName = fileName.Replace(":", ".");
                            //fileName = fileName.Replace("/", ".");
                            //StringBuilder sb = new StringBuilder();
                            //sb.AppendLine(AppResources.NoteName + ": " + notes[i].NoteName);
                            //sb.AppendLine(AppResources.FolderName + ": " + noteFolder.NoteFolderName);
                            //sb.AppendLine(AppResources.Password + ": " + noteFolder.IsPasswordProtected);
                            //sb.AppendLine(AppResources.CreationDate + ": " + notes[i].CreationDate);
                            //sb.AppendLine(AppResources.ModificationDate + ": " + notes[i].ModificationDate);
                            //sb.AppendLine(AppResources.Note + ": " + notes[i].NoteDescription);


                            myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();//deletes the file if it already exists
                            //if (myIsolatedStorage.FileExists(fileName))
                            //{
                            //myIsolatedStorage.DeleteFile(fileName);
                            //}//now we use a StreamWriter to write inputBox.Text to the file and save it to IsolatedStorage
                            using (StreamWriter writeFile = new StreamWriter
                            (new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, myIsolatedStorage)))
                            {
                                writeFile.WriteLine(AppResources.BookName + ": " + books[i].BookName);
                                writeFile.WriteLine(AppResources.CategoryName + ": " + category.CategoryName);
                                string authorNames = "";
                                for (int l = 0; l < authors.Count; l++)
                                {
                                    authorNames = authorNames + authors[l].AuthorName + ", ";
                                }
                                authorNames = authorNames.Substring(0, authorNames.Length - 2);
                                writeFile.WriteLine(AppResources.AuthorName + ": " + authorNames);
                                writeFile.WriteLine(AppResources.PageNumber + ": " + books[i].BookPageNumber);
                                writeFile.WriteLine(AppResources.PublisherName + ": " + books[i].BookPublisherName);
                                writeFile.WriteLine(AppResources.StartDate + ": " + books[i].ReadStartDate.ToShortDateString());
                                writeFile.WriteLine(AppResources.FinishDate + ": " + books[i].ReadFinishDate.ToShortDateString());
                                writeFile.WriteLine(AppResources.BookRating + ": " + books[i].BookRating + "/10");
                                writeFile.WriteLine(AppResources.BookComment + ": " + books[i].BookComment);
                                writeFile.Close();
                            }
                            IsolatedStorageFileStream isfs = myIsolatedStorage.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                            var res = await liveClient.UploadAsync(skyDriveFolder, fileName, isfs, OverwriteOption.Overwrite);
                            pbSync.Value = (i + 1) * (100) / books.Count;
                            //var res = await liveClient.UploadAsync("me/skydrive/" + folderName, fileName, isfs, OverwriteOption.Overwrite);
                        }
                    }    
                }
                else
                {
                    using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                    {
                        //var noteFolders = context.NoteFolders.ToList() as List<NoteFolder>;
                        var books = context.Books.OrderBy(j => j.ReadFinishDate).ToList() as List<Book>;
                        var bookFirst = books.First();
                        var bookLast = books.Last();

                        string fileName = Guid.NewGuid() + ". Awesome Library (" + bookFirst.ReadFinishDate.ToShortDateString() + " - " + bookLast.ReadFinishDate.ToShortDateString() + ").txt";
                        fileName = DesignFileName(fileName);

                        myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();//deletes the file if it already exists

                        sb = new StringBuilder();

                        for (int i = 0; i < books.Count; i++)
                        {
                            var bookAuthor =
                                context.BookAuthors.Where(j => j.BookId.Equals(books[i].BookId)).ToList() as
                                    List<BookAuthor>;

                            var category = context.Categories.Where(j => j.CategoryId.Equals(books[i].BookCategoryId)).Single() as Category;

                            List<Author> authors = new List<Author>();

                            for (int k = 0; k < bookAuthor.Count; k++)
                            {
                                authors.Add(context.Authors.Where(j => j.AuthorId.Equals(bookAuthor[k].AuthorId)).Single() as Author);
                            }

                            sb.AppendLine();
                            sb.AppendLine(AppResources.BookName + ": " + books[i].BookName);
                            sb.AppendLine(AppResources.CategoryName + ": " + category.CategoryName);
                            string authorNames = "";
                            for (int l = 0; l < authors.Count; l++)
                            {
                                authorNames = authorNames + authors[l].AuthorName + ", ";
                            }
                            authorNames = authorNames.Substring(0, authorNames.Length - 2);
                            sb.AppendLine(AppResources.AuthorName + ": " + authorNames);
                            sb.AppendLine(AppResources.PageNumber + ": " + books[i].BookPageNumber);
                            sb.AppendLine(AppResources.PublisherName + ": " + books[i].BookPublisherName);
                            sb.AppendLine(AppResources.StartDate + ": " + books[i].ReadStartDate.ToShortDateString());
                            sb.AppendLine(AppResources.FinishDate + ": " + books[i].ReadFinishDate.ToShortDateString());
                            sb.AppendLine(AppResources.BookRating + ": " + books[i].BookRating + "/10");
                            sb.AppendLine(AppResources.BookComment + ": " + books[i].BookComment);
                            sb.AppendLine();

                            //if (myIsolatedStorage.FileExists(fileName))
                            //{
                            //myIsolatedStorage.DeleteFile(fileName);
                            //}//now we use a StreamWriter to write inputBox.Text to the file and save it to IsolatedStorage
                            //pbSync.Value = (i + 1) * (100) / books.Count;
                            //var res = await liveClient.UploadAsync("me/skydrive/" + folderName, fileName, isfs, OverwriteOption.Overwrite);
                        }
                        using (StreamWriter writeFile = new StreamWriter
                            (new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, myIsolatedStorage)))
                        {
                            writeFile.Write(sb.ToString());
                            writeFile.Close();
                        }
                        IsolatedStorageFileStream isfs = myIsolatedStorage.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                        var res = await liveClient.UploadAsync(skyDriveFolder, fileName, isfs, OverwriteOption.Overwrite);
                    }
                }
                
                //this.infoTextBlock.Text = string.Join(" ", "Created folder:", result.name, "ID:", result.id);
                MessageBox.Show(AppResources.OneDriveSyncCompleted);

                pbSync.Visibility = Visibility.Collapsed;
                txtSyncronizing.Visibility = Visibility.Collapsed;
                pbSync.Value = 0;
                btnOneDrive.IsEnabled = true;
            }
            catch (Exception exception)
            {
                //this.infoTextBlock.Text = "Error creating folder: " + exception.Message;
                MessageBox.Show(AppResources.SystemFault);
            }
        }

        public string DesignFileName(string fileName)
        {
            fileName = fileName.Replace(":", ".");
            fileName = fileName.Replace("?", ".");
            fileName = fileName.Replace("\"", ".");
            fileName = fileName.Replace("/", ".");
            fileName = fileName.Replace("<", ".");
            fileName = fileName.Replace(">", ".");
            fileName = fileName.Replace("|", ".");
            fileName = fileName.Replace("*", ".");
            return fileName;
        }

        private void btnBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BackgroundColorSettingsPage.xaml", UriKind.Relative));
        }

        private void btnBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask objPhotoChooser = new PhotoChooserTask();
            objPhotoChooser.Completed += new EventHandler<PhotoResult>(PhotoChooseCall);
            objPhotoChooser.Show();
        }

        private void PhotoChooseCall(object sender, PhotoResult e)
        {
            switch (e.TaskResult)
            {
                case TaskResult.OK:
                    using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
                    {
                        var appSettings = context.AppSettings;
                        foreach (var appSetting in appSettings)
                        {
                            appSetting.AppBackgroundImage = new byte[e.ChosenPhoto.Length];
                            e.ChosenPhoto.Position = 0;
                            e.ChosenPhoto.Read(appSetting.AppBackgroundImage, 0, appSetting.AppBackgroundImage.Length);
                            //noteFolder.NoteFolderPassword = "";
                        }
                        context.SubmitChanges();
                        MessageBox.Show(AppResources.BackgroundImageChangeSuccess);
                    }
                    break;
                case TaskResult.Cancel:
                    //MessageBox.Show("Cancelled");
                    break;
                case TaskResult.None:
                    //MessageBox.Show("Nothing Entered");
                    break;
            }
            SetBackgroundColor();
        }

        private void btnRemoveBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppBackgroundImage = null;
                }
                context.SubmitChanges();
                MessageBox.Show(AppResources.BackgroundImageRemoveSuccess);
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            //pvGeneralSettings.Title = AppResources.GeneralSettings;

            //piLanguage.Header = AppResources.Language;
            //piSync.Header = AppResources.Sync;
            //piOtherSettings.Header = AppResources.OtherSettings;
            //piBackground.Header = AppResources.Background;

            ////lblOneDrive.Text = AppResources.OneDrive;

            //btnCategoryOrder.Content = AppResources.Select;
            //btnCategoryOrderStyle.Content = AppResources.Select;
            //btnLanguage.Content = AppResources.Select;
            //btnBackgroundColor.Content = AppResources.Select;
            ////btnOneDrive.Content = AppResources.Login;
            ////btnOneDrive.SignInText = AppResources.SignIn;
            ////btnOneDrive.SignOutText = AppResources.SignOut;
            //btnOneDriveSync.Content = AppResources.Sync;
            //lblOneDrive.Text = AppResources.OneDrive;
            //txtSyncronizing.Text = AppResources.Synchronizing;

            //pbSync.Visibility = Visibility.Collapsed;
            //txtSyncronizing.Visibility = Visibility.Collapsed;
            //txtSyncronizing.BorderBrush = this.LayoutRoot.Background;

            //btnRemoveBackgroundImage.Content = AppResources.RemoveBackgroundImage;
            //lblBackgroundImage.Text = AppResources.BackgroundImage;
            //btnBackgroundImage.Content = AppResources.Select;
            //btnResetSettings.Content = AppResources.ResetSettings;

            //btnOneDriveSync.IsEnabled = false;
            //cbSync.Content = AppResources.SyncOnOneFile;
            //cbSync.IsEnabled = false;
            //btnOneDrive.Content = "Sign In";

            //SetBackgroundColor();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void btnOneDrive_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                //the session status is connected so we need to set this session status to client
                this.liveClient = new LiveConnectClient(e.Session);
            }
            else
            {
                this.liveClient = null;
            }
        }

        private void btnLanguage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LanguageSettingsPage.xaml", UriKind.Relative));
        }

        private void btnCategoryOrder_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderSettingsPage.xaml", UriKind.Relative));
        }

        private void btnCategoryOrderStyle_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderStyleSettingsPage.xaml", UriKind.Relative));
        }

        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppBackgroundImage = null;
                    appSetting.AppBackgroundColor = "BLA";
                }
                context.SubmitChanges();
                this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                MessageBox.Show(AppResources.SuccessfulResetSettings);
            }
        }
    }
}