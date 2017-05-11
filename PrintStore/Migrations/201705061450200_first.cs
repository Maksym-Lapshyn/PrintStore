namespace PrintStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsBlocked");
        }
    }
}
