namespace EBSM.Repo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseOrderAndEstimationEnitity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstimationProductRelation",
                c => new
                    {
                        EstimationProductId = c.Int(nullable: false, identity: true),
                        ProjectEstimationId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        BatchNo = c.String(),
                        Barcode = c.String(),
                        Dp = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        TotalPrice = c.Double(),
                        Status = c.Byte(),
                        ZoneId = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.EstimationProductId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectEstimations", t => t.ProjectEstimationId, cascadeDelete: true)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.ProjectEstimationId)
                .Index(t => t.ProductId)
                .Index(t => t.ZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ProjectEstimations",
                c => new
                    {
                        ProjectEstimationId = c.Int(nullable: false, identity: true),
                        EstimationNumber = c.String(),
                        EstimationDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                        CustomerBranchId = c.Int(),
                        TotalPrice = c.Double(),
                        TotalQuantity = c.Double(),
                        Note = c.String(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectEstimationId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.CustomerProjects", t => t.CustomerBranchId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CustomerId)
                .Index(t => t.CustomerBranchId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PurchaseOrderProducts",
                c => new
                    {
                        PurchaseOrderProductId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        BatchNo = c.String(),
                        Barcode = c.String(),
                        Quantity = c.Double(nullable: false),
                        UnitPrice = c.Double(),
                        TotalPrice = c.Double(),
                        ZoneId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.PurchaseOrderProductId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId, cascadeDelete: true)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.ProductId)
                .Index(t => t.ZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false, identity: true),
                        PurchaseOrderDate = c.DateTime(nullable: false),
                        PurchaseOrderNumber = c.String(),
                        TotalQuantity = c.Double(),
                        TotalPrice = c.Double(),
                        PurchaseDiscount = c.Double(),
                        PaidAmount = c.Double(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        SupplierId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.PurchaseOrderId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.SupplierId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PurchaseOrderRelation",
                c => new
                    {
                        PurchaseOrderRelationId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        PurchaseOrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderRelationId)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId, cascadeDelete: true)
                .Index(t => t.PurchaseId)
                .Index(t => t.PurchaseOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrderProducts", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.PurchaseOrders", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderRelation", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrderRelation", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseOrderProducts", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.PurchaseOrderProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseOrderProducts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrderProducts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.EstimationProductRelation", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.ProjectEstimations", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.EstimationProductRelation", "ProjectEstimationId", "dbo.ProjectEstimations");
            DropForeignKey("dbo.ProjectEstimations", "CustomerBranchId", "dbo.CustomerProjects");
            DropForeignKey("dbo.ProjectEstimations", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.ProjectEstimations", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ProjectEstimations", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.EstimationProductRelation", "ProductId", "dbo.Products");
            DropForeignKey("dbo.EstimationProductRelation", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.EstimationProductRelation", "CompanyId", "dbo.CompanyProfiles");
            DropIndex("dbo.PurchaseOrderRelation", new[] { "PurchaseOrderId" });
            DropIndex("dbo.PurchaseOrderRelation", new[] { "PurchaseId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CompanyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "UpdatedBy" });
            DropIndex("dbo.PurchaseOrders", new[] { "CreatedBy" });
            DropIndex("dbo.PurchaseOrders", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrderProducts", new[] { "CompanyId" });
            DropIndex("dbo.PurchaseOrderProducts", new[] { "CreatedBy" });
            DropIndex("dbo.PurchaseOrderProducts", new[] { "ZoneId" });
            DropIndex("dbo.PurchaseOrderProducts", new[] { "ProductId" });
            DropIndex("dbo.PurchaseOrderProducts", new[] { "PurchaseOrderId" });
            DropIndex("dbo.ProjectEstimations", new[] { "CompanyId" });
            DropIndex("dbo.ProjectEstimations", new[] { "UpdatedBy" });
            DropIndex("dbo.ProjectEstimations", new[] { "CreatedBy" });
            DropIndex("dbo.ProjectEstimations", new[] { "CustomerBranchId" });
            DropIndex("dbo.ProjectEstimations", new[] { "CustomerId" });
            DropIndex("dbo.EstimationProductRelation", new[] { "CompanyId" });
            DropIndex("dbo.EstimationProductRelation", new[] { "CreatedBy" });
            DropIndex("dbo.EstimationProductRelation", new[] { "ZoneId" });
            DropIndex("dbo.EstimationProductRelation", new[] { "ProductId" });
            DropIndex("dbo.EstimationProductRelation", new[] { "ProjectEstimationId" });
            DropTable("dbo.PurchaseOrderRelation");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.PurchaseOrderProducts");
            DropTable("dbo.ProjectEstimations");
            DropTable("dbo.EstimationProductRelation");
        }
    }
}
