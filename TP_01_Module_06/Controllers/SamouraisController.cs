using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using TP_01_Module_06.Data;
using TP_01_Module_06.Models;

namespace TP_01_Module_06.Controllers
{
    public class SamouraisController : Controller
    {
        private DojoContext db = new DojoContext();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {

            SamouraiViewModel vm = new SamouraiViewModel();

            vm.Armes = db.Armes.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            return View(vm);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (vm.IdArme != null)
                    {
                        var samouraisAvecArme = db.Samourais.Where(x => x.Arme.Id == vm.IdArme).ToList();
                        Arme arme = null;
                        foreach (var samourai in samouraisAvecArme)
                        {
                            arme = samourai.Arme;
                            samourai.Arme = null;
                            db.Entry(samourai).State = EntityState.Modified;
                        }
                        if (arme == null)
                        {
                            vm.Samourai.Arme = db.Armes.FirstOrDefault(x => x.Id == vm.IdArme);
                        }
                        else
                        {
                            vm.Samourai.Arme = arme;
                        }
                    }

                    db.Samourais.Add(vm.Samourai);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    vm.Armes = db.Armes.Select(
                    x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                    .ToList();

                    return View(vm);
                }
            }
            catch
            {
                vm.Armes = db.Armes.Select(
                    x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                    .ToList();

                return View(vm);
            }
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamouraiViewModel vm = new SamouraiViewModel();
            vm.Samourai = db.Samourais.Find(id);
            if (vm.Samourai == null)
            {
                return HttpNotFound();
            }

            vm.Armes = db.Armes.Select(
                    x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                    .ToList();

            return View(vm);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiViewModel vm)
        {
            if (ModelState.IsValid)
            {

                db.Samourais.Attach(vm.Samourai);

                if(vm.IdArme != null)
                {
                    var samouraisAvecArme = db.Samourais.Where(x => x.Arme.Id == vm.IdArme).ToList();

                    Arme arme = null;
                    foreach(var samourai in samouraisAvecArme)
                    {
                        arme = samourai.Arme;
                        samourai.Arme = null;
                        db.Entry(samourai).State = EntityState.Modified;
                    }
                    if(arme == null)
                    {
                        vm.Samourai.Arme = db.Armes.FirstOrDefault(x => x.Id == vm.IdArme);
                    }
                    else
                    {
                        vm.Samourai.Arme = arme;
                    }
                }
                else
                {
                    vm.Samourai.Arme = null;
                }
                db.Entry(vm.Samourai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
