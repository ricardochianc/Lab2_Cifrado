using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BibliotecaDeClases.Cifrado.Cesar;

namespace Lab2_Cifrado.Models.Serie1
{
    public class Cesar
    {
        [Display(Name = "Clave")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Debe de ingresar una clave para esta operacion")]
        public string PalabraClave { get; set; }

        private string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        private string Extension { get; set; }

        private CifradoCesar CesarCif { get; set; }
        private DescifradoCesar CesarDescif { get; set; }

        public Cesar()
        {
            PalabraClave = string.Empty;
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
                    CesarCif = new CifradoCesar(PalabraClave, NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer);
                    CesarCif.Cifrar();
                    break;

                case "cif":
                    CesarDescif = new DescifradoCesar(PalabraClave, NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer);
                    CesarDescif.Descifrar();
                    break;
            }
        }
    }
}