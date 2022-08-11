using ApiDB.Options;

namespace ApiDB.IService
{
    public interface ICustom
    {
        public string GetToken(TokenInfo info);
    }
}
