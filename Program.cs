using cpuspike;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    return "App is running!";
});

app.MapGet("/spike", () =>
{
    Task.Run(() => CPUStressTest.RunStressTest());
    return "Spike started!";
});

app.Run();