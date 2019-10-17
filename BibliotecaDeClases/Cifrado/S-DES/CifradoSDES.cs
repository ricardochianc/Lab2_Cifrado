﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.S_DES
{
    public class CifradoSDES
    {
        //Paths, Clave y Ruta de archivo .ini
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string RutaAbsolutaArchivoCif { get; set; } //Para el .scif

        private int Clave { get; set; }
        private SDES_Base UtilidadeSDES { get; set; }

        private const int LargoBuffer = 100;
        
        public CifradoSDES(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, int clave, string rutaArchivoPermutaciones)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = RutaAbsArchivo;
            RutaAbsolutaServer = RutaAbsServer;
            Clave = clave;

            UtilidadeSDES = new SDES_Base();
            UtilidadeSDES.AsignarPermutaciones(rutaArchivoPermutaciones);
        }

        public void Cifrar()
        {
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

                        foreach (var caracter in buffer)
                        {
                            var caracterBits = Convert.ToString(caracter, 2); //Se convierte a binario el byte

                            if (caracterBits.Length <= 8)
                            {
                                caracterBits = caracterBits.PadLeft(8, '0'); //Se rellena de ceros a la izquierda, si son menos de 8 bits

                                UtilidadeSDES.AplicarPermutacion("PI",ref caracterBits); //Se le hace permutacion inicial

                                //RONDA 1--------------------------------------------------------------------------------------------------------------------------------
                                var bloqueIzquierdo = "";
                                var bloqueDerecho = "";
                                var bloqueDerechoOriginal = bloqueDerecho;

                                UtilidadeSDES.DividirCadenaBits(4,caracterBits,ref bloqueIzquierdo,ref bloqueDerecho);
                                bloqueDerechoOriginal = bloqueDerecho;

                                UtilidadeSDES.AplicarPermutacion("ExpandirPermutar",ref bloqueDerecho); //Se le aplica Expandir y permutar al bloque derecho de 4 bits, para obtener un expandido de 8 bits

                                var binarioResultante = UtilidadeSDES.XOR(bloqueDerecho, key1);

                                //Estos serviran para luego hacer las consultas en las SBoxes
                                var MasIzquierdos = (binarioResultante[0] + binarioResultante[1] + binarioResultante[2] +binarioResultante[3]).ToString();
                                var MasDerechos = (binarioResultante[4] + binarioResultante[5] + binarioResultante[6] + binarioResultante[7]).ToString();

                                var fila = 0;
                                var columna = 0;

                                UtilidadeSDES.ObtenerFilaColumna(MasIzquierdos, ref fila, ref columna);

                                var bitsResultantesSBoxes = UtilidadeSDES.ObtenerBitsSBox0(fila, columna);

                                UtilidadeSDES.ObtenerFilaColumna(MasDerechos,ref fila,ref columna);

                                bitsResultantesSBoxes += UtilidadeSDES.ObtenerBitsSBox1(fila, columna);

                                UtilidadeSDES.AplicarPermutacion("P4",ref bitsResultantesSBoxes); //Se le aplica una permutacion de 4 bits

                                var resultadoBits = UtilidadeSDES.XOR(bitsResultantesSBoxes, bloqueIzquierdo);

                                var resultadoRonda1 = resultadoBits.ToString() + bloqueDerechoOriginal.ToString();
                                //-----------------------------------------------------------------------------------------------------------------------------------------

                                //RONDA 2-----------------------------------------------------------------------------------------------------------------------------------

                                var bloqueIzquierdo2 = "";
                                var bloqueDerecho2 = "";

                                //Se ingresa el izquierdo en el derecho y derecho en izquierdo con el motivo de intercambiar de lados
                                UtilidadeSDES.DividirCadenaBits(4,resultadoRonda1, ref bloqueDerecho2,ref bloqueIzquierdo2);

                                UtilidadeSDES.AplicarPermutacion("ExpandirPermutar",ref bloqueDerecho2);

                            }
                            else
                            {
                                throw new Exception("Mayor a 8 bits");
                            }
                        }
                    }
                }
            }
        }
    }
}