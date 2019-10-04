using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2_Cifrado.Controllers.Serie1
{
    public class ZigZagController : Controller
    {
        // GET: ZigZag
        public ActionResult Index()
        {
            return View();
        }

        // GET: ZigZag/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZigZag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZigZag/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ZigZag/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZigZag/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ZigZag/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZigZag/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
