[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Insuree insuree)
{
    if (ModelState.IsValid)
    {
        // Start with base $50
        decimal quote = 50m;

        // Calculate age
        int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
        if (DateTime.Now.DayOfYear < insuree.DateOfBirth.DayOfYear)
            age--;

        // Age-based pricing
        if (age <= 18) quote += 100;
        else if (age >= 19 && age <= 25) quote += 50;
        else if (age >= 26) quote += 25;

        // Car year pricing
        if (insuree.CarYear < 2000) quote += 25;
        if (insuree.CarYear > 2015) quote += 25;

        // Car make/model pricing
        if (insuree.CarMake.ToLower() == "porsche")
        {
            quote += 25;
            if (insuree.CarModel.ToLower() == "911 carrera")
                quote += 25; // Total $50 for 911 Carrera
        }

        // Speeding tickets
        quote += insuree.SpeedingTickets * 10;

        // DUI increases total by 25%
        if (insuree.DUI) quote *= 1.25m;

        // Full coverage increases total by 50%
        if (insuree.CoverageType) quote *= 1.5m;

        // Assign calculated quote
        insuree.Quote = quote;

        // Save to database
        db.Insurees.Add(insuree);
        db.SaveChanges();

        return RedirectToAction("Admin");
    }
    return View(insuree);
}
