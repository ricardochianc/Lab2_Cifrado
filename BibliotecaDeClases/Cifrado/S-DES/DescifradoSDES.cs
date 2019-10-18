using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BibliotecaDeClases.Cifrado.S_DES
{
    public class DescifradoSDES
    {

        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string RutaAbsolutaArchivoDescif { get; set; } //Para el .scif

        private int Clave { get; set; }
        private SDES_Base UtilidadeSDES { get; set; }

        private const int LargoBuffer = 100;

        public DescifradoSDES(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, int clave, string rutaArchivoPermutaciones)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = RutaAbsArchivo;
            RutaAbsolutaServer = RutaAbsServer;
            RutaAbsolutaArchivoDescif = "";
            Clave = clave;

            UtilidadeSDES = new SDES_Base();

            if (rutaArchivoPermutaciones != "")
            {
                UtilidadeSDES.AsignarPermutaciones(rutaArchivoPermutaciones);
            }
        }

        public void Descifrar()
        {
            RutaAbsolutaArchivoDescif = RutaAbsolutaServer + NombreArchivo + ".txt";

            var key1 = "";
            var key2 = "";

            UtilidadeSDES.GenerarLlaves(ref key1, ref key2, Clave); //Se genera las llaves Key1 y key2

            var buffer = new byte[LargoBuffer];
            var bufferEscritura = new byte[LargoBuffer];

            using (var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using (var reader = new BinaryReader(file,Encoding.UTF8))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(LargoBuffer);

                        var contBuffer = 0;

                        foreach (var caracter in buffer)
                        {
                            var caracterBits = Convert.ToString(caracter, 2); //Se convierte a binario el byte

                            if(caracterBits.Length <= 8)
                            {
                                caracterBits = caracterBits.PadLeft(8, '0'); //Se rellena de ceros a la izquierda, si son menos de 8 bits

                                UtilidadeSDES.AplicarPermutacion("PI",ref caracterBits); //Se le hace permutacion inicial

                                //RONDA 1--------------------------------------------------------------------------------------------------------------------------------
                                var bloqueIzquierdo = "";
                                var bloqueDerecho = "";
                                var bloqueDerechoOriginal = bloqueDerecho;

                                //Los 8 bits del caracter se dividen en 4
                                UtilidadeSDES.DividirCadenaBits(4,caracterBits,ref bloqueIzquierdo,ref bloqueDerecho);
                                bloqueDerechoOriginal = bloqueDerecho; //Se guarda un original o copia del bloque derecho para trabajar con uno y tener la copia para usarla al final de la ronda 1

                                UtilidadeSDES.AplicarPermutacion("ExpandirPermutar",ref bloqueDerecho); //Se le aplica Expandir y permutar al bloque derecho de 4 bits, para obtener un expandido de 8 bits

                                var binarioResultante = UtilidadeSDES.XOR(bloqueDerecho, key2);

                                //Del resultante del XOR se toman los 4 bits más a la derecha para consultar SBox0 y los 4 más a la izquierda para consultar a SBox1
                                var MasIzquierdos = binarioResultante[0].ToString() + binarioResultante[1].ToString() + binarioResultante[2].ToString() +binarioResultante[3].ToString();
                                var MasDerechos = binarioResultante[4].ToString() + binarioResultante[5].ToString() + binarioResultante[6].ToString() + binarioResultante[7].ToString();

                                var fila = 0;
                                var columna = 0;

                                UtilidadeSDES.ObtenerFilaColumna(MasIzquierdos, ref fila, ref columna);

                                var bitsResultantesSBoxes = UtilidadeSDES.ObtenerBitsSBox0(fila, columna); //Se obitnene los primeros 2 bits que devulve la SBox0

                                UtilidadeSDES.ObtenerFilaColumna(MasDerechos,ref fila,ref columna); //Se vuelve a obtener filas y columnas pero ahora del bloque 2 para SBox1

                                bitsResultantesSBoxes += UtilidadeSDES.ObtenerBitsSBox1(fila, columna); //A los 2 bits de SBox0, se le concatenan los nuevos 2 bits obtenidos SBox1

                                UtilidadeSDES.AplicarPermutacion("P4",ref bitsResultantesSBoxes); //Se le aplica una permutacion de 4 bits, a la cadena resultante de ambas SBox

                                var resultadoBits = UtilidadeSDES.XOR(bitsResultantesSBoxes, bloqueIzquierdo); //Se hace un Xor de los 4 bits resultantes de las SBoxes con los 4 bits del bloque izquierdo que no se usó en la ronda 1, hasta ahora.

                                var resultadoRonda1 = resultadoBits.ToString() + bloqueDerechoOriginal.ToString(); //Resultado de la ronda 1
                                //Termina ronda 1------------------------------------------------------------------------------------------------------------------------------


                            }
                            else
                            {
                                throw new Exception("Mayor a 8 bits");
                            }
                        }
                        
                    }
                }
            }
            File.Delete(RutaAbsolutaArchivo);
        }

        private void EscribirBuffer(byte[] buffer)
        {
            using (var file = new FileStream(RutaAbsolutaArchivoDescif,FileMode.Append))
            {
                using (var writer = new BinaryWriter(file,Encoding.UTF8))
                {
                    writer.Write(buffer);
                }
            }
        }

    }
}
