namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemKhoaNgoaiStore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store", "IdManager", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Store", "IdManager");
            AddForeignKey("dbo.Store", "IdManager", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store", "IdManager", "dbo.AspNetUsers");
            DropIndex("dbo.Store", new[] { "IdManager" });
            DropColumn("dbo.Store", "IdManager");
        }
    }
}
