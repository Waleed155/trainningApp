
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Reflection.Metadata.Ecma335;
using trainningApp.ConsumerMessages;
using trainningApp.Models;
using trainningApp.Repositories;
using trainningApp.Service;

namespace trainningApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Dependencies
            var connectionString = builder.
                Configuration.
                GetConnectionString("Default");
            builder.
                Services.
                AddDbContext<TrainingContext>(op => op.
                UseSqlServer(connectionString));
            builder.
                Services.
                AddScoped<DbContext,TrainingContext>();
            builder.
                Services.
                AddScoped(typeof(IRepository<>),typeof(Repository<>));

            builder.
                Services.
                AddSingleton<IRabbitMQService,RabbitMQService>();
            builder.
                Services.
                AddSingleton<IRabbitMQService1,RabbitMQService1>();
           // builder.
           //Services.
           //AddSingleton<RabbitMQConsumerService>();
            builder.
            Services.
            AddSingleton<RabbitMQConsumerService1>();
            //builder.
            //    Services.
            //    AddHostedService<RabbitMQConsumerService>();
            //builder.
            //    Services.
            //    AddHostedService<RabbitMQConsumerService1>();



            #endregion
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            #region scopes for RabbitMQservices
            using (var scope = app.Services.CreateScope())
            {
                var rabbit = scope.ServiceProvider.GetRequiredService<IRabbitMQService>();
                await rabbit.InitializeAsync();
            }
            //using (var scope = app.Services.CreateScope())
            //{
            //    var rabbit = scope.ServiceProvider.GetRequiredService<RabbitMQConsumerService>();
            //    await rabbit.InitializeAsync();
            //}
            using (var scope = app.Services.CreateScope())
            {
                var rabbit = scope.ServiceProvider.GetRequiredService<IRabbitMQService1>();
                await rabbit.InitializeAsync();

                }
                //using (var scope = app.Services.CreateScope())
                //{
                //    var rabbit = scope.ServiceProvider.GetRequiredService<RabbitMQConsumerService1>();
                //    await rabbit.InitializeAsync();

                //}
                #endregion
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
        }
    }
}
