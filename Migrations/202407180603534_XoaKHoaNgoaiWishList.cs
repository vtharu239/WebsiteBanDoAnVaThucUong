namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XoaKHoaNgoaiWishList : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Wishlist", "CustomerId", "dbo.AspNetUsers");
            DropIndex("dbo.Wishlist", new[] { "CustomerId" });
            DropColumn("dbo.Wishlist", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wishlist", "CustomerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Wishlist", "CustomerId");
            AddForeignKey("dbo.Wishlist", "CustomerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
