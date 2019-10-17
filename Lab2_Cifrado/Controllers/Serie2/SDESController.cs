using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers.Serie2
{
    public class SDESController : Controller
    {
        // GET: SDES
        private static bool Modificar = false;

        public ActionResult IndexSDES()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargarArchivoPermutaciones(HttpPostedFileBase postedFile)
        {
            var FilePath = string.Empty;

            if (postedFile!= null)
            {
                var path = Data.Instancia.RutaAbsolutaServer;
                FilePath = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(FilePath);

                Data.Instancia.RutaPermutaciones = FilePath;
            }

            return RedirectToAction("IndexSDES");
        }

        [HttpPost]
        public ActionResult CargarArchivoSDES(HttpPostedFileBase postedFile)
        {
            var FilePath = string.Empty;

            if (postedFile != null)
            {
                var path = Data.Instancia.RutaAbsolutaServer;

                FilePath = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(FilePath);

                var nombre = postedFile.FileName.Split('.')[0];

                Data.Instancia.SDES_Cif.AsignarRutas(path, FilePath, nombre);
                Data.Instancia.SDES_Cif.AsignarExtension(postedFile.FileName.Split('.')[1]);

                Data.Instancia.ArchivoCargado = true;
            }

            return RedirectToAction("IndexSDES");
        }

        [HttpPost]
        public ActionResult OperarSDES(FormCollection collection)
        {
            try
            {
                var clave = int.Parse(collection["Clave"]);

                if (clave > 0 && clave <= 1023)
                {
                    Data.Instancia.SDES_Cif.Clave = clave;
                    Data.Instancia.EleccionOperacion = true;
                    var path = Data.Instancia.RutaPermutaciones;
                    Data.Instancia.SDES_Cif.Operar(path);
                }

                return RedirectToAction("IndexSDES");
            }
            catch
            {
                return View("IndexSDES");
            }
        }

        //PARA DESCARGAR ARCHIVO
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResultadoSDES(FormCollection collection)
        {
            try
            {
                if (collection["DescargarSDES"] != null)
                {
                    return RedirectToAction("DescargarResultadoSDES");
                }

                if (collection["HomeSerie2"] != null)
                {
                    Data.Instancia.SDES_Cif.Reset();
                    return RedirectToAction("PaginaPrincipalLab", "Home");
                }
            }
            catch (Exception error)
            {
                Data.Instancia.SDES_Cif.Reset();
                return View("IndexSDES");
            }
            return null;
        }

        public FileResult DescargarResultadoSDES()
        {
            var extensionNueva = string.Empty;

            return File(Data.Instancia.SDES_Cif.ArchivoResultante(ref extensionNueva), "*" + extensionNueva, Data.Instancia.SDES_Cif.NombreArchivo + extensionNueva);
        }
    }
}
