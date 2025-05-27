# REST API 项目开发日记

## 项目介绍
这是一个使用 C# 和 .NET 9.0 开发的 REST API 项目。项目采用最新的 .NET 技术栈，使用 Microsoft.AspNetCore.OpenApi 包来支持 OpenAPI 规范。项目的主要目标是构建一个现代化的 Web API 服务。

### 技术栈
- .NET 9.0
- C# 编程语言
- ASP.NET Core Web API
- OpenAPI/Swagger 支持

## 开发日记

### 2025-05-20
今天是项目的启动日，主要完成了以下工作：

1. 项目初始化
   - 创建了基于 .NET 9.0 的 Web API 项目
   - 配置了基本的项目结构和依赖

2. C# 与 Java 的初步对比体验：
   - 语法差异：
     - C# 使用 `using` 语句进行命名空间导入，而 Java 使用 `import`
     - C# 的属性（Property）语法比 Java 的 getter/setter 更简洁
     - C# 的命名空间组织方式与 Java 的包（package）概念类似
   
   - 开发体验：
     - .NET 的项目文件（.csproj）比 Java 的 pom.xml 更简洁
     - C# 的异步编程模型（async/await）比 Java 的 CompletableFuture 更直观

3. 下一步计划：
   - 设计 API 接口
   - 实现基本的 CRUD 操作
   - 添加数据验证和错误处理
   - 配置 Swagger 文档 

### 2025-05-20（补充）
今天对项目结构进行了重构，将所有业务逻辑和数据模型从 Program.cs 中移出，分别放入 Models、Services 和 Controllers 目录中，只保留了应用程序的配置和启动相关代码。

**重构原因：**
- 保持代码结构清晰，遵循单一职责原则
- 提高代码的可维护性和可扩展性
- 便于后续功能的开发和团队协作

**具体做法：**
- 创建 Models 目录，存放数据模型（如 WeatherForecast）
- 创建 Services 目录，存放业务逻辑（如 WeatherService）
- 创建 Controllers 目录，处理 HTTP 请求（如 WeatherController）
- Program.cs 只负责服务注册、管道配置和应用启动
- 删除了 Program.cs 中多余的模型和数据定义

**收获体会：**
- 结构化的项目更易于理解和维护
- 业务逻辑和控制器分离后，代码复用性和测试性大大提升
- 这种分层结构与 Java Spring Boot 项目类似，便于迁移和对比学习 

### 2025-05-21
今天深入了解了 Swagger 的使用方法和优点，并成功在项目中集成。

**Swagger 简介：**
- Swagger（OpenAPI）是一个用于描述 RESTful API 的规范
- 提供了交互式的 API 文档界面
- 可以直接在浏览器中测试 API

**使用方法：**
1. 添加 Swagger 包：
   ```xml
   <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
   ```

2. 配置 Program.cs：
   ```csharp
   // 添加 Swagger 服务
   builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather API", Version = "v1" });
   });

   // 配置中间件
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API V1");
   });
   ```

3. 访问 Swagger UI：
   - 开发环境：http://localhost:5038/swagger
   - 查看 API 文档
   - 测试 API 端点

**Swagger 的优点：**
1. 开发便利：
   - 自动生成 API 文档
   - 无需手动编写文档
   - 直接在浏览器中测试 API

2. 团队协作：
   - 前端开发人员可以快速了解 API
   - 提供清晰的 API 规范
   - 减少沟通成本

3. 测试效率：
   - 提供图形界面测试 API
   - 可以查看请求/响应示例
   - 支持不同参数组合测试

4. 文档维护：
   - 文档与代码同步更新
   - 减少文档维护成本
   - 提高文档准确性

**代码重构：分离接口和实现**
1. 创建了 `Interfaces` 目录
2. 将 `IWeatherService` 接口移到单独的文件中
3. 更新了 `WeatherService` 实现类
4. 修改了 `Program.cs` 中的引用

**分离的好处：**
1. 更好的关注点分离
2. 更容易维护和扩展
3. 符合单一职责原则
4. 接口可以被多个实现类使用
5. 更容易进行单元测试

**下一步计划：**
- 为 API 添加更详细的注释
- 完善 API 文档
- 添加更多 API 端点
- 实现数据验证 

### 2025-05-27
今天对项目进行了功能扩展和代码优化。

**新增功能：**
1. 添加了城市相关的API：
   - 创建了`City`模型
   - 实现了`ICityService`接口
   - 添加了`CityController`控制器
   - 支持按城市查询天气

2. API端点：
   - `GET /city` - 获取所有城市列表
   - `GET /city/{cityName}/weather` - 获取特定城市的天气

**代码优化：**
1. 路由设计：
   - 使用RESTful风格的URL
   - 通过路由参数传递城市名称
   - 清晰的API层次结构

2. 错误处理：
   - 添加了城市不存在的检查
   - 返回适当的HTTP状态码
   - 添加了日志记录

3. 依赖注入：
   - 优化了服务注册
   - 移除了未使用的引用
   - 遵循依赖倒置原则

**技术要点：**
1. 路由参数：
   - 使用`{cityName}`定义路由参数
   - 参数自动绑定到方法参数
   - 支持参数约束

2. 日志记录：
   - 使用结构化日志
   - 记录请求和响应信息
   - 便于问题排查

3. 代码组织：
   - 清晰的目录结构
   - 接口和实现分离
   - 遵循SOLID原则

**下一步计划：**
1. 添加数据验证
2. 实现缓存机制
3. 添加单元测试
4. 完善错误处理