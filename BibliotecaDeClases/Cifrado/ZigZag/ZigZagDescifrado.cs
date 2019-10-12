using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.ZigZag
{
    public class ZigZagDescifrado
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }

        private int Filas { get; set; }
        private char Relleno { get; set; }

        public const int LargoBuffer = 100;

        public string[,] Estructura { get; set; }

        public ZigZagDescifrado(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, int clave)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = RutaAbsArchivo;
            RutaAbsolutaServer = RutaAbsServer;
            Filas = clave;

            Relleno = '^';
        }

        public bool Descifrar()
        {
            var EsCorrecto = false;

            var buffer = new byte[LargoBuffer];

            var longitudCadena = 0;

            var Olas = 0;
            var CantElementosOlas = 0;
            var Columnas = 0;


            using (var file = new FileStream(RutaAbsolutaArchivo,FileMode.Open))
            {
                using (var reader = new BinaryReader(file,Encoding.UTF8))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(LargoBuffer);

                        var caracteresCadena = System.Text.Encoding.UTF8.GetString(buffer);

                        if (caracteresCadena.Contains('/'))
                        {
                            caracteresCadena = caracteresCadena.Replace('/', (char)92);
                        }

                        longitudCadena = caracteresCadena.Length-1;

                        Calcular(ref Columnas,ref CantElementosOlas,ref Olas,Convert.ToDouble(longitudCadena));

                        if (longitudCadena == Columnas)
                        {
                            Estructura = new string[Filas, Columnas];

                            var cresta = caracteresCadena.Substring(0, Olas+1);
                            var valle = caracteresCadena.Substring((Columnas - Olas)+1, Olas);

                            caracteresCadena = caracteresCadena.Remove(0, Olas+1);
                            caracteresCadena = caracteresCadena.Remove((caracteresCadena.Length - 1) - Olas, Olas);

                            List<string> Niveles = new List<string>(Filas);

                            for (int i = 0; i < Filas; i++)
                            {
                                Niveles.Add("");
                            }

                            Niveles[0] = cresta;
                            Niveles[Filas - 1] = valle;


                            var elementosRestantes = longitudCadena-(Olas * 2);
                            var rielesRestantes = (Filas - 2);
                            var longitudRieles = elementosRestantes / rielesRestantes;

                            for (int i = 1; i <= rielesRestantes; i++)
                            {
                                Niveles[i] = caracteresCadena.Substring(0, longitudRieles);
                                caracteresCadena = caracteresCadena.Remove(0, longitudRieles);
                            }

                            var ContadorFilas = 0;
                            var elementos = 0;
                            var OlasAux = 0;

                            for (int i = 0; i < Columnas; i++)
                            {
                                if (elementos != CantElementosOlas)
                                {
                                    if (ContadorFilas < Filas)
                                    {
                                        if (ContadorFilas == 0)
                                        {
                                            Estructura[ContadorFilas, i] = Niveles[ContadorFilas].Substring(0, 2);
                                            Niveles[ContadorFilas] = Niveles[ContadorFilas].Remove(1, 1);
                                        }
                                        else
                                        {
                                            Estructura[ContadorFilas, i] = Niveles[ContadorFilas].Substring(0, 1);
                                            Niveles[ContadorFilas] = Niveles[ContadorFilas].Remove(0, 1);
                                        }

                                        ContadorFilas++;
                                        elementos++;
                                    }
                                    else
                                    {
                                        var diferencia = ContadorFilas + 1;
                                        diferencia -= Filas;

                                        Estructura[(ContadorFilas - (diferencia * 2)), i] = Niveles[ContadorFilas - (diferencia * 2)].Substring(0, 1);
                                        Niveles[ContadorFilas - (diferencia * 2)] = Niveles[ContadorFilas - (diferencia * 2)].Remove(0, 1);

                                        elementos++;
                                        ContadorFilas++;

                                        if (elementos == CantElementosOlas)
                                        {
                                            elementos = 0;
                                            ContadorFilas = 0;
                                            OlasAux++;
                                        }
                                    }
                                }
                                else
                                {
                                    elementos = 0;
                                    OlasAux++;
                                    ContadorFilas = 0;
                                }
                            }
                            Escribir(Columnas);
                        }
                        else
                        {
                            File.Delete(RutaAbsolutaArchivo);
                            throw new Exception("Clave incorrecta");
                        }
                    }
                }
            }
            File.Delete(RutaAbsolutaArchivo);
            return EsCorrecto;
        }

        private void Calcular(ref int columnas, ref int elementosOla, ref int Olas, double longitud)
        {
            var _elementosOla = (Filas * 2) - 2;
            var olas = (Math.Ceiling(longitud / _elementosOla)).ToString("####");

            elementosOla = int.Parse(_elementosOla.ToString("####"));
            Olas = int.Parse(olas);

            columnas = elementosOla * Olas;
        }

        private void Escribir(int columnas)
        {
            using (var file = new FileStream(RutaAbsolutaServer + NombreArchivo+".txt",FileMode.Append))
            {
                using (var writer = new StreamWriter(file,Encoding.UTF8))
                {
                    for (int i = 0; i < columnas; i++)
                    {
                        for (int j = 0; j < Filas; j++)
                        {
                            if (Estructura[j,i] != null && Estructura[j, i] != Relleno.ToString())
                            {
                                writer.Write(Estructura[j,i]);
                            }
                        }
                    }
                }
            }
        }
    }
}
