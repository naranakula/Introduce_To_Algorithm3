种种迹象表明RabbitMq优于ActiveMq（nuget安装量,官方文档，网络文章等）

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














































































