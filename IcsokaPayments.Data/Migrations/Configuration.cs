using System.Collections.Generic;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<IscokaPaymentEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IscokaPaymentEntities context)
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
            InstallSettings(context);
        }

        private void InstallSettings(IscokaPaymentEntities context)
        {
            /*
             *  var setting = new FlutterWaveSetting()
            {
                MerchantKey = "tk_Bg36tT5iby",
                APIKey = "tk_RknixtdDhIumEZU4jTF7",
                UseSandBox = true,
                ValidateOption = "SMS",
                AuthModel = "PIN",
                Country = "NG",
                Currency = "NGN"
            };
             */
            var settings = new List<Setting>()
            {


            
             new Setting()
            {
                
                Name = "paymentsettings.activepaymentmethodsystemnames",
                Value = "FlutterWave",

            },
             new Setting()
            {
                
                Name = "flutterwavesetting.merchantkey",
                Value = "tk_Bg36tT5iby",

            },
              new Setting()
            {
                
                Name = "flutterwavesetting.apikey",
                Value = "tk_RknixtdDhIumEZU4jTF7",

            },
              new Setting()
            {
                
                Name = "flutterwavesetting.usesandbox",
                Value = "true",

            }
            ,  new Setting()
            {
                
                Name = "flutterwavesetting.currency",
                Value = "NGN",

            }
            ,  new Setting()
            {
                
                Name = "flutterwavesetting.country",
                Value = "NG",

            },  new Setting()
            {
                
                Name = "flutterwavesetting.authmodel",
                Value = "PIN",

            },
             new Setting()
            {
                
                Name = "flutterwavesetting.validateoption",
                Value = "SMS",

            }
            };
            settings.ForEach(x => context.Settings.AddOrUpdate(p => p.Name, x));
            context.SaveChanges();
        }
    }
}
