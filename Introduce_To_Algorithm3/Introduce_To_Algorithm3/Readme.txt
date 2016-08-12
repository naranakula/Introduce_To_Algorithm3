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
Quartz.net
RestSharp (Simple REST and HTTP API Client for .NET)
Ninject>Autofac
aspose.cells NPOI  两者均不需要安装office
SharpZipLib   压缩包库
FTP.dll (收费，nuget是评估版) >(优于) > edtFTPnet/Free(使用) > System.Net.FtpClient
CefSharp  （wpf和winform的chromium嵌入）
Topshelf (构建windows service)
EntityFramework.Extended (Entity framework的拓展，谨慎使用)
MailKit

//webservice
Nancy > SeviceStack

//中间件
ActiveMq
RocketMq (阿里的MQ)
Kafka
RabbitMq>ActiveMq  (可以肯定的是rabbitmq好于activemq)
Redis>memcached
Sqlite LiteDB
FileZilla  (FTP)
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

//UI
FineUI
WinForm
Wpf

Jquery+Bootstrap

//网络框架
Netty>Mina(Mina社区不活跃)
DotNetty
Wcf
ZeroMQ (NetMq是C#实现)
NetworkComms.Net（已开源，建议使用）
HP-Socket(开源  http://www.jessma.org/)
SuperSocket(bug太多)

//winform wpf
Live-Charts

//python
pythonnet


//图片
http://www.easyicon.net/