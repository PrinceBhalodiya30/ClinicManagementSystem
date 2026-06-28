using ClinicManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Register HttpClient, IHttpContextAccessor, and AuthService
builder.Services.AddHttpClient("CMSApi", client =>
{
    client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();

// ✅ Enable Session for JWT storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Enable Session Middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}"); // Default route changed to Login

app.Run();
