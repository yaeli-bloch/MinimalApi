// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()   
//               .AllowAnyMethod()  
//               .AllowAnyHeader(); 
//     });
// });
// builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
//     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));
// var app = builder.Build();
// app.UseCors("AllowAll");

//    app.UseSwagger();
//    app.UseSwaggerUI();


// app.MapGet("/", () => "auth server is running now:)");
// app.MapGet("/api/tasks", async (ToDoDbContext dbContext) =>
// {
//     var tasks = await dbContext.Items.ToListAsync();
//     return Results.Ok(tasks);
// });
// app.MapPost("/api/tasks", async (ToDoDbContext dbContext, Item newTask) =>
// {
//     dbContext.Items.Add(newTask);
//     await dbContext.SaveChangesAsync();
//     return Results.Created($"/api/tasks/{newTask.Id}", newTask);
// });

// // 3. עדכון משימה
// app.MapPut("/api/items/{id}", async (ToDoDbContext dbContext, int id, bool IsComplete) =>
// {

//     var existingItem = await dbContext.Items.FindAsync(id);

//     if (existingItem == null)
//     {
//         return Results.NotFound();
//     }

//     existingItem.IsComplete = IsComplete;
//     await dbContext.SaveChangesAsync();

//     return Results.Ok(existingItem);

// });


// // 4. מחיקת משימה
// app.MapDelete("/api/tasks/{id}", async (ToDoDbContext dbContext, int id) =>
// {
//     var task = await dbContext.Items.FindAsync(id);
//     if (task is null) return Results.NotFound();

//     dbContext.Items.Remove(task);
//     await dbContext.SaveChangesAsync();
//     return Results.NoContent();
// });
// app.Run();
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// הוספת מדיניות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // זו תוספת חשובה עבור Swagger
builder.Services.AddSwaggerGen();

// הוספת Swagger generation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ToDo API",
        Version = "v1",
        Description = "API for managing tasks",
    });
});

// הוספת DbContext
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

var app = builder.Build();

// הפעלת CORS
app.UseCors("AllowAll");

// הפעלת Swagger ו-Swagger UI רק בסביבת פיתוח
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
        c.RoutePrefix = string.Empty;  // זה מאפשר גישה ל-Swagger UI מה-root (לא חובה)
    });
// }

// מיפוי נתיבים עבור ה-API
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

// עדכון משימה
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

// מחיקת משימה
app.MapDelete("/api/tasks/{id}", async (ToDoDbContext dbContext, int id) =>
{
    var task = await dbContext.Items.FindAsync(id);
    if (task is null) return Results.NotFound();

    dbContext.Items.Remove(task);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
