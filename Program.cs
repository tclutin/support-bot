
using Microsoft.Extensions.Configuration;
using SPIAPI;
using SupportBot.Service;

namespace SupportBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Services.AddControllers().AddNewtonsoftJson();

                builder.Services.AddScoped<ISupportService>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    var apiUrl = configuration["SupportService:ApiUrl"];

                    var client = new HttpClient();

                    return new SupportService(apiUrl, client);
                });
            }

            var app = builder.Build();
            {
                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();

            }
        }
    }
}