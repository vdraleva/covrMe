using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Domain.Repos.Implementations;
using CovrMe.WebAPI.IdentityHelpers;
using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Mutations;
using CovrMe.WebAPI.Queries;
using CovrMe.WebAPI.Seeding;
using CovrMe.WebAPI.Services.Caching.Contracts;
using CovrMe.WebAPI.Services.Caching.Implementations;
using CovrMe.WebAPI.Services.Dsk.Contracts;
using CovrMe.WebAPI.Services.Dsk.Implementations;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Implementations;
using CovrMe.WebAPI.Services.Messaging;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Services.Sirma.Implementations;
using CovrMe.WebAPI.Services.Speedy.Contracts;
using CovrMe.WebAPI.Services.Speedy.Implementations;
using CovrMe.WebAPI.Services.Uniqa.Contracts;
using CovrMe.WebAPI.Services.Uniqa.Implementations;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Mapping;
using CovrMe.WebAPI.Subscriptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(IdentityHelper.GetIdentityOptions)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

//Repo
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddMemoryCache();

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration[GlobalConstants.JwtIssuer],
        ValidAudience = builder.Configuration[GlobalConstants.JwtAudience],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[GlobalConstants.JwtKey]))
    };
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration[GlobalConstants.GoogleClientId];
    options.ClientSecret = builder.Configuration[GlobalConstants.GoogleClientSecret];
    options.SaveTokens = true;
    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
    {
        OnTicketReceived = async context =>
        {
            await Console.Out.WriteLineAsync();
        }
    };
}).AddFacebook(options =>
{
    options.AppId = builder.Configuration[GlobalConstants.FacebookAppId];
    options.AppSecret = builder.Configuration[GlobalConstants.FacebookAppSecret];
    //options.Scope.Add("email");
    //options.Scope.Add("public_profile");
    //options.Fields.Add("id");
    //options.Fields.Add("name");
    //options.Fields.Add("email");
}).Services.AddAuthorization();

//Services
builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<IVehicleService, VehicleService>();
builder.Services.AddTransient<ISirmaAuthenticationService, SirmaAuthenticationService>();
builder.Services.AddTransient<IInsuranceCompanyService, InsuranceCompanyService>();
builder.Services.AddTransient<IInsuranceService, InsuranceService>();
builder.Services.AddTransient<ISirmaCivilInsuranceService, SirmaCivilInsuranceService>();
builder.Services.AddTransient<IVehicleService, VehicleService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ISettingsService, SettingsService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IDskPaymentService, DskPaymentService>();
builder.Services.AddTransient<ISpeedyShipmentService, SpeedyShipmentService>();
builder.Services.AddTransient<IDeliveryService, DeliveryService>();
builder.Services.AddTransient<ICurrencyService, CurrencyService>();
builder.Services.AddTransient<IUniqaInsuranceService, UniqaInsuranceService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IReadWriteService, ReadWriteService>();
builder.Services.AddTransient<IErrorService, ErrorService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IFtpService, FtpService>();
builder.Services.AddTransient<ISirmaLocationService, SirmaLocationService>();
builder.Services.AddTransient<ISirmaVehicleService, SirmaVehicleService>();


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<SirmaAuthenticationSettings>(builder.Configuration.GetSection("SirmaAuthSettings"));
builder.Services.Configure<DskAuthenticationSettings>(builder.Configuration.GetSection("DskAuthSettings"));
builder.Services.Configure<SpeedyAuthenticationSettings>(builder.Configuration.GetSection("SpeedyAuthSettings"));

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<BaseQuery>()
        .AddTypeExtension<UserQuery>()
        .AddTypeExtension<VehicleQuery>()
        .AddTypeExtension<DocumentsQuery>()
        .AddTypeExtension<InsuranceQuery>()
        .AddTypeExtension<QuestionQuery>()
    .AddMutationType<BaseMutation>()
        .AddTypeExtension<UserMutation>()
        .AddTypeExtension<InsuranceMutation>()
        .AddTypeExtension<DeliveryMutations>()
        .AddTypeExtension<PaymentMutation>()
        .AddTypeExtension<VehicleMutations>()
        .AddTypeExtension<DocumentsMutation>()
    .AddSubscriptionType<BaseSubscription>()
        .AddTypeExtension<PaymentSubscription>()
    .AddFiltering()
    .AddMutationConventions()
    .AddSorting()
    .AddInMemorySubscriptions()
.AddTypeConverter<DateTime, DateTimeOffset>(x =>
    new DateTimeOffset(DateTime.SpecifyKind(x, DateTimeKind.Utc), TimeSpan.Zero))
.AddTypeConverter<DateTimeOffset, DateTime>(x =>
    x.UtcDateTime);


//builder.Services.AddCors();

builder.Services.AddCors(options => options.AddPolicy("EnableCORS", build =>
{
    build
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
}));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Seed data on application startup
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();
app.UseRouting();

app.UseWebSockets();

app.UseCors("EnableCORS");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.MapBananaCakePop();

app.MapControllers();

app.Run();
