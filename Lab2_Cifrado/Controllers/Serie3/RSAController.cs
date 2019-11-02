﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers.Serie3
{
    public class RSAController : Controller
    {
        // GET: RSA
        public ActionResult IndexRSA()
        {
            //Verifica si es necesario borrar los archivos de llaves del servidor
            if (Data.Instancia.DescargarLlaves == false && System.IO.File.Exists(Data.Instancia.RutaAbsolutaServer + "private.key") && System.IO.File.Exists(Data.Instancia.RutaAbsolutaServer + "public.key"))
            {
                System.IO.File.Delete(Data.Instancia.RutaAbsolutaServer + "private.key");
                System.IO.File.Delete(Data.Instancia.RutaAbsolutaServer + "public.key");
                Data.Instancia.ExisteError = false; //Se niega para que la vista no muestre la ventana emergente de errores
            }

            return View();
        }

        [HttpPost]
        public ActionResult GenerarLlavesRSA(FormCollection collection)
        {
            try
            {
                var p = int.Parse(collection["P"]);
                var q = int.Parse(collection["Q"]);

                if (p > 16 && q > 16)
                {
                    if (!Data.Instancia.RSA_Cif.GeneradorLlaves.EsPrimo(p) ||
                        !Data.Instancia.RSA_Cif.GeneradorLlaves.EsPrimo(q))
                    {
                        Data.Instancia.ExisteError = true;
                        Data.Instancia.Error = 1;
                    }
                    else if (Data.Instancia.RSA_Cif.GeneradorLlaves.MCD(p, q) != 1
                    ) //Se está validando de P y Q sean coprimos
                    {
                        Data.Instancia.ExisteError = true;
                        Data.Instancia.Error = 2;
                    }

                    if (Data.Instancia.ExisteError != true)
                    {
                        var path = Data.Instancia.RutaAbsolutaServer;
                        Data.Instancia.RSA_Cif.GeneradorLlaves.AsignarRutaRaiz(path);

                        if (Data.Instancia.RSA_Cif.GeneradorLlaves.GenerarClaves(p, q))
                        {
                            Data.Instancia.GenerarLlaves = false;
                            Data.Instancia.DescargarLlaves = true;
                            Data.Instancia.ExisteError = false;
                        }
                        else
                        {
                            Data.Instancia.GenerarLlaves = true;
                            Data.Instancia.DescargarLlaves = false;
                            Data.Instancia.ExisteError = true;
                        }

                        
                    }
                }
                else
                {
                    Data.Instancia.ExisteError = true;
                    Data.Instancia.Error = 0;
                }

                return RedirectToAction("IndexRSA");
            }
            catch
            {
                return View("IndexRSA");
            }
        }
        
        //PARA DESCARGAR ALGUN ARCHIVO, YA SEA DE LLAVES O RESULTANTE
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResultadoRSA(FormCollection collection)
        {
            try
            {
                if (collection["DescargarRSA"] != null)
                {
                    return RedirectToAction("DescargarResultadoRSA");
                }

                if (collection["DescargarPublica"] != null)
                {
                    return RedirectToAction("DescargaLlavePublica");
                }

                if (collection["DescargarPrivada"] != null)
                {
                    return RedirectToAction("DescargaLlavePrivada");
                }

                if (collection["HomeSerie3"] != null)
                {
                    Data.Instancia.RSA_Cif.Reset();
                    return RedirectToAction("PaginaPrincipalLab", "Home");
                }

                if (collection["ContinuarRSA"] != null)
                {
                    return RedirectToAction("IndexRSA");
                }

            }
            catch
            {
                Data.Instancia.RSA_Cif.Reset();
                return View("IndexRSA");
            }
            return null;
        }

        public FileResult DescargarResultadoRSA()
        {
            var extensionNueva = string.Empty;

            return File(Data.Instancia.RSA_Cif.ArchivoResultante(ref extensionNueva), "*" + extensionNueva, Data.Instancia.RSA_Cif.NombreArchivo + extensionNueva);
        }

        public FileResult DescargaLlavePublica()
        {
            Data.Instancia.DescargarLlaves = false;
            return File(Data.Instancia.RSA_Cif.ArchivoResultanteLlave("public"), "*.key", "public.key");
        }

        public FileResult DescargaLlavePrivada()
        {
            Data.Instancia.DescargarLlaves = false;
            return File(Data.Instancia.RSA_Cif.ArchivoResultanteLlave("private"), "*.key", "private.key");
        }
    }
}