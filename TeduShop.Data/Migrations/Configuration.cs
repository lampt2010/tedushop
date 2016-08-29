namespace TeduShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TeduShop.Data.TeduShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TeduShop.Data.TeduShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new TeduShopDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new TeduShopDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "tedu",
                Email = "tedu.international@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Technology Education"

            };

            manager.Create(user, "123456");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("tedu.international@gmail.com");

            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
            CreateProductCategorySampble(context);
            CreateFooterSample(context);
            CreateSlideSample(context);
        }

        private void CreateProductCategorySampble(TeduShop.Data.TeduShopDbContext context)
        {
            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listCategory = new List<ProductCategory>()
            {
                new ProductCategory(){ Name=@"Máy tính", Alias="may-tinh", Status=true },
                 new ProductCategory(){ Name=@"Điện thoại", Alias="dien-thoai", Status=true },
                  new ProductCategory(){ Name=@"Xe máy", Alias="xe-may", Status=true },
                   new ProductCategory(){ Name=@"Ô tô", Alias="o-to", Status=true }
            };

                context.ProductCategories.AddRange(listCategory);
                context.SaveChanges();
            }
        }

        private void CreateFooterSample(TeduShop.Data.TeduShopDbContext context)
        {
            if (context.Footers.Count(x => x.ID == Common.CommonConstants.FooterIdDefault) == 0)
            {
                Footer footer = new Footer()
                {
                    ID = Common.CommonConstants.FooterIdDefault,
                    Content = @"<p>đây là phần chân trang</p>"
                };
                context.Footers.Add(footer);
                context.SaveChanges();
            }
        }

        private void CreateSlideSample(TeduShop.Data.TeduShopDbContext context)
        {
            if (context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                    new Slide() {Name="Slide 1",DisplayOrder=1,Status=true,Url="#",Image="/Assets/client/images/1.jpg" },
                    new Slide() {Name="Slide 2",DisplayOrder=2,Status=true,Url="#",Image="/Assets/client/images/2.jpg" },
                    new Slide() {Name="Slide 3",DisplayOrder=3,Status=true,Url="#",Image="/Assets/client/images/3.jpg" },
                    new Slide() {Name="Slide 4",DisplayOrder=4,Status=true,Url="#",Image="/Assets/client/images/4.jpg" }

                };

                context.Slides.AddRange(listSlide);
                context.SaveChanges();
            }
        }
    }
}
