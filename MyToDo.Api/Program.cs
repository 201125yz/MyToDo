using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Extensions;
using MyToDo.Api.Service;

var builder = WebApplication.CreateBuilder(args);
// ? 固定 API 端口为 3333
builder.WebHost.UseUrls("http://localhost:3333");

// Add services to the container.注册服务

//注册服务ConfigureServices 
builder.Services.AddDbContext<MyToDoContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
    options.UseSqlite(connectionString);

}).AddUnitOfWork<MyToDoContext>()
.AddCustomRepository<ToDo, ToDoRepository>()
.AddCustomRepository<Memo, MemoRepository>()
.AddCustomRepository<User, UserRepository>();

// 创建 MapperConfigurationExpression 并添加 Profile
var mce = new MapperConfigurationExpression();
mce.AddProfile<AutoMapperProFile>(); // 或 mce.AddProfile(new AutoMapperProFile());

// 创建一个 ILoggerFactory（可根据需要配置）
var loggerFactory = LoggerFactory.Create(builder =>
{
    // 可按需添加日志提供者
    builder.AddConsole();
});

// 用新的构造器创建配置
var automapperConfig = new MapperConfiguration(mce, loggerFactory);

// 创建 IMapper 并注入容器
var mapper = automapperConfig.CreateMapper();
builder.Services.AddSingleton<IMapper>(mapper);

//builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

//var automapperConfig = new MapperConfiguration(cfg =>
//{
//    cfg.AddProfile<AutoMapperProFile>();
//});
//builder.Services.AddSingleton(automapperConfig.CreateMapper());

builder.Services.AddTransient<IToDoService, ToDoService>();
builder.Services.AddTransient<IMemoService, MemoService>();
builder.Services.AddTransient<ILoginService, LoginService>();

builder.Services.AddControllers();//注册控制器（API 路由）
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MyToDo.Api",
        Version = "v1"
    });
});//注册 Swagger 文档生成服务


var app = builder.Build();

// Configure the HTTP request pipeline.配置HTTP请求管道
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();//启用 Swagger UI

app.UseAuthorization();//启用授权中间件

app.MapControllers();//映射控制器路由

app.Run();//启动应用程序
