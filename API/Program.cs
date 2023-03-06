using API.Data.Contex;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        const string DevEnvValue = "Development";
        const string AzureDBPath = @"D:\home\stockapp.db";

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddAuthorization();

            string DBPath = builder.Configuration.GetConnectionString("DefaultConnection");
            bool isDevEnv = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == DevEnvValue ? true : false;
            
           if(!isDevEnv && !File.Exists(AzureDBPath))
                CopyDb(DBPath);

            builder.Services.AddDbContext<StockContext>(options =>
            {
                options.UseSqlite($"data source ={(isDevEnv ? DBPath : AzureDBPath)}");
                //options.UseSqlite($"data source ={DBPath}");
            });

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IStocksService, StocksService>();
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
            app.MapControllers();
            app.UseHttpsRedirection();
            app.UseAuthorization();           
            app.Run();
        }


        private static void CopyDb(string DBPath)
        {
            File.Copy(DBPath, AzureDBPath);
            File.SetAttributes(AzureDBPath, FileAttributes.Normal);
        }
    }
}