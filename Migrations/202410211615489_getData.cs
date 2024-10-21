namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class getData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberRanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Points = c.Int(nullable: false),
                        Rank = c.String(),
                        TotalSpent = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProductViewHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(),
                        ViewedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductViewHistory", "ProductId", "dbo.Product");
            DropForeignKey("dbo.MemberRanks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ProductViewHistory", new[] { "ProductId" });
            DropIndex("dbo.MemberRanks", new[] { "UserId" });
            DropTable("dbo.ProductViewHistory");
            DropTable("dbo.MemberRanks");
        }
    }
}
