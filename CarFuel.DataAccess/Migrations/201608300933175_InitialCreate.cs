namespace CarFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FillUps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Odometer = c.Int(nullable: false),
                        Liters = c.Double(nullable: false),
                        IsFull = c.Boolean(nullable: false),
                        NextFillUp_Id = c.Guid(),
                        Car_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FillUps", t => t.NextFillUp_Id)
                .ForeignKey("dbo.Cars", t => t.Car_Id)
                .Index(t => t.NextFillUp_Id)
                .Index(t => t.Car_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FillUps", "Car_Id", "dbo.Cars");
            DropForeignKey("dbo.FillUps", "NextFillUp_Id", "dbo.FillUps");
            DropIndex("dbo.FillUps", new[] { "Car_Id" });
            DropIndex("dbo.FillUps", new[] { "NextFillUp_Id" });
            DropTable("dbo.FillUps");
            DropTable("dbo.Cars");
        }
    }
}
