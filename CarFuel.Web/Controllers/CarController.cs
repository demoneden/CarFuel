using CarFuel.Models;
using CarFuel.Services;
using CarFuel.Services.Bases;
using CarFuel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace CarFuel.Web.Controllers
{
    [Authorize]
    public class CarController : AppControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService, IUserService userService)
            : base(userService) {
            _carService = carService;
        }

        //CarFuelDb context = new CarFuelDb();

        //protected override void Dispose(bool disposing) {
        //    if (disposing) {
        //        context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        // GET: Car
        public ActionResult Index() {
            return View(_carService.All());
        }

        public ActionResult AddCar() {
            return View();
        }

        [HttpPost]
        public ActionResult AddCar(Car car) {
            //if (ModelState.IsValid) {
                _carService.Add(car);
                _carService.SaveChanges();
            //}
            return RedirectToAction("Index");
        }

        public ActionResult ShowFillUp(Guid id) {
            Car car = _carService.Find(id);

            if (car == null) {
                return HttpNotFound();
            }
            return View(car);
        }

        public ActionResult AddFillUp(Guid id) {
            Car car = _carService.Find(id);

            if (car == null) {
                return HttpNotFound();
            }

            FillUp fill = new FillUp();
            fill.Odometer = car.LatestFill != null ? car.LatestFill.Odometer : 0;
            fill.Liters = car.LatestFill != null ? car.LatestFill.Liters : 0;
            //context.Entry(fill).Reference(x => x.NextFillUp).Load();
            if (car == null || string.IsNullOrEmpty(car.Name)) {
                return HttpNotFound();
            }

            ViewBag.Car = car;
            
            return View(fill);
        }

        [HttpPost]
        public ActionResult AddFillUp(Car car, FillUp fill) {
            //if (ModelState.IsValid) {
                Car dbCar = _carService.Find(car.Id);
                dbCar.AddFillUp(fill.Odometer, fill.Liters);
                _carService.SaveChanges();

                return RedirectToAction("Index");
            //}

            //return View();
        }

    }
}