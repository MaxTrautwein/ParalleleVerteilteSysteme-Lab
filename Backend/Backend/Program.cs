using Backend;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Allow all as Required by the LAB
// Should not be used in Production  
builder.Services.AddCors(options =>
{
    options.AddPolicy("RequiredAllowAll", policy => policy.AllowAnyOrigin());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument();

// Connect a DB
builder.Services.AddDbContext<ItemsContext>(options => 
    options.UseNpgsql(Configuration.GetSecretOrEnvVar("ConnectionString")));

var app = builder.Build();

Configuration.CustomBinding(app);


// OpenAPI if in Develop
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.MapGet("/items", (ItemsContext db) => db.Items.ToList())
    .WithName("getItems");
app.MapPost("/items", (Item item, ItemsContext db) =>
{
    var existingItem = db.Items.FirstOrDefault(entry => entry.Name == item.Name);
    if (existingItem is not null)
    {
        existingItem.Quantity = item.Quantity;
        db.SaveChanges();
        return Results.Ok(existingItem);
    }
    db.Items.Add(item);
    db.SaveChanges();
    return Results.Created("",item);
    
}).AddEndpointFilter<ItemValidator>();
app.MapGet("/items/{itemId}", (string itemId, ItemsContext db) =>
{
    if (!int.TryParse(itemId, out var id))
    {
        return Results.BadRequest();
    }
    
    var item = db.Items.FirstOrDefault(entry => entry.Id == id);
    return item is null ? Results.NotFound() : Results.Ok(item);
});
app.MapPut("/items/{itemId}", (string itemId, Item item, ItemsContext db) =>
{
    if (!int.TryParse(itemId, out var id))
    {
        return Results.BadRequest();
    }
    var existingItem = db.Items.FirstOrDefault(entry => entry.Id == id);
    if (existingItem is null) return Results.NotFound();
    existingItem.Update(item);
    db.SaveChanges();
    return Results.Ok(existingItem);
        
}).AddEndpointFilter<ItemValidator>();
app.MapDelete("/items/{itemId}", (string itemId, ItemsContext db) =>
{
    if (!int.TryParse(itemId, out var id))
    {
        return Results.BadRequest();
    }
    
    var existingItem = db.Items.FirstOrDefault(entry => entry.Id == id);
    if (existingItem is null) return Results.NotFound();
    
    var removed = db.Items.Remove(existingItem); 
    db.SaveChanges();
    return Results.Ok(removed.Entity);
});

app.Run();
