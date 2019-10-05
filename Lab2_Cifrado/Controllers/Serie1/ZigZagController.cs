using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers.Serie1
{
    public class ZigZagController : Controller
    {
        // GET: ZigZag
        public ActionResult IndexZigZag()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaArchivoZigZag(HttpPostedFileBase postedFile)
        {
            var FilePath = string.Empty;

            if (postedFile != null)
            {
                var path = Data.Instancia.RutaAbsolutaServer;

                FilePath = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(FilePath);

                var nombre = postedFile.FileName.Split('.')[0];

                Data.Instancia.ZigZagCif.AsignarRutas(path,FilePath,nombre);
                Data.Instancia.ZigZagCif.AsignarExtension(postedFile.FileName.Split('.')[1]);

                Data.Instancia.ArchivoCargado = true;
            }

            return RedirectToAction("IndexZigZag");
        }
        
        // POST: ZigZag/Create
        [HttpPost]
        public ActionResult OperarZigZag(FormCollection collection)
        {
            try
            {
                var clave = int.Parse(collection["Clave"]);

                if (clave > 0)
                {
                    Data.Instancia.ZigZagCif.Clave = clave;
                    Data.Instancia.EleccionOperacion = true;
                    Data.Instancia.ZigZagCif.Operar();
                }
                
                return RedirectToAction("IndexZigZag");
            }
            catch
            {
                return View("IndexZigZag");
            }
        }

        //NUEVO PARA DESCARGAR ARCHIVO
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Resultado(FormCollection collection)
        {
            try
            {
                if (collection["DescargarZZ"] != null)
                {
                    return RedirectToAction("DescargarResultadoZigZag");
                }

                if (collection["HomeSerie1"] != null)
                {
                    Data.Instancia.ZigZagCif.Reset();
                    return RedirectToAction("PaginaPrincipalLab", "Home");
                }
            }
            catch(Exception error)
            {
                Data.Instancia.ZigZagCif.Reset();
                return View("IndexZigZag");
            }

            return null;
        }
        
        public FileResult DescargarResultadoZigZag()
        {
            var extensionNueva = string.Empty;

            return File(Data.Instancia.ZigZagCif.ArchivoResultante(ref extensionNueva), "*" + extensionNueva,Data.Instancia.ZigZagCif.NombreArchivo + extensionNueva);
        }
    }
}
