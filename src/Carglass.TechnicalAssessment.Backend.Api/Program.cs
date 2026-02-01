using Carglass.TechnicalAssessment.Backend.Api.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Host.UseSerilog((ctx, lc) =>
	lc.ReadFrom.Configuration(ctx.Configuration)
);

builder.Services.ConfigurationServices(builder.Configuration);
builder.Host.ConfigurationAutofac();
var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseHttpsRedirection()
   .UseAuthorization();

app.UseCustomEndpoints(builder.Configuration);
#endregion
app.Run();