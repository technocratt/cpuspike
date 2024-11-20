using cpuspike;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("cpuspikeservice"))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();
        
        metrics.AddConsoleExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();

        tracing.AddConsoleExporter();
    });

builder.Logging.AddOpenTelemetry(logging => logging.AddConsoleExporter());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    return "App is running!";
});

app.MapGet("/health", () =>
{
    return "Ok";
});

app.MapGet("/spike", () =>
{
    Task.Run(() => CPUStressTest.RunStressTest());
    return "Spike started!";
});

app.Run();