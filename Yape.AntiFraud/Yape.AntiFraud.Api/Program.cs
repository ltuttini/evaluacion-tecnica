using Yape.AntiFraud.Core;
using Yape.AntiFraud.Core.Settings;
using Yape.AntiFraud.Strategy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<TransactionSettings>(builder.Configuration.GetSection("Transaction"));

builder.Services.AddSingleton<AntiFraudStrategyFactory>(x=> {
    var factory = new AntiFraudStrategyFactory();
    factory.AddStrategy(new TransactionLimitStrategy(2000));
    factory.AddStrategy(new AccumulatedPerDayStrategy(20000));
    return factory;
});

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
