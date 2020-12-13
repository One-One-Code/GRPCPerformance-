using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using User;

namespace CoreTest.Services
{
    public class UserServ : User.UserService.UserServiceBase
    {
        public override Task<GetUserStatusOutput> GetUserStatus(GetUserStatusInput request, ServerCallContext context)
        {
            if (!string.IsNullOrEmpty(request.Token))
            {
                return Task.FromResult(new GetUserStatusOutput { IsNormal = true });
            }
            return Task.FromResult(new GetUserStatusOutput { IsNormal = false });
        }

        public override Task<LoginOutput> Login(LoginInput request, ServerCallContext context)
        {
            if (request.Name.Equals("huang") && request.Password.Equals("123456"))
            {
                return Task.FromResult(new LoginOutput { Token = $"{request.Name}{request.Password}" });
            }
            return Task.FromResult(new LoginOutput { Token = string.Empty });
        }
    }
}
