using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WCFService;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();
            var wcfClient = new Service1Client();

            for (var i = 0; i < 1000; i++)
            {
                var loginresult1 = wcfClient.LoginAsync(new LoginInput { Name = "huang", Password = "123456" }).Result;
                var userStatus1 = wcfClient.GetUserStatusAsync(new GetUserStatusInput { Token = loginresult1.Token }).Result;
            }

            watch.Stop();
            var m1 = watch.Elapsed.TotalSeconds;
            //Console.WriteLine(watch.ElapsedMilliseconds);


            watch = new Stopwatch();
            watch.Start();

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new User.UserService.UserServiceClient(channel);
            for (var i = 0; i < 1000; i++)
            {
                var loginResult = client.Login(new User.LoginInput { Name = "huang", Password = "123456" });
                var userStatus = client.GetUserStatus(new User.GetUserStatusInput { Token = loginResult.Token });
            }

            watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            var m2 = watch.Elapsed.TotalSeconds;

            watch = new Stopwatch();
            watch.Start();

            var client2 = new HttpClient();
            for (var i = 0; i < 1000; i++)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(new User.LoginInput { Name = "huang", Password = "123456" }), Encoding.UTF8, "application/json");

                var resultstr = client2.PostAsync("http://localhost:64238/api/user/Login", content).Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<LoginOutput>(resultstr);

                content = new StringContent(JsonConvert.SerializeObject(new GetUserStatusInput { Token = result.Token }), Encoding.UTF8, "application/json");

                resultstr = client2.PostAsync("http://localhost:64238/api/user/CheckUserStatus", content).Result.Content.ReadAsStringAsync().Result;
                var result1 = JsonConvert.DeserializeObject<GetUserStatusOutput>(resultstr);
            }

            watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            var m3 = watch.Elapsed.TotalSeconds;


            watch = new Stopwatch();
            watch.Start();

            var client3 = new HttpClient();
            for (var i = 0; i < 1000; i++)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(new User.LoginInput { Name = "huang", Password = "123456" }), Encoding.UTF8, "application/json");

                var resultstr = client3.PostAsync("http://localhost:56728/user/Login", content).Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<LoginOutput>(resultstr);

                content = new StringContent(JsonConvert.SerializeObject(new GetUserStatusInput { Token = result.Token }), Encoding.UTF8, "application/json");

                resultstr = client3.PostAsync("http://localhost:56728/user/CheckUserStatus", content).Result.Content.ReadAsStringAsync().Result;
                var result1 = JsonConvert.DeserializeObject<GetUserStatusOutput>(resultstr);
            }

            watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            var m4 = watch.Elapsed.TotalSeconds;
            Console.WriteLine($"================单线程1000次============");
            Console.WriteLine($"WCF:{m1}");
            Console.WriteLine($"GRPC:{m2}");
            Console.WriteLine($"WebAPI(Framework):{m3}");
            Console.WriteLine($"WebAPI(Core):{m4}");



            MutiThread(10);

            Console.ReadLine();
        }

        private static void MutiThread(int count)
        {
            var watch = new Stopwatch();
            watch.Start();
            var wcfClient = new Service1Client();
            var threads = new List<Task>();
            for(var i = 0; i < count; i++)
            {
                var task=Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 100; i++)
                    {
                        var loginresult1 = wcfClient.LoginAsync(new LoginInput { Name = "huang", Password = "123456" }).Result;
                        var userStatus1 = wcfClient.GetUserStatusAsync(new GetUserStatusInput { Token = loginresult1.Token }).Result;
                    }
                });
                threads.Add(task);
            }
            Task.WaitAll(threads.ToArray());

            watch.Stop();
            var m1 = watch.Elapsed.TotalSeconds;
            //Console.WriteLine(watch.ElapsedMilliseconds);


            watch = new Stopwatch();
            watch.Start();

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new User.UserService.UserServiceClient(channel);

            threads = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 100; i++)
                    {
                        var loginResult = client.Login(new User.LoginInput { Name = "huang", Password = "123456" });
                        var userStatus = client.GetUserStatus(new User.GetUserStatusInput { Token = loginResult.Token });
                    }
                });
                threads.Add(task);
            }
            Task.WaitAll(threads.ToArray());
          
            watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            var m2 = watch.Elapsed.TotalSeconds;

            watch = new Stopwatch();
            watch.Start();

            var client2 = new HttpClient();

            threads = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 100; i++)
                    {

                        StringContent content = new StringContent(JsonConvert.SerializeObject(new User.LoginInput { Name = "huang", Password = "123456" }), Encoding.UTF8, "application/json");

                        var resultstr = client2.PostAsync("http://localhost:64238/api/user/Login", content).Result.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<LoginOutput>(resultstr);

                        content = new StringContent(JsonConvert.SerializeObject(new GetUserStatusInput { Token = result.Token }), Encoding.UTF8, "application/json");

                        resultstr = client2.PostAsync("http://localhost:64238/api/user/CheckUserStatus", content).Result.Content.ReadAsStringAsync().Result;
                        var result1 = JsonConvert.DeserializeObject<GetUserStatusOutput>(resultstr);
                    }
                });
                threads.Add(task);
            }
            Task.WaitAll(threads.ToArray());

           

            watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            var m3 = watch.Elapsed.TotalSeconds;


            watch = new Stopwatch();
            watch.Start();

            var client3 = new HttpClient();

            threads = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 50; i++)
                    {

                        StringContent content = new StringContent(JsonConvert.SerializeObject(new User.LoginInput { Name = "huang", Password = "123456" }), Encoding.UTF8, "application/json");

                        var resultstr = client3.PostAsync("http://localhost:56728/user/Login", content).Result.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<LoginOutput>(resultstr);

                        content = new StringContent(JsonConvert.SerializeObject(new GetUserStatusInput { Token = result.Token }), Encoding.UTF8, "application/json");

                        resultstr = client3.PostAsync("http://localhost:56728/user/CheckUserStatus", content).Result.Content.ReadAsStringAsync().Result;
                        var result1 = JsonConvert.DeserializeObject<GetUserStatusOutput>(resultstr);
                    }
                });
                threads.Add(task);
            }
            Task.WaitAll(threads.ToArray());



            watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            var m4 = watch.Elapsed.TotalSeconds;
            Console.WriteLine($"================{count}线程，每个线程100次============");
            Console.WriteLine($"WCF:{m1}");
            Console.WriteLine($"GRPC:{m2}");
            Console.WriteLine($"WebAPI(Framework):{m3}");
            Console.WriteLine($"WebAPI(Core):{m4}");
        }
    }
}
