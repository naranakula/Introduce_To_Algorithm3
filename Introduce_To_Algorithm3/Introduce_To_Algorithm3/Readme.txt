 如何添加管理员运行
        1、右键属性>安全性>启用ClickOnce安全设置,自动在Properties下生成app.manifest
        2、修改app.maifest为 requestedExecutionLevel level="requireAdministrator" uiAccess="false" 
        3、禁用 安全性 ClickOnce安全设置

//组件
Entity framework
Dapper (轻量级 半自动的sqlhelper替代品)
AutoMapper
NLog（建议使用 NLog>Log4net）
Log4net
Newtonsoft.Json (Json)
Polly(重试执行框架)
Quartz.net  http://cronexpressiondescriptor.azurewebsites.net/
RestSharp (Simple REST and HTTP API Client for .NET)
Ninject>Autofac
aspose.cells NPOI  两者均不需要安装office
SharpZipLib   压缩包库
FTP.dll (收费，nuget是评估版) >(优于) > edtFTPnet/Free(使用, Nuget版本比较旧，从官网下载) > System.Net.FtpClient
CefSharp  （wpf和winform的chromium嵌入）
Topshelf (构建windows service)
EntityFramework.Extended (Entity framework的拓展，谨慎使用)
MailKit
CsvHelper
FileHelpers
RX.NET
NET Transactional File Manager
BinaryRage//本地key value存储
//webservice
Nancy > SeviceStack
dotnet-state-machine/stateless   //状态机
//中间件
ActiveMq
RocketMq (阿里的MQ)
Kafka
RabbitMq>ActiveMq  (可以肯定的是rabbitmq好于activemq)
Redis>memcached
Sqlite LiteDB
FileZilla  (FTP)
hMailServer
Wcf
Web api
Wpf
NServicebus (服务总线) >  MassTransit >Rebus
Opserver  (监控)
Cat (监控)
MyCat (开源分布式数据库中间件)
Zookeeper
akka
Orleans（微软分布式框架）
Hadoop
Docker

//机器学习
accord-net
mathnet-numerics 
AForge.NET
OpenCv
numl

//音频库
cscore

//证书
Portable.Licensing

//图像处理
ImageProcessor
DotImage(收费)

//远程管理
Ulterius/server

//UI  http://www.cnblogs.com/jyz/p/3658444.html
FineUI
Ext.Net
WinForm
Wpf
Mahapps.metro
ReactiveUI
Avalonia 
ModernUI
LiveCharts
Extended WPF Toolkit™ Community Edition
WPFNotification
WPF NotifyIcon
//下面的谨慎使用，上面的按顺序使用
Xceed Toolkit Plus for WPF
Visifire：一套效果非常好的WPF图表控件，支持3D绘制、曲线、折线、扇形、环形和梯形。
SparrowToolkit：一套WPF图表控件集，支持绘制动态曲线，可绘制示波器、CPU使用率和波形。
DynamicDataDisplay：微软开源的WPF动态曲线图，线图、气泡图和热力图。


Jquery+Bootstrap

//网络框架
Netty>Mina(Mina社区不活跃)
DotNetty
Wcf
ZeroMQ (NetMq是C#实现)
NetworkComms.Net（已开源，建议使用）
HP-Socket(开源  http://www.jessma.org/)
SuperSocket(bug太多)
Griffin.Framework(待考察)

//winform wpf
Live-Charts

//python
pythonnet

//游戏服务器引擎
Scut

//A wrapper executable that can be used to host any executable as an Windows service
winsw

//A high performance websocket server library powering Stack Overflow.
NetGain


//图片
http://www.easyicon.net/

//负载均衡
（HAProxy(无windows版本)>Ngnix）+ keepalived

//AOP
PostSharp(收费)>Aspect Injector 

//自动更新
NetSparkle
AutoUpdater


//测试工具
Fiddler//Http 测试工具


//二维码
QRCoder
ZXing.Net

/////////////////////////////////////////////////////////////////////////
//最近学习目标
Ngnix
ZooKeeper