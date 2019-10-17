﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.S_DES
{
    internal class SDES_Base
    {
        public int[] P10 { get; set; } //Permunataciones para llave de 10 bits
        public int[] PCompresionKeys { get; set; } //Permutacion de compresion para K1 y K2 (no incluyen el bit1 y bit2)
        public int[] P8 { get; set; } //Permutacion inicial de 8 bits para texto plano
        public int[] ExpandirPermutar { get; set; } //Expandir y permutar (del 1 al 4, se repite 2 veces para una longitud total de 8 bits)
        public int[] P4 { get; set; } //Permutancion de 4 bits

        private string[,] SBox0 { get; set; }
        private string[,] SBox1 { get; set; }

        public SDES_Base()
        {
            P10 = new int[10];
            PCompresionKeys = new int[8];
            P8 = new int[8];
            ExpandirPermutar = new int[8];
            P4 = new int[4];

            SBox0 = new string[4, 4]{
                {"01", "00", "11", "10"},
                {"11", "10", "01", "00"},
                {"00", "10", "01", "11"},
                {"11", "01", "11", "10"}
            };

            SBox1 = new string[4, 4]{
                {"00", "01", "10", "11"},
                {"10", "00", "01", "11"},
                {"11", "00", "01", "00"},
                {"10", "01", "00", "11"}
            };
        }

        public void AsignarPermutaciones(string pathPermutaciones) //Necesita el path del archivo SDES.ini para obtener las permutaciones del archivo
        {
            try
            {
                List<string> lineasArchivo = new List<string>();

                using (var file = new FileStream(pathPermutaciones, FileMode.Open))
                {
                    using (var reader = new StreamReader(file))
                    {
                        while (!reader.EndOfStream)
                        {
                            lineasArchivo.Add(reader.ReadLine().Split('|')[1]);
                        }
                    }
                }


                string[] p10 = lineasArchivo[0].Split(' ');
                string[] pCompresion = lineasArchivo[1].Split(' ');
                string[] p8 = lineasArchivo[2].Split(' ');
                string[] expandirPermutar = lineasArchivo[3].Split(' ');
                string[] p4 = lineasArchivo[4].Split(' ');

                for (int i = 0; i < p10.Length; i++)
                {
                    P10[i] = int.Parse(p10[i]);
                }

                for (int i = 0; i < pCompresion.Length; i++)
                {
                    PCompresionKeys[i] = int.Parse(pCompresion[i]);
                }

                for (int i = 0; i < p8.Length; i++)
                {
                    P8[i] = int.Parse(p8[i]);
                }

                for (int i = 0; i < expandirPermutar.Length; i++)
                {
                    ExpandirPermutar[i] = int.Parse(expandirPermutar[i]);
                }

                for (int i = 0; i < p4.Length; i++)
                {
                    P4[i] = int.Parse(p4[i]);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Error en la asignacion de las permutaciones, verifique formato de archivo SDES.ini | " + e.Message);
            }
        }

        public string XOR(string cadenaBinario1, string cadenaBinario2)
        {
            try
            {
                var resultado = "";

                for (int i = 0; i < cadenaBinario1.Length; i++)
                {
                    resultado += cadenaBinario1[i] ^ cadenaBinario2[i];
                }

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("Error en XOR | " + e.Message);
            }
        }

        public void DividirCadenaBits(int longitudEsperada, string cadenaBinario, ref string ParteIzquierda, ref string ParteDerecha)
        {
            try
            {
                ParteIzquierda = cadenaBinario.Substring(0, longitudEsperada);
                ParteDerecha = cadenaBinario.Substring(longitudEsperada, longitudEsperada);
            }
            catch (Exception e)
            {
                throw new Exception("Error en dividir cadena | " + e.Message);
            }
        }

        public string LShift(string cadenaBinario)
        {
            try
            {
                var numeroDecimal = Convert.ToInt32(cadenaBinario, 2);

                var resultado1 = (numeroDecimal << 1);

                var resultado1Binario = Convert.ToString(resultado1, 2);

                if (resultado1Binario.Length > 5)
                {
                    resultado1Binario = resultado1Binario.Remove(0, resultado1Binario.Length - 5);
                }

                resultado1 = Convert.ToInt32(resultado1Binario, 2);

                var resultado2 = (numeroDecimal >> (5 - 1));

                var resultadoDesplazamiento = resultado1 | resultado2;

                var final = Convert.ToString(resultadoDesplazamiento, 2);

                return final = final.PadLeft(5, '0');
            }
            catch (Exception e)
            {
                throw new Exception("Error en L-Shift | " + e.Message);
            }
        }

        public int[] PermutacionInversa(int[] PermutacionInicial)
        {
            try
            {
                var resultado = new int[8];

                for (int i = 0; i < resultado.Length; i++)
                {
                    resultado[PermutacionInicial[i] - 1] = i + 1;
                }

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("Error en hacer la permutación inversa | " + e.Message);
            }
        }

        public void ObtenerFilaColumna(string cadena4bits, ref int fila, ref int columna)
        {
            //Fila = bit1 y bit4
            switch (cadena4bits[0].ToString() + cadena4bits[3].ToString())
            {
                case "00":
                    fila = 0;
                    break;

                case "01":
                    fila = 1;
                    break;

                case "10":
                    fila = 2;
                    break;

                case "11":
                    fila = 3;
                    break;
            }

            //Columna = bit2 y bit3
            switch (cadena4bits[1].ToString() + cadena4bits[2].ToString())
            {
                case "00":
                    columna = 0;
                    break;

                case "01":
                    columna = 1;
                    break;

                case "10":
                    columna = 2;
                    break;

                case "11":
                    columna = 3;
                    break;
            }
        }

        public string ObtenerBitsSBox0(int fila, int columna)
        {
            return SBox0[fila, columna];
        }

        public string ObtenerBitsSBox1(int fila, int columna)
        {
            return SBox1[fila, columna];
        }
    }
}
