using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using BibliotecaDeClases.Cifrado.Cesar;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Models.Serie1
{
    public class Cesar
    {
        [Display(Name = "Clave")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Debe de ingresar una clave para esta operacion")]
        public string PalabraClave { get; set; }

        public string NombreArchivo { get; set; }
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
            var valida = false;

            switch (Extension)
            {
                case "txt":

                    CesarCif = new CifradoCesar(PalabraClave, NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer);

                    valida = CesarCif.ValidarClave(PalabraClave.ToCharArray());
                    if (valida)
                    {                        
                        CesarCif.Cifrar();
                    }                    
                    break;

                case "cif":

                    CesarDescif = new DescifradoCesar(PalabraClave, NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer);

                    valida = CesarDescif.ValidarClave(PalabraClave.ToCharArray());
                    if (valida)
                    {                        
                        CesarDescif.Descifrar();
                    }
                    break;
            }
        }

        //BUSCA DENTRO DE LA CARPETA DE "MisCifrados" DEL SERVER EL ARCHIVO PARA DÁRSELO AL USUARIO
        public FileStream ArchivoResultante(ref string extension)
        {
            switch (Extension)
            {
                case "txt":
                    var path = RutaAbsolutaServer + NombreArchivo + ".cif";
                    var file = new FileStream(path,FileMode.Open,FileAccess.Read);
                    extension = ".cif";
                    return file;

                case "cif":
                    var path2 = RutaAbsolutaServer + NombreArchivo + ".txt";
                    var file2 = new FileStream(path2, FileMode.Open, FileAccess.Read);
                    extension = ".txt";
                    return file2;
            }
            return null;
        }


        //Reset, para cuando se le da home y que vuelva instanciar
        public void Reset()
        {
            switch (Extension)
            {
                case "cif":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".txt");
                    CesarDescif = new DescifradoCesar("","","","");
                    break;

                case "txt":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".cif");
                    CesarCif = new CifradoCesar("", "", "", "");
                    break;
            }
            Data.Instancia.ArchivoCargado = false;
            Data.Instancia.EleccionOperacion = false;
        }
    }
}