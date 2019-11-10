using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BibliotecaDeClases.Cifrado.Espiral
{
    public class Descifrado
    {
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        public string DireccionRecorrido { get; set; }
        public int Clave { get; set; }
        public int posBufferEscritura { get; set; }

        const int largoBuffer = 100;
        private byte[] bufferEscritura = new byte[largoBuffer];
        private byte[] bufferLectura = new byte[largoBuffer];

        char[,] matriz;

        public Descifrado(int clave, string direccion, string nombreArchivo, string rutaAbsoluta, string rutaServer)
        {
            Clave = clave;
            DireccionRecorrido = direccion.ToUpper();
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = rutaAbsoluta;
            RutaAbsolutaServer = rutaServer;
            posBufferEscritura = 0;
        }
        
        public void Descifrar()
        {
            var pos = 0;
            var sigValorColumna = 0;
            var sigValorFila = 0;
            var xarab = 0;
            var xabar = 0;
            var yderiz = 0;
            var yizder = 0;

            var carsEnMatriz = 0;
            var area = 0;

            using(var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
            {
                using(var reader = new BinaryReader(file, Encoding.UTF8))
                {
                    while(reader.BaseStream.Position != reader.BaseStream.Length)
                    {  
                        bufferLectura = new byte[largoBuffer];
                        bufferEscritura = new byte[largoBuffer];

                        bufferLectura = reader.ReadBytes(largoBuffer);
                        matriz = new char[CalculoColumnas(Clave, largoBuffer), Clave];
                        
                        area = matriz.GetLength(0) * matriz.GetLength(1);

                        sigValorColumna = matriz.GetLength(0);
                        sigValorFila = matriz.GetLength(1);
                        
                        posBufferEscritura = 0;                        
                        pos = 0;

                        switch (DireccionRecorrido)
                        {
                            case "D":

                                xarab = 0;
                                xabar = matriz.GetLength(0) - 1;
                                yderiz = 0;
                                yizder = matriz.GetLength(1) - 1;

                                while(carsEnMatriz < matriz.Length && pos < bufferLectura.Length)
                                {               
                                    //DE ARRIBA HACIA ABAJO
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int j = yderiz; j < sigValorFila; j++)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                //if(bufferLectura[pos] != 65279)
                                                //{
                                                //    matriz[xarab,j] = bufferLectura[pos];
                                                //    pos++;
                                                //    carsEnMatriz++;
                                                //}
                                                //else
                                                //{
                                                    matriz[xarab,j] = (char)bufferLectura[pos];
                                                    pos ++;
                                                    carsEnMatriz++;
                                                //}                                                
                                            }                                
                                        }

                                        xarab++;
                                    }

                                    //DE IZQUIERDA A DERECHA
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int i = xarab; i < sigValorColumna; i++)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                matriz[i,yizder] = (char)bufferLectura[pos];
                                                pos++;
                                                carsEnMatriz++;
                                            }                                
                                        }

                                        yizder--;
                                    }                    
                    
                                    //DE ABAJO HACIA ARRIBA
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int j = yizder; j >= yderiz; j--)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                matriz[xabar,j] = (char)bufferLectura[pos];
                                                pos++;
                                                carsEnMatriz++;
                                            }                                
                                        }

                                        xabar--;
                                        sigValorColumna -= 1;
                                    }                    

                                    //DE DERECHA A IZQUIERDA
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int i = xabar; i >= xarab; i--)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                matriz[i,yderiz] = (char)bufferLectura[pos];
                                                pos++;
                                                carsEnMatriz++;
                                            }                                
                                        }

                                        yderiz++;
                                        sigValorFila -= 1;
                                    }
                                }

                                break;
                            case "I":

                                xabar = 0;
                                xarab = matriz.GetLength(0) - 1;
                                yizder = 0;
                                yderiz = matriz.GetLength(1) - 1;                    

                                while(carsEnMatriz < matriz.Length && pos < bufferLectura.Length)
                                {               
                                    //DE IZQUIERDA A DERECHA
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int i = yizder; i < sigValorColumna; i++)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                //if(bufferLectura[pos] != 65279)
                                                //{
                                                //    matriz[i,yizder] = bufferLectura[pos];
                                                //    pos++;
                                                //    carsEnMatriz++;
                                                //}
                                                //else
                                                //{
                                                    matriz[i,yizder] = (char)bufferLectura[pos];
                                                    pos++;
                                                    carsEnMatriz++;
                                                //}                                                
                                            }                                
                                        }

                                        yizder++;
                                    }

                                    //DE ARRIBA HACIA ABAJO
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int j = yizder; j < sigValorFila; j++)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                matriz[xarab,j] = (char)bufferLectura[pos];
                                                pos++;
                                                carsEnMatriz++;
                                            }                                
                                        }

                                        xarab--;
                                        sigValorColumna -= 1;
                                    }                    
                    
                                    //DE DERECHA A IZQUIERDA
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int i = xarab; i >= xabar; i--)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                matriz[i,yderiz] = (char)bufferLectura[pos];
                                                pos++;
                                                carsEnMatriz++;
                                            }                                
                                        }

                                        yderiz--;
                                        sigValorFila -= 1;
                                    }                    

                                    //DE ABAJO HACIA ARRIBA
                                    if(pos < bufferLectura.Length)
                                    {
                                        for (int j = yderiz; j >= yizder; j--)
                                        {
                                            if(pos < bufferLectura.Length)
                                            {
                                                matriz[xabar,j] = (char)bufferLectura[pos];
                                                pos++;
                                                carsEnMatriz++;
                                            }                                
                                        }

                                        xabar++;
                                    }
                                }

                                break;
                        }

                        //SI DESPUES DE ESCRIBIR TODO EL TEXTO CIFRADO, LA MATRIZ SIGUE TENIENDO ESPACIOS VACIOS, ENTONCES ESTOS SE RELLENAN
                        if(pos == bufferLectura.Length && carsEnMatriz < matriz.Length)
                        {
                            for (int i = 0; i < matriz.GetLength(0); i++)
                            {
                                for (int j = 0; j < matriz.GetLength(1); j++)
                                {
                                    if(matriz[i,j] == '\0')
                                    {
                                        matriz[i,j] = '$';
                                    }
                                }
                            }
                        }

                        //SE RECORRE LA MATRIZ PARA DESCIFRAR EL TEXTO
                        for (int i = 0; i < matriz.GetLength(0); i++)
                        {
                            for (int j = 0; j < matriz.GetLength(1); j++)
                            {
                                if(posBufferEscritura >= largoBuffer)
                                {
                                    EscribirBuffer();
                                    posBufferEscritura = 0;
                                }

                                //if(posBufferEscritura < largoBuffer)
                                //{
                                    
                                //}
                                bufferEscritura[posBufferEscritura] = (byte)matriz[i,j];
                                posBufferEscritura++;
                            }
                        }

                        EscribirBuffer();

                    } 
                } 
            }
            File.Delete(RutaAbsolutaArchivo);
        }
        
        //PARA GUARDAR EL ARCHIVO rutaServer + nombreArchivo + ".txt"
        public void EscribirBuffer()
        {
            using(var file = new FileStream(RutaAbsolutaServer + NombreArchivo + ".txt", FileMode.Append))
            {
                using(var writer = new BinaryWriter(file, Encoding.UTF8))
                {
                    for (int i = 0; i < bufferEscritura.Length; i++)
                    {
                        //if(bufferEscritura[i] == '\n')
                        //{
                        //    writer.Write(Environment.NewLine);
                        //}
                        //else if(bufferEscritura[i] == '$')
                        //{
                        //    writer.Write(" ");
                        //}
                        //else
                        //{
                        //    writer.Write(bufferEscritura[i]);
                        //}
                        if((char)bufferEscritura[i] != '$')
                        {
                            writer.Write(bufferEscritura[i]);
                        }                        
                    }
                }
            }
        }
        
        private int CalculoColumnas(int filas, int longitudTexto)
        {
            var columnas = 0;
            
            columnas = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(longitudTexto)/Convert.ToDouble(filas)));

            return columnas;
        }
    }
}
