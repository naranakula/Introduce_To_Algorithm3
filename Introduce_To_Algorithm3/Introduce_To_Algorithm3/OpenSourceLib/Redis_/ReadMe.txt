Redis key是binary safe string的，可以是任意binary序列。 
该string类型最长可以512M.建议只使用string作为key。

redis客户端:StackExchange.Redis
客户端官网:https://github.com/StackExchange/StackExchange.Redis

redis官网:https://redis.io/
https://github.com/antirez/redis

redis官方不支持windows(memcached也不支持,不建议使用memcached)
Microsoft open tech group支持win64版本地址:https://github.com/MicrosoftArchive/redis

windows上提供两种安装包:
	Redis-x64-3.2.100.msi
	Redis-x64-3.2.100.zip

msi将redis安装位windows service,然后只需要更改redis.windows.conf配置文件
zip解压后可以安装为服务，也可以独立运行:
	1、安装为服务，运行后将服务设置为Autostart，并且launched as "NT AUTHORITY\\NetworkService"
		redis-server.exe --service-install redis.windows.conf --loglevel warning
		日志级别有:debug,verbose,notice,warning
	2、卸载服务
		redis-server.exe --service-uninstall
	
	3、独立运行
		redis-server.exe redis.windows.conf


redis.windows.conf配置(以Redis-x64-3.2.100为例)
	L56 bind 127.0.0.1  # redis监听的地址,注释掉该行,将会监听所有的网卡地址
	L75 protected-mode yes #禁用保护模式 如果启用保护模式将需要设置密码
	L79 port 6379  #指定监控的端口
	L88 tcp-backlog 511 #指定tcp listen() 的 backlog
	L100 timeout 0 # N秒后如果client is idle close the connection (0 to disable) 建议保留0或者86400一天
	L116 tcp-keepalive 0 #n秒后发送tcp ack到客户端，避免dead client 建议60
	L152 loglevel notice #指定日志级别
	L156 logfile "" #指定log file name
	L170 database 16 #指定数据库的个数，默认使用db0
	L194 save 900 1   #save db if both the given number of seconds and the given number of write operations against db occured
	L211 stop-writes-on-bgsave-error no #禁用stop-writes-on-bgsave-error
	L443 # requirepass foobared  #配置密码，默认没有密码
	L475 maxclients 10000 #限制客户端的个数
	L485 persistence-available [no] #禁用持久化
	L525 maxmemory 2gb #最大内存
	L548 maxmemory-policy volatile-lru #配置内存到达时的key清除策略
	L825 slowlog-log-slower-than 700000 #当redis命令执行慢时，记录日志







Redis key是binary safe string的，可以是任意binary序列。 
该string类型最长可以512M.建议只使用string作为key。


redis is an open soce, high performance, key-value stor. values can be strings, hashes, lists, sets and sorted sets.




https://github.com/CodisLabs/codis   Proxy based Redis cluster solution supporting pipeline and scaling dynamically
https://github.com/twitter/twemproxy A fast, light-weight proxy for memcached and redis
https://github.com/uglide/RedisDesktopManager Cross-platform GUI management tool for Redis



















