using MWR.MedWaytoR.Config;
using MWR.MedWaytoR.DI;
using MWR.MedWaytoR.RequestResponse;
using WebApiMedWaytoR.Application.Behaviour;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<InMemoryDbContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//builder.Services.AddScoped(typeof(IRequestResponsePipe<,>), typeof(CommandBehaviourPipe<,>));

builder.Services.AddMedWayToR_PubSub();

builder.Services.AddMedWayToR_RequestResponse();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();