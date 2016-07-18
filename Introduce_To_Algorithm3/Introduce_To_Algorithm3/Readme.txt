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
System.Net.FtpClient
CefSharp  （wpf和winform的chromium嵌入）
Topshelf (构建windows service)
EntityFramework.Extended (Entity framework的拓展，谨慎使用)

//webservice
Nancy > SeviceStack

//中间件
ActiveMq
RocketMq (阿里的MQ)
Kafka
RabbitMq>ActiveMq  (可以肯定的是rabbitmq好于activemq)
Redis>memcached
Sqlite
Ftp
Wcf
Web api
Wpf
NServicebus (服务总线)
Opserver  (监控)
Cat (监控)
MyCat (开源分布式数据库中间件)
FileZilla  (FTP)
Zookeeper
akka


//机器学习
accord-net
mathnet-numerics 



//UI
FineUI
WinForm
Wpf

Jquery+Bootstrap