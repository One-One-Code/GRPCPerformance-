using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiService.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public GetUserStatusOutput CheckUserStatus([FromBody] GetUserStatusInput request)
        {
            if (!string.IsNullOrEmpty(request.Token))
            {
                return new GetUserStatusOutput { IsNormal = true };
            }
            return new GetUserStatusOutput { IsNormal = false };
        }

        [HttpPost]
        public LoginOutput Login([FromBody] LoginInput request)
        {
            if (request.Name.Equals("huang") && request.Password.Equals("123456"))
            {
                return new LoginOutput { Token = $"{request.Name}{request.Password}" };
            }
            return new LoginOutput { Token = string.Empty };
        }
    }

    public class GetUserStatusInput
    {
        public string Token { get; set; }
    }

    public class GetUserStatusOutput
    {
        public bool IsNormal { get; set; }
    }

    public class LoginInput
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }

    public class LoginOutput
    {
        public string Token { get; set; }
    }
}
