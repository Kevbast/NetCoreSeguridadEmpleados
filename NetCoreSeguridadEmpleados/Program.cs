using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Repositories;

var builder = WebApplication.CreateBuilder(args);
/*
 Dentro de nuestro Program debemos habilitar la autorización mediante
Políticas de Roles, indicando los Roles que deseamos incluir en las políticas. 
 */
builder.Services.AddAuthorization(options =>
{
    //DEBEMOS CREAR LAS POLICIES QUE NECESITEMOS PARA LOS ROLES
    options.AddPolicy("SOLOJEFES", policy => policy.RequireRole("PRESIDENTE", "DIRECTOR", "ANALISTA"));
});//AHORA LO APLICAREMOS DENTRO DE COMPIES,EN EL CONTROLLER DE EMPLEADOS


//estos 2 los utilizaremos para implementar temdata,a la hora de que nos registremos
//nos rediriga donde queríamos ir principalmente
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme= CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme= CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        //Se le indica el controller y el action
        config.AccessDeniedPath = "/Managed/ErrorAcceso";
    });//CAMBIAMOS ESTO para indicar dode redirigir para el error



// Add services to the container.//lo editamos
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider();//lo implementamos para usarlo junto a session y
                                       //tempdata para que añada un proveedor de rutas 
                                       //Para el TempData  para el attribute(filter)

string connectionstring = builder.Configuration.GetConnectionString("SqlHospital");
builder.Services.AddTransient<RepositoryHospital>();
builder.Services.AddDbContext<HospitalContext>
    (options => options.UseSqlServer(connectionstring));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseRouting();
//IMPLEMENTAMOS ESTAS ANTES DE AUTHORIZATIONS
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();

app.UseSession();//ImplementarUsession siempre que añadamos builder.Services.AddSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
