using System;
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
            return View();
        }

        [HttpPost]
        public ActionResult GenerarLlavesRSA(FormCollection collection)
        {
            try
            {
                var p = int.Parse(collection["P"]);
                var q = int.Parse(collection["Q"]);

                if (!Data.Instancia.RSA_Cif.GeneradorLlaves.EsPrimo(p) || !Data.Instancia.RSA_Cif.GeneradorLlaves.EsPrimo(q))
                {
                    Data.Instancia.ExisteError = true;
                    Data.Instancia.Error = 0;
                }
                
                //Se está validando de P y Q sean coprimos
                if (Data.Instancia.RSA_Cif.GeneradorLlaves.MCD(p,q) != 1)
                {
                    Data.Instancia.ExisteError = true;
                    Data.Instancia.Error = 1;
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
            }
            catch (Exception error)
            {
                Data.Instancia.RSA_Cif.Reset();
                //return View();
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
            return File(Data.Instancia.RSA_Cif.ArchivoResultanteLlave("public"), "*.key", "public.key");
        }

        public FileResult DescargaLlavePrivada()
        {
            return File(Data.Instancia.RSA_Cif.ArchivoResultanteLlave("private"), "*.key", "private.key");
        }
    }
}
