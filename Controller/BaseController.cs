using Microsoft.AspNetCore.Mvc;
using Serilog;
using ApiDB.Options;

namespace ApiDB.Controller
{
    public class BaseController : ControllerBase
    {
        protected virtual void Logs(string str)
        {
            Log.Error(str);
        }
        #region 统一返回结构 : 返回时间和请求相关信息
        protected virtual IActionResult ActionResultStatus(object obj)
        {
            return obj != null ? 
            Ok(new ActionResultStatus
            {
                Code = StatusCodes.Status200OK,
                Message = Options.ActionResultStatus.Ok,
                Data = obj,
                MsgTime = DateTime.Now.ToString("G")
            }) :
            NotFound(new ActionResultStatus
            {
                Code = StatusCodes.Status404NotFound,
                Message = Options.ActionResultStatus.Err,
                MsgTime = DateTime.Now.ToString("G"),
                Data = obj
            });
        }

        protected virtual IActionResult ActionResultStatus(bool s)
        {
            return Ok(new ActionResultStatus
            {
                Code = s ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                Message = s ? Options.ActionResultStatus.Ok : Options.ActionResultStatus.Err,
                MsgTime = DateTime.Now.ToString("G")
            });
        }
        #endregion
    }
}
