namespace CarFuel.DataAccess.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CarFuel.DataAccess.Contexts.CarFuelDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CarFuel.DataAccess.Contexts.CarFuelDb";
        }

        protected override void Seed(CarFuel.DataAccess.Contexts.CarFuelDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            
            context.Users.AddOrUpdate(
                x => x.UserId,
                new User { UserId = new Guid(), DisplayName = "Default User" }   
            );

        }
    }
}
