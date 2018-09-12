nuget:MongoDB.Driver
官网: 
	http://mongodb.github.io/mongo-csharp-driver/
	https://docs.mongodb.com/manual/introduction/


	MongoDB Compass是MongoDB的可视化管理工具


MongoDB是开源的文档数据库，提供高性能、高可用和 automatic scaling.

A record in mongodb is a document, which is a data structure composed of field and value pair.The values of fields may include other documents, arrays, and arrays of documents.


MongoDB使用BSON document作为记录（相当于表项），存储在Collections(相当于表)中，Collections存在于database中

默认collection中的document不需要有相同的schema， the documents in a single collection do not need to have the same set of fields and the data type for a field can differ across documents within a collection.

MongoDB documents are composed of field-and-value pairs and have the following structure:


{
   field1: value1,
   field2: value2,
   field3: value3,
   ...
   fieldN: valueN
}
The value of a field can be any of the BSON data types, including other documents, arrays, and arrays of documents.

Document有如下限制:
	field name _id 保留用来做主键，必须唯一，不可修改，may be of any type other than an array.
	field name不能包含$字符
	field name不能包含.字符
	field name不能包含null字符

单个Document不可超过16mb
每个Document都要有一个_id字段，表示主键，唯一不可变更，_id应该是document的第一个字段。


CRUD  CRUD列举的函数都是原子的
	Create:add a new document to a collection, if collection not exist,create collection automatically.
			db.collection.insertOne()
			db.collection.insertMany()  原子的，要么全部成功，要么全部失败

	Read:retrieves documents from a collection.
			db.collection.find()

	Update:update existing document(s) in a collection
			db.collection.updateOne()
			db.collection.updateMany() 原子的，要么全部成功，要么全部失败
			db.collection.replaceOne()

	Delete:delete documents from a collection
			db.collection.deleteOne()
			db.collection.deleteMany()  原子的，要么全部成功，要么全部失败
















