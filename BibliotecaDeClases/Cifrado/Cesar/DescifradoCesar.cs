using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.Cesar
{
    public class DescifradoCesar
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string DireccionRecorrido { get; set; }
        public string Clave { get; set; }

        public DescifradoCesar(string clave, string nombreArchivo, string rutaAbsoluta, string rutaServer)
        {
            Clave = clave;
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = rutaAbsoluta;
            RutaAbsolutaServer = rutaServer;
        }

        public void Descifrar()
        {

        }
    }
}
