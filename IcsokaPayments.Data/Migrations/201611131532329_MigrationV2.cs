namespace IcsokaPayments.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinValue = c.Double(nullable: false),
                        MaxValue = c.Double(nullable: false),
                        Flat = c.String(),
                        Percentage = c.Double(nullable: false),
                        FeeSharing_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeeSharings", t => t.FeeSharing_Id)
                .Index(t => t.FeeSharing_Id);
            
            CreateTable(
                "dbo.FeeSharings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeeId = c.Int(nullable: false),
                        MerchantId = c.Int(nullable: false),
                        Percentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Merchants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Domain = c.String(),
                        CallBackURL = c.String(),
                        FeeSharing_Id = c.Int(),
                        MerchantPaymentGateway_Id = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeeSharings", t => t.FeeSharing_Id)
                .ForeignKey("dbo.MerchantPaymentGateways", t => t.MerchantPaymentGateway_Id)
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id)
                .Index(t => t.FeeSharing_Id)
                .Index(t => t.MerchantPaymentGateway_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "dbo.MerchantBanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MerchantId = c.Int(nullable: false),
                        BankId = c.Int(nullable: false),
                        AccountNo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MerchantPaymentGateways",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MerchantId = c.Int(nullable: false),
                        PaymentGatewayId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentGateways",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        URL = c.String(),
                        ConfirmationAPI = c.String(),
                        MerchantPaymentGateway_Id = c.Int(),
                        PaymentsStatus_Id = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MerchantPaymentGateways", t => t.MerchantPaymentGateway_Id)
                .ForeignKey("dbo.PaymentsStatus", t => t.PaymentsStatus_Id)
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id)
                .Index(t => t.MerchantPaymentGateway_Id)
                .Index(t => t.PaymentsStatus_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "dbo.PaymentsStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentGatewayId = c.Int(nullable: false),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionHTML = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        paymentReference = c.String(),
                        TransactionReferrence = c.String(),
                        Amount = c.Double(nullable: false),
                        FeeSharing = c.Int(nullable: false),
                        MerchantId = c.Int(nullable: false),
                        PaymentGatewayId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Verified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentGateways", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.Merchants", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.PaymentGateways", "PaymentsStatus_Id", "dbo.PaymentsStatus");
            DropForeignKey("dbo.PaymentGateways", "MerchantPaymentGateway_Id", "dbo.MerchantPaymentGateways");
            DropForeignKey("dbo.Merchants", "MerchantPaymentGateway_Id", "dbo.MerchantPaymentGateways");
            DropForeignKey("dbo.Merchants", "FeeSharing_Id", "dbo.FeeSharings");
            DropForeignKey("dbo.Fees", "FeeSharing_Id", "dbo.FeeSharings");
            DropIndex("dbo.PaymentGateways", new[] { "Transaction_Id" });
            DropIndex("dbo.PaymentGateways", new[] { "PaymentsStatus_Id" });
            DropIndex("dbo.PaymentGateways", new[] { "MerchantPaymentGateway_Id" });
            DropIndex("dbo.Merchants", new[] { "Transaction_Id" });
            DropIndex("dbo.Merchants", new[] { "MerchantPaymentGateway_Id" });
            DropIndex("dbo.Merchants", new[] { "FeeSharing_Id" });
            DropIndex("dbo.Fees", new[] { "FeeSharing_Id" });
            DropTable("dbo.Transactions");
            DropTable("dbo.TransactionLogs");
            DropTable("dbo.PaymentsStatus");
            DropTable("dbo.PaymentGateways");
            DropTable("dbo.MerchantPaymentGateways");
            DropTable("dbo.MerchantBanks");
            DropTable("dbo.Merchants");
            DropTable("dbo.FeeSharings");
            DropTable("dbo.Fees");
            DropTable("dbo.Banks");
        }
    }
}
