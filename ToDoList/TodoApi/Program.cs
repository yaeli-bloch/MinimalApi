using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));
var app = builder.Build();
app.UseCors("AllowAll");

   app.UseSwagger();
   app.UseSwaggerUI();


app.MapGet("/", () => "auth server is running now:)");
app.MapGet("/api/tasks", async (ToDoDbContext dbContext) =>
{
    var tasks = await dbContext.Items.ToListAsync();
    return Results.Ok(tasks);
});
app.MapPost("/api/tasks", async (ToDoDbContext dbContext, Item newTask) =>
{
    dbContext.Items.Add(newTask);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/api/tasks/{newTask.Id}", newTask);
});

// 3. עדכון משימה
app.MapPut("/api/items/{id}", async (ToDoDbContext dbContext, int id, bool IsComplete) =>
{

    var existingItem = await dbContext.Items.FindAsync(id);

    if (existingItem == null)
    {
        return Results.NotFound();
    }

    existingItem.IsComplete = IsComplete;
    await dbContext.SaveChangesAsync();

    return Results.Ok(existingItem);

});


// 4. מחיקת משימה
app.MapDelete("/api/tasks/{id}", async (ToDoDbContext dbContext, int id) =>
{
    var task = await dbContext.Items.FindAsync(id);
    if (task is null) return Results.NotFound();

    dbContext.Items.Remove(task);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();
