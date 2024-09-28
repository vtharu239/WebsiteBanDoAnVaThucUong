namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XoaKhoaNgoaiOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "CustomerId", "dbo.AspNetUsers");
            DropIndex("dbo.Order", new[] { "CustomerId" });
            AlterColumn("dbo.Order", "CustomerId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "CustomerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Order", "CustomerId");
            AddForeignKey("dbo.Order", "CustomerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
