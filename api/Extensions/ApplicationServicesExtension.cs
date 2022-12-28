using api.DAL;
using api.Services;

namespace api.Extensions
{
  public static class ApplicationServicesExtension
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddScoped<UnitOfWork, UnitOfWork>();
      services.AddScoped<TokenService, TokenService>();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      return services;
    }
  }
}