namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDatabaseAgain : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductPromotion", newName: "PromotionProduct");
            DropForeignKey("dbo.ComboPromotionProduct", "IdComboPromotion", "dbo.ComboPromotion");
            DropForeignKey("dbo.ComboPromotionProduct", "IdProduct", "dbo.Product");
            DropForeignKey("dbo.ComboPromotion", "PromotionID", "dbo.Promotion");
            DropForeignKey("dbo.PromotionRule", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.PromotionRule", "PromotionTypeId", "dbo.PromotionType");
            DropIndex("dbo.ComboPromotion", new[] { "PromotionID" });
            DropIndex("dbo.ComboPromotionProduct", new[] { "IdProduct" });
            DropIndex("dbo.ComboPromotionProduct", new[] { "IdComboPromotion" });
            DropIndex("dbo.PromotionRule", new[] { "PromotionId" });
            DropIndex("dbo.PromotionRule", new[] { "PromotionTypeId" });
            AddColumn("dbo.Promotion", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Promotion", "DiscountType", c => c.Int(nullable: false));
            AddColumn("dbo.Promotion", "DiscountValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Promotion", "ComboQuantity", c => c.Int());
            AddColumn("dbo.Promotion", "BuyQuantity", c => c.Int());
            AddColumn("dbo.Promotion", "GetQuantity", c => c.Int());
            AlterColumn("dbo.Promotion", "Name", c => c.String());
            AlterColumn("dbo.Promotion", "Description", c => c.String());
            DropColumn("dbo.OrderDetailPromotion", "Description");
            DropColumn("dbo.Promotion", "EnDate");
            DropTable("dbo.ComboPromotion");
            DropTable("dbo.ComboPromotionProduct");
            DropTable("dbo.PromotionRule");
            DropTable("dbo.PromotionType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PromotionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameType = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComboPromotionProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProduct = c.Int(nullable: false),
                        IdComboPromotion = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Promotion", "EnDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderDetailPromotion", "Description", c => c.String());
            AlterColumn("dbo.Promotion", "Description", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Promotion", "Name", c => c.String(nullable: false, maxLength: 250));
            DropColumn("dbo.Promotion", "GetQuantity");
            DropColumn("dbo.Promotion", "BuyQuantity");
            DropColumn("dbo.Promotion", "ComboQuantity");
            DropColumn("dbo.Promotion", "DiscountValue");
            DropColumn("dbo.Promotion", "DiscountType");
            DropColumn("dbo.Promotion", "EndDate");
            CreateIndex("dbo.PromotionRule", "PromotionTypeId");
            CreateIndex("dbo.PromotionRule", "PromotionId");
            CreateIndex("dbo.ComboPromotionProduct", "IdComboPromotion");
            CreateIndex("dbo.ComboPromotionProduct", "IdProduct");
            CreateIndex("dbo.ComboPromotion", "PromotionID");
            AddForeignKey("dbo.PromotionRule", "PromotionTypeId", "dbo.PromotionType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PromotionRule", "PromotionId", "dbo.Promotion", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComboPromotion", "PromotionID", "dbo.Promotion", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComboPromotionProduct", "IdProduct", "dbo.Product", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComboPromotionProduct", "IdComboPromotion", "dbo.ComboPromotion", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.PromotionProduct", newName: "ProductPromotion");
        }
    }
}
