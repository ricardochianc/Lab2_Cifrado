using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab2_Cifrado.Models.Serie1;
using Lab2_Cifrado.Models.Serie2;

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

        public bool primeraVez = true;
        public bool ArchivoCargado = false;
        public bool EleccionOperacion = false;
        
        public string RutaAbsolutaServer { get; set; }

        public ZigZag ZigZagCif = new ZigZag();
        public Espiral EspiralCif = new Espiral();
        public Cesar CesarCif = new Cesar();

        public SDES SDES_Cif = new SDES();
        public string RutaPermutaciones = string.Empty;
        public bool ModificarPermutaciones = false;
    }
}