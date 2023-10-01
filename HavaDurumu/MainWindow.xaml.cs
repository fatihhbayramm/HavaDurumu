using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace HavaDurumu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
                                  
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Thread havaDurumuThread = new Thread(HavaDurumuGetir);
            havaDurumuThread.Start(new string[] { "istanbul", "f8f404f93665fc502d248dba623efefc" });
        }

        void HavaDurumuGetir(object obj)
        {
            //unboxing
            string[] paramatreler = obj as string[];
            string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}&mode=xml&units=metric", paramatreler[0], paramatreler[1]);
            var xmlDokuman = XDocument.Load(url).Element("current");
            //var havaDurumu = new Hava
            var havaDurumu = new

            {
                Sehir = xmlDokuman.Element ("city").Attribute("name").Value,
                Tarih=DateTime.Parse(xmlDokuman.Element("lastupdate").Attribute("value").Value).ToString("dd.MM.yyyy dddd"),
                EnYuksek=double.Parse(xmlDokuman.Element("temperature").Attribute("max").Value.Replace(".",",")),
                EnDusuk = double.Parse(xmlDokuman.Element("temperature").Attribute("min").Value.Replace(".", ",")),
                Sicaklik= double.Parse(xmlDokuman.Element("temperature").Attribute("value").Value.Replace(".", ",")),
                HissedilenSicaklik=double.Parse(xmlDokuman.Element("feels_like").Attribute("value").Value.Replace(".", ",")),
                RuzgarHizi= double.Parse(xmlDokuman.Element("wind").Element("speed").Attribute("value").Value.Replace(".", ",")),
                RuzgarYonu= double.Parse(xmlDokuman.Element("wind").Element("direction").Attribute("value").Value.Replace(".", ",")),
                Resim=new Uri("https://openweathermap.org/img/wn/"+xmlDokuman.Element("weather").Attribute("icon").Value+"@2x.png")

            };
            Thread.Sleep(5000);
            //UI Thread(main thread) üzerinde kod çalıştırmak için kullanılır.
            gridHavaDurumu.Dispatcher.BeginInvoke(new Action(() => 
            {
                gridHavaDurumu.DataContext = havaDurumu;
            }));

        }
    }
    //public class Hava
    //{
    //    public string Sehir { get; set; }
    //    public string Tarih { get; set; }
    //    public double EnYuksek { get; set; }
    //    public double EnDusuk { get; set; }
    //    public double Sicaklik { get; set; }
    //    public double HissedilenSicaklik { get; set; }
    //    public double RuzgarHizi { get; set; }
    //    public double RuzgarYonu { get; set; }
    //    public ImageSource Resim { get; set; }

    //}
public class DereceToYukseklikConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double derece=double.Parse(value.ToString());
            return derece + 99;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class YonToAciConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double yon=System.Convert.ToDouble(value);
            return -yon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BitmapImage(value as Uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
