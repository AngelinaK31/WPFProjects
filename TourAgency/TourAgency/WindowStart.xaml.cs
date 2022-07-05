using System;
using System.Collections.Generic;
using System.IO;
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
using TourAgency.BD;

namespace TourAgency
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class WindowStart : Window
    {
        public WindowStart()
        {
            InitializeComponent();

            
        }

        private void ImportTours()
        {
            var fileData = File.ReadAllLines(@"D:\Экзамен\Ресурсы\Туры.txt");
            var images = Directory.GetFiles(@"D:\Экзамен\Ресурсы\Туры фото");

            foreach (var line in fileData)
            {
                var data = line.Split('\t');

                var tempTour = new Tour
                {
                    Name = data[0].Replace("\"",""),
                    TicketCount = int.Parse(data[2]),
                    Price = decimal.Parse(data[3]),
                    IsActual = (data[4] == "0") ? false : true
                };

                foreach( var tourType in data[5].Split(new string[] { ", "}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentType = TourAgencyEntities.GetContext().Types.ToList().FirstOrDefault(p => p.Name == tourType);
                    if (currentType != null)
                        tempTour.Types.Add(currentType);
                }

                try
                {
                    tempTour.ImagePreview = File.ReadAllBytes(images.FirstOrDefault(p => p.Contains(tempTour.Name)));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                TourAgencyEntities.GetContext().Tours.Add(tempTour);
                TourAgencyEntities.GetContext().SaveChanges();
            }
            
        } 
    }
}
