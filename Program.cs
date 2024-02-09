using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using gestor_de_limitres_krt.Services.Interfaces;
using gestor_de_limitres_krt.Services.Implementations;
using gestor_de_limitres_krt.NovaPasta;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
EnvConfig.Load(dotenv);

builder.Configuration["AWS:AccessKey"] = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
builder.Configuration["AWS:SecretKey"] = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");


var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new BasicAWSCredentials(builder.Configuration["AWS:AccessKey"], builder.Configuration["AWS:SecretKey"]);
builder.Services.AddDefaultAWSOptions(awsOptions);

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IRegistrosRepository, RegistrosRepository>();
builder.Services.AddScoped<IGestaoLimitesRepository, GestaoLimitesRepository>();
builder.Services.AddScoped<ITransacoesPixRepository, TransacoesPixRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
