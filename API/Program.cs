using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region paso 3
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7173";
        options.Audience = "weatherapi";
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        /*
         * This specification registers the "application/at+jwt" media type, which can be used to indicate that the content is a JWT access token. JWT access tokens MUST include this media type in the "typ" header parameter to explicitly declare that the JWT represents an access token complying with this profile. Per the definition of "typ" in Section 4.1.9 of [RFC7515], it is RECOMMENDED that the "application/" prefix be omitted. Therefore, the "typ" value used SHOULD be "at+jwt". See the Security Considerations section for details on the importance of preventing OpenID Connect ID Tokens (as defined by Section 2 of [OpenID.Core]) from being accepted as access tokens by resource servers implementing this profile.
         * */
    });
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region paso 3
app.UseAuthentication();
#endregion
app.UseAuthorization();

app.MapControllers();

app.Run();
