
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TodoApi;

//var builder = WebApplication.CreateBuilder(args);

//// הוספת מדיניות CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.WithOrigins("https://minimalapi-jsdk.onrender.com")  // הוספת הכתובת שלך
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

//// הוספת Controllers ו-Swagger
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer(); // זו תוספת חשובה עבור Swagger
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "ToDo API",
//        Version = "v1",
//        Description = "API for managing tasks",
//    });
//});

//// הוספת DbContext
//builder.Services.AddDbContext<ToDoDbContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
//    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

//var app = builder.Build();

//// הפעלת CORS
//app.UseCors("AllowAll");

//// הפעלת Swagger ו-Swagger UI רק בסביבת פיתוח
//// if (app.Environment.IsDevelopment())
//// {
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
//        c.RoutePrefix = string.Empty;  // זה מאפשר גישה ל-Swagger UI מה-root (לא חובה)
//    });
//// }

//// מיפוי נתיבים עבור ה-API
//app.MapGet("/", () => "auth server is running now:)");

//app.MapGet("/api/tasks", async (ToDoDbContext dbContext) =>
//{
//    var tasks = await dbContext.Items.ToListAsync();
//    return Results.Ok(tasks);
//});

//app.MapPost("/api/tasks", async (ToDoDbContext dbContext, Item newTask) =>
//{
//    dbContext.Items.Add(newTask);
//    await dbContext.SaveChangesAsync();
//    return Results.Created($"/api/tasks/{newTask.Id}", newTask);
//});
//app.MapPut("/api/tasks/{id}", async (ToDoDbContext dbContext, int id, [FromBody]bool IsComplete) =>
//{

//    var existingItem = await dbContext.Items.FindAsync(id);

//    if (existingItem == null)
//    {
//        return Results.NotFound();
//    }

//    existingItem.IsComplete = IsComplete;
//    await dbContext.SaveChangesAsync();
//    return Results.Ok(existingItem);

//});
//// מחיקת משימה
//app.MapDelete("/api/tasks/{id}", async (ToDoDbContext dbContext, int id) =>
//{
//    var task = await dbContext.Items.FindAsync(id);
//    if (task is null) return Results.NotFound();

//    dbContext.Items.Remove(task);
//    await dbContext.SaveChangesAsync();
//    return Results.NoContent();
//});

//app.Run();
using Microsoft.EntityFrameworkCore;
using TodoApi;
var builder = WebApplication.CreateBuilder(args);

// הגדרת חיבור למסד נתונים
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));
// הגדרת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// הוסף את Swagger לפני יצירת ה-build של האפליקציה
builder.Services.AddSwaggerGen();  // הוספת Swagger
builder.Services.AddEndpointsApiExplorer(); // יוצר את ה-EndPoints עבור Swagger

var app = builder.Build();

// קביעת אם אנחנו בסביבת פיתוח
app.UseCors("AllowSpecificOrigin");

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();  // מפעיל את Swagger
app.UseSwaggerUI();  // מפעיל את UI של Swagger
// }


app.MapGet("/", () => "Hello World!");


app.MapGet("/api/items", async (ToDoDbContext dbContext) =>
{
    var items = await dbContext.Items.ToListAsync();
    return Results.Ok(items);

});

app.MapPost("/api/items", async (ToDoDbContext dbContext, Item newItem) =>
{
    dbContext.Items.Add(newItem);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/api/items/{newItem.Id}", newItem);
});

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

app.MapDelete("/api/items/{id}", async (ToDoDbContext dbContext, int id) =>
{

    var existingItem = await dbContext.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }
    dbContext.Items.Remove(existingItem);
    await dbContext.SaveChangesAsync();
    return Results.Ok(existingItem + " deleted");

});

app.Run();