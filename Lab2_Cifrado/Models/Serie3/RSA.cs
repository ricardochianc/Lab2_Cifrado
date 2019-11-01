using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using BibliotecaDeClases.Cifrado.RSA;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Models.Serie3
{
    public class RSA
    {
        //P y Q mayores que 0, 1 y 2... son los valores más faciles de descartar en vista, las otras validaciones se harán directo en el controlador
        [Display(Name = "Número P")]
        [Range(17, Int32.MaxValue, ErrorMessage = "P debe ser mayor a 3")]
        [Required(ErrorMessage = "Debe ingresar un valor P")]
        public int P { get; set; }

        [Display(Name = "Número Q")]
        [Range(17, Int32.MaxValue, ErrorMessage = "Q debe ser mayor a 3")]
        [Required(ErrorMessage = "Debe ingresar un valor P")]
        public int Q { get; set; }

        public string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaArchivoOperacional { get; set; } //Tendrá la ruta del archivo ya sea private.key o public.key
        private string RutaAbsolutaServer { get; set; }
        private string Extension { get; set; }

        public GenerarLlaves GeneradorLlaves { get; set; }

        public RSA()
        {
            P = 0;
            Q = 0;

            GeneradorLlaves = new GenerarLlaves(Data.Instancia.RutaAbsolutaServer);

            NombreArchivo = string.Empty;
            RutaAbsolutaArchivo = string.Empty;
            RutaAbsolutaArchivoOperacional = string.Empty;
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

        public void Operar(string rutaArchivoOperacional)
        {
            try
            {
                switch (Extension)
                {
                    case "txt": //Cifra

                        //CifradoSDES = new CifradoSDES(NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer, Clave, rutaArchivoPermutaciones);
                        //CifradoSDES.Cifrar();
                        break;

                    case "rsacif": //Descifra

                        //DescifradoSDES = new DescifradoSDES(NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer, Clave, rutaArchivoPermutaciones);
                        //DescifradoSDES.Descifrar();
                        break;

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public FileStream ArchivoResultante(ref string extension)
        {
            switch (Extension)
            {
                case "txt": //Devuelve uno cifrado
                    var pathtxt = RutaAbsolutaServer + NombreArchivo + ".rsacif";
                    var filescif = new FileStream(pathtxt, FileMode.Open, FileAccess.Read);
                    extension = ".scif";
                    return filescif;

                case "rsacif": //Devulve uno descifrado
                    var pathscif = RutaAbsolutaServer + NombreArchivo + ".txt";
                    var filetxt = new FileStream(pathscif, FileMode.Open, FileAccess.Read);
                    extension = ".txt";
                    return filetxt;
            }

            return null;
        }

        public FileStream ArchivoResultanteLlave(string tipoLlave)
        {
            switch (tipoLlave)
            {
                case "public":
                    var path = RutaAbsolutaServer + "public.key";
                    var file = new FileStream(path,FileMode.Open,FileAccess.Read);
                    return file;

                case "private":
                    var path2 = RutaAbsolutaServer + "private.key";
                    var file2 = new FileStream(path2, FileMode.Open, FileAccess.Read);
                    return file2;
            }

            return null;
        }

        public void Reset()
        {
            switch (Extension)
            {
                case "scif":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".txt");
                    //DescifradoSDES = new DescifradoSDES("", "", "", 0, "");
                    break;

                case "txt":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".rsacif");
                    //CifradoSDES = new CifradoSDES("", "", "", 0, "");
                    break;
            }

            Data.Instancia.ArchivoCargado = false;
            Data.Instancia.EleccionOperacion = false;
        }
    }
}