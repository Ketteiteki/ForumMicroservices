

namespace ForumApi.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHost(args).Build().Run();
    }

    private static IHostBuilder CreateWebHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });
}