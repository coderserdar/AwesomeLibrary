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
    public partial class StatisticsPage : PhoneApplicationPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
            lblStatistics.Text = AppResources.Statistics;
            SetBackgroundColor();
            SetStatistic();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        private void SetStatistic()
        {
            StringBuilder sb = new StringBuilder();
            string authorName = "", categoryName = "", bookName, publisherName = "", bestBook = "", worstBook = "";
            double pagePerDay, bookPerDay;
            int bookCount;
            int pageSum = 0;
            DateTime beginDate, endDate;
            using (var context = new AwesomeLibraryDataContext(AwesomeLibraryDataContext.ConnectionString))
            {
                bookCount = context.Books.Count();

                var categories = context.Categories.OrderByDescending(j => j.CategoryBookCount).ToList() as List<Category>;
                categoryName = categories.First().CategoryNameCount;

                var authors = context.Authors.OrderByDescending(j => j.AuthorBookCount).ToList() as List<Author>;
                authorName = authors.First().AuthorNameCount;

                var books = context.Books.OrderBy(j => j.ReadStartDate).ToList() as List<Book>;
                beginDate = books.First().ReadStartDate;
                foreach (var item in books)
                {
                    pageSum = pageSum + item.BookPageNumber;
                }

                // bugünün tarihi ile ilgili olarak son tarih düzenleniyor
                endDate = DateTime.Now;

                var books3 = context.Books.GroupBy(j => j.BookPublisherName).Select(j => new { Name = j.Key, Total = j.Count()}).OrderByDescending(k => k.Total);
                publisherName = books3.First().Name + " (" + books3.First().Total + ")";

                int diffDays = (endDate.Date - beginDate.Date).Days;

                pagePerDay = pageSum / Convert.ToDouble(diffDays);
                bookPerDay = bookCount / Convert.ToDouble(diffDays);

                var books4 = context.Books.OrderByDescending(j => j.BookRating).ToList() as List<Book>;
                int bestBookRating = books4.First().BookRating;
                int worstBookRating = books4.Last().BookRating;
                // en iyi ve en kötü puana sahip birden fazla albüm varsa hepsini listelemeye yarayan
                // kod parçası buradadır.
                var bestBooks = context.Books.Where(j => j.BookRating.Equals(bestBookRating)).ToList() as List<Book>;
                if (bestBooks.Count < 2)
                {
                    bestBook = books4.First().BookNameRating;

                }
                else
                {
                    for (int i = 0; i < bestBooks.Count; i++)
                    {
                        bestBook = bestBook + bestBooks[i].BookNameRating + ", ";
                    }
                    bestBook = bestBook.Substring(0, bestBook.Length - 2);
                }

                var worstBooks = context.Books.Where(j => j.BookRating.Equals(worstBookRating)).ToList() as List<Book>;
                if (worstBooks.Count < 2)
                {
                    worstBook = books4.Last().BookNameRating;

                }
                else
                {
                    for (int i = 0; i < worstBooks.Count; i++)
                    {
                        worstBook = worstBook + worstBooks[i].BookNameRating + ", ";
                    }
                    worstBook = worstBook.Substring(0, worstBook.Length - 2);
                }

            }

            sb.AppendLine(beginDate.ToShortDateString() + " - " + endDate.ToShortDateString());
            sb.AppendLine();
            sb.AppendLine(AppResources.TotalBookCount + ": " + bookCount);
            sb.AppendLine(AppResources.MostReadCategory + ": " + categoryName);
            sb.AppendLine(AppResources.MostReadAuthor + ": " + authorName);
            sb.AppendLine(AppResources.MostReadPublisher + ": " + publisherName);
            sb.AppendLine(AppResources.BookPerDay + ": " + String.Format("{0:0.00}", bookPerDay) );
            sb.AppendLine(AppResources.PagePerDay + ": " + String.Format("{0:0.00}", pagePerDay) );
            sb.AppendLine(AppResources.BestBook + ": " + bestBook);
            sb.AppendLine(AppResources.WorstBook + ": " + worstBook);
            
            txtStatistics.Text = sb.ToString();
            txtStatistics.IsReadOnly = true;
        }
    }
}