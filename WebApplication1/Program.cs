using Microsoft.EntityFrameworkCore;
using WebApplication1.Repository;

var builder = WebApplication.CreateBuilder(args);

// 🔥 THIS LINE MUST EXIST 🔥
builder.Services.AddDbContext<IDSDatabaseDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("IDSConnection"))
);

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
