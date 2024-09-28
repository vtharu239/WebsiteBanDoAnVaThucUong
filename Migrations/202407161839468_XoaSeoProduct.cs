namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XoaSeoProduct : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Product", "SeoTitle");
            DropColumn("dbo.Product", "SeoDescription");
            DropColumn("dbo.Product", "SeoKeywords");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "SeoKeywords", c => c.String(maxLength: 250));
            AddColumn("dbo.Product", "SeoDescription", c => c.String(maxLength: 500));
            AddColumn("dbo.Product", "SeoTitle", c => c.String(maxLength: 250));
        }
    }
}
