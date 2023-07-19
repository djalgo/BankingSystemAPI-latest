using BankingSystemAPI.Controllers;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBankingOperationsRepository, BankingOperationsRepository>()
    .AddSingleton<IBankOperationsService, BankOperationsService>()
    .AddSingleton<ITransactionService, TransactionService>()
    .AddSingleton<ILoggingService, TestLogger>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //adding global exception handling feature
    app.UseExceptionHandler(

        options =>
        {
            options.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            await context.Response.WriteAsync(ex.Error.Message);
                        }
                    }
                );
        }
    );
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
