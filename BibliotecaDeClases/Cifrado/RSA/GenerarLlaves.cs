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

        //previamente se va a verificar que sean primos y coprimos entre sí, entonces se asume que ya vienen primos
        public bool GenerarClaves(int numeroP, int numeroQ)
        {
            var correcto = false;

            PrimoP = numeroP;
            PrimoQ = numeroQ;

            ModuloN = PrimoP * PrimoQ; //n = p*q

            Phi = (PrimoP - 1) * (PrimoQ - 1); //phi = (p-1)(q-1)

            PrimoE = GenerarE();

            //InversoModularD = GenerarInversoD();

            //Escribir archivos

            return correcto;
        }


        //Métodos privados que sirven para verificar los números primos, máximo común divisor y generar e
        private bool EsPrimo(int numero)
        {
            for (int i = 2; i <= Math.Sqrt(numero); i++)
            {
                if (numero % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private int MCD(int numero1, int numero2)
        {
            if (numero1 < numero2)
            {
                MCD(numero2, numero1);
            }

            while (numero2 != 0)
            {
                var r = numero1 % numero2;
                numero1 = numero2;
                numero2 = r;
            }
            return numero1;
        }

        private int GenerarE()
        {
            var numeroE = 0;
            var listadoPosibles = new List<int>(); //Para que E no sea repetido, las diferentes veces que se use el cifrado. Se implementó una lista que guardará los posibles E
            //Luego se elegirá uno al azar y ese será elegido como E

            //1 < e < Phi y a parte debe ser primo...
            for (int i = 3; i < Phi; i++)
            {
                //Se descartan los pares y debe ser diferente a P y Q...
                if (i % 2 != 0 && i != PrimoP && i != PrimoQ)
                {
                    for (int j = 3; j < i; j++) // i propone número y j trata de dividirlo para saber si es primo
                    {
                        //Se verifica si es primo
                        if ((i % j != 0))
                        {
                            //Phi no puede ser divisible dentro de E (j es nuestro candidato a E) y E debe ser CoPrimo de Phi
                            if ((Phi % j != 0) && (MCD(Convert.ToInt32(Phi), j) == 1))
                            {
                                listadoPosibles.Add(i);
                                
                                if (listadoPosibles.Count == 10)
                                {
                                    i = Convert.ToInt32(Phi);
                                }
                            }
                        }
                        else
                        {
                            j = i;
                        }

                        
                    }
                }
            }

            var randomPosicion = new Random();
            numeroE = listadoPosibles[randomPosicion.Next(0, listadoPosibles.Count - 1)];

            return numeroE;
        }
    }
}
