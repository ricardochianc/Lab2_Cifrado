using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BibliotecaDeClases.Cifrado.RSA
{
    class Operaciones
    {

        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string RutaAbsolutaArchivoRSACif { get; set; } //Para el .rsacif
        public string RutaArchivoLlave { get; set; }

        //Esto se leerá en el archivo *.key que ingrese el usario, independientemente de qué llave ingrese
        private int Modulo { get; set; }
        private int Llave { get; set; }

        private const int LargoBuffer = 100;

        public Operaciones(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, string rutaLlave)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = RutaAbsArchivo;
            RutaAbsolutaServer = RutaAbsServer;
            RutaAbsolutaArchivoRSACif = "";
            RutaArchivoLlave = rutaLlave;
        }

        private void LeerLlave()
        {
            using (var file = new FileStream(RutaArchivoLlave, FileMode.Open))
            {
                using (var reader = new StreamReader(file, Encoding.UTF8))
                {
                    //Campos linea va a leer la linea y de una vez separarlos
                    var camposLinea = reader.ReadLine().Split(',');
                    Modulo = int.Parse(camposLinea[0]);
                    Llave = int.Parse(camposLinea[1]);
                }
            }

            File.Delete(RutaArchivoLlave);
        }

        public void OperarRSA(string extension)
        {
            if (extension == "rsacif")
            {
                RutaAbsolutaArchivoRSACif = RutaAbsolutaServer + NombreArchivo + ".txt";
            }
            else if (extension == "txt")
            {
                RutaAbsolutaArchivoRSACif = RutaAbsolutaServer + NombreArchivo + ".rsacif";
            }
			
			LeerLlave();

            var buffer = new byte[LargoBuffer];
            var bufferEscritura = new char[LargoBuffer];

            using (var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using (var reader = new BinaryReader(file, Encoding.UTF8))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(LargoBuffer);

                        var contBuffer = 0;

                        foreach (var caracter in buffer)
                        {

                            //Recordando de N = C^d mod n && C = N^e mod n

                            var potencia = Potencia(Convert.ToInt32(caracter), Convert.ToInt32(Llave));

                            var Caractercifrado = potencia % Modulo;

                            bufferEscritura[contBuffer] = (char)Caractercifrado;
							contBuffer++;
                        }
                        EscribirBuffer(bufferEscritura);
                    }
                }
            }

            File.Delete(RutaArchivoLlave);
			File.Delete(RutaAbsolutaArchivo);
        }

        private decimal Potencia(int numBase, int exponente)
        {
            decimal respuesta = 1;

            for (int i = 0; i < exponente; i++)
            {
                respuesta *= numBase;
            }

            return respuesta;
        }

        private void EscribirBuffer(char[] buffer)
        {
            using (var file = new FileStream(RutaAbsolutaArchivoRSACif, FileMode.Append))
            {
                using (var writer = new BinaryWriter(file, Encoding.UTF8))
                {
                    writer.Write(buffer);
                }
            }
        }

    }
}
