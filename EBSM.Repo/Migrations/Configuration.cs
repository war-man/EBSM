using System.Collections.Generic;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;

namespace WarehouseApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WarehouseApp.Models.WmsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WarehouseApp.Models.WmsDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //var companyProfile = new List<CompanyProfile>
            //{
            //    new CompanyProfile {CompanyName = "EBSM",CompanyAddress = "Gulshan, Bangladesh"}

            //};
            //companyProfile.ForEach(s => context.CompanyProfiles.AddOrUpdate(c => c.CompanyName, s));
            //context.SaveChanges("");

            //var roles = new List<Role>
            //{
            //    new Role {RoleName = "SuperAdmin", Status=2}
            //};
            //roles.ForEach(s => context.Roles.AddOrUpdate(r => r.RoleName, s));
            //context.SaveChanges("");

            //var deptRoles = new List<RoleTask>
            //{
            //    new RoleTask {RoleId = 1, Task = "Global_SupAdmin"}
            //};
            //deptRoles.ForEach(s => context.RoleTasks.AddOrUpdate(r => r.Task, s));
            //context.SaveChanges("");

            //var users = new List<User>
            //{
            //    new User {FullName = "WMS Super Admin",Email="wms@gmail.com",RoleId = 1, UserName = "administrator",Password = "827ccb0eea8a706c4c34a16891f84e7b",SupUser = true, CompanyId = 1,Status=2},//12345
            //    new User {FullName = "Development",Email="dev@gmail.com",RoleId = 1, UserName = "dev",Password = "90f2c9c53f66540e67349e0ab83d8cd0",SupUser = true,CompanyId = 1, Status=2}, //p@ssword
            //    new User {FullName = "Ebsm",Email="ebsm@gmail.com",RoleId = 1, UserName = "ebsm",Password = "827ccb0eea8a706c4c34a16891f84e7b",SupUser = true,CompanyId = 1, Status=1},//12345
            //};
            //users.ForEach(s => context.Users.AddOrUpdate(u => u.UserName, s));
            //context.SaveChanges("");


            //var cash = new List<Cash>
            //{
            //    new Cash {CashName = "Cash",Balance = 0,Status = 1,CreatedDate = DateTime.Now}
            //};
            //cash.ForEach(s => context.Cash.AddOrUpdate(u => u.CashName, s));
            //context.SaveChanges("");
            //var warehouses = new List<Warehouse>
            //{
            //    new Warehouse {WarehouseName = "Default Warehouse",IsDefault = true,Status = 1,CreatedDate = DateTime.Now}
            //};
            //warehouses.ForEach(s => context.Warehouses.AddOrUpdate(u => u.WarehouseName, s));
            //context.SaveChanges("");


            //var productAttributes = new List<ProductAttribute>      //seed for unit=========================================
            //{
            //   // new ProductAttribute {AttributeName = "BP",AttributeType = "Textbox", Values = " ",DefaultValue = " ", IsRequired = false,Status = 1,CreatedDate = DateTime.Now}
            //    new ProductAttribute {AttributeName = "Unit",AttributeType = "Selectbox", Values = "Pcs,Kg,Liter,Dozzen,Rft",DefaultValue = "Pcs", IsRequired = true,Status = 1,CreatedDate = DateTime.Now}
            //};
            //productAttributes.ForEach(s => context.ProductAttributes.AddOrUpdate(u => u.AttributeName, s));
            //context.SaveChanges("");

            //var productAttributeSets = new List<ProductAttributeSet>      //seed for attribute set=========================================
            //{
            //   new ProductAttributeSet {AttributeSetName = "General Items",Status = 1,CreatedDate = DateTime.Now}
            //};
            //productAttributeSets.ForEach(s => context.ProductAttributeSets.AddOrUpdate(u => u.AttributeSetName, s));
            //context.SaveChanges("");
            //var attributeAttributeSetRelation = new List<AttributeSetAttribute>      
            //{
            //   new AttributeSetAttribute {AttributeSetId = 1,AttributeId = 1}
            //};
            //attributeAttributeSetRelation.ForEach(s => context.AttributeSetAttributes.AddOrUpdate(u => u.AttributeSetId, s));
            //context.SaveChanges("");

            //var customers = new List<Customer>  //seed for Customer=========================================
            //{
            //    new Customer {FullName = "Swapno",Status = 1,CreatedDate = DateTime.Now},
            //    new Customer {FullName = "Agora",Status = 1,CreatedDate = DateTime.Now},
            //    new Customer {FullName = "Nandan",Status = 1,CreatedDate = DateTime.Now},
            //    new Customer {FullName = "CSD Super",Status = 1,CreatedDate = DateTime.Now},
            //    new Customer {FullName = "CSD Exclusive",Status = 1,CreatedDate = DateTime.Now},
            //    new Customer {FullName = "Happy Mart",Status = 1,CreatedDate = DateTime.Now},
            //    new Customer {FullName = "One Stop",Status = 1,CreatedDate = DateTime.Now}
            //};
            //customers.ForEach(s => context.Customers.AddOrUpdate(u => u.FullName, s));
            //context.SaveChanges("");
        }
    }
}
