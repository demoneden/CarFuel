using CarFuel.Models;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarFuel.Tests.Models {

  public class FillUpTest {

    public class KmlProperty { //Inner Class

      [Fact]
      public void NewFillUpDontHasKml() {
        //arrange
        var f1 = new FillUp();
        //act
        var kml = f1.Kml;
        //assert
        Assert.Null(kml);
        Assert.True(f1.IsFull);
      }

      [Fact]
      public void TwoFillUpHasKml() {
        //arrange
        var f1 = new FillUp(1000, 40.0);
        var f2 = new FillUp(2000, 50.0);

        f1.NextFillUp = f2;

        //act
        var kml1 = f1.Kml;
        var kml2 = f2.Kml;

        //assert
        Assert.Equal(20.0, kml1);
        Assert.Null(kml2);
      }

      [Fact]
      public void ThreeFillUpHasKml() {
        //arrange
        var f1 = new FillUp(1000,40.0);
        var f2 = new FillUp(2000,50.0);
        var f3 = new FillUp(2500,20.0);

        f1.NextFillUp = f2;
        f2.NextFillUp = f3;

        //act
        var kml1 = f1.Kml;
        var kml2 = f2.Kml;
        var kml3 = f3.Kml;

        //assert
        Assert.Equal(20.0, kml1);
        Assert.Equal(25.0, kml2);
        Assert.Null(kml3);
      }

      /// <summary>
      /// Test where kml can be calculated.
      /// </summary>
      /// <param name="odo1"></param>
      /// <param name="liters1"></param>
      /// <param name="odo2"></param>
      /// <param name="liters2"></param>
      /// <param name="expectedKml"></param>
      [Theory]
      [InlineData(1000,40,2000,50,20)]
      [InlineData(2000, 50, 2500, 20, 25)]
      public void FillUpHasKml(int odo1, double liters1, 
        int odo2, double liters2, 
        double expectedKml) {

        var f1 = new FillUp(odo1, liters1);
        var f2 = new FillUp(odo2, liters2);

        f1.NextFillUp = f2;

        //act
        var kml1 = f1.Kml;
        var kml2 = f2.Kml;

        //assert
        Assert.Equal(expectedKml, kml1);
        Assert.Null(kml2);
      }

      

      [Fact]
      public void FillLesserOdo() {
        var f1 = new FillUp(5000, 50);
        var f2 = new FillUp(4900, 40); //Intension: invalid
        f1.NextFillUp = f2;
        
        var ex = Assert.Throws<Exception>(() => {
          var kml1 = f1.Kml;
        });
        
      }

    }
    
  }

}
