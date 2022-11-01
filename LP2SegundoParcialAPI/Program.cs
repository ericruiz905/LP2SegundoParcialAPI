using Microsoft.EntityFrameworkCore;
using LP2SegundoParcialAPI.Models;
using LP2SegundoParcialAPI.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//getOrder 
app.MapGet("/order/OrderID", (NorthwindContext db, int id) =>
{
    var order = db.Orders.Find(id);
    var showord = JsonSerializer.Serialize(order);
    return Results.Ok(order);
});

//createOrder
app.MapPost("/order/addorder", (NorthwindContext db, Order order) => {
    db.Orders.Add(order);
    db.SaveChanges();
    return Results.Created($"/order/OrderID/{order.OrderId}", order);
});

//create order detail

app.MapPost("/orderdetail/addorderdetail", (NorthwindContext db, OrderDetail orderDetail) => {
    db.OrderDetails.Add(orderDetail);
    db.SaveChanges();
    return Results.Created($"/order/OrderID/{orderDetail.OrderId}", orderDetail);
});

app.Run();

