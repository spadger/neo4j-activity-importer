using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper.Configuration;
using DependencyImporter.Application;
using DependencyImporter.Application.Entities;
using DependencyImporter.Application.Storage;

namespace DependencyImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int progress = 50;
        private CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnDo_Click(object sender, RoutedEventArgs e)
        {
            btnDo.IsEnabled = false;
            btnCancel.IsEnabled = true;

            cts = new CancellationTokenSource();

            try
            {
                switch (cboAction.Text)
                {
                    case "Import data":
                        await Import();
                        break;
                    case "Import play set":
                        await Play();
                        break;
                    case "Test Dataset":
                        await Test();
                        break;
                    case "Delete all":
                        await Delete();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            

            btnDo.IsEnabled = true;
            btnCancel.IsEnabled = false;

            MessageBox.Show("Done");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }

        private async Task Import()
        {
            await ImportInternal(GetStorage());
        }

        private async Task Play()
        {
            var activities = new[]
            {
                new Activity("a", "a", "a", "a"),
                new Activity("b", "b", "b", "b"),
                new Activity("c", "c", "c", "c"),
                new Activity("d", "d", "d", "d"),
                new Activity("e", "e", "e", "e"),
            };

            var edgeDefinitions = new[]
            {
                "A,A,B,B,FS,0,Yes,No,0",
                "B,B,C,C,FS,0,Yes,No,0",
                "C,C,D,D,FS,0,Yes,No,0,", 
                "D,D,E,E,FS,0,Yes,No,0,",
                "A,A,E,E,FS,0,Yes,No,0"
            };

            var importer = new Importer(GetStorage());
            await Task.Run(()=>importer.Import(activities, edgeDefinitions, new Progress<ImportProgress>(), cts.Token));
        }

        private async Task Test()
        {
            await ImportInternal(new FakeStorageProvider());
        }

        private async Task Delete()
        {
            await new Importer(GetStorage()).DeleteAll();
        }

        private async Task ImportInternal(IProvideStorage storage)
        {

            var progress = new Progress<ImportProgress>();
            progress.ProgressChanged += (sender, p) =>
            {
                prgProgress.Value = p.TotalPercentage;
                txtProgress.Text = p.Message;
            };

            var importer = new Importer(storage);

            using (var reader = File.OpenText(@".\Activities.csv"))
            using(var csvReader = new CsvHelper.CsvReader(reader))
            {
                var activities = csvReader.GetRecords<Activity>().ToList();
                var edgeDefinitions = File.ReadAllLines(@".\Relationships.csv");

                await Task.Run(() => importer.Import(activities, edgeDefinitions, progress, cts.Token));
            }
        }

        private Neo4JStorageProvider GetStorage()
        {
            return new Neo4JStorageProvider(txtServer.Text, txtUsername.Text, txtPassword.Password);
        }
    }
}
