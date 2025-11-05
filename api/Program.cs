using api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddProjectOptions(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddApiLayer(builder.Configuration);

var app = builder.Build();

app.UseProjectPipeline();

await app.RunAsync();