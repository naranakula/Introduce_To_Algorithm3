 如何添加管理员运行
        1、右键属性>安全性>启用ClickOnce安全设置,自动在Properties下生成app.manifest
        2、修改app.maifest为 requestedExecutionLevel level="requireAdministrator" uiAccess="false" 
        3、禁用 安全性 ClickOnce安全设置


//组件
Entity framework
AutoMapper
NLog（建议使用 NLog>Log4net）
Log4net
Newtonsoft.Json
Polly(重试执行框架)
Quartz.net
RestSharp (Simple REST and HTTP API Client for .NET)
Ninject>Autofac
aspose.cells NPOI  两者均不需要安装office
SharpZipLib   压缩包库
System.Net.FtpClient

//webservice
Nancy
SeviceStack

//中间件
ActiveMq
Redis>memcached
Sqlite
Ftp
Wcf
Wpf