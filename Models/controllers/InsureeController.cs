using System;
using System.Linq;
using System.Web.Mvc;
using InsuranceMVC.Models;

namespace InsuranceMVC.Controllers
{
    public class InsureeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Start with base
                decimal quote = 50m;

                // Calculate age
                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < insuree.DateOfBirth.DayOfYear)
                {
                    age--;
                }

                // Age conditions
                if (age <= 18) quote += 100;
                else if (age >= 19 && age <= 25) quote += 50;
                else if (age >= 26) quote += 25;

                // Car year
                if (insuree.CarYear < 2000) quote += 25;
                else if (insuree.CarYear > 2015) quote += 25;

                // Car make / model
                if (insuree.CarMake.ToLower() == "porsche")
                {
                    quote += 25;
                    if (insuree.CarModel.ToLower() == "911 carrera")
                        quote += 25;
                }

                // Tickets
                quote += insuree.SpeedingTickets * 10;

                // DUI
                if (insuree.DUI) quote *= 1.25m;

                // Full coverage
                if (insuree.CoverageType) quote *= 1.5m;

                // Save
                insuree.Quote = quote;
                db.Insurees.Add(insuree);
                db.SaveChanges();

                return RedirectToAction("Admin");
            }
            return View(insuree);
        }

        // GET: Insuree/Admin
        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }
    }
}
