using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Yape.Transaction.Infrastructure.Data;
using Yape.Transaction.Infrastructure.Kafka;
using Yape.Transaction.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TransactionDb")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IProducer<Null, string>>(x =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = builder.Configuration["KafkaSettings:BootstrapServers"]
    };

    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();



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

app.Run();
