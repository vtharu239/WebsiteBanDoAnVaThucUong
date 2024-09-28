namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemThuocTinh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "CustomerName", c => c.String(nullable: false));
            AddColumn("dbo.Order", "Phone", c => c.String(nullable: false));
            AddColumn("dbo.Order", "Address", c => c.String(nullable: false));
            AddColumn("dbo.Order", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "Email");
            DropColumn("dbo.Order", "Address");
            DropColumn("dbo.Order", "Phone");
            DropColumn("dbo.Order", "CustomerName");
        }
    }
}
