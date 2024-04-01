using Microsoft.EntityFrameworkCore;
using RecruitmentTask;
using RecruitmentTask.Entities;
using RecruitmentTask.Middleware;
using RecruitmentTask.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<RecruitmentTaskDbContext>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddHostedService<TagDataInitialization>();

builder.Services.AddDbContext<RecruitmentTaskDbContext>
    (options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RecruitmentTaskDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
