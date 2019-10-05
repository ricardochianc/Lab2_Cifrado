using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BibliotecaDeClases.Cifrado.Espiral
{
    public class Cifrado
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string DireccionRecorrido { get; set; }
        public int Clave { get; set; }
        public int posBufferEscritura { get; set; }

        const int largoBuffer = 100;
        private char[] bufferEscritura = new char[largoBuffer];
        private char[] bufferLectura = new char[largoBuffer];

        int pos = 0;
        char[,] matriz;

        public Cifrado(int clave, string direccion, string nombreArchivo, string rutaAbsoluta, string rutaServer)
        {
            Clave = clave;
            DireccionRecorrido = direccion.ToUpper();
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = rutaAbsoluta;
            RutaAbsolutaServer = rutaServer;
            posBufferEscritura = 0;
        }

        public void Cifrar()
        {
            using(var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using(var reader = new BinaryReader(file, Encoding.UTF8))
                {
                    while(reader.BaseStream.Position != reader.BaseStream.Length)
                    {                        
                        bufferLectura = reader.ReadChars(largoBuffer);
                        matriz = new char[Clave, CalculoFilas(Clave, largoBuffer)];

                        posBufferEscritura = 0;

                        var sigValorColumna = matriz.GetLength(0);
                        var sigValorFila = matriz.GetLength(1);
                        var xarab = 0;
                        var xabar = 0;
                        var yderiz = 0;
                        var yizder = 0;

                        var area = matriz.GetLength(0) * matriz.GetLength(1);

                        //SE RELLENAN TODOS LOS ESPACIOS DE LA MATRIZ CON LOS CARACTERES Y LOS QUE QUEDEN VACIOS SE RELLENAN CON EL CARACTER $
                        for (int j = 0; j < matriz.GetLength(1); j++)
                        {
                            for (int i = 0; i < matriz.GetLength(0); i++)
                            {
                                if(pos != bufferLectura.Length)
                                {
                                    if(bufferLectura[pos] != '\r')
                                    {
                                        matriz[i,j] = bufferLectura[pos];
                                        pos++;
                                    }                                    
                                }
                                else
                                {
                                    matriz[i,j] = '$';
                                }                    
                            }
                        }

                        //RECORRIDO DE LA MATRIZ M*N PARA OBTENER EL TEXTO CIFRADO
                        switch (DireccionRecorrido)
                        {
                            case "D":

                                xabar = 0;
                                xarab = matriz.GetLength(0) - 1;
                                yizder = 0;
                                yderiz = matriz.GetLength(1) - 1;                    
                    
                                while(area > posBufferEscritura)
                                {
                                    //DE IZQUIERDA A DERECHA
                                    if(area > posBufferEscritura) //area > textoCifrado.Length
                                    {
                                        for (int i = yizder; i < sigValorColumna; i++)
                                        {
                                            //textoCifrado += matriz[i, yizder];
                                            bufferEscritura[posBufferEscritura] = matriz[i, yizder];
                                            posBufferEscritura++;
                                        }

                                        yizder++;
                                    }

                                    //DE ARRIBA HACIA ABAJO
                                    if(area > posBufferEscritura)
                                    {
                                        for (int j = yizder; j < sigValorFila; j++)
                                        {
                                            //textoCifrado += matriz[xarab,j];
                                            bufferEscritura[posBufferEscritura] = matriz[xarab, j];
                                            posBufferEscritura++;
                                        }

                                        xarab--;
                                        sigValorColumna -= 1;
                                    }                    
                    
                                    //DE DERECHA A IZQUIERDA
                                    if(area > posBufferEscritura)
                                    {
                                        for (int i = xarab; i >= xabar; i--)
                                        {
                                            //textoCifrado += matriz[i, yderiz];
                                            bufferEscritura[posBufferEscritura] = matriz[i, yderiz];
                                            posBufferEscritura++;
                                        }

                                        yderiz--;
                                        sigValorFila -= 1;
                                    }                    

                                    //DE ABAJO HACIA ARRIBA
                                    if(area > posBufferEscritura)
                                    {
                                        for (int j = yderiz; j >= yizder; j--)
                                        {
                                            //textoCifrado += matriz[xabar,j];
                                            bufferEscritura[posBufferEscritura] = matriz[xabar, j];
                                            posBufferEscritura++;
                                        }

                                        xabar++;
                                    }
                                }

                                break;
                            case "I":

                                xarab = 0;
                                xabar = matriz.GetLength(0) - 1;
                                yderiz = 0;
                                yizder = matriz.GetLength(1) - 1;

                                while(area > posBufferEscritura)
                                {               
                                    //DE ARRIBA HACIA ABAJO
                                    if(area > posBufferEscritura)
                                    {
                                        for (int j = yderiz; j < sigValorFila; j++)
                                        {
                                            //textoCifrado += matriz[xarab,j];
                                            bufferEscritura[posBufferEscritura] = matriz[xarab, j];
                                            posBufferEscritura++;
                                        }

                                        xarab++;
                                    }                    

                                    //DE IZQUIERDA A DERECHA
                                    if(area > posBufferEscritura)
                                    {
                                        for (int i = xarab; i < sigValorColumna; i++)
                                        {
                                            //textoCifrado += matriz[i, yizder];
                                            bufferEscritura[posBufferEscritura] = matriz[i, yizder];
                                            posBufferEscritura++;
                                        }

                                        yizder--;
                                    }                    
                    
                                    //DE ABAJO HACIA ARRIBA
                                    if(area > posBufferEscritura)
                                    {
                                        for (int j = yizder; j >= yderiz; j--)
                                        {
                                            //textoCifrado += matriz[xabar,j];
                                            bufferEscritura[posBufferEscritura] = matriz[xabar, j];
                                            posBufferEscritura++;
                                        }

                                        xabar--;
                                        sigValorColumna -= 1;
                                    }                    

                                    //DE DERECHA A IZQUIERDA
                                    if(area > posBufferEscritura)
                                    {
                                        for (int i = xabar; i >= xarab; i--)
                                        {
                                            //textoCifrado += matriz[i, yderiz];
                                            bufferEscritura[posBufferEscritura] = matriz[i, yderiz];
                                            posBufferEscritura++;
                                        }

                                        yderiz++;
                                        sigValorFila -= 1;
                                    }
                                }
                                break;
                        }

                        EscribirBuffer();
                    }
                }
            }
        }

        public void EscribirBuffer()
        {
            using(var file = new FileStream(RutaAbsolutaServer + NombreArchivo + ".cif", FileMode.Append))
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

        public int CalculoFilas(int columnas, int longitudTexto)
        {
            var filas = 0;

            filas = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(longitudTexto)/Convert.ToDouble(columnas)));

            return filas;
        }
        //PARA GUARDAR EL ARCHIVO rutaServer + nombreArchivo + ".cif"
    }
}
