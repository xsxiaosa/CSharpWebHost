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
