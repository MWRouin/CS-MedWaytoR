using MWR.MedWaytoR.DI;
using MWR.MedWaytoR.RequestResponse;
using WebApiMedWaytor.Application.Behaviour;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<InMemoryDbContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped(typeof(IRequestResponsePipe<,>), typeof(CommandBehaviourPipe<,>));

builder.Services.AddMedWayToR_PubSub(ServiceLifetime.Scoped);

builder.Services.AddMedWayToR_RequestResponse(ServiceLifetime.Scoped);

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