using B2B.Models;
using B2B.Models.Cart;
using B2B.Models.Orders;
using B2B.Models.Packages;
using B2B.Models.Shopkeeper;
using B2B.Models.Social;
using B2B.Models.Store;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace B2B.DAL
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DbConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Locations> Location { get; set; }

        //////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////Store/////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////

        public DbSet<Store_Category> StoreCategory { get; set; }
        public DbSet<Store_Company> StoreCompany { get; set; }
        public DbSet<Store_Measurement_Units> StoreMeasurementUnits { get; set; }
        public DbSet<Store_Product_Gallery> StoreProductGallery { get; set; }
        public DbSet<Store_Products> StoreProducts { get; set; }
        public DbSet<Store_Qty_Movement> StoreQtyMovements { get; set; }

        //////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////Packages/////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////

        public DbSet<PackageFeatures> PackageFeatures { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<MerchantPackageMeta> MerchantPackageMeta { get; set; }

        //////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////Orders/////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeader { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLine { get; set; }
        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////Shopkeeper//////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        public DbSet<Branch> Branch { get; set; }
        ////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////Social////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        public DbSet<MerchantConnection> MerchantConnection { get; set; }
        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////Cart//////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<FavouriteMerchants> FavouriteMerchant { get; set; }
        public DbSet<Complains> Complains { get; set; }
        public DbSet<Notification> Notification { get; set; }
       

    }
}