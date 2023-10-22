using BlazorApp1.Server.Data;
using BlazorApp1.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
	.AddIdentityServerJwt()
	.AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = "797394826698-vc315f8puhi1lb7l0iovglrkjahfh5h6.apps.googleusercontent.com";
				googleOptions.ClientSecret = "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";
			})
	.AddOpenIdConnect("Microsoft", "Login with Microsoft", options =>
	 {
		 options.ClientId = "6ccb26cd-4ca6-459c-a14b-3ba16c89d111";
		 options.ClientSecret = "aYj8Q~jrLubBT7wN3ewIV3R8OVPb2~-P8KGcGbK3";
		 options.Authority = "https://login.microsoftonline.com/b767a72e-40b6-476d-b144-248da07c71b4";
		 options.CallbackPath = "/";
		 options.ResponseType = "code";
		 options.SaveTokens = true;
		 options.Scope.Add("openid");
		 options.Scope.Add("profile");
	 });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
