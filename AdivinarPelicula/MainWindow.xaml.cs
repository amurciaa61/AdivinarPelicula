using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdivinarPelicula
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string DIRECTORIO_DATOS = "Datos";
        const string DIRECTORIO_IMAGENES = "Imagenes";
        ObservableCollection<Pelicula> peliculas;
        Pelicula pelicula = new Pelicula();
        List<int> puntos = new List<int>();
        List<string> detalleJugadas = new List<string>();
        List<Pelicula> peliculasJuego = new List<Pelicula>();
        const int PUNTOS_FACIL = 10;
        const int PUNTOS_NORMAL = 20;
        const int PUNTOS_DIFICIL = 30;
        public MainWindow()
        {
            InitializeComponent();
            borrarButton.IsEnabled = false;
            gestionarGrid.DataContext = pelicula;
            peliculas = Pelicula.GetSamples();
            ObservableCollection<string> tipoGenero = new ObservableCollection<string> { "Acción", "Drama", "Comedia", "Terror", "Ciencia-ficción" };
            generoComboBox.ItemsSource = tipoGenero;
            listaListBox.DataContext = peliculas;
            if (peliculas.Count == 0)
            {
                exportarButton.IsEnabled = false;
            }
        }

        private void ListaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gestionarGrid.DataContext = pelicula;
            gestionarGrid.DataContext = (sender as ListBox).SelectedItem;
            borrarButton.IsEnabled = true;
            añadirButton.IsEnabled = false;
            AsignarForegroundPorDefecto();
        }
        private void AsignarForegroundPorDefecto()
        {
            tituloTextBox.Background = Brushes.White;
            pistaTextBox.Background = Brushes.White;
            imagenTextBox.Background = Brushes.White;
            generoTextBlock.Foreground = Brushes.Black;
        }
        private void Borrar_Button_Click(object sender, RoutedEventArgs e)
        {
            pelicula = (Pelicula)gestionarGrid.DataContext;
            if (listaListBox.SelectedIndex < 0)
                MessageBox.Show("Debe seleccionar la película a borrar", "Borrar película",
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                MessageBoxResult respuesta = MessageBox.Show("¿Desea borrar la película " + pelicula.Titulo + "?",
                                             "Borrar película", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (respuesta == MessageBoxResult.OK)
                {
                    peliculas.Remove(pelicula);
                    borrarButton.IsEnabled = false;
                    añadirButton.IsEnabled = true;
                    if (peliculas.Count == 0)
                    {
                        exportarButton.IsEnabled = false;
                    }
                    MessageBox.Show("Eliminada película: " + pelicula.Titulo, "Baja Película",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    pelicula = null;
                    pelicula = new Pelicula();
                    gestionarGrid.DataContext = pelicula;
                }
            }
        }
        private void AñadirPelicula_Button_Click(object sender, RoutedEventArgs e)
        {
            if (listaListBox.SelectedIndex < 0)
            {
                bool error = false;
                if (pelicula.Titulo == null)
                {
                    error = true;
                    tituloTextBox.Background = Brushes.Red;
                }
                else
                    tituloTextBox.Background = Brushes.White;
                if (pelicula.Pista == null)
                {
                    error = true;
                    pistaTextBox.Background = Brushes.Red;
                }
                else
                    pistaTextBox.Background = Brushes.White;
                if (pelicula.Imagen == null)
                {
                    error = true;
                    imagenTextBox.Background = Brushes.Red;
                }
                else
                    imagenTextBox.Background = Brushes.White;
                if (!pelicula.Facil &&
                    !pelicula.Normal &&
                    !pelicula.Dificil)
                {
                    facilRadioButton.IsChecked = true;
                }
                if (pelicula.Genero == null)
                {
                    error = true;
                    generoTextBlock.Foreground = Brushes.Red;
                }
                else
                    generoTextBlock.Foreground = Brushes.Black;
                if (error)
                {
                    MessageBox.Show("Debe rellenar los campos marcados en rojo", "Alta película",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    peliculas.Add(pelicula);
                    MessageBox.Show("Añadida película: " + pelicula.Titulo, "Alta Película",
                                     MessageBoxButton.OK, MessageBoxImage.Information);
                    pelicula = null;
                    pelicula = new Pelicula();
                    gestionarGrid.DataContext = pelicula;
                    exportarButton.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Tiene una película seleccionada, no se permite el ALTA", "Alta película",
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        private void Importar_Json_Button_Click(object sender, RoutedEventArgs e)
        {
            if (peliculas.Count > 0)
            {
                MessageBoxResult respuesta = MessageBox.Show("Se van a recargar las películas, ¿conforme?", "Conformidad",
                                                             MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (respuesta == MessageBoxResult.OK)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    List<Pelicula> pl;
                    openFileDialog.InitialDirectory = DirectorioActual(DIRECTORIO_DATOS);
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
                            MessageBox.Show("Se ha importado el fichero JSON: " + openFileDialog.FileName,
                                            "Importacion Json", MessageBoxButton.OK, MessageBoxImage.Information);
                            borrarButton.IsEnabled = false;
                            añadirButton.IsEnabled = true;
                            pelicula = null;
                            pelicula = new Pelicula();
                            gestionarGrid.DataContext = pelicula;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void Exportar_Json_Button_Click(object sender, RoutedEventArgs e)
        {
            string peliculasJson = JsonConvert.SerializeObject(peliculas);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = DirectorioActual(DIRECTORIO_DATOS);
            saveFileDialog.Filter = "Json files |*.json";
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "json";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, peliculasJson);
                    MessageBox.Show("Fichero JSON: " + saveFileDialog.FileName + " generado en " + Directory.GetCurrentDirectory(),
                                    "Exportación Json", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SeleccionarImagen_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DirectorioActual(DIRECTORIO_IMAGENES);
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.ico";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    imagenTextBox.Text = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void NuevaPartida_Button_Click(object sender, RoutedEventArgs e)
        {
            const int NUMERO_JUGADAS = 5;
            if (peliculas.Count < NUMERO_JUGADAS)
            {
                MessageBox.Show("Deben haber al menos 5 películas para poder jugar, y hay " + peliculas.Count,
                    "Jugar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                puntos.Clear();
                detalleJugadas.Clear();
                puntuacionTextBox.Text = "0";
                puntuacionJugadaTextBox.Text = "0";
                puntuacionDetalleTextBox.Text = "";
                verPistaCheckBox.IsEnabled = true;
                validarButton.IsEnabled = true;
                for (int i = 0; i < NUMERO_JUGADAS; i++)
                {
                    puntos.Add(0);
                    detalleJugadas.Add("");
                }
                List<Pelicula> pelis = peliculas.ToList();
                List<int> aleatorias = new List<int>();
                peliculasJuego.Clear();
                Random semilla = new Random();
                while (aleatorias.Count < NUMERO_JUGADAS)
                {
                    int numeroPelicula = semilla.Next(0, pelis.Count);
                    if (!aleatorias.Contains(numeroPelicula))
                    {
                        aleatorias.Add(numeroPelicula);
                        peliculasJuego.Add(pelis[numeroPelicula]);
                    }
                }
                verPistaCheckBox.IsChecked = false;
                BindingDelObjeto(1);
                ActualizarIndice(1);
            }
        }
        public void BindingDelObjeto(int indice)
        {
            indice--;
            jugarDockPanel.DataContext = peliculasJuego[indice];
        }
        public void ActualizarIndice(int indice)
        {
            peliculaActualTextBlock.Text = indice.ToString() + " / " + peliculasJuego.Count.ToString();
        }

        private void FlechaAtras_Button_Click(object sender, RoutedEventArgs e)
        {
            int indiceActual = ObtenerIndiceActual();
            if (indiceActual > 0)
            {
                tituloPeliculaTextBox.Text = "";
                verPistaCheckBox.IsChecked = false;
                verPistaCheckBox.IsEnabled = true;
                int indice = indiceActual == 1 ? 1 : indiceActual - 1;
                BindingDelObjeto(indice);
                ActualizarIndice(indice);
                MostrarPuntosJugada(indice-1);
            }
        }
        private void FlechaAdelante_Button_Click(object sender, RoutedEventArgs e)
        {
            AvanzarPeliculaJuego();

        }
        private void AvanzarPeliculaJuego()
        {
            int indiceActual = ObtenerIndiceActual();
            if (indiceActual > 0)
            {
                tituloPeliculaTextBox.Text = "";
                verPistaCheckBox.IsChecked = false;
                verPistaCheckBox.IsEnabled = true;
                int indice = indiceActual == peliculasJuego.Count ? peliculasJuego.Count : indiceActual + 1;
                BindingDelObjeto(indice);
                ActualizarIndice(indice);
                MostrarPuntosJugada(indice-1);
            }
        }
        private void MostrarPuntosJugada(int indice)
        {
            puntuacionJugadaTextBox.Text = puntos[indice].ToString();
        }
        public int ObtenerIndiceActual()
        {
            string[] valores = peliculaActualTextBlock.Text.Split('/');
            return int.Parse(valores[0]);
        }

        private void Validar_Button_Click(object sender, RoutedEventArgs e)
        {
            int puntuacion = 0;
            int indice = ObtenerIndiceActual() - 1;
            string textoJugada = "";   // para guardar el detalle de la jugada
            if (tituloPeliculaTextBox.Text.Length > 0 &&
                peliculasJuego[indice].Titulo.ToUpper() == tituloPeliculaTextBox.Text.ToUpper())
            {
                if (peliculasJuego[indice].Facil)
                {
                    puntuacion = PUNTOS_FACIL;
                    textoJugada += "acumulas " + PUNTOS_FACIL + " puntos fácil";
                }
                else if (peliculasJuego[indice].Normal)
                {
                    puntuacion = PUNTOS_NORMAL;
                    textoJugada += "acumulas " + PUNTOS_NORMAL + " puntos normal";
                }
                else
                {
                    puntuacion = PUNTOS_DIFICIL;
                    textoJugada += "acumulas " + PUNTOS_DIFICIL + " puntos dificil";
                }
            }
            else
            {
                textoJugada = "No acumulas puntos.";
            }
            if (verPistaCheckBox.IsChecked == true && puntuacion > 0)
            {
                puntuacion /= 2;
                textoJugada += "\n, pero divides por 2, has usado la pista";
            }
            puntos[indice] = puntuacion;            // Guardamos puntos de cada partida
            detalleJugadas[indice] = textoJugada;   // Guardamos detalle de cada partida
            MostrarPuntosJugada(indice);
            MuestraPuntos();
            AvanzarPeliculaJuego();  // Avanzamos a película siguiente de forma automática
        }
        public void MuestraPuntos()
        {
            int puntosTotales = 0;
            StringBuilder textoJugadas = new StringBuilder();
            for (int i = 0; i < puntos.Count; i++)
            {
                puntosTotales += puntos[i];
                textoJugadas.Append("** Jugada ");
                textoJugadas.Append(i + 1);
                textoJugadas.Append(": ");
                textoJugadas.Append(puntos[i]);
                textoJugadas.Append("\n");
                textoJugadas.Append(detalleJugadas[i]);
                textoJugadas.Append("\n");
            }
            puntuacionTextBox.Text = puntosTotales.ToString();       // Cantidad numérica de puntos
            puntuacionDetalleTextBox.Text = textoJugadas.ToString(); // Detalle de las jugadas y puntos por jugada
        }

        private void DeshabilitarPista_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (verPistaCheckBox.IsChecked == true)
                verPistaCheckBox.IsEnabled = false;
        }
        private void MostrarOcultarDetallePuntos_Button_Click(object sender, RoutedEventArgs e)
        {
            if (mostrarPuntosButton.Content.ToString() == "Mostrar detalle")
            {
                mostrarPuntosButton.Content = "Ocultar detalle";
                puntuacionDetalleTextBox.Visibility = Visibility.Visible;
                detallePuntosTextBlock.Text = "Detalle puntos";
            }
            else
            {
                mostrarPuntosButton.Content = "Mostrar detalle";
                puntuacionDetalleTextBox.Visibility = Visibility.Hidden;
                detallePuntosTextBlock.Text = "";
            }

        }
        static private string DirectorioActual(string directorioFinal)
        {
            // Proponemos el directorio deseado para el OpenDialog y SaveDialog
            string directorioActual = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            directorioActual = Path.GetDirectoryName(directorioActual);
            directorioActual += Path.DirectorySeparatorChar + directorioFinal;
            return directorioActual;
        }
    }
}
