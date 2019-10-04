using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BibliotecaDeClases.Cifrado.ZigZag;

namespace Lab2_Cifrado.Models.Serie1
{
    public class ZigZag
    {
        [Display(Name = "Clave/Niveles")]
        [Range(0,Int32.MaxValue,ErrorMessage = "El valor {0} no es válido")]
        [Required(ErrorMessage = "Debe de ingresar una clave para este cifrado")]
        public int Clave { get; set; }
        
        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        private string Extension { get; set; }

        private Cifrado CifradoZigZag { get; set; }
        //Poner propiedad de descifrado

        public ZigZag()
        {
            Clave = 0;
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
                    CifradoZigZag = new Cifrado(NombreArchivo,RutaAbsolutaArchivo,RutaAbsolutaServer,Clave);
                    break;

                case "cif":
                    break;
            }
        }

    }
}