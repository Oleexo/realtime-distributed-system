var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Get all messages
app.MapGet("/message/:channel", () => "Hello World!");
// Get one message
app.MapGet("/message/:channel/:id", () => "Hello world");
// Post a message
app.MapPost("/message/:channel", () => "Hello world");
// Edit message
app.MapPut("/message/:channel/:messageId", () => "Hello world");
// Delete message
app.MapDelete("/message/:channel/:messageId", () => "Hello world");
// Get all message id for a user
app.MapGet("/message/lastMessageId", () => "Hello world");
app.Run();