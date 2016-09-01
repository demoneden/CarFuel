using CarFuel.DataAccess;
using CarFuel.DataAccess.Bases;
using CarFuel.Models;
using CarFuel.Services;
using CarFuel.Services.Bases;
using CarFuel.Tests.Fakes;
using Moq;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace CarFuel.Tests.Services {
    public class CarServiceTest {


        public class AddMethodWithMoq {

            [Fact]
            public void AddNewCar_Success() {
                //arrange
                var user = new User { DisplayName = "user1" };
                var mock = new Mock<IRepository<Car>>();
                var mockUser = new Mock<IUserService>();
                var carService = new CarService(mock.Object, mockUser.Object);
                
                Car c = new Car("Car1");
                //act

                mockUser.Setup(x => x.CurrentUser).Returns(user);
                carService.Add(c);
                //assert
                mock.Verify(x => x.Add(c), Times.Once);
            }

            [Fact]
            public void AddNewCarswithDuplicatedName_ShoudlNotBeAdded() {
                //arrange
                var user = new User { DisplayName = "user1" };
                var mock = new Mock<IRepository<Car>>();
                var mockUser = new Mock<IUserService>();
                var carService = new CarService(mock.Object, mockUser.Object);

                Car c1 = new Car("Same Name");
                Car c2 = new Car("Same Name");

                var collection = new HashSet<Car>();
                mock.Setup(x => x.Add(It.IsAny<Car>()))
                    .Callback<Car>((c) => {
                        collection.Add(c);
                    });

                mockUser.Setup(x => x.CurrentUser).Returns(user);

                mock.Setup(x => x.Query(It.IsAny<Func<Car, bool>>()))
                    .Returns(collection.AsQueryable());

                //act
                carService.Add(c1);

                //assert
                var ex = Assert.Throws<Exception>(() => {
                    carService.Add(c2);
                });
                ex.Message.ShouldEqual("This name has been used already.");

                var cars = carService.All();

                cars.ShouldContain(c1);
                cars.ShouldNotContain(c2);
            }


        }

        public class AddMethod {

            private FakeRepository<Car> context;
            private FakeRepository<User> userContext;
            private CarService carService;
            private UserService userService;

            public AddMethod() {
                //Fake repositories for both car and user
                context = new FakeRepository<Car>();
                userContext = new FakeRepository<User>();

                //Use user service with fake repository
                userService = new UserService(userContext);
                //Initialize current logged in user
                userService.CurrentUser = new User { DisplayName = "Golf" };
                //Use car service with fake repository and user service
                carService = new CarService(context, userService);
            }
            
            [Fact]
            public void AddNewCar_Success() {
                //arrange
                Car c = new Car("Car1");
                //act
                carService.Add(c);
                //assert
                var cars = carService.All();
                cars.ShouldContain(c);
            }

            [Fact]
            public void AddNewCarswithDuplicatedName_ShoudlNotBeAdded() {
                //arrange
                Car c1 = new Car("Same Name");
                Car c2 = new Car("Same Name");
                //act
                carService.Add(c1);

                //assert
                var ex = Assert.Throws<Exception>(() => {
                    carService.Add(c2);
                });
                ex.Message.ShouldEqual("This name has been used already.");

                var cars = carService.All();

                cars.ShouldContain(c1);
                cars.ShouldNotContain(c2);
            }

        }

    }
}
