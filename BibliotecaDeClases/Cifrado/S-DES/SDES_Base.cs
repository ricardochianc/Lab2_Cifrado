using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.S_DES
{
    internal class SDES_Base
    {
        private int[] P10 { get; set; } //Permunataciones para llave de 10 bits
        private int[] PCompresionKeys { get; set; } //Permutacion de compresion para K1 y K2 (no incluyen el bit1 y bit2)
        private int[] P8 { get; set; } //Permutacion inicial de 8 bits para texto plano
        private int[] ExpandirPermutar { get; set; } //Expandir y permutar (del 1 al 4, se repite 2 veces para una longitud total de 8 bits)
        private int[] P4 { get; set; } //Permutancion de 4 bits

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
    }
}
