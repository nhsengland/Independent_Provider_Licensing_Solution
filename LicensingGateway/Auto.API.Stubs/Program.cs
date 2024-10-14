var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/NotifySendMail", () => "Hello World!");

app.Run();
