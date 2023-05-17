using MongoDB.Driver;
using Repositories.interfaces;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoConnectionString = "mongodb://text-snippets:zQoltQVTpWFZaxg7iSBSSZsRjn4UI834miazWU7Ue8DAKGcoMwdVzGTW27uM9TWjmfz7HHaFamQJACDbWr2gVw%3D%3D@text-snippets.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@text-snippets@";
var mongoDatabaseName = "snippets";
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
builder.Services.AddScoped<ISnippetRepository, MongoDbSnippetRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "";
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
