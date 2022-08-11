using ApiDB.IService;
using ApiDB.Options;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiDB.Service
{
    public class CustomJWT : ICustom
    {
        private readonly IConfiguration _configuration;

        public CustomJWT(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(TokenInfo info)
        {
            Claim[] claims;
            try
            {
                claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, _configuration["Authentication:Audience"]),//添加URL？
                new Claim(ClaimTypes.Name,info.Name),    //添加经过验证的角色名
                new Claim(ClaimTypes.Role,info.Status)   //添加鉴权级别
                };
            }
            catch (Exception ex) {
                Console.WriteLine("服务器项目环境部分信息为null" + ex.Message);

                //Console.WriteLine($"info.Name={info.Name}");
                //Console.WriteLine($"info.Status={info.Status}");
                ////Console.WriteLine($"info.Audience={_tokenOptions.Audience}");
                //Console.WriteLine($"info.configuration={_configuration["Authentication:Audience"]}");
                return null;
            }

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Authentication:SecurityKey"]));//获得加密密钥

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);//设置加密方式

            JwtSecurityToken token = new(
                issuer: _configuration["Authentication:Isuser"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,     //写入验证信息
                expires: info.Expires,//过期时间
                signingCredentials: creds);//加密 Key

            return new JwtSecurityTokenHandler().WriteToken(token);//生成token信息头
        }
    }
}
