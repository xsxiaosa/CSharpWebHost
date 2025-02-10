using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.IO; // Add this using directive
using Microsoft.Extensions.Configuration; // Add this using directive
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace CSharpWebHost.Services
{
  public class WebApiService
  {
    private IHost? _host;

    public async Task StartAsync()
    {
      var builder = WebApplication.CreateBuilder();

      // 读取配置文件
      builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

      builder.Services.AddEndpointsApiExplorer();


      // 配置 JSON 序列化选项
      builder.Services.AddControllers().AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
      });

      builder.Services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
          Title = "CSharpWebHost API",
          Version = "v1"
        });
      });

      _host = builder.Build();

      var app = _host as WebApplication;

      // // 使用配置文件中的URL设置
      // if (!string.IsNullOrEmpty(app.Configuration["Urls"]))
      // {
      //     app.Urls.Add(app.Configuration["Urls"]);
      // }
      //
      app!.UseSwagger();
      app.UseSwaggerUI();
      app.MapControllers();

      await _host.StartAsync();
    }

    public async Task StopAsync()
    {
      if (_host != null)
      {
        await _host.StopAsync();
        _host.Dispose();
      }
    }
  }
}