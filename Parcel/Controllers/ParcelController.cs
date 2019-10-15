using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Parcel.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Parcel.Controllers
{
    public class ParcelsController : Controller
    {
        private readonly ParcelContext _db;

        public ParcelsController(ParcelContext db)
        {
            _db = db;
        }

        [HttpGet("/parcels")]
        public ActionResult Index()
        {
            List<ParcelVariable> model = _db.ParcelTable.Include(parcels => parcels.Category).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ParcelVariable newParcel)
        {
            if (newParcel.SideA > 0 && newParcel.SideB > 0 && newParcel.SideC >0 && newParcel.Weight > 0 && newParcel.Note.Length > 0)
            {
                Console.WriteLine(newParcel);
                _db.ParcelTable.Add(newParcel);
                _db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var thisParcel = _db.ParcelTable.FirstOrDefault(parcels => parcels.ID == id);
            return View(thisParcel);
        }
        [HttpPost]
        public ActionResult Edit(ParcelVariable foundParcel)
        {
            _db.Entry(foundParcel).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var thisParcel = _db.ParcelTable.FirstOrDefault(parcels => parcels.ID == id);
            return View(thisParcel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var thisParcel = _db.ParcelTable.FirstOrDefault(parcels => parcels.ID == id);
            _db.ParcelTable.Remove(thisParcel);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}

