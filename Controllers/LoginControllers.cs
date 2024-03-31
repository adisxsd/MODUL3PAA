using Microsoft.AspNetCore.Mvc;
using MODUL_PAA_2043.Models;

namespace MODUL_PAA_2043.Controllers
{
    public class LoginController : Controller
    {
        private string __constr;
        private IConfiguration __config;
        public LoginController(IConfiguration configuration)
        {
            __config = configuration;
            __constr = configuration.GetConnectionString("WebApiDatabase");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("api/Login")]
        public IEnumerable<Login> LoginUser(string namaUser, string password)
        {
            LoginContext context = new LoginContext(this.__constr);
            List<Login> listlogin = context.Authentifikasi(namaUser, password, __config);
            return (listlogin).ToArray();
        }
    }
}
