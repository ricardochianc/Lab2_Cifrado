﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BibliotecaDeClases.Cifrado.Cesar
{
    public class CifradoCesar
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string DireccionRecorrido { get; set; }
        public string Clave { get; set; }
        public int posBufferEscritura { get; set; }

        const int largoBuffer = 100;
        private byte[] bufferEscritura = new byte[largoBuffer];
        //private char[] bufferLectura = new char[largoBuffer];
        private byte[] buffLect = new byte[largoBuffer];

        public CifradoCesar(string clave, string nombreArchivo, string rutaAbsoluta, string rutaServer)
        {
            Clave = clave.ToLower();
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

        public void Cifrar()
        {
            //var valida = false;
            var alfabeto = "0123456789abcdefghijklmnopqrstuvwxyz";
            var pos = Clave.Length;
            char[] alfabetoCifrado = new char[alfabeto.Length];

            //AL ALFABETO CIFRADO LE METO LA PALABRA Clave
            for (int i = 0; i < Clave.Length; i++)
            {
                alfabetoCifrado[i] = Clave[i];
            }

            //AL ALFABETO CIFRADO SE METEN LOS CARACTERES DEL ALFABETO NORMAL QUE NO APAREZCAN DENTRO DE LA PALABRA Clave
            for (int i = 0; i < alfabeto.Length; i++)
            {
                if (alfabetoCifrado.Contains(alfabeto[i]))
                {

                }
                else
                {
                    alfabetoCifrado[pos] = alfabeto[i];
                    pos++;
                }                         
            }

            using(var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using(var reader = new BinaryReader(file, Encoding.UTF8))
                {
                    while(reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        //bufferLectura = new char[largoBuffer];
                        bufferEscritura = new byte[largoBuffer];
                        buffLect = new byte[largoBuffer];

                        buffLect = reader.ReadBytes(largoBuffer);
                        //bufferLectura = reader.ReadChars(largoBuffer);
                        
                        posBufferEscritura = 0;

                        for (int i = 0; i < buffLect.Length; i++)
                        {
                            for (int j = 0; j < alfabeto.Length; j++)
                            {
                                if (alfabeto.Contains((char)buffLect[i]))
                                {
                                    if((char)buffLect[i] == alfabeto[j])
                                    {
                                        bufferEscritura[posBufferEscritura] += (byte)alfabetoCifrado[j];
                                        posBufferEscritura++;
                                        j = alfabeto.Length;
                                    }
                                }
                                else
                                {
                                    bufferEscritura[posBufferEscritura] += (byte)buffLect[i];
                                    posBufferEscritura++;
                                    j = alfabeto.Length;
                                }
                            }
                        }

                        EscribirBuffer();
                    }
                }
            }
            File.Delete(RutaAbsolutaArchivo);
        }

        private void EscribirBuffer()
        {
            using(var file = new FileStream(RutaAbsolutaServer + NombreArchivo + ".cif", FileMode.Append))
            {
                using(var writer = new BinaryWriter(file, Encoding.UTF8))
                {
                    for (int i = 0; i < posBufferEscritura; i++)
                    {
                        //if(bufferEscritura[i] == '\n')
                        //{
                        //    writer.Write(Environment.NewLine);
                        //}
                        //else
                        //{
                            
                        //}
                        writer.Write(bufferEscritura[i]);
                    }
                }
            }
        }
    }
}
