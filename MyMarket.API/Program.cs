using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyMarket.Application.Abstractions;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Application.Features.Products.Queries;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Application.Features.Users.Queries;
using MyMarket.Application.Validators;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;
using MyMarket.Infrastructure.Auth;
using MyMarket.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================================ //
// Builder = How to create the necessary artifacts  //
// ================================================ //
// Register of AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Add and configure the authentication JWT
// builder.Services
//     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//         };
//     });

// Add authorization supports
builder.Services.AddAuthorization();

var connectionString = "Data Source=MyMarket.db";

builder.Services.AddDbContext<MyMarketDbContext>(options => 
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Query product handlers
builder.Services.AddScoped<GetAllProductsQueryHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<GetProductByCategoryQueryHandler>();
//Query user handlers
builder.Services.AddScoped<GetAllUsersQueryHandler>();

// Update Product handlers
builder.Services.AddScoped<CreateProductCommandHandler>();
builder.Services.AddScoped<UpdateProductNameCommandHandler>();
builder.Services.AddScoped<UpdateProductDescriptionCommandHandler>();
builder.Services.AddScoped<UpdateProductSkuCommandHandler>();
builder.Services.AddScoped<UpdateProductPriceCommandHandler>();
builder.Services.AddScoped<UpdateProductCategoryCommandHandler>();
builder.Services.AddScoped<UpdateProductStockCommandHandler>();
// Update Product handlers
builder.Services.AddScoped<ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>>, CreateUserCommandHandler>();

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

// Pode colocar no Program.cs ou em um arquivo separado

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// ===================================================
// ENDPOINTS - Minimal API
// ===================================================
// PRODUCTS
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
// USER
app.MapPost("/api/users", async (CreateUserCommand command, ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>> handler, IValidator<CreateUserCommand> validator) => 
{
    var validationResult = await validator.ValidateAsync(command);
    
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    var userId = await handler.HandleAsync(command);
    
    return Results.Created($"/api/users/{userId}", new { Id = userId });
});

app.MapGet("/api/users", async (GetAllUsersQueryHandler handler) =>
{
    var query = new GetAllUsersQuery();
    var result = await handler.HandleAsync(query);
    return Results.Ok(result);
});

app.Run();