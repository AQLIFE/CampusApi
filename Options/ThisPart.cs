using Microsoft.AspNetCore.Mvc;

namespace ApiDB.Options
{
    public class ThisPart
    {
        public string? partName { get; set; }//角色用名

        public string? partPass { get; set; }//角色密码
    }

    public class TokenInfo
    {
        /// <summary>
        /// token
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? Name { get; set; }
    }
}
