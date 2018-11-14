namespace IcsokaPayments.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settlements", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Settlements", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settlements", "Email");
            DropColumn("dbo.Settlements", "Amount");
        }
    }
}
