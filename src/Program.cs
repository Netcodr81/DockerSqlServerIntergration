using DockerSqlServerIntegration.Api.Data;
using DockerSqlServerIntegration.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/todos", async (IDbContextFactory<AppDbContext> context) =>
{
    using var dbContext = context.CreateDbContext();
    return await dbContext.ToDos.ToListAsync();
})
.WithName("GetAllToDos")
.WithOpenApi();

app.MapPost("/todos/add-todo", async (IDbContextFactory<AppDbContext> context, ToDo toDo) =>
{
    try
    {
        using var dbContext = context.CreateDbContext();
        dbContext.ToDos.Add(toDo);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPut("/todos/update-todo/{id}", async (IDbContextFactory<AppDbContext> context, int id, ToDo toDo) =>
{
    try
    {
        using var dbContext = context.CreateDbContext();
        var existingToDo = await dbContext.ToDos.FindAsync(id);
        if (existingToDo == null)
        {
            return Results.NotFound();
        }
        existingToDo.Title = toDo.Title;
        existingToDo.Description = toDo.Description;
        existingToDo.IsCompleted = toDo.IsCompleted;
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/todos/delete-todo/{id}", async (IDbContextFactory<AppDbContext> context, int id) =>
{
    try
    {
        using var dbContext = context.CreateDbContext();
        var existingToDo = await dbContext.ToDos.FindAsync(id);
        if (existingToDo == null)
        {
            return Results.NotFound();
        }
        dbContext.ToDos.Remove(existingToDo);
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();