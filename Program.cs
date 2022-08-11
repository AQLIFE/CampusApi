using ApiDB.Comon;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using ApiDB.Options;
using ApiDB.IService;
using ApiDB.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer(); //使用 最小Api
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();//添加控制器 : 手动新增代码

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader() // 允许任何头文件
         .AllowAnyMethod()      // 允许任何请求
         .AllowAnyOrigin();     // 允许任何接口？
    });
});//跨域设置

builder.Services.AddDbContext<WebContext>(
    options => options.UseMySQL(builder.Configuration["ConnectionStrings:MysqlContext"])
);//添加数据库链接上下文

#region 配置文件信息注入

TokenOptions tokenOption = new();
builder.Configuration.Bind("Authentication", tokenOption);//绑定配置数据至对象

builder.Services.AddScoped<ICustom, CustomJWT>();
#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        #region JWT校验配置
        byte[] secretByte = Encoding.UTF8.GetBytes(tokenOption.SecurityKey);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,//验证发布者
            ValidIssuer = tokenOption.Isuser,//验证信息 ： 发布者
            ValidateAudience = true,// 验证管理员？
            ValidAudience = tokenOption.Audience,
            ValidateLifetime = true,//验证失效时间
            ValidateIssuerSigningKey = true,//验证公钥
            IssuerSigningKey = new SymmetricSecurityKey(secretByte),

            /* - 使用自定义 验证规则
             *  AudienceValidator = (x, y, z) => { return x == y && y!=null; } 发布验证
             *  ...
             */

        };//认证鉴权的配置
        #endregion

        #region JWT校验失败处理
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.WriteAsJsonAsync(
                    new ActionResultStatus {
                        Code = StatusCodes.Status401Unauthorized,
                        Message= "你的登录信息不存在或已失效，请登录后再试",
                        MsgTime = DateTime.Now.ToString("G")
                    }
                 );
                return Task.FromResult(0);
            }
        };// 认证失败事件处理
        #endregion
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();使用Https协议
app.UseCors();          //使用跨域处理
app.UseAuthorization(); //认证
app.UseAuthentication();//授权
app.MapControllers();   //使用 绝对路由

app.Run();