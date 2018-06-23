namespace AbstractShopService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommodityDetalis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommodityId = c.Int(nullable: false),
                        DetaliId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commodities", t => t.CommodityId, cascadeDelete: true)
                .ForeignKey("dbo.Detalis", t => t.DetaliId, cascadeDelete: true)
                .Index(t => t.CommodityId)
                .Index(t => t.DetaliId);
            
            CreateTable(
                "dbo.Commodities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommodityName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zakazs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZakazchikId = c.Int(nullable: false),
                        CommodityId = c.Int(nullable: false),
                        RabochiId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commodities", t => t.CommodityId, cascadeDelete: true)
                .ForeignKey("dbo.Rabochis", t => t.RabochiId)
                .ForeignKey("dbo.Zakazchiks", t => t.ZakazchikId, cascadeDelete: true)
                .Index(t => t.ZakazchikId)
                .Index(t => t.CommodityId)
                .Index(t => t.RabochiId);
            
            CreateTable(
                "dbo.Rabochis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RabochiFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zakazchiks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZakazchikFIO = c.String(nullable: false),
                        Mail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.String(),
                        FromMailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        DateDelivery = c.DateTime(nullable: false),
                        ZakazchikId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zakazchiks", t => t.ZakazchikId)
                .Index(t => t.ZakazchikId);
            
            CreateTable(
                "dbo.Detalis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetaliName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreDetalis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        DetaliId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Detalis", t => t.DetaliId, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.DetaliId);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreDetalis", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.StoreDetalis", "DetaliId", "dbo.Detalis");
            DropForeignKey("dbo.CommodityDetalis", "DetaliId", "dbo.Detalis");
            DropForeignKey("dbo.Zakazs", "ZakazchikId", "dbo.Zakazchiks");
            DropForeignKey("dbo.MessageInfoes", "ZakazchikId", "dbo.Zakazchiks");
            DropForeignKey("dbo.Zakazs", "RabochiId", "dbo.Rabochis");
            DropForeignKey("dbo.Zakazs", "CommodityId", "dbo.Commodities");
            DropForeignKey("dbo.CommodityDetalis", "CommodityId", "dbo.Commodities");
            DropIndex("dbo.StoreDetalis", new[] { "DetaliId" });
            DropIndex("dbo.StoreDetalis", new[] { "StoreId" });
            DropIndex("dbo.MessageInfoes", new[] { "ZakazchikId" });
            DropIndex("dbo.Zakazs", new[] { "RabochiId" });
            DropIndex("dbo.Zakazs", new[] { "CommodityId" });
            DropIndex("dbo.Zakazs", new[] { "ZakazchikId" });
            DropIndex("dbo.CommodityDetalis", new[] { "DetaliId" });
            DropIndex("dbo.CommodityDetalis", new[] { "CommodityId" });
            DropTable("dbo.Stores");
            DropTable("dbo.StoreDetalis");
            DropTable("dbo.Detalis");
            DropTable("dbo.MessageInfoes");
            DropTable("dbo.Zakazchiks");
            DropTable("dbo.Rabochis");
            DropTable("dbo.Zakazs");
            DropTable("dbo.Commodities");
            DropTable("dbo.CommodityDetalis");
        }
    }
}
