using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace AdivinarPelicula
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Pelicula> peliculas;
        public MainWindow()
        {
            InitializeComponent();
            peliculas = Pelicula.GetSamples();
            ObservableCollection<string> tipoGenero = new ObservableCollection<string> { "Acción", "Drama", "Comedia", "Terror", "Ciencia-ficción" };
            generoComboBox.ItemsSource = tipoGenero;
            listaListBox.DataContext = peliculas;
        }

        private void listaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gestionarGrid.DataContext = (sender as ListBox).SelectedItem;
        }

        private void importarJsonButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult respuesta = MessageBox.Show("Se van a borrar las películas de la lista, ¿conforme?", "Conformidad",
                                                         MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (respuesta == MessageBoxResult.OK)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                List<Pelicula> pl;
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "Json files |*.json";
                try
                {
                    if (openFileDialog.ShowDialog() == true)
                    {
                        using (StreamReader jsonStream = File.OpenText(openFileDialog.FileName))
                        {
                            peliculas.Clear();  // Limpiamos la lista de películas
                            var json = jsonStream.ReadToEnd();
                            pl = JsonConvert.DeserializeObject<List<Pelicula>>(json);
                            foreach (Pelicula peli in pl)
                            {
                                peliculas.Add(peli);
                            }
                        }
                        MessageBox.Show("Se ha importado el fichero JSON '" + openFileDialog.FileName + "'",
                                        "Importacion Json", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void exportarButton_Click(object sender, RoutedEventArgs e)
        {
            string peliculasJson = JsonConvert.SerializeObject(peliculas);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files |*.json";
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "json";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, peliculasJson);
                    string texto = "Fichero Json: " + saveFileDialog.FileName + " generado en " + Directory.GetCurrentDirectory();
                    MessageBox.Show(texto, "Exportación Json", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
