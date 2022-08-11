using ApiDB.Comon;
using ApiDB.IService;
using ApiDB.Model;
using ApiDB.Options;
using ApiDB.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

//using System.Data.Entity.Validation;

namespace ApiDB.Controller
{
    [ApiController]
    [EnableCors]
    public class AuthController : BaseController
    {
        private readonly WebContext _context;
        private readonly ILogger<AuthController> _logger;

        private readonly ICustom _iCustom;

        public AuthController(WebContext context, ILogger<AuthController> logger,ICustom icustom) {
            _context = context; //获得项目的数据连接
            _logger = logger;
            _iCustom = icustom;
        }

        // 授权
        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(typeof(TokenInfo), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync(ThisPart dto)
        {
            #region 校验用户信息
            List<Parts> obj;
            try
            {
                Console.WriteLine("尝试登录");
                obj = await _context.Part.Where<Parts>(q => q.Name == dto.partName && q.Pass == dto.partPass).ToListAsync<Parts>();

                Console.WriteLine(obj[0].ToString());//为什么需要这样写 才能触发我的错误捕获？
                // 下标溢出=>
                // Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine($"{dto.partName} 信息验证失败");
                //Console.WriteLine(ex);
            }
            #endregion

            #region 认证通过，发送 JWT
            if (obj != null)
            {
                TokenInfo info = new ()
                {
                    Name = dto.partName,                //鉴权用户
                    Status = obj[0].Status.ToString(),     //权限级别
                    Expires = DateTime.Now.AddDays(3)  //失效时间
                };//创建一个对象，作为登录接口的返回信息

                return ActionResultStatus( _iCustom.GetToken(info));
            }
            else return ActionResultStatus(obj);
            #endregion
        }
    }
}
