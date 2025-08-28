using ABCRetail.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Get connection strings
var cs = builder.Configuration.GetConnectionString("StorageAccount")!;
var azureCs = builder.Configuration.GetConnectionString("AzureStorage");

// Read Azure Storage settings (make sure they exist in appsettings.json)
var cfg = builder.Configuration.GetSection("AzureStorage");

// Register services
builder.Services.AddSingleton(new TableStorageService(azureCs ?? cs));
builder.Services.AddSingleton(new BlobStorageService(cs, cfg.GetValue<string>("BlobContainer") ?? "customer-images"));
builder.Services.AddSingleton(new QueueService(cs, cfg.GetValue<string>("QueueName") ?? "customer-queue"));
builder.Services.AddSingleton(new FileShareService(cs, cfg.GetValue<string>("FileShare") ?? "contracts"));

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
