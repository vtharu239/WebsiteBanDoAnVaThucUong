namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xoaAvatartrongReview : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Review", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Review", "Avatar", c => c.String());
        }
    }
}
