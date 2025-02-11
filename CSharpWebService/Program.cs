using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CSharpWebService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace CSharpWebService;

public class WebApiService
{
    private IHost? _host;

    public async Task StartAsync()
    {
        var builder = WebApplication.CreateBuilder();

        // 读取配置文件
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                reloadOnChange: true);

        // builder.Services.AddControllers()
        //     .AddApplicationPart(Assembly.GetExecutingAssembly());

        // 配置 JSON 序列化选项
        var services= builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.WriteIndented = true;
        });
       
        var partManager = services.PartManager;

        // 获取当前程序集所在目录的所有程序集
        var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Select(x => Assembly.LoadFrom(x));

        // 添加包含控制器的程序集
        foreach (var assembly in assemblies)
        {
            if (assembly.GetTypes().Any(t =>
                    !t.IsAbstract &&
                    t.IsSubclassOf(typeof(ControllerBase))))
            {
                partManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }
        }   

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "CSharpWebHost API",
                Version = "v1"
            });
        });
        // builder.Services.AddControllers()
        //     .AddApplicationPart(typeof(WeatherController).Assembly);
        _host = builder.Build();
        var assemblieList = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblieList)
        {
            Console.WriteLine($"Loaded Assembly: {assembly.FullName}");
        }

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
        var controllerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t));

        foreach (var type in controllerTypes)
        {
            Console.WriteLine($"Detected Controller: {type.FullName}");
        }


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