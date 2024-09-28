namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaLaiDatabasenhun : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Orders", newName: "Order");
            DropForeignKey("dbo.Voucher", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Voucher", new[] { "CreatedBy" });
            CreateTable(
                "dbo.Promotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NamePromo = c.String(nullable: false, maxLength: 250),
                        DiscountType = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 250),
                        Detail = c.String(nullable: false, maxLength: 250),
                        DiscountValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinimumPurchase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EnDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotionOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        PromotionId = c.Int(nullable: false),
                        DiscountAmount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.PromotionProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.PromotionId)
                .Index(t => t.ProductId);
            
           
            
            AddColumn("dbo.Order", "SubTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "DiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "GrandTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "TotalQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "VoucherId", c => c.Int(nullable: true));
            AddColumn("dbo.Product", "SalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Store", "IdManager", c => c.String(nullable: true, maxLength: 128));
            CreateIndex("dbo.Order", "VoucherId");
            CreateIndex("dbo.Store", "IdManager");
            AddForeignKey("dbo.Order", "VoucherId", "dbo.Voucher", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Store", "IdManager", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Order", "TotalAmount");
            DropColumn("dbo.Order", "Quantity");
            DropColumn("dbo.Product", "Price");
            DropColumn("dbo.Product", "PriceSale");
            DropColumn("dbo.Product", "IsHome");
            DropColumn("dbo.Product", "IsSale");
            DropColumn("dbo.Product", "IsFeature");
            DropColumn("dbo.Product", "IsHot");
            DropColumn("dbo.Voucher", "CreatedDate");
            DropColumn("dbo.Voucher", "CreatedBy");
            DropColumn("dbo.Voucher", "ModifiedDate");
            DropColumn("dbo.Voucher", "ModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Voucher", "ModifiedBy", c => c.String());
            AddColumn("dbo.Voucher", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Voucher", "CreatedBy", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Voucher", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Product", "IsHot", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "IsFeature", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "IsSale", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "IsHome", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "PriceSale", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Product", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.PromotionProduct", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.PromotionProduct", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Store", "IdManager", "dbo.AspNetUsers");
            DropForeignKey("dbo.Order", "VoucherId", "dbo.Voucher");
            DropForeignKey("dbo.PromotionOrder", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.PromotionOrder", "OrderId", "dbo.Order");
            DropForeignKey("dbo.PromotionProduct1", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.PromotionProduct1", "Promotion_Id", "dbo.Promotion");
            DropIndex("dbo.PromotionProduct1", new[] { "Product_Id" });
            DropIndex("dbo.PromotionProduct1", new[] { "Promotion_Id" });
            DropIndex("dbo.PromotionProduct", new[] { "ProductId" });
            DropIndex("dbo.PromotionProduct", new[] { "PromotionId" });
            DropIndex("dbo.Store", new[] { "IdManager" });
            DropIndex("dbo.PromotionOrder", new[] { "PromotionId" });
            DropIndex("dbo.PromotionOrder", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "VoucherId" });
            AlterColumn("dbo.Store", "IdManager", c => c.String());
            DropColumn("dbo.Product", "SalePrice");
            DropColumn("dbo.Order", "VoucherId");
            DropColumn("dbo.Order", "TotalQuantity");
            DropColumn("dbo.Order", "GrandTotal");
            DropColumn("dbo.Order", "DiscountTotal");
            DropColumn("dbo.Order", "SubTotal");
            DropTable("dbo.PromotionProduct1");
            DropTable("dbo.PromotionProduct");
            DropTable("dbo.PromotionOrder");
            DropTable("dbo.Promotion");
            CreateIndex("dbo.Voucher", "CreatedBy");
            AddForeignKey("dbo.Voucher", "CreatedBy", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Order", newName: "Orders");
        }
    }
}
