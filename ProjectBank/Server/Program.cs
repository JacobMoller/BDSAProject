
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters.RoleClaimType = "Roles";
});

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectBank.Server", Version = "v1" });
    c.UseInlineDefinitionsForEnums();
});

builder.Services.AddDbContext<ProjectBankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectBank")));
builder.Services.AddScoped<IProjectBankContext, ProjectBankContext>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.Seed();

app.Run();

