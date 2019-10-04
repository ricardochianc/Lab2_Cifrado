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
                    //Mandar a llamar lo de hacer Zig Zag
                }
                
                return RedirectToAction("IndexZigZag");
            }
            catch
            {
                return View("IndexZigZag");
            }
        }
    }
}
