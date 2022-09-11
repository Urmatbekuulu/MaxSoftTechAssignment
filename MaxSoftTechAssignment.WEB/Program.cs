using MaxSoftTechAssignment.DAL;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using MaxSoftTechAssignment.WEB;
using MaxSoftTechAssignment.WEB.Configurations;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

//Data access layer services added
builder.Services.AddDALServices(builder.Configuration);
builder.Services.AddJwtIdentity(builder.Configuration);


builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(options =>
{
    options
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await using var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    using var userManager = services.GetRequiredService<UserManager<User>>();
    using var roleManager = services.GetRequiredService<RoleManager<Role>>();
    await DataSeeding.SeedDataAsync(userManager, roleManager,context,builder.Configuration);
    
}


await app.RunAsync();