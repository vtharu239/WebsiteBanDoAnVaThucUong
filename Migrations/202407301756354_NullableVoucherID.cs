namespace WebsiteBanDoAnVaThucUong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableVoucherID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "VoucherId", "dbo.Voucher");
            DropIndex("dbo.Order", new[] { "VoucherId" });
            AlterColumn("dbo.Order", "VoucherId", c => c.Int());
            CreateIndex("dbo.Order", "VoucherId");
            AddForeignKey("dbo.Order", "VoucherId", "dbo.Voucher", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "VoucherId", "dbo.Voucher");
            DropIndex("dbo.Order", new[] { "VoucherId" });
            AlterColumn("dbo.Order", "VoucherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Order", "VoucherId");
            AddForeignKey("dbo.Order", "VoucherId", "dbo.Voucher", "Id", cascadeDelete: true);
        }
    }
}
