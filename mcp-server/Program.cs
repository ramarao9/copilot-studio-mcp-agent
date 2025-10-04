using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RRK.Interfaces;
using RRK.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

Console.WriteLine("MCP Tool Metadata Enabled");
builder.EnableMcpToolMetadata();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddSingleton<IProductService, ProductService>()
    .AddSingleton<IProductReviewService, ProductReviewService>();
Console.WriteLine("Starting MCP Server...");
builder.Build().Run();
