namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemUserNameLaiVaoWishList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wishlist", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Wishlist", "UserName");
        }
    }
}
