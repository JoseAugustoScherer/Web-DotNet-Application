using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyMarket.Application.Abstractions;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Application.Features.Products.Queries;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Application.Features.Users.Queries;
using MyMarket.Application.Services;
using MyMarket.Application.Validators;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;
using MyMarket.Infrastructure.Auth;
using MyMarket.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Adicionar a Definição de Segurança (Security Definition)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    // 2. Adicionar o Requisito de Segurança (Security Requirement)
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// ================================================ //
// Builder = How to create the necessary artifacts  //
// ================================================ //
// Register of AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add authorization supports
builder.Services.AddAuthorization();

var connectionString = "Data Source=MyMarket.db";

builder.Services.AddDbContext<MyMarketDbContext>(options => 
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//Query product handlers
builder.Services.AddScoped<GetAllProductsQueryHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<GetProductByCategoryQueryHandler>();
//Query user handlers
builder.Services.AddScoped<GetAllUsersQueryHandler>();

// Product handlers
builder.Services.AddScoped<CreateProductCommandHandler>();
builder.Services.AddScoped<UpdateProductNameCommandHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler>();
builder.Services.AddScoped<UpdateProductDescriptionCommandHandler>();
builder.Services.AddScoped<UpdateProductSkuCommandHandler>();
builder.Services.AddScoped<UpdateProductPriceCommandHandler>();
builder.Services.AddScoped<UpdateProductCategoryCommandHandler>();
builder.Services.AddScoped<UpdateProductStockCommandHandler>();

// User handlers
builder.Services.AddScoped<ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>>, CreateUserCommandHandler>();
builder.Services.AddScoped<LoginUserCommandHandler>();

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

// Cors Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Pode colocar no Program.cs ou em um arquivo separado

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Cors 
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

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
})
.RequireAuthorization();

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
// Delete
app.MapDelete("/api/products/{id}/delete", async (Guid id, DeleteProductCommandHandler handler) =>
{
    var command = new DeleteProductCommand(id);
    var result = await handler.HandleAsync(command);
   
    if(result.IsSuccess)
        return Results.NoContent();
    
    return Results.StatusCode(result.StatusCode);
})
.RequireAuthorization();

// Path Updates
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

app.MapPost("/api/users/login", async (LoginUserCommand command, LoginUserCommandHandler handler) =>
{
    var result = await handler.HandleAsync(command);

    if (result.IsSuccess)
    {
        return Results.Ok(result);
    }

    return Results.StatusCode(result.StatusCode);
});

app.MapGet("/api/users", async (GetAllUsersQueryHandler handler) =>
{
    var query = new GetAllUsersQuery();
    var result = await handler.HandleAsync(query);
    return Results.Ok(result);
});

app.Run();