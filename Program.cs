
namespace SupportBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Services.AddControllers().AddNewtonsoftJson();
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