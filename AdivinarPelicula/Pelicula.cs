using System.Collections.ObjectModel;
using System.ComponentModel;

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

            peliculas.Add(new Pelicula("Mision imposible", "Tom Cruise en difícil acción ... ",
                "E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/misionImposible.jpg",
                "Acción", false, true, false));

            peliculas.Add(new Pelicula("Torrente, el brazo tonto de la ley", "Santiago Segura - To1",
                "E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/torrente1.jpg",
                "Comedia", false, false, true));
            peliculas.Add(new Pelicula("Ben-Hur", "Un clásico de Semana Santa",
                "E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/benhur.jpg",
                "Drama", true, false, false));
            peliculas.Add(new Pelicula("El rey león", "Hakuna Matata",
                "E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/elreyleon.jpg",
                "Ciencia-ficción", true, false, false));
            peliculas.Add(new Pelicula("Titanic", "Barco hundido por un iceberg",
    "E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/titanic2.jpg",
    "Drama", true, false, false));
            peliculas.Add(new Pelicula("El exorcista", "Poseida por lucifer",
"E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/elexorcista.jpg",
"Terror", false, false, true));
            peliculas.Add(new Pelicula("El bueno, el feo y el malo", "Tres protagonistas",
"E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/elbuenoelmaloelfeo.jpg",
"Acción", true, false, false));
            peliculas.Add(new Pelicula("El gran dictador", "Como Fidel Castro, pero a lo grande",
"E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/elgrandictador.jpg",
"Comedia", false, true, false));
            peliculas.Add(new Pelicula("El padrino", "Il capo",
"E:/DAM2A/Desarrollo Interfaces/Proyectos/AdivinarPelicula/AdivinarPelicula/Imagenes/elpadrino.jpeg",
"Drama", false, true, false));

            return peliculas;
        }
    }
}
