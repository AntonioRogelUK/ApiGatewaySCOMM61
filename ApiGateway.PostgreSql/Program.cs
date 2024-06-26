using ApiGateway.PostgreSql.Data;
using ApiGateway.PostgreSql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add connection
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<SubscriptionsDB>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//code
app.MapPost("/Subscriptions/", async (Subscriptions e, SubscriptionsDB db) =>
{
    db.Subscriptions.Add(e);
    await db.SaveChangesAsync();
    return Results.Created($"/Subscritions/{e.SubscriptionId}", e);

});

app.MapGet("/Subscriptions/{SubscriptionId:guid}", async (Guid SubscriptionId, SubscriptionsDB db) =>
{
    var subscription = await db.Subscriptions.FindAsync(SubscriptionId);
    return subscription is Subscriptions e
        ? Results.Ok(e)
        : Results.NotFound();
});

app.MapPut("/Subscriptions/{SubscriptionId:guid}", async (Guid SubscriptionId, Subscriptions updatedSubscription, SubscriptionsDB db) =>
{
    var subscription = await db.Subscriptions.FindAsync(SubscriptionId);
    if (subscription is null)
    {
        return Results.NotFound();
    }

    // Actualizar los campos de la suscripción existente con los datos proporcionados en updatedSubscription
    subscription.UserId = updatedSubscription.UserId;
    subscription.SubscriptionType = updatedSubscription.SubscriptionType;
    subscription.StartDate = updatedSubscription.StartDate;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/Subscriptions/{SubscriptionId:guid}", async (Guid SubscriptionId, SubscriptionsDB db) =>
{
    var subscription = await db.Subscriptions.FindAsync(SubscriptionId);
    if (subscription is null)
    {
        return Results.NotFound();
    }

    db.Subscriptions.Remove(subscription);
    await db.SaveChangesAsync();

    return Results.NoContent();
});




app.Run();
