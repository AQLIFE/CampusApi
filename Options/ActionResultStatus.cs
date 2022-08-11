//using Microsoft.AspNetCore.Mvc;

namespace ApiDB.Options
{
    public class ActionResultStatus
    {
        public static string Ok = "操作成功";
        public static string Err = "操作失败";

        public int Code { get; set; }// 信息代码

        public string? Message { get; set; }// 信息

        public string? MsgTime { get; set; }// 信息时间

        public object? Data { get; set; }
    }


}
