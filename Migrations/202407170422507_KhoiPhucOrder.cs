namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KhoiPhucOrder : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Order", newName: "Orders");
            AddColumn("dbo.Orders", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "TotalQuantity");
            DropColumn("dbo.Orders", "OrderStatus");
            DropColumn("dbo.Orders", "ShippingStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "ShippingStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "TotalQuantity", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.Orders", "Quantity");
            RenameTable(name: "dbo.Orders", newName: "Order");
        }
    }
}
