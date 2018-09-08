namespace WarehouseApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleTransfers",
                c => new
                    {
                        ArticleTransferId = c.Int(nullable: false, identity: true),
                        TransferDate = c.DateTime(nullable: false),
                        FromStockId = c.Int(nullable: false),
                        ToStockId = c.Int(nullable: false),
                        TransferQuantity = c.Double(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ArticleTransferId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Stocks", t => t.FromStockId)
                .ForeignKey("dbo.Stocks", t => t.ToStockId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.FromStockId)
                .Index(t => t.ToStockId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CompanyProfiles",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        CompanyAddress = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Tin = c.String(),
                        VatRegNo = c.String(),
                        WebSite = c.String(),
                        CompanyLogo = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Address = c.String(),
                        Email = c.String(),
                        ContactNo = c.String(),
                        Gender = c.String(),
                        NationalId = c.String(),
                        RoleId = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        LastPassChangeDate = c.DateTime(),
                        PasswordChangedCount = c.Int(),
                        SupUser = c.Boolean(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                        Role_RoleId = c.Int(),
                        Department_DepartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Roles", t => t.Role_RoleId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId)
                .Index(t => t.Role_RoleId)
                .Index(t => t.Department_DepartmentId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.RoleTasks",
                c => new
                    {
                        RoleTaskId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Task = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleTaskId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BatchNo = c.String(),
                        Barcode = c.String(),
                        Mfg = c.DateTime(),
                        Exp = c.DateTime(),
                        Warranty = c.DateTime(),
                        PurchasePrice = c.Double(),
                        SalePrice = c.Double(),
                        TotalQuantity = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.ProductId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(),
                        ProductName = c.String(nullable: false),
                        ProductFullName = c.String(),
                        Details = c.String(),
                        Tp = c.Double(),
                        Dp = c.Double(),
                        PerCarton = c.Int(),
                        DiscountType = c.String(),
                        DiscountAmount = c.Double(),
                        Vat = c.Double(),
                        ExpiryDate = c.DateTime(),
                        MinStockLimit = c.Double(),
                        ProductImage = c.String(),
                        Status = c.Byte(),
                        AttributeSetId = c.Int(),
                        GroupNameId = c.Int(),
                        ManufacturerId = c.Int(),
                        DefaultZoneId = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                        Category_CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.ProductAttributeSets", t => t.AttributeSetId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.WarehouseZones", t => t.DefaultZoneId)
                .ForeignKey("dbo.Groups", t => t.GroupNameId)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId)
                .ForeignKey("dbo.Suppliers", t => t.ManufacturerId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.AttributeSetId)
                .Index(t => t.GroupNameId)
                .Index(t => t.ManufacturerId)
                .Index(t => t.DefaultZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId)
                .Index(t => t.Category_CategoryId);
            
            CreateTable(
                "dbo.ProductAttributeSets",
                c => new
                    {
                        AttributeSetId = c.Int(nullable: false, identity: true),
                        AttributeSetName = c.String(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.AttributeSetId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.AttributeSetAttributes",
                c => new
                    {
                        AttributeSetAttributeId = c.Int(nullable: false, identity: true),
                        AttributeSetId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttributeSetAttributeId)
                .ForeignKey("dbo.ProductAttributes", t => t.AttributeId)
                .ForeignKey("dbo.ProductAttributeSets", t => t.AttributeSetId)
                .Index(t => t.AttributeSetId)
                .Index(t => t.AttributeId);
            
            CreateTable(
                "dbo.ProductAttributes",
                c => new
                    {
                        AttributeId = c.Int(nullable: false, identity: true),
                        AttributeName = c.String(nullable: false),
                        AttributeType = c.String(nullable: false),
                        Values = c.String(),
                        ShowingText = c.String(),
                        DefaultValue = c.String(),
                        IsRequired = c.Boolean(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.AttributeId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ProductAttributeRelations",
                c => new
                    {
                        ProductAttributeRelationId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ProductAttributeRelationId)
                .ForeignKey("dbo.ProductAttributes", t => t.AttributeId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AttributeId);
            
            CreateTable(
                "dbo.ProductCustomerRelations",
                c => new
                    {
                        ProductCustomerRelationId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        ProductDescription = c.String(),
                        ProductCode = c.String(),
                        UnitPrice = c.Double(),
                        Mrp = c.Double(),
                    })
                .PrimaryKey(t => t.ProductCustomerRelationId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Address = c.String(),
                        Email = c.String(),
                        ContactNo = c.String(),
                        Gender = c.String(),
                        Age = c.Int(),
                        PreviousBalance = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CustomerProjects",
                c => new
                    {
                        CustomerProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 80),
                        ProjectPhoneNo = c.String(maxLength: 20),
                        ProjectAddress = c.String(maxLength: 255),
                        ProjectDescription = c.String(),
                        CustomerId = c.Int(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CustomerProjectId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CustomerId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                        CustomerBranchId = c.Int(),
                        TotalPrice = c.Double(),
                        TotalQuantity = c.Double(),
                        DiscountType = c.String(),
                        DiscountAmount = c.Double(),
                        TotalVat = c.Double(),
                        PaidAmount = c.Double(),
                        SalesmanId = c.Int(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        Note = c.String(),
                        CashPaid = c.Double(),
                        WorkOrderNumber = c.String(maxLength: 30),
                        WorkOrderDate = c.DateTime(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.CustomerProjects", t => t.CustomerBranchId)
                .ForeignKey("dbo.Salesman", t => t.SalesmanId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CustomerId)
                .Index(t => t.CustomerBranchId)
                .Index(t => t.SalesmanId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Damages",
                c => new
                    {
                        DamageId = c.Int(nullable: false, identity: true),
                        StockId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Note = c.String(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                        Invoice_InvoiceId = c.Int(),
                        InvoiceProduct_InvoiceProductId = c.Int(),
                        Product_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.DamageId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Stocks", t => t.StockId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.Invoices", t => t.Invoice_InvoiceId)
                .ForeignKey("dbo.InvoiceProducts", t => t.InvoiceProduct_InvoiceProductId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .Index(t => t.StockId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId)
                .Index(t => t.Invoice_InvoiceId)
                .Index(t => t.InvoiceProduct_InvoiceProductId)
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "dbo.DamageDismisses",
                c => new
                    {
                        DamageDismissId = c.Int(nullable: false, identity: true),
                        DismissDate = c.DateTime(nullable: false),
                        DamageId = c.Int(),
                        Quantity = c.Double(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.DamageDismissId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Damages", t => t.DamageId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.DamageId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.DamageReturns",
                c => new
                    {
                        DamageReturnId = c.Int(nullable: false, identity: true),
                        ReturnDate = c.DateTime(nullable: false),
                        DamageId = c.Int(),
                        Quantity = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.DamageReturnId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Damages", t => t.DamageId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.DamageId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.InvoiceBills",
                c => new
                    {
                        BillId = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.BillId, t.InvoiceId })
                .ForeignKey("dbo.Bills", t => t.BillId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.BillId)
                .Index(t => t.InvoiceId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        BillId = c.Int(nullable: false, identity: true),
                        BillNo = c.String(maxLength: 80),
                        BillDate = c.DateTime(),
                        BillAmount = c.Double(nullable: false),
                        BillPaid = c.Double(),
                        CustomerId = c.Int(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BillId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CustomerId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.InvoiceProducts",
                c => new
                    {
                        InvoiceProductId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        BatchNo = c.String(),
                        Barcode = c.String(),
                        Dp = c.Double(nullable: false),
                        DiscountType = c.String(),
                        DiscountAmount = c.Double(),
                        DpDiscounted = c.Double(),
                        Quantity = c.Double(nullable: false),
                        TotalPrice = c.Double(),
                        Status = c.Byte(),
                        ZoneId = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.InvoiceProductId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.InvoiceId)
                .Index(t => t.ProductId)
                .Index(t => t.ZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ReturnProducts",
                c => new
                    {
                        ReturnProductId = c.Int(nullable: false, identity: true),
                        ReturnId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        InvoiceProductId = c.Int(),
                        Quantity = c.Double(),
                        UnitPrice = c.Double(),
                        TotalPrice = c.Double(),
                        ZoneId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ReturnProductId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.InvoiceProducts", t => t.InvoiceProductId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Returns", t => t.ReturnId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.ReturnId)
                .Index(t => t.ProductId)
                .Index(t => t.InvoiceProductId)
                .Index(t => t.ZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Returns",
                c => new
                    {
                        ReturnId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(),
                        TotalQuantity = c.Double(),
                        TotalPrice = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ReturnId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.InvoiceId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.WarehouseZones",
                c => new
                    {
                        ZoneId = c.Int(nullable: false, identity: true),
                        ZoneName = c.String(nullable: false),
                        Capacity = c.Double(),
                        WarehouseId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ZoneId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.StockWarehouseRelations",
                c => new
                    {
                        StockWarehouseRelationId = c.Int(nullable: false, identity: true),
                        StockId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                        Quantity = c.Double(),
                    })
                .PrimaryKey(t => t.StockWarehouseRelationId)
                .ForeignKey("dbo.Stocks", t => t.StockId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.StockId)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        WarehouseId = c.Int(nullable: false, identity: true),
                        WarehouseName = c.String(nullable: false),
                        WarehouseNo = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.WarehouseId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Salesman",
                c => new
                    {
                        SalesmanId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Address = c.String(),
                        Email = c.String(),
                        ContactNo = c.String(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.SalesmanId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.DamageStocks",
                c => new
                    {
                        DamageStockId = c.Int(nullable: false, identity: true),
                        StockId = c.Int(nullable: false),
                        Quantity = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                        Product_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.DamageStockId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Stocks", t => t.StockId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .Index(t => t.StockId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId)
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupNameId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.GroupNameId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ProductCategoryId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductCategoryId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        CategoryParentId = c.Int(),
                        Count = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.Categories", t => t.CategoryParentId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CategoryParentId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PurchaseProducts",
                c => new
                    {
                        PurchaseProductId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.PurchaseProductId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.PurchaseId)
                .Index(t => t.ProductId)
                .Index(t => t.ZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        PurchaseNumber = c.String(),
                        PurchaseCost = c.Double(),
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
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.SupplierId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PurchaseCosts",
                c => new
                    {
                        PurchaseCostId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        PaidAmount = c.Double(),
                        Note = c.String(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.PurchaseCostId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.PurchaseId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PurchasePayments",
                c => new
                    {
                        PurchasePaymentId = c.Int(nullable: false, identity: true),
                        PaymentDate = c.DateTime(nullable: false),
                        PurchaseId = c.Int(nullable: false),
                        PurchaseCostId = c.Int(),
                        PaidAmount = c.Double(),
                        Note = c.String(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.PurchasePaymentId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId)
                .ForeignKey("dbo.PurchaseCosts", t => t.PurchaseCostId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.PurchaseId)
                .Index(t => t.PurchaseCostId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(nullable: false),
                        SupplierType = c.Int(),
                        SupplierAddress = c.String(),
                        ContactPersonName = c.String(),
                        Position = c.String(),
                        Email = c.String(),
                        ContactNo = c.String(),
                        Balance = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.SupplierId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Guid(nullable: false),
                        EventType = c.String(),
                        TableName = c.String(nullable: false),
                        PrimaryKeyName = c.String(),
                        PrimaryKeyValue = c.String(),
                        ColumnName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        CreatedUser = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.AuditLogId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedUser)
                .Index(t => t.CreatedUser)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        BankId = c.Int(nullable: false, identity: true),
                        BankName = c.String(nullable: false),
                        Branch = c.String(),
                        AccountName = c.String(nullable: false),
                        AccountNo = c.String(nullable: false),
                        Balance = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.BankId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Cash",
                c => new
                    {
                        CashId = c.Int(nullable: false, identity: true),
                        CashName = c.String(nullable: false),
                        Balance = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.CashId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CustomerBalances",
                c => new
                    {
                        CustomerBalanceId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(),
                        CustomerId = c.Int(),
                        Balance = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerBalanceId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.DepartmentId)
                .Index(t => t.CustomerId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.DeptRoleConfigs",
                c => new
                    {
                        DeptRoleConfigId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        DepartmentRole = c.String(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DeptRoleConfigId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.DepartmentId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseId = c.Int(nullable: false, identity: true),
                        ExpenseTypeId = c.Int(nullable: false),
                        ExpenseBy = c.String(),
                        Amount = c.Double(nullable: false),
                        Description = c.String(),
                        ExpenseDate = c.DateTime(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ExpenseId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.ExpenseTypes", t => t.ExpenseTypeId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.ExpenseTypeId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ExpenseTypes",
                c => new
                    {
                        ExpenseTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        ParentId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ExpenseTypeId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.ExpenseTypes", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.ParentId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.InvoiceOrders",
                c => new
                    {
                        InvoiceOrderId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceOrderId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.SalesOrders", t => t.OrderId)
                .Index(t => t.InvoiceId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.SalesOrders",
                c => new
                    {
                        SalesOrderId = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                        CustomerBranchId = c.Int(),
                        TotalPrice = c.Double(),
                        TotalQuantity = c.Double(),
                        DiscountType = c.String(),
                        DiscountAmount = c.Double(),
                        TotalVat = c.Double(),
                        AdvancePaid = c.Double(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        Note = c.String(),
                        CashPaid = c.Double(),
                        WorkOrderNumber = c.String(maxLength: 30),
                        WorkOrderDate = c.DateTime(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.SalesOrderId)
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
                "dbo.OrderProducts",
                c => new
                    {
                        OrderProductId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.OrderProductId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.SalesOrders", t => t.OrderId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId)
                .Index(t => t.ZoneId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        LoginsId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        SessionId = c.String(),
                        LoggedIn = c.Boolean(nullable: false),
                        LoggedInDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.LoginsId);
            
            CreateTable(
                "dbo.MobileBangking",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        AccountName = c.String(nullable: false),
                        AccountOwner = c.String(),
                        AccountNumber = c.String(nullable: false),
                        Balance = c.Double(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.NoticeAttachments",
                c => new
                    {
                        NoticeAttachmentId = c.Guid(nullable: false),
                        FileName = c.String(),
                        FileExtension = c.String(),
                        NoticeId = c.Int(nullable: false),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.NoticeAttachmentId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Notices", t => t.NoticeId)
                .Index(t => t.NoticeId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        NoticeId = c.Int(nullable: false, identity: true),
                        Heading = c.String(nullable: false),
                        Description = c.String(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.NoticeId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingsId = c.Int(nullable: false, identity: true),
                        ReturnsDayAllow = c.Int(),
                        Printing = c.Boolean(nullable: false),
                        PrinterName = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.SettingsId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        TransactionHead = c.String(),
                        TransactionMode = c.String(),
                        TransactionModeId = c.Int(),
                        Amount = c.Double(nullable: false),
                        TypeOfTransaction = c.String(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        TableName = c.String(nullable: false),
                        PrimaryKeyName = c.String(nullable: false),
                        PrimaryKeyValue = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.TransferProducts",
                c => new
                    {
                        ProductTransferId = c.Int(nullable: false, identity: true),
                        TransferDate = c.DateTime(nullable: false),
                        StockId = c.Int(nullable: false),
                        ZoneFromId = c.Int(nullable: false),
                        ZoneToId = c.Int(nullable: false),
                        TransferQuantity = c.Double(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductTransferId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Stocks", t => t.StockId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneFromId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneToId)
                .Index(t => t.StockId)
                .Index(t => t.ZoneFromId)
                .Index(t => t.ZoneToId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.WarehouseRows",
                c => new
                    {
                        WarehouseRowId = c.Int(nullable: false, identity: true),
                        RowName = c.String(nullable: false),
                        Capacity = c.Double(),
                        ZoneId = c.Int(),
                        WarehouseId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.WarehouseRowId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.ZoneId)
                .Index(t => t.WarehouseId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.WarehouseTracks",
                c => new
                    {
                        TrackId = c.Int(nullable: false, identity: true),
                        TrackName = c.String(nullable: false),
                        Capacity = c.Double(),
                        ZoneId = c.Int(),
                        WarehouseId = c.Int(),
                        Status = c.Byte(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.TrackId)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .ForeignKey("dbo.WarehouseZones", t => t.ZoneId)
                .Index(t => t.ZoneId)
                .Index(t => t.WarehouseId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseTracks", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.WarehouseTracks", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseTracks", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.WarehouseTracks", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.WarehouseTracks", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.WarehouseRows", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.WarehouseRows", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseRows", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.WarehouseRows", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.WarehouseRows", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.TransferProducts", "ZoneToId", "dbo.WarehouseZones");
            DropForeignKey("dbo.TransferProducts", "ZoneFromId", "dbo.WarehouseZones");
            DropForeignKey("dbo.TransferProducts", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.TransferProducts", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.TransferProducts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.TransferProducts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Transactions", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Transactions", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Transactions", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Settings", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Settings", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Settings", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Notices", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.NoticeAttachments", "NoticeId", "dbo.Notices");
            DropForeignKey("dbo.Notices", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Notices", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.NoticeAttachments", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.MobileBangking", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.MobileBangking", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.MobileBangking", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.InvoiceOrders", "OrderId", "dbo.SalesOrders");
            DropForeignKey("dbo.SalesOrders", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.OrderProducts", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.OrderProducts", "OrderId", "dbo.SalesOrders");
            DropForeignKey("dbo.OrderProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.OrderProducts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.SalesOrders", "CustomerBranchId", "dbo.CustomerProjects");
            DropForeignKey("dbo.SalesOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.SalesOrders", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.SalesOrders", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.InvoiceOrders", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Expenses", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ExpenseTypes", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ExpenseTypes", "ParentId", "dbo.ExpenseTypes");
            DropForeignKey("dbo.Expenses", "ExpenseTypeId", "dbo.ExpenseTypes");
            DropForeignKey("dbo.ExpenseTypes", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ExpenseTypes", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Expenses", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Expenses", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.CustomerBalances", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.CustomerBalances", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Users", "Department_DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.DeptRoleConfigs", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.DeptRoleConfigs", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.DeptRoleConfigs", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DeptRoleConfigs", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Departments", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.CustomerBalances", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerBalances", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.CustomerBalances", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Cash", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Cash", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Cash", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.BankAccounts", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.BankAccounts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.BankAccounts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.AuditLogs", "CreatedUser", "dbo.Users");
            DropForeignKey("dbo.AuditLogs", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.ArticleTransfers", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ArticleTransfers", "ToStockId", "dbo.Stocks");
            DropForeignKey("dbo.ArticleTransfers", "FromStockId", "dbo.Stocks");
            DropForeignKey("dbo.Stocks", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "ManufacturerId", "dbo.Suppliers");
            DropForeignKey("dbo.Stocks", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseProducts", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.Purchases", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Suppliers", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Purchases", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Suppliers", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Suppliers", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.PurchaseProducts", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchasePayments", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchasePayments", "PurchaseCostId", "dbo.PurchaseCosts");
            DropForeignKey("dbo.PurchasePayments", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchasePayments", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchasePayments", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.PurchaseCosts", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseCosts", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseCosts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseCosts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Purchases", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Purchases", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.PurchaseProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseProducts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseProducts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.ProductCategories", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Categories", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ProductCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Categories", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Categories", "CategoryParentId", "dbo.Categories");
            DropForeignKey("dbo.Groups", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "GroupNameId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Groups", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.DamageStocks", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.DamageStocks", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.DamageStocks", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.DamageStocks", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.DamageStocks", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Damages", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCustomerRelations", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCustomerRelations", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Invoices", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Salesman", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Invoices", "SalesmanId", "dbo.Salesman");
            DropForeignKey("dbo.Salesman", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Salesman", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.InvoiceProducts", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.ReturnProducts", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.WarehouseZones", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Warehouses", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Warehouses", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Warehouses", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.WarehouseZones", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.StockWarehouseRelations", "ZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.StockWarehouseRelations", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.Products", "DefaultZoneId", "dbo.WarehouseZones");
            DropForeignKey("dbo.WarehouseZones", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.WarehouseZones", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Returns", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ReturnProducts", "ReturnId", "dbo.Returns");
            DropForeignKey("dbo.Returns", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Returns", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Returns", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.ReturnProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ReturnProducts", "InvoiceProductId", "dbo.InvoiceProducts");
            DropForeignKey("dbo.ReturnProducts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ReturnProducts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.InvoiceProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.InvoiceProducts", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Damages", "InvoiceProduct_InvoiceProductId", "dbo.InvoiceProducts");
            DropForeignKey("dbo.InvoiceProducts", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.InvoiceProducts", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.InvoiceBills", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceBills", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Bills", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.InvoiceBills", "BillId", "dbo.Bills");
            DropForeignKey("dbo.Bills", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Bills", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Damages", "Invoice_InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Damages", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Damages", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.DamageReturns", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.DamageReturns", "DamageId", "dbo.Damages");
            DropForeignKey("dbo.DamageReturns", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.DamageReturns", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.DamageDismisses", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.DamageDismisses", "DamageId", "dbo.Damages");
            DropForeignKey("dbo.DamageDismisses", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.DamageDismisses", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Damages", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Damages", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Invoices", "CustomerBranchId", "dbo.CustomerProjects");
            DropForeignKey("dbo.Invoices", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Invoices", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Invoices", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.CustomerProjects", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.CustomerProjects", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerProjects", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Customers", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Customers", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Products", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.ProductAttributeSets", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "AttributeSetId", "dbo.ProductAttributeSets");
            DropForeignKey("dbo.ProductAttributeSets", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ProductAttributeSets", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.AttributeSetAttributes", "AttributeSetId", "dbo.ProductAttributeSets");
            DropForeignKey("dbo.ProductAttributes", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ProductAttributeRelations", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductAttributeRelations", "AttributeId", "dbo.ProductAttributes");
            DropForeignKey("dbo.ProductAttributes", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ProductAttributes", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.AttributeSetAttributes", "AttributeId", "dbo.ProductAttributes");
            DropForeignKey("dbo.Stocks", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Stocks", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.ArticleTransfers", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ArticleTransfers", "CompanyId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.CompanyProfiles", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.CompanyProfiles", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.Roles", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.RoleTasks", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleTasks", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Roles", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Users", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Users", "CompanyId", "dbo.CompanyProfiles");
            DropIndex("dbo.WarehouseTracks", new[] { "CompanyId" });
            DropIndex("dbo.WarehouseTracks", new[] { "UpdatedBy" });
            DropIndex("dbo.WarehouseTracks", new[] { "CreatedBy" });
            DropIndex("dbo.WarehouseTracks", new[] { "WarehouseId" });
            DropIndex("dbo.WarehouseTracks", new[] { "ZoneId" });
            DropIndex("dbo.WarehouseRows", new[] { "CompanyId" });
            DropIndex("dbo.WarehouseRows", new[] { "UpdatedBy" });
            DropIndex("dbo.WarehouseRows", new[] { "CreatedBy" });
            DropIndex("dbo.WarehouseRows", new[] { "WarehouseId" });
            DropIndex("dbo.WarehouseRows", new[] { "ZoneId" });
            DropIndex("dbo.TransferProducts", new[] { "CompanyId" });
            DropIndex("dbo.TransferProducts", new[] { "UpdatedBy" });
            DropIndex("dbo.TransferProducts", new[] { "CreatedBy" });
            DropIndex("dbo.TransferProducts", new[] { "ZoneToId" });
            DropIndex("dbo.TransferProducts", new[] { "ZoneFromId" });
            DropIndex("dbo.TransferProducts", new[] { "StockId" });
            DropIndex("dbo.Transactions", new[] { "CompanyId" });
            DropIndex("dbo.Transactions", new[] { "UpdatedBy" });
            DropIndex("dbo.Transactions", new[] { "CreatedBy" });
            DropIndex("dbo.Settings", new[] { "CompanyId" });
            DropIndex("dbo.Settings", new[] { "UpdatedBy" });
            DropIndex("dbo.Settings", new[] { "CreatedBy" });
            DropIndex("dbo.Notices", new[] { "CompanyId" });
            DropIndex("dbo.Notices", new[] { "UpdatedBy" });
            DropIndex("dbo.Notices", new[] { "CreatedBy" });
            DropIndex("dbo.NoticeAttachments", new[] { "CompanyId" });
            DropIndex("dbo.NoticeAttachments", new[] { "NoticeId" });
            DropIndex("dbo.MobileBangking", new[] { "CompanyId" });
            DropIndex("dbo.MobileBangking", new[] { "UpdatedBy" });
            DropIndex("dbo.MobileBangking", new[] { "CreatedBy" });
            DropIndex("dbo.OrderProducts", new[] { "CompanyId" });
            DropIndex("dbo.OrderProducts", new[] { "CreatedBy" });
            DropIndex("dbo.OrderProducts", new[] { "ZoneId" });
            DropIndex("dbo.OrderProducts", new[] { "ProductId" });
            DropIndex("dbo.OrderProducts", new[] { "OrderId" });
            DropIndex("dbo.SalesOrders", new[] { "CompanyId" });
            DropIndex("dbo.SalesOrders", new[] { "UpdatedBy" });
            DropIndex("dbo.SalesOrders", new[] { "CreatedBy" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerBranchId" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerId" });
            DropIndex("dbo.InvoiceOrders", new[] { "OrderId" });
            DropIndex("dbo.InvoiceOrders", new[] { "InvoiceId" });
            DropIndex("dbo.ExpenseTypes", new[] { "CompanyId" });
            DropIndex("dbo.ExpenseTypes", new[] { "UpdatedBy" });
            DropIndex("dbo.ExpenseTypes", new[] { "CreatedBy" });
            DropIndex("dbo.ExpenseTypes", new[] { "ParentId" });
            DropIndex("dbo.Expenses", new[] { "CompanyId" });
            DropIndex("dbo.Expenses", new[] { "UpdatedBy" });
            DropIndex("dbo.Expenses", new[] { "CreatedBy" });
            DropIndex("dbo.Expenses", new[] { "ExpenseTypeId" });
            DropIndex("dbo.DeptRoleConfigs", new[] { "UpdatedBy" });
            DropIndex("dbo.DeptRoleConfigs", new[] { "CreatedBy" });
            DropIndex("dbo.DeptRoleConfigs", new[] { "RoleId" });
            DropIndex("dbo.DeptRoleConfigs", new[] { "DepartmentId" });
            DropIndex("dbo.Departments", new[] { "UpdatedBy" });
            DropIndex("dbo.Departments", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerBalances", new[] { "CompanyId" });
            DropIndex("dbo.CustomerBalances", new[] { "UpdatedBy" });
            DropIndex("dbo.CustomerBalances", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerBalances", new[] { "CustomerId" });
            DropIndex("dbo.CustomerBalances", new[] { "DepartmentId" });
            DropIndex("dbo.Cash", new[] { "CompanyId" });
            DropIndex("dbo.Cash", new[] { "UpdatedBy" });
            DropIndex("dbo.Cash", new[] { "CreatedBy" });
            DropIndex("dbo.BankAccounts", new[] { "CompanyId" });
            DropIndex("dbo.BankAccounts", new[] { "UpdatedBy" });
            DropIndex("dbo.BankAccounts", new[] { "CreatedBy" });
            DropIndex("dbo.AuditLogs", new[] { "CompanyId" });
            DropIndex("dbo.AuditLogs", new[] { "CreatedUser" });
            DropIndex("dbo.Suppliers", new[] { "CompanyId" });
            DropIndex("dbo.Suppliers", new[] { "UpdatedBy" });
            DropIndex("dbo.Suppliers", new[] { "CreatedBy" });
            DropIndex("dbo.PurchasePayments", new[] { "CompanyId" });
            DropIndex("dbo.PurchasePayments", new[] { "UpdatedBy" });
            DropIndex("dbo.PurchasePayments", new[] { "CreatedBy" });
            DropIndex("dbo.PurchasePayments", new[] { "PurchaseCostId" });
            DropIndex("dbo.PurchasePayments", new[] { "PurchaseId" });
            DropIndex("dbo.PurchaseCosts", new[] { "CompanyId" });
            DropIndex("dbo.PurchaseCosts", new[] { "UpdatedBy" });
            DropIndex("dbo.PurchaseCosts", new[] { "CreatedBy" });
            DropIndex("dbo.PurchaseCosts", new[] { "PurchaseId" });
            DropIndex("dbo.Purchases", new[] { "CompanyId" });
            DropIndex("dbo.Purchases", new[] { "UpdatedBy" });
            DropIndex("dbo.Purchases", new[] { "CreatedBy" });
            DropIndex("dbo.Purchases", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseProducts", new[] { "CompanyId" });
            DropIndex("dbo.PurchaseProducts", new[] { "CreatedBy" });
            DropIndex("dbo.PurchaseProducts", new[] { "ZoneId" });
            DropIndex("dbo.PurchaseProducts", new[] { "ProductId" });
            DropIndex("dbo.PurchaseProducts", new[] { "PurchaseId" });
            DropIndex("dbo.Categories", new[] { "CompanyId" });
            DropIndex("dbo.Categories", new[] { "UpdatedBy" });
            DropIndex("dbo.Categories", new[] { "CreatedBy" });
            DropIndex("dbo.Categories", new[] { "CategoryParentId" });
            DropIndex("dbo.ProductCategories", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategories", new[] { "ProductId" });
            DropIndex("dbo.Groups", new[] { "CompanyId" });
            DropIndex("dbo.Groups", new[] { "UpdatedBy" });
            DropIndex("dbo.Groups", new[] { "CreatedBy" });
            DropIndex("dbo.DamageStocks", new[] { "Product_ProductId" });
            DropIndex("dbo.DamageStocks", new[] { "CompanyId" });
            DropIndex("dbo.DamageStocks", new[] { "UpdatedBy" });
            DropIndex("dbo.DamageStocks", new[] { "CreatedBy" });
            DropIndex("dbo.DamageStocks", new[] { "StockId" });
            DropIndex("dbo.Salesman", new[] { "CompanyId" });
            DropIndex("dbo.Salesman", new[] { "UpdatedBy" });
            DropIndex("dbo.Salesman", new[] { "CreatedBy" });
            DropIndex("dbo.Warehouses", new[] { "CompanyId" });
            DropIndex("dbo.Warehouses", new[] { "UpdatedBy" });
            DropIndex("dbo.Warehouses", new[] { "CreatedBy" });
            DropIndex("dbo.StockWarehouseRelations", new[] { "ZoneId" });
            DropIndex("dbo.StockWarehouseRelations", new[] { "StockId" });
            DropIndex("dbo.WarehouseZones", new[] { "CompanyId" });
            DropIndex("dbo.WarehouseZones", new[] { "UpdatedBy" });
            DropIndex("dbo.WarehouseZones", new[] { "CreatedBy" });
            DropIndex("dbo.WarehouseZones", new[] { "WarehouseId" });
            DropIndex("dbo.Returns", new[] { "CompanyId" });
            DropIndex("dbo.Returns", new[] { "UpdatedBy" });
            DropIndex("dbo.Returns", new[] { "CreatedBy" });
            DropIndex("dbo.Returns", new[] { "InvoiceId" });
            DropIndex("dbo.ReturnProducts", new[] { "CompanyId" });
            DropIndex("dbo.ReturnProducts", new[] { "CreatedBy" });
            DropIndex("dbo.ReturnProducts", new[] { "ZoneId" });
            DropIndex("dbo.ReturnProducts", new[] { "InvoiceProductId" });
            DropIndex("dbo.ReturnProducts", new[] { "ProductId" });
            DropIndex("dbo.ReturnProducts", new[] { "ReturnId" });
            DropIndex("dbo.InvoiceProducts", new[] { "CompanyId" });
            DropIndex("dbo.InvoiceProducts", new[] { "CreatedBy" });
            DropIndex("dbo.InvoiceProducts", new[] { "ZoneId" });
            DropIndex("dbo.InvoiceProducts", new[] { "ProductId" });
            DropIndex("dbo.InvoiceProducts", new[] { "InvoiceId" });
            DropIndex("dbo.Bills", new[] { "UpdatedBy" });
            DropIndex("dbo.Bills", new[] { "CreatedBy" });
            DropIndex("dbo.Bills", new[] { "CustomerId" });
            DropIndex("dbo.InvoiceBills", new[] { "CreatedBy" });
            DropIndex("dbo.InvoiceBills", new[] { "InvoiceId" });
            DropIndex("dbo.InvoiceBills", new[] { "BillId" });
            DropIndex("dbo.DamageReturns", new[] { "CompanyId" });
            DropIndex("dbo.DamageReturns", new[] { "UpdatedBy" });
            DropIndex("dbo.DamageReturns", new[] { "CreatedBy" });
            DropIndex("dbo.DamageReturns", new[] { "DamageId" });
            DropIndex("dbo.DamageDismisses", new[] { "CompanyId" });
            DropIndex("dbo.DamageDismisses", new[] { "UpdatedBy" });
            DropIndex("dbo.DamageDismisses", new[] { "CreatedBy" });
            DropIndex("dbo.DamageDismisses", new[] { "DamageId" });
            DropIndex("dbo.Damages", new[] { "Product_ProductId" });
            DropIndex("dbo.Damages", new[] { "InvoiceProduct_InvoiceProductId" });
            DropIndex("dbo.Damages", new[] { "Invoice_InvoiceId" });
            DropIndex("dbo.Damages", new[] { "CompanyId" });
            DropIndex("dbo.Damages", new[] { "UpdatedBy" });
            DropIndex("dbo.Damages", new[] { "CreatedBy" });
            DropIndex("dbo.Damages", new[] { "StockId" });
            DropIndex("dbo.Invoices", new[] { "CompanyId" });
            DropIndex("dbo.Invoices", new[] { "UpdatedBy" });
            DropIndex("dbo.Invoices", new[] { "CreatedBy" });
            DropIndex("dbo.Invoices", new[] { "SalesmanId" });
            DropIndex("dbo.Invoices", new[] { "CustomerBranchId" });
            DropIndex("dbo.Invoices", new[] { "CustomerId" });
            DropIndex("dbo.CustomerProjects", new[] { "UpdatedBy" });
            DropIndex("dbo.CustomerProjects", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerProjects", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "CompanyId" });
            DropIndex("dbo.Customers", new[] { "UpdatedBy" });
            DropIndex("dbo.Customers", new[] { "CreatedBy" });
            DropIndex("dbo.ProductCustomerRelations", new[] { "CustomerId" });
            DropIndex("dbo.ProductCustomerRelations", new[] { "ProductId" });
            DropIndex("dbo.ProductAttributeRelations", new[] { "AttributeId" });
            DropIndex("dbo.ProductAttributeRelations", new[] { "ProductId" });
            DropIndex("dbo.ProductAttributes", new[] { "CompanyId" });
            DropIndex("dbo.ProductAttributes", new[] { "UpdatedBy" });
            DropIndex("dbo.ProductAttributes", new[] { "CreatedBy" });
            DropIndex("dbo.AttributeSetAttributes", new[] { "AttributeId" });
            DropIndex("dbo.AttributeSetAttributes", new[] { "AttributeSetId" });
            DropIndex("dbo.ProductAttributeSets", new[] { "CompanyId" });
            DropIndex("dbo.ProductAttributeSets", new[] { "UpdatedBy" });
            DropIndex("dbo.ProductAttributeSets", new[] { "CreatedBy" });
            DropIndex("dbo.Products", new[] { "Category_CategoryId" });
            DropIndex("dbo.Products", new[] { "CompanyId" });
            DropIndex("dbo.Products", new[] { "UpdatedBy" });
            DropIndex("dbo.Products", new[] { "CreatedBy" });
            DropIndex("dbo.Products", new[] { "DefaultZoneId" });
            DropIndex("dbo.Products", new[] { "ManufacturerId" });
            DropIndex("dbo.Products", new[] { "GroupNameId" });
            DropIndex("dbo.Products", new[] { "AttributeSetId" });
            DropIndex("dbo.Stocks", new[] { "CompanyId" });
            DropIndex("dbo.Stocks", new[] { "UpdatedBy" });
            DropIndex("dbo.Stocks", new[] { "CreatedBy" });
            DropIndex("dbo.Stocks", new[] { "ProductId" });
            DropIndex("dbo.RoleTasks", new[] { "CreatedBy" });
            DropIndex("dbo.RoleTasks", new[] { "RoleId" });
            DropIndex("dbo.Roles", new[] { "UpdatedBy" });
            DropIndex("dbo.Roles", new[] { "CreatedBy" });
            DropIndex("dbo.Users", new[] { "Department_DepartmentId" });
            DropIndex("dbo.Users", new[] { "Role_RoleId" });
            DropIndex("dbo.Users", new[] { "CompanyId" });
            DropIndex("dbo.Users", new[] { "CreatedBy" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.CompanyProfiles", new[] { "UpdatedBy" });
            DropIndex("dbo.CompanyProfiles", new[] { "CreatedBy" });
            DropIndex("dbo.ArticleTransfers", new[] { "CompanyId" });
            DropIndex("dbo.ArticleTransfers", new[] { "UpdatedBy" });
            DropIndex("dbo.ArticleTransfers", new[] { "CreatedBy" });
            DropIndex("dbo.ArticleTransfers", new[] { "ToStockId" });
            DropIndex("dbo.ArticleTransfers", new[] { "FromStockId" });
            DropTable("dbo.WarehouseTracks");
            DropTable("dbo.WarehouseRows");
            DropTable("dbo.TransferProducts");
            DropTable("dbo.Transactions");
            DropTable("dbo.Settings");
            DropTable("dbo.Notices");
            DropTable("dbo.NoticeAttachments");
            DropTable("dbo.MobileBangking");
            DropTable("dbo.Logins");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.SalesOrders");
            DropTable("dbo.InvoiceOrders");
            DropTable("dbo.ExpenseTypes");
            DropTable("dbo.Expenses");
            DropTable("dbo.DeptRoleConfigs");
            DropTable("dbo.Departments");
            DropTable("dbo.CustomerBalances");
            DropTable("dbo.Cash");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.Suppliers");
            DropTable("dbo.PurchasePayments");
            DropTable("dbo.PurchaseCosts");
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseProducts");
            DropTable("dbo.Categories");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Groups");
            DropTable("dbo.DamageStocks");
            DropTable("dbo.Salesman");
            DropTable("dbo.Warehouses");
            DropTable("dbo.StockWarehouseRelations");
            DropTable("dbo.WarehouseZones");
            DropTable("dbo.Returns");
            DropTable("dbo.ReturnProducts");
            DropTable("dbo.InvoiceProducts");
            DropTable("dbo.Bills");
            DropTable("dbo.InvoiceBills");
            DropTable("dbo.DamageReturns");
            DropTable("dbo.DamageDismisses");
            DropTable("dbo.Damages");
            DropTable("dbo.Invoices");
            DropTable("dbo.CustomerProjects");
            DropTable("dbo.Customers");
            DropTable("dbo.ProductCustomerRelations");
            DropTable("dbo.ProductAttributeRelations");
            DropTable("dbo.ProductAttributes");
            DropTable("dbo.AttributeSetAttributes");
            DropTable("dbo.ProductAttributeSets");
            DropTable("dbo.Products");
            DropTable("dbo.Stocks");
            DropTable("dbo.RoleTasks");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.CompanyProfiles");
            DropTable("dbo.ArticleTransfers");
        }
    }
}
