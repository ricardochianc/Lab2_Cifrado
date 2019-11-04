using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace BibliotecaDeClases.Cifrado.RSA
{
	public class Operaciones
	{

		private string NombreArchivo { get; set; }
		private string RutaAbsolutaArchivo { get; set; }
		private string RutaAbsolutaServer { get; set; }
		public string RutaAbsolutaArchivoRSACif { get; set; } //Para el .rsacif
		public string RutaArchivoLlave { get; set; }

		//Esto se leerá en el archivo *.key que ingrese el usario, independientemente de qué llave ingrese
		private BigInteger Modulo { get; set; }
		private BigInteger Llave { get; set; }

		private const int LargoBuffer = 100;

		public Operaciones(string nombreArchivo, string RutaAbsArchivo, string RutaAbsServer, string rutaLlave)
		{
			NombreArchivo = nombreArchivo;
			RutaAbsolutaArchivo = RutaAbsArchivo;
			RutaAbsolutaServer = RutaAbsServer;
			RutaAbsolutaArchivoRSACif = "";
			RutaArchivoLlave = rutaLlave;
		}

		private void LeerLlave()
		{
			using (var file = new FileStream(RutaArchivoLlave, FileMode.Open))
			{
				using (var reader = new StreamReader(file, Encoding.UTF8))
				{
					//Campos linea va a leer la linea y de una vez separarlos
					var camposLinea = reader.ReadLine().Split(',');
					Modulo = BigInteger.Parse(camposLinea[0]);
					Llave = BigInteger.Parse(camposLinea[1]);
				}
			}

			File.Delete(RutaArchivoLlave);
		}

		public void OperarRSA(string extension)
		{
			if (extension == "rsacif")
			{
				RutaAbsolutaArchivoRSACif = RutaAbsolutaServer + NombreArchivo + ".txt";
			}
			else if (extension == "txt")
			{
				RutaAbsolutaArchivoRSACif = RutaAbsolutaServer + NombreArchivo + ".rsacif";
			}
			
			LeerLlave();

			var buffer = new char[LargoBuffer];
			var bufferEscritura = new char[LargoBuffer];

			using (var file = new FileStream(RutaAbsolutaArchivo, FileMode.Open))
			{
				using (var reader = new StreamReader(file, Encoding.UTF8))
				{
					while (reader.BaseStream.Position != reader.BaseStream.Length)
					{
						reader.Read(buffer,0,LargoBuffer);

						var contBuffer = 0;

						foreach (var caracter in buffer)
                        {
                            //Recordando de N = C^d mod n && C = N^e mod n
                            var caracterByte = (int) caracter;
                            var caracterCifrado = BigInteger.ModPow(caracterByte, Llave, Modulo);

                            //var potencia = Potencia(Convert.ToInt32(caracter), Convert.ToInt32(Llave));

							//var Caractercifrado = potencia % Modulo;

							bufferEscritura[contBuffer] = (char)caracterCifrado;
							contBuffer++;
						}
						EscribirBuffer(bufferEscritura);
					}
				}
			}

			File.Delete(RutaArchivoLlave);
			File.Delete(RutaAbsolutaArchivo);
		}

		private double Potencia(int numBase, int exponente)
		{
			double respuesta = 1;

			for (int i = 0; i < exponente; i++)
			{
				respuesta *= numBase;
			}

			return respuesta;
		}

		private void EscribirBuffer(char[] buffer)
		{
			using (var file = new FileStream(RutaAbsolutaArchivoRSACif, FileMode.Append))
			{
				using (var writer = new StreamWriter(file, Encoding.UTF8))
				{
					writer.Write(buffer);
				}
			}
		}

	}
}
