namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaLaiDatabasenhun1 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.PromotionProduct1");
        }
        
        public override void Down()
        {
        }
    }
}
