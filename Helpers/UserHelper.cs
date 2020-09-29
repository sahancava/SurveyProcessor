using Microsoft.AspNetCore.Mvc;

namespace Anket.Helpers
{
    public class UserHelper : Controller
    {
        public bool GetUser(string UserName, string Password)
        {
            if (
                (UserName=="sahan" && Password=="password") || (UserName == "kulslanıcı" && Password == "password"))
            {
                return true;
            }
            return false;
        }
    }
}
