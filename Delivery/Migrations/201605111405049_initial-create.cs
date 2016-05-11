namespace Delivery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        AddressesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Locations", t => t.AddressesID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.AddressesID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlaceName = c.String(maxLength: 100),
                        Remark = c.String(maxLength: 200),
                        Users_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Users_ID)
                .Index(t => t.Users_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Account = c.String(maxLength: 20),
                        Password = c.String(maxLength: 20),
                        Name = c.String(maxLength: 20),
                        Sex = c.String(maxLength: 20),
                        Grade = c.String(maxLength: 20),
                        Degree = c.String(maxLength: 20),
                        PhoneNumber = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderCompetitors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SenderID = c.Int(),
                        ReceiverID = c.Int(),
                        PickLocationID = c.Int(nullable: false),
                        ReceiverLocationID = c.Int(),
                        RewardID = c.Int(),
                        Status = c.String(maxLength: 20),
                        Mark = c.Int(nullable: false),
                        Comment = c.String(maxLength: 200),
                        PublishTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Locations", t => t.PickLocationID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ReceiverID)
                .ForeignKey("dbo.Locations", t => t.ReceiverLocationID)
                .ForeignKey("dbo.Rewards", t => t.RewardID)
                .ForeignKey("dbo.Users", t => t.SenderID)
                .Index(t => t.SenderID)
                .Index(t => t.ReceiverID)
                .Index(t => t.PickLocationID)
                .Index(t => t.ReceiverLocationID)
                .Index(t => t.RewardID);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(),
                        ExpressCompany = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Rewards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 50),
                        Money = c.Int(nullable: false),
                        Remark = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderCompetitors", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrderCompetitors", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "SenderID", "dbo.Users");
            DropForeignKey("dbo.Orders", "RewardID", "dbo.Rewards");
            DropForeignKey("dbo.Orders", "ReceiverLocationID", "dbo.Locations");
            DropForeignKey("dbo.Orders", "ReceiverID", "dbo.Users");
            DropForeignKey("dbo.Orders", "PickLocationID", "dbo.Locations");
            DropForeignKey("dbo.Packages", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Addresses", "UserID", "dbo.Users");
            DropForeignKey("dbo.Locations", "Users_ID", "dbo.Users");
            DropForeignKey("dbo.Addresses", "AddressesID", "dbo.Locations");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Packages", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "RewardID" });
            DropIndex("dbo.Orders", new[] { "ReceiverLocationID" });
            DropIndex("dbo.Orders", new[] { "PickLocationID" });
            DropIndex("dbo.Orders", new[] { "ReceiverID" });
            DropIndex("dbo.Orders", new[] { "SenderID" });
            DropIndex("dbo.OrderCompetitors", new[] { "OrderID" });
            DropIndex("dbo.OrderCompetitors", new[] { "UserID" });
            DropIndex("dbo.Locations", new[] { "Users_ID" });
            DropIndex("dbo.Addresses", new[] { "AddressesID" });
            DropIndex("dbo.Addresses", new[] { "UserID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Rewards");
            DropTable("dbo.Packages");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderCompetitors");
            DropTable("dbo.Users");
            DropTable("dbo.Locations");
            DropTable("dbo.Addresses");
        }
    }
}
