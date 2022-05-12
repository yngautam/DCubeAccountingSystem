using DCubeHotelDomain.Models.Roles;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.MenuCategory;
using DCubeHotelDomain.Models.Settings;
using DCubeHotelDomain.Models;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using DCubeHotelDomain.Models.Tickets;
using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DCubeHotelSystem.DataContext
{
    public class HotelDbContext : IdentityDbContext<HotelUser>
    {
        static HotelDbContext()
        {
            Database.SetInitializer<DCubeHotelSystem.DataContext.HotelDbContext>(new MigrateDatabaseToLatestVersion<DCubeHotelSystem.DataContext.HotelDbContext, DCubeHotelSystem.Migrations.Configuration>());
        }
        public HotelDbContext()
            : base(("DCubeConnection"))
        {
            bool db = Database.Exists("DCubeConnection");
            if (db == false)
            {
                Database.Create();
                Database.CommandTimeout = 180;
            }
        }

        //Error Handling Log
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        //
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        //Inventory
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<InventoryReceipt> InventoryReceipts { get; set; }
        public DbSet<InventoryReceiptDetails> InventoryReceiptDetails { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<InventoryTransactionDocument> InventoryTransactionDocuments { get; set; }
        public DbSet<InventoryTransactionDocumentType> InventoryTransactionDocumentTypes { get; set; }
        public DbSet<InventoryTransactionType> InventoryTransactionTypes { get; set; }
        public DbSet<PeriodicConsumption> PeriodicConsumptions { get; set; }
        public DbSet<PeriodicConsumptionItem> PeriodicConsumptionItem { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseType> WarehouseTypes { get; set; }

        //Accounting and Finance
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<AccountTransactionDocument> AccountTransactionDocuments { get; set; }
        public DbSet<AccountTransactionType> AccountTransactionTypes { get; set; }
        public DbSet<AccountTransactionValue> AccountTransactionValues { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }


        //Billing Menu
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemPortion> MenuItemPortions { get; set; }
        public DbSet<MenuConsumption> MenuConsumptions { get; set; }
        public DbSet<MenuConsumptionDetail> MenuConsumptionDetails { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ScreenMenuCategories> ScreenMenuCategories { get; set; }
        public DbSet<DCubeHotelDomain.Models.Settings.ForeignCurrency> ForeignCurrencys { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MenuCategoryItem> MenuCategoryItems { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<MenuCategory> Menucategories { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationDetail> QuotationDetails { get; set; }
        public DbSet<OrderManagement> OrderManagements { get; set; }
        public DbSet<OrderManagementDetail> OrderManagementDetails { get; set; }
        public DbSet<MenuItemPortionPriceRange> MenuItemPortionPriceRanges { get; set; }
        public DbSet<MenuItemPhoto> MenuItemPhotos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<HotelUser>().ToTable("DcubeUsers", "dbo");
            //modelBuilder.Entity<IdentityRole>().ToTable("DcubeRoles", "dbo");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("DcubeUserRoles", "dbo");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("DcubeUserClaims", "dbo");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("DcubeUserLogins", "dbo");

            modelBuilder.Entity<Account>().Property(x => x.Name).IsMaxLength();
            modelBuilder.Entity<Ticket>().Property(x => x.TicketStates).IsMaxLength();
            modelBuilder.Entity<Ticket>().Property(x => x.Note).IsMaxLength();
            modelBuilder.Entity<Ticket>().Property(x => x.TicketStates).IsMaxLength();

            modelBuilder.Entity<MenuItemPortionPriceRange>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuItemPortionPriceRange>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MenuItemPortionPriceRange>().Property(p => p.QtyMax).IsRequired();
            modelBuilder.Entity<MenuItemPortionPriceRange>().Property(p => p.QtyMin).IsRequired();
            modelBuilder.Entity<MenuItemPortionPriceRange>().Property(p => p.Price).IsRequired();

            modelBuilder.Entity<Account>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Account>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Account>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<AccountType>().HasKey(p => new { p.Id });
            modelBuilder.Entity<AccountType>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AccountType>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<AccountTransactionType>().HasKey(p => new { p.Id });
            modelBuilder.Entity<AccountTransactionType>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AccountTransactionType>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<AccountTransactionDocument>().HasKey(p => new { p.Id });
            modelBuilder.Entity<AccountTransactionDocument>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AccountTransactionDocument>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<AccountTransaction>().Ignore(p => p.Discount);
            modelBuilder.Entity<AccountTransaction>().Ignore(p => p.PercentAmount);
            modelBuilder.Entity<AccountTransaction>().Ignore(p => p.NetAmount);
            modelBuilder.Entity<AccountTransaction>().Ignore(p => p.VATAmount);
            modelBuilder.Entity<AccountTransaction>().Ignore(p => p.GrandAmount);
            modelBuilder.Entity<AccountTransaction>().Ignore(p => p.IsDiscountPercentage);
            modelBuilder.Entity<AccountTransaction>().HasKey(p => new { p.Id });
            modelBuilder.Entity<AccountTransaction>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AccountTransactionDocument>().HasMany(p => p.AccountTransactions).WithRequired().HasForeignKey(x => x.AccountTransactionDocumentId);

            modelBuilder.Entity<AccountTransactionValue>().HasKey(p => new { p.Id });
            modelBuilder.Entity<AccountTransactionValue>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AccountTransactionValue>().Property(p => p.Name).IsRequired();
            //modelBuilder.Entity<AccountTransaction>().HasMany(p => p.AccountTransactionValues).WithRequired().HasForeignKey(x => new { x.AccountTransactionId });

            modelBuilder.Entity<Category>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Category>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Category>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<Company>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Company>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Company>().Property(p => p.NameEnglish).IsRequired();
            modelBuilder.Entity<Company>().Property(p => p.NameNepali).IsRequired();

            modelBuilder.Entity<Department>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Department>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Department>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<FinancialYear>().HasKey(p => new { p.Id });
            modelBuilder.Entity<FinancialYear>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<FinancialYear>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<InventoryItem>().HasKey(p => new { p.Id });
            modelBuilder.Entity<InventoryItem>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<InventoryItem>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<UnitType>().HasKey(p => new { p.Id });
            modelBuilder.Entity<UnitType>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UnitType>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<InventoryReceipt>().HasKey(p => new { p.Id });
            modelBuilder.Entity<InventoryReceipt>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<InventoryReceipt>().Property(p => p.ReceiptNumber).IsRequired();

            modelBuilder.Entity<InventoryReceiptDetails>().HasKey(p => new { p.Id });
            modelBuilder.Entity<InventoryReceiptDetails>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<InventoryReceipt>().HasMany(p => p.InventoryReceiptDetails).WithRequired().HasForeignKey(x => new { x.InventoryReceiptId });

            modelBuilder.Entity<MenuCategoryItem>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuCategoryItem>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MenuCategory>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuCategory>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MenuConsumption>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuConsumption>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MenuConsumptionDetail>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuConsumptionDetail>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MenuConsumption>().HasMany(p => p.MenuConsumptionDetails).WithRequired().HasForeignKey(x => new { x.MenuConsumptionId });

            modelBuilder.Entity<MenuItem>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuItem>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MenuItemPortion>().HasKey(p => new { p.Id });
            modelBuilder.Entity<MenuItemPortion>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<MenuItem>().HasMany(p => p.MenuItemPortions).WithRequired().HasForeignKey(x => new { x.MenuItemPortionId });

            modelBuilder.Entity<Menu>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Menu>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Order>().Ignore(p => p.IsSelected);
            modelBuilder.Entity<Order>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Order>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Ticket>().HasMany(p => p.Orders).WithRequired().HasForeignKey(x => x.TicketId);

            modelBuilder.Entity<PeriodicConsumption>().HasKey(p => new { p.Id });
            modelBuilder.Entity<PeriodicConsumption>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PeriodicConsumption>().HasMany(p => p.PeriodicConsumptionItems).WithRequired().HasForeignKey(x => new { x.PeriodicConsumptionId });

            modelBuilder.Entity<PeriodicConsumptionItem>().HasKey(p => new { p.Id });
            modelBuilder.Entity<PeriodicConsumptionItem>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PeriodicConsumptionItem>().Ignore(p => p.UnitName);
            modelBuilder.Entity<PeriodicConsumptionItem>().Ignore(p => p.UnitMultiplier);
            modelBuilder.Entity<PeriodicConsumptionItem>().Ignore(p => p.InStock);
            modelBuilder.Entity<PeriodicConsumptionItem>().Ignore(p => p.PhysicalInventory);
            modelBuilder.Entity<PeriodicConsumptionItem>().Ignore(p => p.Cost);

            modelBuilder.Entity<PurchaseDetails>().Property(p => p.PurchaseId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PurchaseDetails>().HasKey(p => new { p.PurchaseId });
            //modelBuilder.Entity<AccountTransaction>().HasMany(p => p.PurchaseDetails).WithRequired().HasForeignKey(x => new { x.AccountTransactionId });

            //modelBuilder.Entity<PurchaseOrderDetails>().Property(p => p.PurchaseOrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<PurchaseOrderDetails>().HasKey(p => new { p.PurchaseOrderId, p.AccountTransactionId, p.AccountTransactionDocumentId });

            modelBuilder.Entity<ScreenMenuCategories>().HasKey(p => new { p.Id });
            modelBuilder.Entity<ScreenMenuCategories>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ScreenOrderDetails>().HasKey(p => new { p.Id });
            modelBuilder.Entity<ScreenOrderDetails>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Table>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Table>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Ticket>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Ticket>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Warehouse>().HasKey(p => new { p.Id });
            modelBuilder.Entity<Warehouse>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<WarehouseType>().HasKey(p => new { p.Id });
            modelBuilder.Entity<WarehouseType>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            const int qscale = 3;
            const int scale = 2;
            const int precision = 16;

            modelBuilder.Properties<DateTime>().Configure(property => property.HasColumnType("datetime2"));

            //Account
            modelBuilder.Entity<Account>().Property(x => x.Amount).HasPrecision(precision, scale);

            //Account Transaction
            modelBuilder.Entity<AccountTransaction>().Property(x => x.Amount).HasPrecision(precision, scale);
            modelBuilder.Entity<AccountTransaction>().Property(x => x.ExchangeRate).HasPrecision(precision, scale);

            //Account Transaction Value
            modelBuilder.Entity<AccountTransactionValue>().Property(x => x.Debit).HasPrecision(precision, scale);
            modelBuilder.Entity<AccountTransactionValue>().Property(x => x.Credit).HasPrecision(precision, scale);
            modelBuilder.Entity<AccountTransactionValue>().Property(x => x.Exchange).HasPrecision(precision, scale);

            //Foreign Currency
            modelBuilder.Entity<ForeignCurrency>().Property(x => x.ExchangeRate).HasPrecision(precision, scale);

            //InventoryReceiptDetails
            modelBuilder.Entity<InventoryReceiptDetails>().Property(x => x.Quantity).HasPrecision(precision, scale);
            modelBuilder.Entity<InventoryReceiptDetails>().Property(x => x.Rate).HasPrecision(precision, scale);
            modelBuilder.Entity<InventoryReceiptDetails>().Property(x => x.TotalAmount).HasPrecision(precision, scale);

            //MenuItemPortion
            modelBuilder.Entity<MenuItemPortion>().Property(x => x.Price).HasPrecision(precision, scale);

            //MenuItemPrice
            modelBuilder.Entity<MenuItemPrice>().Property(x => x.Price).HasPrecision(precision, scale);

            //TransactionItem
            modelBuilder.Entity<InventoryTransaction>().Property(x => x.Price).HasPrecision(precision, scale);
            modelBuilder.Entity<InventoryTransaction>().Property(x => x.Quantity).HasPrecision(precision, qscale);

            //PeriodicConsumptionItem
            modelBuilder.Entity<PeriodicConsumptionItem>().Property(x => x.Consumption).HasPrecision(precision, qscale);

            //PurchaseDetails
            modelBuilder.Entity<PurchaseDetails>().Property(x => x.PurchaseAmount).HasPrecision(precision, qscale);
            modelBuilder.Entity<PurchaseDetails>().Property(x => x.PurchaseRate).HasPrecision(precision, qscale);
            modelBuilder.Entity<PurchaseDetails>().Property(x => x.Quantity).HasPrecision(precision, qscale);

            //Order
            modelBuilder.Entity<Order>().Property(x => x.Quantity).HasPrecision(precision, qscale);
            modelBuilder.Entity<Order>().Property(x => x.Price).HasPrecision(precision, scale);

            //Ticket
            modelBuilder.Entity<Ticket>().Property(x => x.RemainingAmount).HasPrecision(precision, scale);
            modelBuilder.Entity<Ticket>().Property(x => x.TotalAmount).HasPrecision(precision, scale);

            //   modelBuilder.Entity<Numerator>().Property(x => x.LastUpdateTime).IsConcurrencyToken().HasColumnType("timestamp");
            base.OnModelCreating(modelBuilder);
        }
    }
}