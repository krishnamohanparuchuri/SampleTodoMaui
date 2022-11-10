using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("sqliteConnection"));
});

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapGet("api/todos", async (TodoContext context) =>
{
    var items = await context.Todos.ToListAsync();
    return Results.Ok(items);
});

app.MapPost("api/todos", async (TodoContext context,Todo todo) =>
{
    await context.Todos.AddAsync(todo);
    await context.SaveChangesAsync();
    return Results.Created($"api/todos/{todo.Id}",todo);
});

app.MapPut("api/todos/{id}", async (TodoContext context,int id,Todo todo) =>
{
    var item = await context.Todos.Where(x => x.Id == id).SingleOrDefaultAsync();
    if(item != null)
    {
        item.TodoName = todo.TodoName;
        await context.SaveChangesAsync();

        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapDelete("api/todos/{id}", async (TodoContext context, int id) =>
{
    var item = await context.Todos.Where(x => x.Id == id).SingleOrDefaultAsync();
    if (item != null)
    {
        context.Todos.Remove(item);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();

