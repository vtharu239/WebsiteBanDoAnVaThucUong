namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChuongTrinhKhuyenMaiDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromotionProduct1", "Promotion_Id", "dbo.Promotion");
            DropForeignKey("dbo.PromotionProduct1", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.PromotionOrder", "OrderId", "dbo.Order");
            DropForeignKey("dbo.PromotionOrder", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.PromotionProduct", "ProductId", "dbo.Product");
            DropForeignKey("dbo.PromotionProduct", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.PromotionOrder", new[] { "OrderId" });
            DropIndex("dbo.PromotionOrder", new[] { "PromotionId" });
            DropIndex("dbo.PromotionProduct", new[] { "PromotionId" });
            DropIndex("dbo.PromotionProduct", new[] { "ProductId" });
            DropIndex("dbo.PromotionProduct1", new[] { "Promotion_Id" });
            DropIndex("dbo.PromotionProduct1", new[] { "Product_Id" });
            CreateTable(
                "dbo.OrderDetailPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDetailId = c.Int(nullable: false),
                        PromotionId = c.Int(nullable: false),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderDetail", t => t.OrderDetailId, cascadeDelete: true)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.OrderDetailId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.ComboPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionID = c.Int(nullable: false),
                        ComboName = c.String(),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionID, cascadeDelete: true)
                .Index(t => t.PromotionID);
            
            CreateTable(
                "dbo.ComboPromotionProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProduct = c.Int(nullable: false),
                        IdComboPromotion = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComboPromotion", t => t.IdComboPromotion, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.IdProduct, cascadeDelete: true)
                .Index(t => t.IdProduct)
                .Index(t => t.IdComboPromotion);
            
            CreateTable(
                "dbo.ProductPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        PromotionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.PromotionRule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionId = c.Int(nullable: false),
                        PromotionTypeId = c.Int(nullable: false),
                        Condition = c.String(),
                        Reward = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .ForeignKey("dbo.PromotionType", t => t.PromotionTypeId, cascadeDelete: true)
                .Index(t => t.PromotionId)
                .Index(t => t.PromotionTypeId);
            
            CreateTable(
                "dbo.PromotionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameType = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Order", "ProductDiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "VoucherDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "FinalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderDetail", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderDetail", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderDetail", "FinalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Promotion", "Name", c => c.String(nullable: false, maxLength: 250));
            AddColumn("dbo.Voucher", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Voucher", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Order", "DiscountTotal");
            DropColumn("dbo.Order", "GrandTotal");
            DropColumn("dbo.OrderDetail", "TotalPrice");
            DropColumn("dbo.Promotion", "NamePromo");
            DropColumn("dbo.Promotion", "DiscountType");
            DropColumn("dbo.Promotion", "Detail");
            DropColumn("dbo.Promotion", "DiscountValue");
            DropColumn("dbo.Promotion", "MinimumPurchase");
            DropColumn("dbo.Promotion", "MaximumDiscount");
            DropTable("dbo.PromotionOrder");
            DropTable("dbo.PromotionProduct");
        }
        
        public override void Down()
        {         
            CreateTable(
                "dbo.PromotionProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Promotion", "MaximumDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Promotion", "MinimumPurchase", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Promotion", "DiscountValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Promotion", "Detail", c => c.String(nullable: false, maxLength: 250));
            AddColumn("dbo.Promotion", "DiscountType", c => c.Int(nullable: false));
            AddColumn("dbo.Promotion", "NamePromo", c => c.String(nullable: false, maxLength: 250));
            AddColumn("dbo.OrderDetail", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "GrandTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "DiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.OrderDetailPromotion", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.PromotionRule", "PromotionTypeId", "dbo.PromotionType");
            DropForeignKey("dbo.PromotionRule", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.ComboPromotion", "PromotionID", "dbo.Promotion");
            DropForeignKey("dbo.ComboPromotionProduct", "IdProduct", "dbo.Product");
            DropForeignKey("dbo.ProductPromotion", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.ProductPromotion", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ComboPromotionProduct", "IdComboPromotion", "dbo.ComboPromotion");
            DropForeignKey("dbo.OrderDetailPromotion", "OrderDetailId", "dbo.OrderDetail");
            DropIndex("dbo.PromotionRule", new[] { "PromotionTypeId" });
            DropIndex("dbo.PromotionRule", new[] { "PromotionId" });
            DropIndex("dbo.ProductPromotion", new[] { "PromotionId" });
            DropIndex("dbo.ProductPromotion", new[] { "ProductId" });
            DropIndex("dbo.ComboPromotionProduct", new[] { "IdComboPromotion" });
            DropIndex("dbo.ComboPromotionProduct", new[] { "IdProduct" });
            DropIndex("dbo.ComboPromotion", new[] { "PromotionID" });
            DropIndex("dbo.OrderDetailPromotion", new[] { "PromotionId" });
            DropIndex("dbo.OrderDetailPromotion", new[] { "OrderDetailId" });
            DropColumn("dbo.Voucher", "IsActive");
            DropColumn("dbo.Voucher", "Quantity");
            DropColumn("dbo.Promotion", "Name");
            DropColumn("dbo.OrderDetail", "FinalAmount");
            DropColumn("dbo.OrderDetail", "DiscountAmount");
            DropColumn("dbo.OrderDetail", "Subtotal");
            DropColumn("dbo.Order", "FinalAmount");
            DropColumn("dbo.Order", "VoucherDiscount");
            DropColumn("dbo.Order", "ProductDiscountTotal");
            DropTable("dbo.PromotionType");
            DropTable("dbo.PromotionRule");
            DropTable("dbo.ProductPromotion");
            DropTable("dbo.ComboPromotionProduct");
            DropTable("dbo.ComboPromotion");
            DropTable("dbo.OrderDetailPromotion");
            CreateIndex("dbo.PromotionProduct", "ProductId");
            CreateIndex("dbo.PromotionProduct", "PromotionId");
            CreateIndex("dbo.PromotionOrder", "PromotionId");
            CreateIndex("dbo.PromotionOrder", "OrderId");
            AddForeignKey("dbo.PromotionProduct", "PromotionId", "dbo.Promotion", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PromotionProduct", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PromotionOrder", "PromotionId", "dbo.Promotion", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PromotionOrder", "OrderId", "dbo.Order", "Id", cascadeDelete: true);
        }
    }
}
