namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaPromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromotionProduct", "IsBuyProduct", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromotionProduct", "IsBuyProduct");
        }
    }
}
