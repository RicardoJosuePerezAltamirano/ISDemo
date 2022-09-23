using AuthenticationsProvider;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

var builder = WebApplication.CreateBuilder(args);
#region paso 5
builder.Services.AddRazorPages();
#endregion

#region paso 1
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.EmitStaticAudienceClaim = true;//// check
})
//    .AddTestUsers( new List<TestUser>())
//.AddInMemoryClients(new List<Client>())
//.AddInMemoryApiResources(new List<ApiResource>())
//.AddInMemoryApiScopes(new List<ApiScope>())
//.AddInMemoryIdentityResources(new List<IdentityResource>());// empezamos con inmemory

#endregion

#region paso 2
        .AddTestUsers(TestUsers.Users)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources);
#endregion


builder.Services.AddAuthentication();

var app = builder.Build();
#region paso 1
app.UseIdentityServer();
#endregion

//app.MapGet("/", () => "Hello World!");

#region paso 5
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

#endregion

app.Run();
