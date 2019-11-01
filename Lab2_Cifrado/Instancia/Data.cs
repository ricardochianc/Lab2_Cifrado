using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab2_Cifrado.Models.Serie1;
using Lab2_Cifrado.Models.Serie2;
using Lab2_Cifrado.Models.Serie3;

namespace Lab2_Cifrado.Instancia
{
    public class Data
    {
        private static Data _instancia = null;

        public static Data Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Data();
                }

                return _instancia;
            }
        }

        //GENERAL DE TODO EL LAB
        public bool primeraVez = true;
        public bool ArchivoCargado = false;
        public bool EleccionOperacion = false;
        
        public string RutaAbsolutaServer { get; set; }

        //SERIE 1
        public ZigZag ZigZagCif = new ZigZag();
        public Espiral EspiralCif = new Espiral();
        public Cesar CesarCif = new Cesar();

        //SERIE 2
        public SDES SDES_Cif = new SDES();
        public string RutaPermutaciones = string.Empty;
        public bool ModificarPermutaciones = false;

        //SERIE 3
        public RSA RSA_Cif{ get; set; }
        public bool GenerarLlaves = false;
    }
}