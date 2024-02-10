using Qoniac.CodingTask.CurrencyConverter.BusinessLogic;
using Qoniac.CodingTask.CurrencyConverter.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

// Register the generic currency converter manager and its concrete implementation
builder.Services.AddScoped<ICurrencyConverterManager, CurrencyConverterManager>();

builder.Services.AddScoped<ICurrencyRangeValidator, DollarCurrencyRangeValidator>();
builder.Services.AddScoped<ICurrencyFormatValidator, DollarCurrencyFormatValidator>();
builder.Services.AddScoped<ICurrencyConverter, DollarCurrencyConverter>();

// Register MemoryCache
builder.Services.AddMemoryCache();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
                    policy =>
                    {
                        policy.AllowAnyMethod();
                        policy.SetIsOriginAllowed((host) => true);
                        policy.AllowAnyHeader();
                        policy.AllowAnyOrigin();
                    });
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

app.UseRouting();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();
