using System.ComponentModel.DataAnnotations;
using Backend;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// In memory for now
List<Item> items =
[
    new Item( "banana", 1),
    new Item( "apple", 2),
    new Item("orange", 3)
];

app.MapGet("/items", () => items)
    .WithName("getItems");
app.MapPost("/items", (Item item) =>
{
    var existingItem = items.FirstOrDefault(entry => entry.Name == item.Name);
    if (existingItem is not null)
    {
        existingItem.Quantity = item.Quantity;
        return Results.Ok(existingItem);
    }
    items.Add(item);
    return Results.Created("",item);
    
}).AddEndpointFilter<ItemValidator>();
app.MapGet("/items/{itemId}", (string itemId) =>
{
    if (!int.TryParse(itemId, out var id))
    {
        return Results.BadRequest();
    }
    
    var item = items.FirstOrDefault(entry => entry.Id == id);
    return item is null ? Results.NotFound() : Results.Ok(item);
});
app.MapPut("/items/{itemId}", (string itemId, Item item) =>
{
    if (!int.TryParse(itemId, out var id))
    {
        return Results.BadRequest();
    }
    var existingItem = items.FirstOrDefault(entry => entry.Id == id);
    if (existingItem is null) return Results.NotFound();
    existingItem.Update(item);
    return Results.Ok(existingItem);
        
}).AddEndpointFilter<ItemValidator>();
app.MapDelete("/items/{itemId}", (string itemId) =>
{
    if (!int.TryParse(itemId, out var id))
    {
        return Results.BadRequest();
    }

    var removed = items.RemoveAll(item => item.Id == id);
    return removed > 0 ? Results.Ok(removed) : Results.NotFound();
});


app.Run();

