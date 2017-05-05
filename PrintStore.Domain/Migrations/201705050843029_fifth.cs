namespace PrintStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fifth : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartLines", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.CartLines", new[] { "Product_ProductId" });
            AddColumn("dbo.CartLines", "ProductId", c => c.Int(nullable: false));
            DropColumn("dbo.CartLines", "Product_ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartLines", "Product_ProductId", c => c.Int());
            DropColumn("dbo.CartLines", "ProductId");
            CreateIndex("dbo.CartLines", "Product_ProductId");
            AddForeignKey("dbo.CartLines", "Product_ProductId", "dbo.Products", "ProductId");
        }
    }
}
