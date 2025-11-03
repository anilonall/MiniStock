using MiniStok.Api.Services; 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 
   builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); 
  app.UseSwaggerUI(); 
}
app.UseCors("AllowReactApp");

app.UseAuthorization();
app.UseHttpsRedirection(); 
app.UseAuthorization(); 
app.MapControllers(); 

app.Run(); 