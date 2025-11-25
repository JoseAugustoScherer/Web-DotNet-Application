using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Application.Features.Products.Queries;
using MyMarket.Application.Validators;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;
using MyMarket.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================================ //
// Builder = How to create the necessary artifacts  //
// ================================================ //
var connectionString = "Data Source=MyMarket.db";

builder.Services.AddDbContext<MyMarketDbContext>(options => 
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<CreateProductCommandHandler>();

//Query handlers
builder.Services.AddScoped<GetAllProductsQueryHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<GetProductByCategoryQueryHandler>();

// Update handlers
builder.Services.AddScoped<UpdateProductNameCommandHandler>();
builder.Services.AddScoped<UpdateProductDescriptionCommandHandler>();
builder.Services.AddScoped<UpdateProductSkuCommandHandler>();
builder.Services.AddScoped<UpdateProductPriceCommandHandler>();
builder.Services.AddScoped<UpdateProductCategoryCommandHandler>();
builder.Services.AddScoped<UpdateProductStockCommandHandler>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

// Pode colocar no Program.cs ou em um arquivo separado

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ===================================================
// ENDPOINTS - Minimal API
// ===================================================
app.MapGet("/api/products", async (GetAllProductsQueryHandler handler) =>
{
    var query = new GetAllProductsQuery();
    var result = await handler.HandleAsync(query);
    return Results.Ok(result);
});

app.MapPost("/api/products", async (CreateProductCommand command, CreateProductCommandHandler handler, IValidator<CreateProductCommand> validator) => 
{
    var validationResult = await validator.ValidateAsync(command);
    
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    var productId = await handler.HandleAsync(command);
    return Results.Created($"/api/products/{productId}", new { Id = productId });
});
// By ID
app.MapGet("/api/products/{id}", async (Guid id, GetProductByIdQueryHandler handler) =>
{
    var query = new GetProductByIdQuery(id);
    
    var result = await handler.HandleAsync(query);
    
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});
// By Category
app.MapGet("/api/products/category/{category}", async (Category category, GetProductByCategoryQueryHandler handler) =>
{
    var query = new GetProductByCategoryQuery(category);
    
    var product = await handler.HandleAsync(query);
    
    return Results.Ok(product);
});

app.MapPatch("/api/products/{id}/name", async (Guid id, UpdateProductInputModel input, UpdateProductNameCommandHandler handler) => 
{
    var command = new UpdateProductNameCommand(id, input.Name);

    try
    {
        await handler.HandleAsync(command);
        return Results.NoContent();
    }
    catch (KeyNotFoundException e)
    {
        return Results.NotFound(e.Message);
    }
});

app.MapPatch("/api/products/{id}/description", async (Guid id, UpdateProductInputModel input, UpdateProductDescriptionCommandHandler handler) => 
{
    var command = new UpdateProductDescriptionCommand(id, input.Description);

    try
    {
        await handler.HandleAsync(command);
        return Results.NoContent();
    }
    catch (KeyNotFoundException e)
    {
        return Results.NotFound(e.Message);
    }
});

app.MapPatch("/api/products/{id}/sku", async (Guid id, UpdateProductInputModel input, UpdateProductSkuCommandHandler handler) => 
{
    var command = new UpdateProductSkuCommand(id, input.Sku);

    try
    {
        await handler.HandleAsync(command);
        return Results.NoContent();
    }
    catch (KeyNotFoundException e)
    {
        return Results.NotFound(e.Message);
    }
});

app.MapPatch("/api/products/{id}/price", async (Guid id, UpdateProductInputModel input, UpdateProductPriceCommandHandler handler) => 
{
    var command = new UpdateProductPriceCommand(id, input.Price);

    try
    {
        await handler.HandleAsync(command);
        return Results.NoContent();
    }
    catch (KeyNotFoundException e)
    {
        return Results.NotFound(e.Message);
    }
});

app.MapPatch("/api/products/{id}/category", async (Guid id, UpdateProductInputModel input, UpdateProductCategoryCommandHandler handler) => 
{
    var command = new UpdateProductCategoryCommand(id, input.Category);

    try
    {
        await handler.HandleAsync(command);
        return Results.NoContent();
    }
    catch (KeyNotFoundException e)
    {
        return Results.NotFound(e.Message);
    }
});

app.MapPatch("/api/products/{id}/stock", async (Guid id, UpdateProductInputModel input, UpdateProductStockCommandHandler handler) => 
{
    var command = new UpdateProductStockCommand(id, input.Stock);

    try
    {
        await handler.HandleAsync(command);
        return Results.NoContent();
    }
    catch (KeyNotFoundException e)
    {
        return Results.NotFound(e.Message);
    }
});
// app.MapDelete("/api/products/{id}", async (Guid, id, DeleteProductCommand command, DeleteProductCommandHandler handler) =>
// {
//     
// });

app.Run();