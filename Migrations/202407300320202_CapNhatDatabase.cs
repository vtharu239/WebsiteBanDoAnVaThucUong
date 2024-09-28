namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CapNhatDatabase : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Product", "CreatedDate");
            DropColumn("dbo.Product", "CreatedBy");
            DropColumn("dbo.Product", "ModifiedDate");
            DropColumn("dbo.Product", "ModifiedBy");
            DropColumn("dbo.ProductCategory", "CreatedDate");
            DropColumn("dbo.ProductCategory", "CreatedBy");
            DropColumn("dbo.ProductCategory", "ModifiedDate");
            DropColumn("dbo.ProductCategory", "ModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductCategory", "ModifiedBy", c => c.String());
            AddColumn("dbo.ProductCategory", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProductCategory", "CreatedBy", c => c.String());
            AddColumn("dbo.ProductCategory", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Product", "ModifiedBy", c => c.String());
            AddColumn("dbo.Product", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Product", "CreatedBy", c => c.String());
            AddColumn("dbo.Product", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
