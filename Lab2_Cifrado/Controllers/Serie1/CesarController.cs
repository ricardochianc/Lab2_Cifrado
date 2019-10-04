using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers.Serie1
{
    public class CesarController : Controller
    {
        // GET: Cesar
        public ActionResult IndexCesar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaArchivoCesar(HttpPostedFileBase postedFile)
        {
            var FilePath = string.Empty;

            if (postedFile != null)
            {
                var path = Data.Instancia.RutaAbsolutaServer;

                FilePath = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(FilePath);

                var nombre = postedFile.FileName.Split('.')[0];

                Data.Instancia.CesarCif.AsignarRutas(path,FilePath,nombre);
                Data.Instancia.CesarCif.AsignarExtension(postedFile.FileName.Split('.')[1]);

                Data.Instancia.ArchivoCargado = true;
            }

            return RedirectToAction("IndexCesar");
        }
        
        // POST: Cesar/Create
        [HttpPost]
        public ActionResult OperarCesar(FormCollection collection)
        {
            try
            {
                var clave = collection["PalabraClave"];

                if (clave != "")
                {
                    Data.Instancia.CesarCif.PalabraClave = clave;
                    Data.Instancia.EleccionOperacion = true;
                    Data.Instancia.CesarCif.Operar();
                }
                
                return RedirectToAction("IndexCesar");
            }
            catch
            {
                return View("IndexCesar");
            }
        }
    }
}