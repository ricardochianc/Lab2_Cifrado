﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using BibliotecaDeClases.Cifrado.S_DES;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Models.Serie2
{
    public class SDES
    {
        [Display(Name = "Clave")]
        [Range(0,1023,ErrorMessage = "El valor de la clave debe ser menor a 1023")]
        [Required(ErrorMessage = "Debe ingresar una clave para realizar su operación")]
        public int Clave { get; set; }

        public string NombreArchivo { get; set; }
        private string RutaAbsolutaArchivo { get; set; }
        private string RutaAbsolutaServer { get; set; }
        private string Extension { get; set; }

        private CifradoSDES CifradoSDES { get; set; } //agregar uno igual solo que de DESCIFRADO
        private DescifradoSDES DescifradoSDES { get; set; }

        public SDES()
        {
            Clave = 0;
            NombreArchivo = string.Empty;
            RutaAbsolutaArchivo = string.Empty; ;
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

        public void Operar(string rutaArchivoPermutaciones)
        {
            try
            {
                switch (Extension)
                {
                    case "txt": //Cifra
                        CifradoSDES = new CifradoSDES(NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer, Clave, rutaArchivoPermutaciones);
                        CifradoSDES.Cifrar();
                        break;

                    case "scif": //Descifra
                        DescifradoSDES = new DescifradoSDES(NombreArchivo, RutaAbsolutaArchivo, RutaAbsolutaServer, Clave, rutaArchivoPermutaciones);
                        DescifradoSDES.Descifrar();
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
                    var pathtxt = RutaAbsolutaServer + NombreArchivo + ".scif";
                    var filescif = new FileStream(pathtxt, FileMode.Open, FileAccess.Read);
                    extension = ".scif";
                    return filescif;

                case "scif": //Devulve uno descifrado
                    var pathscif = RutaAbsolutaServer + NombreArchivo + ".txt";
                    var filetxt = new FileStream(pathscif, FileMode.Open, FileAccess.Read);
                    extension = ".txt";
                    return filetxt;
            }

            return null;
        }

        public void Reset()
        {
            switch (Extension)
            {
                case "scif":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".txt");
                    DescifradoSDES = new DescifradoSDES("","","",0,"");
                   break;

                case "txt":
                    File.Delete(RutaAbsolutaServer + NombreArchivo + ".scif");
                    CifradoSDES = new CifradoSDES("","","",0,"");
                    break;
            }

            Data.Instancia.ArchivoCargado = false;
            Data.Instancia.EleccionOperacion = false;
        }
    }
}