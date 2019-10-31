using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Cifrado.RSA
{
    public class GenerarLlaves
    {
        //Basando se en la ecuación n=p*q
        private decimal PrimoP { get; set; } //p
        private decimal PrimoQ { get; set; } //q

        public decimal ModuloN { get; set; } //n
        public decimal PrimoE { get; set; } //e
        private decimal Phi { get; set; } // phi = (p-1)(q-1)
        public decimal InversoModularD { get; set; } //d

        private string RutaAbsolutaServer { get; set; }

        public GenerarLlaves(string rutaServer)
        {
            RutaAbsolutaServer = rutaServer;
        }

        public static bool EsPrimo(int n)
        {
            var _esPrimo = true;

            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    _esPrimo = false;
                }
            }

            return _esPrimo;
        }
    }
}
