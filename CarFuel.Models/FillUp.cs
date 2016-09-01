using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarFuel.Models {
  public class FillUp {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public double? Kml {
        get {
            double? result = null;

            if (NextFillUp == null) {
                return null;
            }

            result = (NextFillUp.Odometer - this.Odometer) / NextFillUp.Liters;

            if (result < 0) {
                throw new Exception(NextFillUp.Odometer + "can't be less than" + this.Odometer);
            }

            return result;
        }
    }

    public int Odometer { get; set; }

    public double Liters { get; set; }

    public bool IsFull { get; set; }

    public FillUp() : this(true) {

    }

    public FillUp(bool isFull = true) {
        IsFull = isFull;
    }

    public FillUp(int odometer, double liters, bool isFull = true) {
      Odometer = odometer;
      Liters = liters;
      IsFull = isFull;
    }

    public FillUp NextFillUp { get; set; }
    
  }
}