using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BibliotecaDeClases.Cifrado.Espiral;

namespace Lab2_Cifrado.Models.Serie1
{
    public class Espiral
    {
        [Display(Name = "Clave")]
        [Range(0,Int32.MaxValue,ErrorMessage = "El valor {0} no es válido")]
        [Required(ErrorMessage = "Debe de ingresar la clave para esta operacion")]
        public int Clave { get; set; }

        [Display(Name = "Recorrido Inicial")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Debe de ingresar una clave para esta operacion")]
        public string DireccionRecorrido { get; set; }

        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        private string Extension { get; set; }

        private Cifrado CifradoEspiral { get; set; }
        private Descifrado DescifradoEspiral { get; set; }

        public Espiral()
        {
            Clave = 0;
            DireccionRecorrido = string.Empty;
            NombreArchivo = string.Empty;
            RutaAbsolutaArchivo = string.Empty;;
            RutaAbsolutaServer = string.Empty;
        }

        public void AsignarExtension(string ext)
        {
            Extension = ext;
        }

        public void AsignarRutas(string rutaAbsServer, string rutaAbsArchivo, string nombreArchivo)
        {
            NombreArchivo = nombreArchivo;
            RutaAbsolutaArchivo = rutaAbsArchivo;
            RutaAbsolutaServer = rutaAbsServer;
        }

        public void Operar()
        {
            switch (Extension)
            {
                case "txt":
                    CifradoEspiral = new Cifrado(Clave, DireccionRecorrido, NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer);
                    CifradoEspiral.Cifrar();
                    break;

                case "cif":
                    DescifradoEspiral = new Descifrado(Clave, DireccionRecorrido, NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer);
                    DescifradoEspiral.Descifrar();
                    break;
            }
        }
    }
}