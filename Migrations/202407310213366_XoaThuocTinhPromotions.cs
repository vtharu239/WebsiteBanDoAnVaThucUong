namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XoaThuocTinhPromotions : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Promotion", "ComboQuantity");
            DropColumn("dbo.Promotion", "BuyQuantity");
            DropColumn("dbo.Promotion", "GetQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promotion", "GetQuantity", c => c.Int());
            AddColumn("dbo.Promotion", "BuyQuantity", c => c.Int());
            AddColumn("dbo.Promotion", "ComboQuantity", c => c.Int());
        }
    }
}
