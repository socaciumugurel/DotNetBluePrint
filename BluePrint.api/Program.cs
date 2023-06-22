using BluePrint.api.Infrastructure;
using BluePrint.core.Infrastructure;
using BluePrint.core.Infrastructure.RegistrationStrategies;
using BluePrint.shared.services.Translations;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var azureConnectionString = builder.Configuration.GetSection("ConnectionStrings:AzureBlob").Value;

var assemblies = AssemblyLoader.GetReferencingAssemblies(nameof(BluePrint));
builder.Services.Execute(() => ServicesRegistrationStrategy.Create(assemblies));

builder.Services.Execute(() => AzureBlobRegistrationStrategy.Create(azureConnectionString));
builder.Services.AddLocalization(l => { l.ResourcesPath = "Resources"; });

var app = builder.Build();
app.Execute(ResourceInitializationStrategy.Create(app));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
