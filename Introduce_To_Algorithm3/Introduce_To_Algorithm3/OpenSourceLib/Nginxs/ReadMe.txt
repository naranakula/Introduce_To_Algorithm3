nginx 有一个master Process和several worker processes. 

nginx的配置文件conf\nginx.conf

启动nginx,cd到nginx所在的目录,运行命令:start nginx

nginx启动后,可以通过如下命令控制:
		nginx -s stop:快速关闭
		nginx -s quit:优雅关闭
		nginx -s reload:重新加载配置文件
		nginx -s reopen:reopening the log files

nginx的配置分为简单指令和块指令
简单指令: 键值对通过空白分割,结尾加分号.
块指令: 
http {
    include       mime.types;
    default_type  application/octet-stream;
}
#表示注释
































































































































