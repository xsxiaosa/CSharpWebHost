# CSharp Web Host

一个基于 .NET 的轻量级 Web 主机框架。

## 项目简介

CSharpWebHost 是一个简单但功能完整的 Web 主机框架，允许开发者快速搭建和部署 Web 应用程序。

## 主要特性

- 轻量级设计
- 易于配置和使用
- 支持中间件扩展
- 内置依赖注入支持
- 灵活的路由系统

## Web API 接口文档

### 天气预报接口

#### 1. 获取天气预报列表
- **接口**: GET /api/weather
- **描述**: 获取所有天气预报信息
- **响应格式**: JSON
- **响应示例**:
```json
[
  {
    "id": 1,
    "date": "2024-03-20",
    "temperatureC": 25,
    "temperatureF": 77,
    "summary": "晴朗"
  }
]
```

#### 2. 获取指定天气预报
- **接口**: GET /api/weather/{id}
- **描述**: 获取指定ID的天气预报详情
- **参数**: 
  - id (路径参数，整数): 天气预报ID
- **响应示例**:
```json
{
  "id": 1,
  "date": "2024-03-20",
  "temperatureC": 25,
  "temperatureF": 77,
  "summary": "晴朗"
}
```

#### 3. 创建天气预报
- **接口**: POST /api/weather
- **描述**: 创建新的天气预报记录
- **请求体**:
```json
{
  "date": "2024-03-20",
  "temperatureC": 25,
  "summary": "晴朗"
}
```
- **响应**: 返回创建的天气预报记录

#### 4. 更新天气预报
- **接口**: PUT /api/weather/{id}
- **描述**: 更新指定ID的天气预报信息
- **参数**: id (路径参数，整数)
- **请求体**: 同创建天气预报
- **响应**: 返回更新后的天气预报记录

#### 5. 删除天气预报
- **接口**: DELETE /api/weather/{id}
- **描述**: 删除指定ID的天气预报记录
- **参数**: id (路径参数，整数)
- **响应**: 204 No Content

## 技术栈

- .NET 6+
- C# 10.0+
- ASP.NET Core
- Microsoft.Extensions.DependencyInjection

## 快速开始

1. 克隆仓库
```bash
git clone https://github.com/yourusername/CSharpWebHost.git
```

2. 构建项目
```bash
cd CSharpWebHost
dotnet build
```

3. 运行示例
```bash
dotnet run --project CSharpWebHost
```

## 配置说明

在 `appsettings.json` 中配置：

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## 使用示例

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

## 贡献指南

欢迎提交 Pull Request 或创建 Issue。

## 许可证

本项目采用 MIT 许可证。详情请参见 [LICENSE](LICENSE) 文件。
