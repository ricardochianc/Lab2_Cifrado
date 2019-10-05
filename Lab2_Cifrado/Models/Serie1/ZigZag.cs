using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using BibliotecaDeClases.Cifrado.ZigZag;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Models.Serie1
{
    public class ZigZag
    {
        [Display(Name = "Clave/Niveles")]
        [Range(0,Int32.MaxValue,ErrorMessage = "El valor {0} no es válido")]
        [Required(ErrorMessage = "Debe de ingresar una clave para este cifrado")]
        public int Clave { get; set; }
        
        public string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        private string Extension { get; set; }

        private ZigZagCifrado CifradoZigZag { get; set; }
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
                    CifradoZigZag = new ZigZagCifrado(NombreArchivo,RutaAbsolutaArchivo,RutaAbsolutaServer,Clave);
                    CifradoZigZag.Cifrar();
                    break;

                case "cif":
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
                    //DescifradoZigZag = new Descifrado();
                    break;

                case "txt":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".cif");
                    CifradoZigZag = new ZigZagCifrado("", "", "", 0);
                    break;
            }
            Data.Instancia.ArchivoCargado = false;
            Data.Instancia.EleccionOperacion = false;
        }
    }
}