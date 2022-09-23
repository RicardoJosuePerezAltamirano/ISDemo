using Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityServerSettings>(builder.Configuration.GetSection("IdentityServerSettings"));
builder.Services.AddSingleton<ITokenService, TokenService>();

#region paso 4
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";

}).AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
    options.Scope.Add(builder.Configuration["InteractiveServiceSettings:Scopes:0"]);
    options.ResponseType = "code";// este matchea con gran_type para usarse en el identity server
    options.UsePkce = true;// proff key of code exchange 
    options.ResponseMode = "query";// responde en query string
    options.SaveTokens = true;// guardara el token el la cookie
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// se usa el cliente Client Credentials

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); // <-- Add it here.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
