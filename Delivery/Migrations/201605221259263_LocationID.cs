namespace Delivery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderCompetitors", "LocationID", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderCompetitors", "LocationID");
            AddForeignKey("dbo.OrderCompetitors", "LocationID", "dbo.Locations", "ID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderCompetitors", "LocationID", "dbo.Locations");
            DropIndex("dbo.OrderCompetitors", new[] { "LocationID" });
            DropColumn("dbo.OrderCompetitors", "LocationID");
        }
    }
}
