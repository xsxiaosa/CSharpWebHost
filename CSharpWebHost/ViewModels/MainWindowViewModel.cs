using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ReactiveUI;
using CSharpWebHost.Models;
using CSharpWebHost.Services;

namespace CSharpWebHost.ViewModels
{
  public class MainWindowViewModel : ReactiveObject
  {
    private readonly HttpClient _httpClient;
    private readonly WebApiService _webApiService;
    private string _apiResponse = "";

    // 获取单个城市天气
    private string _cityId = "";

    // 新增城市天气
    private string _newCity = "";
    private string _newTemperature = "";
    private string _newCondition = "";

    // 修改城市天气
    private string _updateCityId = "";
    private string _updateTemperature = "";
    private string _updateCondition = "";

    // 删除城市天气
    private string _deleteCityId = "";

    public MainWindowViewModel()
    {
      _webApiService = new WebApiService();
      _ = _webApiService.StartAsync();
      _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

      // 初始化所有命令
      GetAllWeather = ReactiveCommand.CreateFromTask(GetAllWeatherAsync);
      GetWeatherById = ReactiveCommand.CreateFromTask(GetWeatherByIdAsync);
      AddWeather = ReactiveCommand.CreateFromTask(AddWeatherAsync);
      UpdateWeather = ReactiveCommand.CreateFromTask(UpdateWeatherAsync);
      DeleteWeather = ReactiveCommand.CreateFromTask(DeleteWeatherAsync);
    }

    public string ApiResponse
    {
      get => _apiResponse;
      set => this.RaiseAndSetIfChanged(ref _apiResponse, value);
    }

    // 属性定义
    public string CityId
    {
      get => _cityId;
      set => this.RaiseAndSetIfChanged(ref _cityId, value);
    }

    public string NewCity
    {
      get => _newCity;
      set => this.RaiseAndSetIfChanged(ref _newCity, value);
    }

    public string NewTemperature
    {
      get => _newTemperature;
      set => this.RaiseAndSetIfChanged(ref _newTemperature, value);
    }

    public string NewCondition
    {
      get => _newCondition;
      set => this.RaiseAndSetIfChanged(ref _newCondition, value);
    }

    public string UpdateCityId
    {
      get => _updateCityId;
      set => this.RaiseAndSetIfChanged(ref _updateCityId, value);
    }

    public string UpdateTemperature
    {
      get => _updateTemperature;
      set => this.RaiseAndSetIfChanged(ref _updateTemperature, value);
    }

    public string UpdateCondition
    {
      get => _updateCondition;
      set => this.RaiseAndSetIfChanged(ref _updateCondition, value);
    }

    public string DeleteCityId
    {
      get => _deleteCityId;
      set => this.RaiseAndSetIfChanged(ref _deleteCityId, value);
    }

    // 命令定义
    public ReactiveCommand<Unit, Unit> GetAllWeather { get; }
    public ReactiveCommand<Unit, Unit> GetWeatherById { get; }
    public ReactiveCommand<Unit, Unit> AddWeather { get; }
    public ReactiveCommand<Unit, Unit> UpdateWeather { get; }
    public ReactiveCommand<Unit, Unit> DeleteWeather { get; }

    // 命令实现
    private async Task GetAllWeatherAsync()
    {
      try
      {
        var response = await _httpClient.GetAsync("/api/weather");
        ApiResponse = await FormatResponse(response);
      }
      catch (Exception ex)
      {
        ApiResponse = $"错误: {ex.Message}";
      }
    }

    private async Task GetWeatherByIdAsync()
    {
      if (string.IsNullOrEmpty(CityId)) return;

      try
      {
        var response = await _httpClient.GetAsync($"/api/weather/{CityId}");
        ApiResponse = await FormatResponse(response);
      }
      catch (Exception ex)
      {
        ApiResponse = $"错误: {ex.Message}";
      }
    }

    private async Task AddWeatherAsync()
    {
      if (string.IsNullOrEmpty(NewCity) || string.IsNullOrEmpty(NewTemperature) ||
          string.IsNullOrEmpty(NewCondition)) return;

      try
      {
        var weather = new Weather
        {
          City = NewCity,
          Temperature = double.Parse(NewTemperature),
          Condition = NewCondition,
          UpdateTime = DateTime.Now
        };

        var response = await _httpClient.PostAsJsonAsync("/api/weather", weather);
        ApiResponse = await FormatResponse(response);
      }
      catch (Exception ex)
      {
        ApiResponse = $"错误: {ex.Message}";
      }
    }

    private async Task UpdateWeatherAsync()
    {
      if (string.IsNullOrEmpty(UpdateCityId) || string.IsNullOrEmpty(UpdateTemperature) ||
          string.IsNullOrEmpty(UpdateCondition)) return;

      try
      {
        var weather = new Weather
        {
          Id = int.Parse(UpdateCityId),
          Temperature = double.Parse(UpdateTemperature),
          Condition = UpdateCondition,
          UpdateTime = DateTime.Now
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/weather/{UpdateCityId}", weather);
        ApiResponse = await FormatResponse(response);
      }
      catch (Exception ex)
      {
        ApiResponse = $"错误: {ex.Message}";
      }
    }

    private async Task DeleteWeatherAsync()
    {
      if (string.IsNullOrEmpty(DeleteCityId)) return;

      try
      {
        var response = await _httpClient.DeleteAsync($"/api/weather/{DeleteCityId}");
        ApiResponse = await FormatResponse(response);
      }
      catch (Exception ex)
      {
        ApiResponse = $"错误: {ex.Message}";
      }
    }

    private async Task<string> FormatResponse(HttpResponseMessage response)
    {
      var content = await response.Content.ReadAsStringAsync();
      try
      {
        // 尝试格式化JSON响应
        var jsonDoc = JsonDocument.Parse(content);
        return JsonSerializer.Serialize(jsonDoc,
          new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
      }
      catch
      {
        // 如果不是JSON，直接返回内容
        return content;
      }
    }
  }
}