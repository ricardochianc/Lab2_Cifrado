using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.S_DES
{
    class DescifradoSDES
    {

        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string RutaAbsolutaArchivoSCif { get; set; } //Para el .scif

        private int Clave { get; set; }
        private SDES_Base UtilidadeSDES { get; set; }

        private const int LargoBuffer = 100;

        public DescifradoSDES(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, int clave, string rutaArchivoPermutaciones)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = RutaAbsArchivo;
            RutaAbsolutaServer = RutaAbsServer;
            RutaAbsolutaArchivoSCif = "";
            Clave = clave;

            UtilidadeSDES = new SDES_Base();

            if (rutaArchivoPermutaciones != "")
            {
                UtilidadeSDES.AsignarPermutaciones(rutaArchivoPermutaciones);
            }
        }

        public void Descifrar()
        {

        }

    }
}
