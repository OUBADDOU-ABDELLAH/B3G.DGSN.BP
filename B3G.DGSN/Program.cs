using B3G.DGSN.REPOSITORY;
using B3G.DGSN.REPOSITORY.Interface;
using B3G.DGSN.SERVICE.ImplemetInterface;
using B3G.DGSN.SERVICE.INTERFACES;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataProtection();
builder.Services.AddControllers();
builder.Services.AddSession();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient(typeof(IGeneriqueCRUD<>), typeof(CRUDMethodes<>));
builder.Services.AddTransient<ISessionService, SessionServices>();
builder.Services.AddTransient<IDGSNFunc, DGSNFuncService>();




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DBContextDGSN>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConDBDGSN")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Configuration de la base de données Redis
});

// Ajouter le middleware de session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Définir la durée de validité de la session
    options.Cookie.HttpOnly = true; // Définir les propriétés du cookie de session
                                    // ...
});



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSession();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
