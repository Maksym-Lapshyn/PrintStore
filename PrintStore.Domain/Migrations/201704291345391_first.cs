namespace PrintStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Size", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Texture", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Texture");
            DropColumn("dbo.Products", "Size");
        }
    }
}
