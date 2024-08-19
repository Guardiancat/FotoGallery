using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PhotoGallery.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

// ��������� ����������� � ���� ������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ��������� Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    // ������ ��������� Identity
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ���������� ������������ � ���������������
builder.Services.AddControllersWithViews();

// ��������� ��������������
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie(options =>
//{
//    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // 
//});
// .AddGoogle(options =>
// {
//     IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
//     options.ClientId = googleAuthNSection["ClientId"];
//     options.ClientSecret = googleAuthNSection["ClientSecret"];
// });

// ���������� CORS-��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ��������� ������ ������

string accountKey = "SGVsbG9Xb3JsZCEyMzQ1Njc4OQ==";
string accountName = "Uknow";
var storageAccount = new CloudStorageAccount(
    new StorageCredentials(accountName, accountKey), true);
var blobClient = storageAccount.CreateCloudBlobClient();
var container = blobClient.GetContainerReference("<container-name>");

builder.Services.AddDataProtection()
    .PersistKeysToAzureBlobStorage(container, "<blob-name>")
    .ProtectKeysWithAzureKeyVault("<Key Vault URI>", "<client-id>", "<client-secret>");
var app = builder.Build();

// ��������� ��������� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ������������� CORS
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
