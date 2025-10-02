using Scalar.AspNetCore;
using TravelBooking.API.Extensions;
using TravelBooking.Application.Extensions;
using TravelBooking.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddGlobalExceptionMiddleware();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseGlobalExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
