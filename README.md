# GRPC 性能分析
测试代码:CoreTest/GrpcClient/Program.cs

## 测试条件
+ GRPC（.NET Core 3.1）
+ WCF（.NET Framework 4.8）
+ WebAPI（.NET Framework 4.8）
+ WebAPI（.NET Core 3.1）

## 测试环境
 windows 10 AMD 3.6GHz 32G

## 测试结果

### Release模式

================单线程1000次============  
WCF:6.9437806  
GRPC:1.2304431  
WebAPI(Framework):4.3744211  
WebAPI(Core):1.547731  

================10线程，每个线程100次============
WCF:3.0362185  
GRPC:0.1799795  
WebAPI(Framework):1.5647574  
WebAPI(Core):0.1305243  


注意：不要通过调试查看消耗时间

### 调试测试

================单线程1000次============  
WCF:10.6782994  
GRPC:50.7242537  
WebAPI(Framework):53.7119758  
WebAPI(Core):47.4203199  

================10线程，每个线程100次============  
WCF:8.1215767  
GRPC:48.4401205  
WebAPI(Framework):42.2054548  
WebAPI(Core):19.3688736  

