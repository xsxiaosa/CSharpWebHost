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

        // ��ȡ�����ļ�
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                reloadOnChange: true);

        // builder.Services.AddControllers()
        //     .AddApplicationPart(Assembly.GetExecutingAssembly());

        // ���� JSON ���л�ѡ��
        var services= builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.WriteIndented = true;
        });
       
        var partManager = services.PartManager;

        // ��ȡ��ǰ��������Ŀ¼�����г���
        var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Select(x => Assembly.LoadFrom(x));

        // ��Ӱ����������ĳ���
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

        // // ʹ�������ļ��е�URL����
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