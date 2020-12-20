using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace AdivinarPelicula
{
    class Pelicula : INotifyPropertyChanged
    {

        private string _titulo;

        public string Titulo
        {
            get { return _titulo; }
            set
            {
                if (_titulo != value)
                {
                    _titulo = value;
                    NotifyPropertyChanged("Titulo");
                }
            }
        }
        private string _pista;

        public string Pista
        {
            get { return _pista; }
            set
            {
                if (_pista != value)
                {
                    _pista = value;
                    NotifyPropertyChanged("Pista");
                }
            }
        }
        private string _imagen;

        public string Imagen
        {
            get { return _imagen; }
            set
            {
                if (_imagen != value)
                {
                    _imagen = value;
                    NotifyPropertyChanged("Imagen");
                }
            }
        }
        private string _genero;

        public string Genero
        {
            get { return _genero; }
            set
            {
                if (_genero != value)
                {
                    _genero = value;
                    NotifyPropertyChanged("Genero");
                }
            }
        }
        private bool _facil;

        public bool Facil
        {
            get { return _facil; }
            set
            {
                if (_facil != value)
                {
                    _facil = value;
                    NotifyPropertyChanged("Facil");
                }
            }
        }
        private bool _normal;
        public bool Normal
        {
            get { return _normal; }
            set
            {
                if (_normal != value)
                {
                    _normal = value;
                    NotifyPropertyChanged("Normal");
                }
            }
        }
        private bool _dificil;
        public bool Dificil
        {
            get { return _dificil; }
            set
            {
                if (_dificil != value)
                {
                    _dificil = value;
                    NotifyPropertyChanged("Dificil");
                }
            }
        }

        public Pelicula()
        {
        }

        public Pelicula(string titulo, string pista, string imagen, string genero, bool facil, bool normal, bool dificil)
        {
            Titulo = titulo;
            Pista = pista;
            Imagen = imagen;
            Genero = genero;
            Facil = facil;
            Normal = normal;
            Dificil = dificil;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<Pelicula> GetSamples()
        {
            ObservableCollection<Pelicula> peliculas = new ObservableCollection<Pelicula>();
            const string DIRECTORIO_IMAGENES = "Imagenes";
            string directorioActual = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            directorioActual = Path.GetDirectoryName(directorioActual);
            directorioActual += $"{Path.DirectorySeparatorChar}{DIRECTORIO_IMAGENES}";
            peliculas.Add(new Pelicula("El gran dictador", "Como Fidel Castro, pero a lo grande",
                          Path.Combine(directorioActual, "elgrandictador.jpg"),"Comedia", false, true, false));
            peliculas.Add(new Pelicula("El padrino", "il capo",
                          Path.Combine(directorioActual, "elpadrino.jpeg"),"Drama", false, true, false));
            peliculas.Add(new Pelicula("Mision imposible", "Tom Cruise en difícil acción ... ",
                          Path.Combine(directorioActual, "misionImposible.jpg"),"Acción", false, true, false));
            peliculas.Add(new Pelicula("Torrente, el brazo tonto de la ley", "Santiago Segura, defensor de la ley con la extremidad izquierda",
                          Path.Combine(directorioActual, "torrente1.jpg"),"Comedia", false, false, true));
            peliculas.Add(new Pelicula("mars attacks", "Venimos en son de paz",
                          Path.Combine(directorioActual, "mars_attacks.jpg"),"Ciencia-ficción", false, false, true));
            peliculas.Add(new Pelicula("Ben-Hur", "Un clásico de Semana Santa",
                          Path.Combine(directorioActual, "benhur.jpg"),"Drama", true, false, false));
            peliculas.Add(new Pelicula("Titanic", "Barco hundido por un iceberg",
                          Path.Combine(directorioActual, "titanic2.jpg"),"Drama", true, false, false));
            peliculas.Add(new Pelicula("El exorcista", "Poseida por lucifer",
                          Path.Combine(directorioActual, "elexorcista.jpg"),"Terror", false, false, true));
            peliculas.Add(new Pelicula("El bueno, el feo y el malo", "Tres protagonistas",
                          Path.Combine(directorioActual, "elbuenoelmaloelfeo.jpg"),"Acción", true, false, false));
            peliculas.Add(new Pelicula("El rey león", "Hakuna Matata",
                          Path.Combine(directorioActual, "elreyleon.jpg"),"Ciencia-ficción", true, false, false));
            return peliculas;
        }
    }
}
