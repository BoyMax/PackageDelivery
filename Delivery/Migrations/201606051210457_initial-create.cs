namespace Delivery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Grade", c => c.String(maxLength: 200));
            AlterColumn("dbo.Users", "Degree", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Degree", c => c.String(maxLength: 200));
            AlterColumn("dbo.Users", "Grade", c => c.String(maxLength: 20));
        }
    }
}
