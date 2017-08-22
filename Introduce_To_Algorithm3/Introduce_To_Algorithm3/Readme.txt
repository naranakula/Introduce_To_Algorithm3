 如何添加管理员运行
        1、右键属性>安全性>启用ClickOnce安全设置,自动在Properties下生成app.manifest
        2、修改app.maifest为 requestedExecutionLevel level="requireAdministrator" uiAccess="false" 
        3、禁用 安全性 ClickOnce安全设置

//可以将wpf，winform程序编译为console程序

https://github.com/quozd/awesome-dotnet#awesome-dotnet
https://github.com/Microsoft/dotnet/blob/master/dotnet-developer-projects.md

网络调试工具和串口调试工具 可以发送 ASCII和16进制数据，不能发送汉字

netstat -ano查看端口占用情况


ConsoleControl:在winform或者wpf中嵌入可输入输出的console。

//组件
Entity framework
Dapper (轻量级 半自动的sqlhelper替代品)
AutoMapper
NLog（建议使用 NLog>Log4net）
Serilog
Log4net
Newtonsoft.Json (Json)  Msgpack protocol buffer 消息格式
EPPlus //excel 读写
Polly(重试执行框架)
Quartz.net  http://cronexpressiondescriptor.azurewebsites.net/
RestSharp (Simple REST and HTTP API Client for .NET)
Ninject>Autofac
Hangfire
aspose.cells NPOI  两者均不需要安装office
SharpZipLib   压缩包库
FTP.dll (收费，nuget是评估版 https://www.limilabs.com/) >(优于) > edtFTPnet/Free(http://enterprisedt.com/products/edtftpnet/使用, Nuget版本比较旧，从官网下载) > System.Net.FtpClient
CefSharp  （wpf和winform的chromium嵌入）
Topshelf (构建windows service) //需要.net 4.5.2
EntityFramework.Extended (Entity framework的拓展，谨慎使用)
MailKit
CsvHelper
FileHelpers
RX.NET
protobuf-net
NET Transactional File Manager
BinaryRage//本地key value存储
AsyncCollections
//webservice
Nancy > SeviceStack
dotnet-state-machine/stateless   //状态机
libsodium-net < SecurityDriven.Inferno（要求.net4.5.2） < bouncycastle(没有文档啊)//加解密

Exceptionless - Exceptionless .NET Client
i18n //smart internationalization for asp.net
//中间件
ActiveMq
RocketMq (阿里的MQ)
Ngnix
Grpc
Kafka
RabbitMq>ActiveMq  (可以肯定的是rabbitmq好于activemq) EasyNetQ
Redis>memcached
Sqlite LiteDB ravendb(不是嵌入式)  Leveldb rocksdb
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
Naudio //Audio and MIDI library for .NET
cscore

//证书
Portable.Licensing

//图像处理
 ImageMagick //这个好
ImageProcessor//目前的最佳选择
DotImage(收费)
ImageProcessor - Open-source .NET library to manipulate images on-the-fly.
DynamicImage - High-performance open-source image manipulation library for ASP.NET.
MetadataExtractor - Extracts Exif, IPTC, XMP, ICC and other metadata from image files.
Emgu CV - Cross-platform .NET wrapper for the OpenCV library.
DotImaging - Minimalistic .NET imaging portable platform
Magick.NET - .NET wrapper for the ImageMagick library.

//远程管理
Ulterius/server

Umbraco CMS > Orchard

//UI  http://www.cnblogs.com/jyz/p/3658444.html
FineUI
Ext.Net
WinForm
Wpf
Mahapps.metro
ReactiveUI
MvvmCross
Avalonia 
ModernUI
LiveCharts
Extended WPF Toolkit™ Community Edition
DockPanelSuite
Dragablz
WPFNotification
WPF NotifyIcon //测试过  文档：http://www.codeproject.com/Articles/36468/WPF-NotifyIcon
Mantin.Controls.Wpf.Notification 
Wpf.BusyIndicatorEasy
//下面的谨慎使用，上面的按顺序使用
Xceed Toolkit Plus for WPF
Visifire：一套效果非常好的WPF图表控件，支持3D绘制、曲线、折线、扇形、环形和梯形。
SparrowToolkit：一套WPF图表控件集，支持绘制动态曲线，可绘制示波器、CPU使用率和波形。
DynamicDataDisplay：微软开源的WPF动态曲线图，线图、气泡图和热力图。

BarCodeLib:条形码生成库

GUI
SuperJMN/XAMLPagingControl
MahApps.Metro - Toolkit for creating Metro styled WPF apps
LoadingIndicators.WPF
Callisto - A control toolkit for Windows 8 XAML applications. Contains some UI controls to make it easier to create Windows UI style apps for the Windows Store in accordance with Windows UI guidelines.
ObjectListView - ObjectListView is a C# wrapper around a .NET ListView. It makes the ListView much easier to use and teaches it some new tricks
DockPanelSuite - The Visual Studio inspired docking library for .NET WinForms
AvalonEdit - The WPF-based text editor component used in SharpDevelop
XWT - A cross-platform UI toolkit for creating desktop applications with .NET and Mono
Gtk# - Gtk# is a Mono/.NET binding to the cross platform Gtk+ GUI toolkit and the foundation of most GUI apps built with Mono
MaterialDesignInXamlToolkit - Toolkit for creating Material styled WPF apps
Eto.Forms - Cross platform GUI framework for desktop and mobile applications in .NET and Mono
Dragablz - Dragable, tearable WPF tab control (similar to Chrome) which supports layouts and is full themeable, including themese compatible with MahApps and Material Design.
Fluent.Ribbon - Fluent Ribbon Control Suite is a library that implements an Office- and Windows 8-like Ribbon for WPF.
Office Ribbon - A library that implements MS Office Ribbon for WinForms.
Perspex - A multi-platform .NET UI framework.
MaterialSkin - Theming .NET WinForms, C# or VB.Net, to Google's Material Design Principles.
Xamarin.Forms - Build native UIs for iOS, Android and Windows from a single, shared C# codebase.
SciterSharp - Create .NET cross-platform desktop apps using not just HTML, but all features of Sciter engine: CSS3, SVG, scripting, AJAX, <video>... Sciter is free for commercial use
Sciter
Empty Keys UI - Multi-platform and multi-engine XAML based user interface library [Free][Proprietary]
UWP Community Toolkit - The UWP Community Toolkit is a collection of helper functions, custom controls, and app services. It simplifies and demonstrates common developer tasks building UWP apps for Windows 10.
Caliburn.Micro//MVVM框架 这个好

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

Entity framework Extensions是收费版本 ， 免费版本是Entity Framework Plus（100% Free and Open Source）nuget(Z.EntityFramework.Plus.EF6)

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
PostSharp(收费)>KingAOP>Aspect Injector 


Stateless//状态机
//自动更新
Squirrel.Windows//An installation and update framework for Windows desktop apps
WIX TOOLSET//THE MOST POWERFUL SET OF TOOLS AVAILABLE TO CREATE YOUR WINDOWS INSTALLATION 
nUpdate
NetSparkle
AutoUpdater
AutoUpdater.net


//测试工具
Fiddler//Http 测试工具
PostMan //Http测试工具 这个好

 xUnit.net  测试工具

//二维码
QRCoder
ZXing.Net

/////////////////////////////////////////////////////////////////////////
//最近学习目标
Ngnix
ZooKeeper


Abot//web爬虫
Scut//开源稳定的游戏服务器

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

//常用类库
StringUtils.cs
ConfigUtils.cs
DirectoryHold.cs
AppExitEx.cs //应用程序确保退出安全措施
NLogHelper.cs
QuartzHelper.cs//任务调度
CleanJob.cs
OneRunAtSameTime.cs
OneRuner.cs //单实例运行
SafeInvoke.cs
Retry.cs
PollyHelper.cs
SerializeHelper.cs
XmlUtils.cs
OneRunTimerEx.cs
SocketClientProxy.cs
SocketMonitorTimer.cs //通过这个启动SocketClientProxy
TaskHelper.cs
Contants.cs//定义常量和配置项
DateTimeUtils.cs
BlockingQueueEx.cs
ThreadPoolHelper.cs
CacheHelper.cs
MailHelper.cs
CsvUtils.cs
SqliteCodeFirstContext.cs//Sqlite数据库
NPOIHelper.cs//Excel读写
LicenseHelper.cs//授权文件
LibsodiumHelper.cs//加解密
TokenBucket.cs
MemoryStreamUtils.cs
CultureUtils.cs






文件夹结构
Models
	Jobs
	Dbs
Utils
ReadMe.txt


