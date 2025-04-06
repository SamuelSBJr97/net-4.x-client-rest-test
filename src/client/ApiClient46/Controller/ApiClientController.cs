using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiClient46.Controller
{
    public class ApiClientController : Controller
    {
        // GET: ApiClient
        public ActionResult Index()
        {
            return View();
        }

        // GET: ApiClient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApiClient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApiClient/Create
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

        // GET: ApiClient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ApiClient/Edit/5
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

        // GET: ApiClient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApiClient/Delete/5
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
