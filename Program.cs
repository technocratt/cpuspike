using cpuspike;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/process", () =>
{
    Task.Run(() => CPUStressTest.RunStressTest());
    return "Ok";
});

app.Run();