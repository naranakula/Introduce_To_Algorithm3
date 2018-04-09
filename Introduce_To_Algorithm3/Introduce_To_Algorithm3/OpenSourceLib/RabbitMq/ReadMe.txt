种种迹象表明RabbitMq优于ActiveMq（nuget安装量,官方文档，网络文章等）

Rabbitmq单个消息的大小限制是2GB，并且是不可更改的
Message size is limited to 2GB
队列的个数没有限制


队列x-max-length参数可以指定队列的深度


缺点是客户端是.net4.5.1的  nuget官方客户端:RabbitMQ.Client

来自 https://www.itcentralstation.com/categories/message-queue#top_rated 的关于mq的排名

IBM MQ>RabbitMQ>Apache Kafka>ActiveMQ



RabbitMQ Cluster
	All data/state required for the operation of a RabbitMQ broker is replicated across all nodes. An exception to this are message queues, which by default reside on one node, but they are visible and reachable from all nodes
	RabbitMQ各节点保存一份数据，除了queue以外的数据。其它的节点接收该队列的消息将转发到queue节点上。

	当创建队列时，队列信息保存在集群的一个节点上。其它节点保存queue的metadata和queue实际上存在于哪个节点上。其它的节点接收该队列的消息将转发到queue节点上。当该节点挂掉后，非持久队列消息丢失。当客户端重连到集群上的其它节点时，如果queue时非持久的，将重建队列并且使用新的队列，如果是持久的抛出404 NOT_FOUND错误。持久queue需要恢复原来的node。
	exchange和binding在所有的node上都有一份，当节点挂掉后，重连其它节点不需要创建exchange和binding。

	rabbitmq的节点要么是RAM node,要么是disk node. RAM node存储所有的queues,exchanges,bindings,users,permissions和vhosts的metadata在ram中,disk node将metadata保存到disk中。单节点系统必须是disk node.集群中至少一个是disk node，一般有两到三个.
	当在集群中声明queue,exchange,binding时，调用直到所有的集群节点commited the metadata changes.
	在集群中当所有的disk node挂掉后，不能进行如下操作（仍然可以发送接收消息）:
			create queue
			create exchange
			create binding
			add user
			change permission
			add or remove cluster node


	一般集群中disk node有两个。当ram node重启时，从预先配置的disk node上下载cluster metadata.ram node 需要知道所有的disk node.
	the only metadata RAM nodes store to disk are the addresses of disk nodes in the cluster
	As long as the RAM node can find at least one disk node, it can restart and happily rejoin the cluster.


	RabbitMQ best practice:
		1、消费者尽快取走消息
		2、Limit queue size, with TTL(存活时间) or max-length(队列最大深度，到达最大深度后将从队列头部删除消息)
		3、Queues are single-threaded in RabbitMQ, and one queue can handle up to about 50k messages/s.  The CPU and RAM usage may also be affected in a negative way if you have too many queues, that is thousands. 
		4、尽量将队列分散在集群的多个节点上来提高性能
		5、不要自己命名临时队列，如果不再使用队列Auto Delete(非持久队列Auto Delete)
		6、发送一个大的消息一般比发送多个小的消息好，单个消息最大2GB
		7、限制连接个数而不是队列个数，连接消耗的资源比队列大  It’s recommended that each process only creates one TCP connection, and uses multiple channels in that connection for different threads. 连接一般应该是Long lived
		channel可以opened and closed more frequently.（当前channel也应该longlived） connection是多线程安全的，channel不是
		8、Don’t open and close connections or channels repeatedly, doing that will give you a higher latency, as more TCP packages have to be sent and received.
		9、publisher 和 consumer应该有自己的connection，Separate connections for publisher and consumer to get high throughput. 
		10、A large number of connections and channels might affect the RabbitMQ management interface performance
		11、Prefech预取 定义一次客户端可以获取多少消息，A typical mistake is to have an unlimited prefetch, where one client receives all messages and runs out of memory and crashes, and then all messages are re-delivered again.
		12、Don't open and close connection or channel repeatedly AMQP connections: 7 TCP packages AMQP channel: 2 TCP packages
		13、Don't use too many connections or channels. 减少连接数量，并且publish和consume不要共用connection.channel不是多线程安全
			You should ideally only have one connection per process, and then use a channel per thread in your application.
			Reuse connections
			1 connection for publishing
			1 connection for consuming
		14、Use multiple queues and consumers and Split your queues over different node
		15、Consume (push), don’t poll (pull) for messages
		16、Don't have an unlimited prefetch value A typical mistake is to have an unlimited prefetch, where one client receives all messages and runs out of memory and crashes, and then all messages are re-delivered again.




修改rabbitmq配置文件，需重启程序
the persistence layer 有两个组件:queue index和message store. queue index maintain where message is in a queue along with whether it has been delivered and acknowledged.
message store is a key-value store for messages.Messages (the body, and any properties and / or 
headers) can either be stored directly in the queue index, or written to the message store.


Mirrored queue: 每个mirrored queue有一个master和一个或多个mirror组成，the oldest mirror to be promoted to new master if the old master disappear.发送到master queue的消息在所有的mirror中复制一份，消费者仍然是从master消费消息(不管消费者连接的是哪一个节点)，当master中消息被消费时，同时所有的mirror也移除该消息。



rabbitmq alarms:
	1、默认使用超过系统内存40%，生产者将被阻塞，建议0.6(60%)  vm_memory_high_watermark 
	2、disk_free_limit默认是50M，当硬盘空间少于50m时将阻塞生产者，建议生产环境至少10G（通常至少1.1倍内存）
	3、When running RabbitMQ in a cluster, the memory and disk alarms are cluster-wide; if one node goes over the limit then all nodes will block connections.
	4、


内存硬盘rabbitmq.config配置项
vm_memory_high_watermark.relative = 0.55
disk_free_limit.absolute = 8GB

配置文件文档
http://www.rabbitmq.com/configure.html#configuration-file


*:代表一个单词
#:代表0或者多个单词
































		









































































