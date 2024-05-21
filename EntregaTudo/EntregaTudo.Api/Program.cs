using EntregaTudo.Mongo.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Método para adicionar os repositórios mongoDB
builder.Services.AddMongo(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
