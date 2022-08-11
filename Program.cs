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
//builder.Services.AddEndpointsApiExplorer(); //ʹ�� ��СApi
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();//��ӿ����� : �ֶ���������

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader() // �����κ�ͷ�ļ�
         .AllowAnyMethod()      // �����κ�����
         .AllowAnyOrigin();     // �����κνӿڣ�
    });
});//��������

builder.Services.AddDbContext<WebContext>(
    options => options.UseMySQL(builder.Configuration["ConnectionStrings:MysqlContext"])
);//������ݿ�����������

#region �����ļ���Ϣע��

TokenOptions tokenOption = new();
builder.Configuration.Bind("Authentication", tokenOption);//����������������

builder.Services.AddScoped<ICustom, CustomJWT>();
#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        #region JWTУ������
        byte[] secretByte = Encoding.UTF8.GetBytes(tokenOption.SecurityKey);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,//��֤������
            ValidIssuer = tokenOption.Isuser,//��֤��Ϣ �� ������
            ValidateAudience = true,// ��֤����Ա��
            ValidAudience = tokenOption.Audience,
            ValidateLifetime = true,//��֤ʧЧʱ��
            ValidateIssuerSigningKey = true,//��֤��Կ
            IssuerSigningKey = new SymmetricSecurityKey(secretByte),

            /* - ʹ���Զ��� ��֤����
             *  AudienceValidator = (x, y, z) => { return x == y && y!=null; } ������֤
             *  ...
             */

        };//��֤��Ȩ������
        #endregion

        #region JWTУ��ʧ�ܴ���
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
                        Message= "��ĵ�¼��Ϣ�����ڻ���ʧЧ�����¼������",
                        MsgTime = DateTime.Now.ToString("G")
                    }
                 );
                return Task.FromResult(0);
            }
        };// ��֤ʧ���¼�����
        #endregion
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();ʹ��HttpsЭ��
app.UseCors();          //ʹ�ÿ�����
app.UseAuthorization(); //��֤
app.UseAuthentication();//��Ȩ
app.MapControllers();   //ʹ�� ����·��

app.Run();