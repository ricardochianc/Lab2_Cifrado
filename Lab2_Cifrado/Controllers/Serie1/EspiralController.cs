﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers.Serie1
{
    public class EspiralController : Controller
    {
        // GET: Espiral
        public ActionResult IndexEspiral()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaArchivoEspiral(HttpPostedFileBase postedFile)
        {
            var FilePath = string.Empty;

            if (postedFile != null)
            {
                var path = Data.Instancia.RutaAbsolutaServer;

                FilePath = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(FilePath);

                var nombre = postedFile.FileName.Split('.')[0];

                Data.Instancia.EspiralCif.AsignarRutas(path,FilePath,nombre);
                Data.Instancia.EspiralCif.AsignarExtension(postedFile.FileName.Split('.')[1]);

                Data.Instancia.ArchivoCargado = true;
            }

            return RedirectToAction("IndexEspiral");
        }
        
        // POST: Espiral/Create
        [HttpPost]
        public ActionResult OperarEspiral(FormCollection collection)
        {
            try
            {
                var clave = int.Parse(collection["Clave"]);
                var direccion = collection["DireccionRecorrido"];

                if (clave > 0)
                {
                    Data.Instancia.EspiralCif.Clave = clave;
                    Data.Instancia.EspiralCif.DireccionRecorrido = direccion;
                    Data.Instancia.EleccionOperacion = true;
                    Data.Instancia.EspiralCif.Operar();
                }
                
                return RedirectToAction("IndexEspiral");
            }
            catch (Exception e)
            {
                return View("IndexEspiral");
            }
        }

        //NUEVO PARA DESCARGAR ARCHIVO
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Resultado(FormCollection collection)
        {
            try
            {
                if (collection["DescargarEspiral"] != null)
                {
                    return RedirectToAction("DescargarResultadoEspiral");
                }

                if (collection["HomeSerie1"] != null)
                {
                    Data.Instancia.EspiralCif.Reset();
                    return RedirectToAction("PaginaPrincipalLab", "Home");
                }
            }
            catch(Exception error)
            {
                Data.Instancia.EspiralCif.Reset();
                return View("IndexEspiral");
            }

            return null;
        }
        
        public FileResult DescargarResultadoEspiral()
        {
            var extensionNueva = string.Empty;

            return File(Data.Instancia.EspiralCif.ArchivoResultante(ref extensionNueva), "*" + extensionNueva,Data.Instancia.EspiralCif.NombreArchivo + extensionNueva);
        }
    }
}