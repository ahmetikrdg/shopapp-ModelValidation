using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;

namespace shopapp.webui
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>(); 
            services.AddScoped<IProductRepository,EfCoreProductRepository>(); 

            services.AddScoped<IProductService,ProductManager>(); 
            services.AddScoped<ICategoryService,CategoryManager>(); 

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(); // wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"                
            });

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
             
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "adminproducts", 
                    pattern: "admin/products",//bu şekilde talep gelirse
                    defaults: new {controller="Admin",action="ProductList"}//admin product çalıştırılır
                );
                endpoints.MapControllerRoute(
                    name: "adminproductscreate", 
                    pattern: "admin/products/create",
                    defaults: new {controller="Admin",action="CreateProduct"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductedit", 
                    pattern: "admin/products/{id?}",//admin product sabit id değişken. ? yaptımki bulamazsam hata göndericem.id verilirse edite gider
                    defaults: new {controller="Admin",action="ProductEdit"}
                );     

                endpoints.MapControllerRoute(
                    name: "admincategories", 
                    pattern: "admin/categories",
                    defaults: new {controller="Admin",action="CategoryList"}
                );

                 endpoints.MapControllerRoute(
                    name: "admincategorycreate", 
                    pattern: "admin/categories/create",
                    defaults: new {controller="Admin",action="CategoryCreate"}
                );
               
                endpoints.MapControllerRoute(
                    name: "admincategoryedit", 
                    pattern: "admin/categories/{id?}",//dikkat navbarda hrefte categries burad abir trek var :D
                    defaults: new {controller="Admin",action="CategoryEdit"}
                );     


                // localhost/search    
                endpoints.MapControllerRoute(
                    name: "search", 
                    pattern: "search",
                    defaults: new {controller="Shop",action="search"}
                );

                endpoints.MapControllerRoute(
                    name: "productdetails", 
                    pattern: "{url}",
                    defaults: new {controller="Shop",action="details"}
                );

                endpoints.MapControllerRoute(
                    name: "products", 
                    pattern: "products/{category?}",
                    defaults: new {controller="Shop",action="list"}
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
