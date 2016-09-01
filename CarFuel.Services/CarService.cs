using CarFuel.Models;
using CarFuel.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFuel.DataAccess.Bases;

namespace CarFuel.Services {
    public class CarService : ServiceBase<Car>, ICarService {

        private readonly IUserService _userService;

        public CarService(IRepository<Car> baseRepo, IUserService userService) 
            : base(baseRepo) {
            _userService = userService;
        }

        public override IQueryable<Car> All() {
            if (_userService.CurrentUser == null) {
                //return Enumerable.Empty<Car>.AsQueryable();
                throw new Exception();
            }
            return Query(x => x.Owner == _userService.CurrentUser);
        }

        public override Car Find(params object[] keys) {
            var key1 = (Guid)keys[0];
            return Query(x => x.Id == key1).SingleOrDefault();
        }

        public override Car Add(Car item) {

            if (_userService.CurrentUser == null) {
                throw new Exception("Not Log in.");
            }

            if (All().Any(x => x.Name == item.Name)) {
                throw new Exception("This name has been used already.");
            }

            item.Owner = _userService.CurrentUser;

            return base.Add(item);
        }

        //public IQueryable<Car> AllCarsForUser(Guid memberId) {
        //    return base.Query(x => x.Owner.UserId == memberId);
        //}
    }
}
