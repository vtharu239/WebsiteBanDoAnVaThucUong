namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemForeignKeyLaiChoWishList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wishlist", "CustomerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Wishlist", "CustomerId");
            AddForeignKey("dbo.Wishlist", "CustomerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Wishlist", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wishlist", "UserName", c => c.String());
            DropForeignKey("dbo.Wishlist", "CustomerId", "dbo.AspNetUsers");
            DropIndex("dbo.Wishlist", new[] { "CustomerId" });
            DropColumn("dbo.Wishlist", "CustomerId");
        }
    }
}
