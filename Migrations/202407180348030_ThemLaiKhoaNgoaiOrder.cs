namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemLaiKhoaNgoaiOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "ShippingStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "CustomerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "CustomerName");
            DropColumn("dbo.Orders", "Phone");
            DropColumn("dbo.Orders", "Address");
            DropColumn("dbo.Orders", "Email");
            DropColumn("dbo.Orders", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Email", c => c.String());
            AddColumn("dbo.Orders", "Address", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "Phone", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "CustomerName", c => c.String(nullable: false));
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            AlterColumn("dbo.Orders", "CustomerId", c => c.String());
            DropColumn("dbo.Orders", "ShippingStatus");
            DropColumn("dbo.Orders", "OrderStatus");
        }
    }
}
