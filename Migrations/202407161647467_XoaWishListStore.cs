namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XoaWishListStore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WishlistStore", "StoreId", "dbo.Store");
            DropIndex("dbo.WishlistStore", new[] { "StoreId" });
            DropTable("dbo.WishlistStore");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WishlistStore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        UserName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.WishlistStore", "StoreId");
            AddForeignKey("dbo.WishlistStore", "StoreId", "dbo.Store", "Id", cascadeDelete: true);
        }
    }
}
