using Microsoft.EntityFrameworkCore;
using Todo_App.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var razorBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
    razorBuilder.AddRazorRuntimeCompilation();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoContext>();
        context.Database.Migrate();

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
 
    app.UseHsts();
}
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.UseAuthorization();

app.Run();
