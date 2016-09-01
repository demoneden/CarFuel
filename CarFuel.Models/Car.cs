using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CarFuel.Models {
    public class Car {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        //IEnumerable can read
        //ICollection can CRUD
        public virtual ICollection<FillUp> FillUps { get; set; }
        public double? AverageKml {
            get {
                double? result = null;
                if (FillUps.Where(x => x.Kml != null).Count() > 0) {
                    //Find min odo and max odo fill up
                    var firstFillUp = FillUps.FirstOrDefault();
                    var lastFillUp = FillUps.LastOrDefault();
                    if (firstFillUp != lastFillUp) {
                        var sumLiterExceptFirst = FillUps.Skip(1).Sum(x => x.Liters);

                        result = (lastFillUp.Odometer - firstFillUp.Odometer) / sumLiterExceptFirst; 
                        result = Math.Round((double)result, 2, MidpointRounding.AwayFromZero);
                    }
                }
                return result;
            }
        }

        [Required]
        public virtual User Owner { get; set; }

        public Car() : this("Car") {

        }

        public Car(string name) {
            Name = name;
            FillUps = new HashSet<FillUp>();
        }

        public FillUp AddFillUp(int odometer, double liters, bool isFull = true) {
            FillUp f = new FillUp(odometer, liters, isFull);
            if (FillUps.Count > 0) {
                FillUps.LastOrDefault().NextFillUp = f;
            }
            if (FillUps.Count == 0 || (FillUps.Count > 0 && FillUps.LastOrDefault().Odometer < f.Odometer)) {
                FillUps.Add(f);
            }
            else {
                throw new InvalidOperationException();
            }
            
            return f;
        }

        public FillUp LatestFill {
            get {
                return FillUps.LastOrDefault();
            }
        }

    }
}