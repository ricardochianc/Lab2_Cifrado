using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BibliotecaDeClases.Cifrado.Cesar
{
    public class DescifradoCesar
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string DireccionRecorrido { get; set; }
        public string Clave { get; set; }
        public int posBufferEscritura { get; set; }

        const int largoBuffer = 100;
        private char[] bufferEscritura = new char[largoBuffer];
        private char[] bufferLectura = new char[largoBuffer];

        public DescifradoCesar(string clave, string nombreArchivo, string rutaAbsoluta, string rutaServer)
        {
            Clave = clave;
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = rutaAbsoluta;
            RutaAbsolutaServer = rutaServer;
            posBufferEscritura = 0;
        }

        public bool ValidarClave(char[] clave)
        {
            var esValida = false;

            for (int i = 0; i < clave.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if(clave[i] == clave[j])
                    {
                        esValida = false;
                        j = clave.Length;
                        i = clave.Length;
                    }
                    else
                    {
                        esValida = true;
                    }
                }
            }

            return esValida;
        }

        public void Descifrar()
        {
            var alfabeto = "0123456789abcdefghijklmnopqrstuvwxyz";
            var pos = Clave.Length;
            char[] alfCifrado = new char[alfabeto.Length];

            //AL ALFABETO CIFRADO LE METO LA PALABRA CLAVE
            for (int i = 0; i < Clave.Length; i++)
            {
                alfCifrado[i] = Clave[i];
            }

            //AL ALFABETO CIFRADO SE METEN LOS CARACTERES DEL ALFABETO NORMAL QUE NO APAREZCAN DENTRO DE LA PALABRA CLAVE
            for (int i = 0; i < alfabeto.Length; i++)
            {
                if (alfCifrado.Contains(alfabeto[i]))
                {

                }
                else
                {
                    alfCifrado[pos] = alfabeto[i];
                    pos++;
                }                         
            }

            using(var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using(var reader = new BinaryReader(file, Encoding.UTF8))
                {
                    while(reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        bufferLectura = reader.ReadChars(largoBuffer);
                        posBufferEscritura = 0;

                        for (int i = 0; i < bufferLectura.Length; i++)
                        {
                            for (int j = 0; j < alfabeto.Length; j++)
                            {
                                if (alfCifrado.Contains(bufferLectura[i]))
                                {
                                    if(bufferLectura[i] == alfCifrado[j])
                                    {
                                        bufferEscritura[posBufferEscritura] += alfabeto[j];
                                        posBufferEscritura++;
                                        j = alfabeto.Length;
                                    }
                                }
                                else
                                {
                                    bufferEscritura[posBufferEscritura] += bufferLectura[i];
                                    posBufferEscritura++;
                                    j = alfabeto.Length;
                                }
                            }
                        }

                        EscribirBuffer();
                    }
                }
            }
        }

        private void EscribirBuffer()
        {
            using(var file = new FileStream(RutaAbsolutaServer + NombreArchivo + ".txt", FileMode.Append))
            {
                using(var writer = new StreamWriter(file, Encoding.UTF8))
                {
                    for (int i = 0; i < posBufferEscritura; i++)
                    {
                        if(bufferEscritura[i] == '\n')
                        {
                            writer.Write(Environment.NewLine);
                        }
                        else
                        {
                            writer.Write(bufferEscritura[i]);
                        }                        
                    }
                }
            }
        }
    }
}
