using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
using System.IO;
using CsvHelper;
using System.Text.RegularExpressions;

namespace OrganizationWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Organization> szervezetek = new List<Organization>();

        private void Betoltes()
        {
            szervezetek = File.ReadAllLines("organizations-100000.csv").Skip(1).Select(x => new Organization(x)).ToList();
            /*
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            var stream = new StreamReader("organizations-100000.csv");
            var reader = new CsvReader(stream,config);
            config.Delimiter = ";";
            config.HasHeaderRecord = true;
            
            szervezetek = reader.GetRecords<Organization>().ToList();
            */
        }
        public MainWindow()
        {
            InitializeComponent();
            Betoltes();
            dgAdatok.ItemsSource = szervezetek;
            cbOrszag.ItemsSource = szervezetek.Select(x => x.Country).OrderBy(x => x).Distinct().ToList();
            cbEv.ItemsSource = szervezetek.Select(x => x.Founded).OrderBy(x => x).Distinct().ToList();
            lbTotalEmpl.Content = szervezetek.Sum(x => x.EmployeesNumber);
        }

        private void cbOrszag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ujLista = szervezetek.Where(x => x.Country.ToString() == cbOrszag.SelectedItem.ToString() && x.Founded == Convert.ToInt32(cbEv.SelectedItem ??= 0)).ToList();
            dgAdatok.ItemsSource = ujLista;
            lbTotalEmpl.Content = ujLista.Sum(x => x.EmployeesNumber);
        }

        private void cbEv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ujLista = szervezetek.Where(x => x.Country.ToString() == (cbOrszag.SelectedItem ??= "").ToString() && x.Founded == Convert.ToInt32(cbEv.SelectedItem)).ToList();
            dgAdatok.ItemsSource = ujLista;
            lbTotalEmpl.Content = ujLista.Sum(x => x.EmployeesNumber);
        }
    }
}
