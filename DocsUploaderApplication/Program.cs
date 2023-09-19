using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "DefaultEndpointsProtocol=https;AccountName=docxuploaderstorage;AccountKey=msYl+hbqOZVH4JNHidWFcfGA8v3JtiW3aenMOEIVufcP2BM2y8as8QwUhfuu9asXgK3ErFnkv3Ky+AStPE5OSg==;EndpointSuffix=core.windows.net";

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(x => new BlobServiceClient(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
