using Azure;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using HomeBuddy.API.Configurations.ChatHub;
using HomeBuddy.API.Configurations.FirebaseCloudMessaging;
using HomeBuddy.API.Configurations.JWT;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Model;
using HomeBuddy.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignores cycles without adding $id or $ref
        options.JsonSerializerOptions.WriteIndented = true; // Optional: pretty-print JSON output for readability
    });

builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IServiceService,ServiceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddDbContext<PRM392_HomeBuddyContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var cloudMessagingConfigSection = builder.Configuration.GetSection("CloudMessaging");
var cloudMessagingConfig = cloudMessagingConfigSection.Get<CloudMessagingConfiguration>();

Console.WriteLine(cloudMessagingConfig.PrivateKey);
var jsonCredential = JsonConvert.SerializeObject(new
{
    type = cloudMessagingConfig.Type,
    project_id = cloudMessagingConfig.ProjectId,
    private_key_id = cloudMessagingConfig.PrivateKeyId,
    private_key = cloudMessagingConfig.PrivateKey,
    client_email = cloudMessagingConfig.ClientEmail,
    client_id = cloudMessagingConfig.ClientId,
    auth_uri = cloudMessagingConfig.AuthUri,
    token_uri = cloudMessagingConfig.TokenUri,
    auth_provider_x509_cert_url = cloudMessagingConfig.AuthProviderX509CertUrl,
    client_x509_cert_url = cloudMessagingConfig.ClientX509CertUrl,
    universe_domain = cloudMessagingConfig.UniverseDomain
});

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(jsonCredential)
});



builder.Services.AddSingleton(FirebaseMessaging.DefaultInstance);



builder.Services.Configure<FireBaseConfigurationModel>(builder.Configuration.GetSection("Firebase"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PRM392_HomeBuddyContext>();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");
app.Run();
