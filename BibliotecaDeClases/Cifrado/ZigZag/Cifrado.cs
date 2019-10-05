using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.ZigZag
{
    public class Cifrado
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string RutaAbsolutaArchivoCif { get; set; } //Para el .cif

        private int Filas { get; set; }
        private char Relleno { get; set; }

        public const int LargoBuffer = 100;

        public string[,] Estructura { get; set; }


        public Cifrado(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, int clave)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = RutaAbsArchivo;
            RutaAbsolutaServer = RutaAbsServer;
            Filas = clave;

            Relleno = '^';
        }

        public void Cifrar()
        {
            var buffer = new byte[LargoBuffer];

            var longitudCadena = 0;

            var Olas = 0;
            var CantElementosOlas = 0;
            var Columnas = 0;


            using (var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using (var reader = new BinaryReader(file, Encoding.UTF8))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(LargoBuffer);

                        var caracteresArray = System.Text.Encoding.UTF8.GetChars(buffer);

                        longitudCadena = caracteresArray.Length;

                        CalcularColumnas(ref Columnas, ref CantElementosOlas, ref Olas, Convert.ToDouble(longitudCadena));

                        Estructura = new string[Filas, Columnas];

                        var elementos = 0;
                        var ContadorFila = 0;
                        var OlasAux = 0;

                        foreach (var caracter in caracteresArray)
                        {
                            if (caracter != '\r')
                            {
                                for (int i = 0; i < Columnas; i++)
                                {
                                    if (elementos != CantElementosOlas)
                                    {
                                        if (ContadorFila < Filas)
                                        {
                                            if (caracter == '\n')
                                            {
                                                Estructura[ContadorFila, i] = "/n";
                                            }
                                            else
                                            {
                                                Estructura[ContadorFila, i] = caracter.ToString();
                                            }

                                            ContadorFila++;
                                            elementos++;
                                        }
                                        else
                                        {
                                            var diferencia = ContadorFila + 1;
                                            diferencia = -Filas;

                                            if (caracter == '\n')
                                            {
                                                Estructura[(ContadorFila - (diferencia * 2)), i] = "/n";
                                            }
                                            else
                                            {
                                                Estructura[(ContadorFila - (diferencia * 2)), i] = caracter.ToString();
                                            }

                                            elementos++;

                                            if (elementos == CantElementosOlas)
                                            {
                                                elementos = 0;
                                                ContadorFila = 0;
                                                OlasAux++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        elementos = 0;
                                        OlasAux++;
                                        ContadorFila = 0;
                                    }
                                }
                            }
                        }

                        if ((Columnas - longitudCadena) != 0)
                        {
                            ContadorFila = 0;

                            for (int i = (((Olas - 1) * CantElementosOlas) - 1); i < Columnas; i++)
                            {
                                if (elementos != CantElementosOlas)
                                {
                                    if (ContadorFila < Filas)
                                    {
                                        if (Estructura[ContadorFila, i] == "")
                                        {
                                            Estructura[ContadorFila, i] = Relleno.ToString();
                                            ContadorFila++;
                                            elementos++;
                                        }
                                    }
                                    else
                                    {
                                        var diferencia = ContadorFila + 1;
                                        diferencia = -Filas;

                                        if (Estructura[(ContadorFila - (diferencia * 2)), i] == "")
                                        {
                                            Estructura[(ContadorFila - (diferencia * 2)), i] = Relleno.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    i = Columnas;
                                }

                            }
                        }

                        ContadorFila = 0;
                        elementos = 0;

                        Escribir(Columnas);
                    }
                }
            }
        }

        private void CalcularColumnas(ref int columnas, ref int elementosOla, ref int Olas, double longitud)
        {
            var _elementosOla = (Filas * 2) - 2;
            var olas = (Math.Ceiling(longitud / elementosOla)).ToString("####");

            elementosOla = int.Parse(_elementosOla.ToString("####"));
            Olas = int.Parse(olas);

            columnas = elementosOla * Olas;
        }

        private void Escribir(int columnas)
        {
            using (var file = new FileStream(RutaAbsolutaServer + NombreArchivo + ".cif", FileMode.Append))
            {
                using (var writer = new StreamWriter(file, Encoding.UTF8))
                {
                    for (int i = 0; i < Filas; i++)
                    {
                        for (int j = 0; j < columnas; j++)
                        {
                            if (Estructura[i, j] != "")
                            {
                                writer.Write(Estructura[i, j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
