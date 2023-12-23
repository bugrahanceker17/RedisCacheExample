using Microsoft.Extensions.Caching.Distributed;
using RedisCacheExample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString");
});

builder.Services.AddSingleton<ICacheManager, CacheManager>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(opt =>
        opt.WithOrigins("*").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();