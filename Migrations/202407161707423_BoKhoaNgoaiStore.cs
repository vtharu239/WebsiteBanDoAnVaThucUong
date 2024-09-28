namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoKhoaNgoaiStore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Store", "IdManager", "dbo.AspNetUsers");
            DropIndex("dbo.Store", new[] { "IdManager" });
            AlterColumn("dbo.Store", "IdManager", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Store", "IdManager", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Store", "IdManager");
            AddForeignKey("dbo.Store", "IdManager", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
