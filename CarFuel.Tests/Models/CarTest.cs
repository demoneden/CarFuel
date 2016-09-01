using CarFuel.Models;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CarFuel.Tests.Models {
    public class CarTest {

        public class General {

            [Fact]
            public void InitialValues() {
                //arrange
                var c = new Car("My Jazz");
                //act
                //assert
                Assert.Equal("My Jazz", c.Name);
                Assert.Empty(c.FillUps);
            }

        }

        public class AddFillUpMethod {

            //Can only be assigned within ctor
            private readonly ITestOutputHelper _output;

            public AddFillUpMethod(ITestOutputHelper output) {
                _output = output;
            }

            [Fact]
            public void AddOneFillUp() {
                //arrange
                var c1 = new Car("My Jazz");
                //act
                var f1 = c1.AddFillUp(1000,30);
                //assert
                Assert.Equal(1,c1.FillUps.Count);
                Assert.Contains(f1, c1.FillUps);
            }

            [Fact]
            public void AddTwoFillUp() {
                //arrange
                var c1 = new Car("My Jazz");
                //act
                var f1 = c1.AddFillUp(1000, 30);
                var f2 = c1.AddFillUp(1500, 20);
                var average = c1.AverageKml;

                Dump(c1);

                //assert
                Assert.Same(f2, f1.NextFillUp);
                Assert.Equal(25, average);

            }

            [Theory]
            [MemberData("RandomFillUpData", 50)]
            public void AddServeralFillUps(int odometer, double liters) {
                var c = new Car("Toyota");
                c.AddFillUp(odometer,liters);
                Dump(c);
                c.FillUps.Count().ShouldEqual(1);
            }

            public static IEnumerable<object> RandomFillUpData(int count) {
                var r = new Random();
                for (int i = 0; i < count; i++) {
                    var odo = r.Next(0, 9999 + 1);
                    var liters = r.Next(0, 9999 + 1) / 100.0;
                    yield return new object[] { odo, liters };
                }
            }

            private void Dump(Car c) {
                _output.WriteLine("Car: {0}", c.Name);
                foreach (FillUp fillup in c.FillUps) {
                    _output.WriteLine("odo: " + fillup.Odometer + " kml: " + fillup.Kml + " liter: " + fillup.Liters);
                }
            }
        }

        public class AverageKmlProperty {

            [Fact]
            public void AverageKmlFillNone_Null() {
                //arrange
                Car c = new Car("Honda");
                //act
                //assert
                Assert.Null(c.AverageKml);
            }

            [Fact]
            public void AverageKmlFillOne_Null() {
                //arrange
                Car c = new Car("Honda");
                //act
                c.AddFillUp(1000, 40);
                //assert
                Assert.Null(c.AverageKml);
            }

            [Fact]
            public void AverageKmlFillTwo_Success() {
                //arrange
                Car c = new Car("Honda");
                //act
                var f1 = c.AddFillUp(1000, 40);
                var f2 = c.AddFillUp(2000, 50);
                //assert
                Assert.Equal(f1.Kml, c.AverageKml);
            }

            [Fact]
            public void AverageKmlFillThree_Success() {
                //arrange
                Car c = new Car("Honda");
                //act
                var f1 = c.AddFillUp(1000, 40);
                var f2 = c.AddFillUp(2000, 50);
                var f3 = c.AddFillUp(2500, 20);
                //assert
                Assert.Equal(21.43,c.AverageKml);
            }

            [Fact]
            public void AverageKmlFillFourWithOneIncorrect_Success() {
                Car c = new Car("Honda");
                c.AddFillUp(1000, 40);
               
                var ex = Assert.Throws<InvalidOperationException>(() => {
                    c.AddFillUp(700, 50); //Incorrect
                });
                
                c.AddFillUp(1700, 50);
                c.AddFillUp(2500, 55);
                Assert.Equal(14.29, c.AverageKml);
            }
        }

    }
}
