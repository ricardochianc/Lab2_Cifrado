using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.Espiral
{
    public class Descifrado
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string DireccionRecorrido { get; set; }
        public int Clave { get; set; }

        public Descifrado(int clave, string direccion, string nombreArchivo, string rutaAbsoluta, string rutaServer)
        {
            Clave = clave;
            DireccionRecorrido = direccion;
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = rutaAbsoluta;
            RutaAbsolutaServer = rutaServer;
        }

        public void Descifrar()
        {

        }
        //PARA GUARDAR EL ARCHIVO rutaServer + nombreArchivo + ".txt"
    }
}
