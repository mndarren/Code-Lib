# Mongo DB Lib
=========<br>
1. Mongo DB Basic
```
Created by C++. Performance-Oriented DB, Document oriented DB
High performance, high availability, easy scalability
No relationship for MongoDB
```
2. Concepts
```
Database - Collection - Document - Field - Embedded Documents - _id         - mongod - mongo
Database - Table      - Row/Tuple- column- Table Join         - Primary Key - mysqld - mysql
```
3. Start MongoDB
```
mongod.exe --dbpath "C:\data"
mongo.exe
mongo.help()
mongo.stats()
```
4. Commands
```
# Create DB
use buzzi_db

# Drop DB
db.dropDatabase()

# Show current db, collections
db
show collections

# Insert Update Delete document
db.movie.insert({"name":"tutorials point"}) # deprecated
db.movie.insertOne({"name":"tutorials point"})
db.movie.insertMany({"name":"tutorials point"})
db.movie.save({"name":"tutorials point"}) # with Id, save() will do update
# updateOne() updateMany() deleteOne() deleteMany() findOneAndDelete()
# findOneAndUpdate() findOneAndReplace()

# Create Collection (auto create collection)
db.createCollection("mycollection")
db.createCollection("mycol", { capped : true, autoIndexID : true, size : 6142800, max : 10000 } )
# insert a document will automatically create collection
db.mycol.insert({"name": "Darren Xie"})

# Drop collection
db.mycol.drop()

# auto Id
_id: ObjectId(4 bytes timestamp, 3 bytes machine id, 2 bytes process id, 3 bytes incrementer)

# Query # $lt, $lte, $gt, $gte, $ne, $in $nin, $and $or $not
db.movie.find().pretty()
db.movie.findOne({Teacher:"Hu"})
db.class_schedule.findOne({Period: {$gt:2}})
db.class_schedule.find({Period: {$gte:2}}).pretty()
db.class_schedule.find({Period: {$in:[0,2]}}).pretty()
db.class_schedule.find({$or: [{"Teacher":"Hu"},{"Period":3}]}).pretty()
db.class_schedule.find({$and: [{"Teacher":"Hu"},{"Period":3}]}).pretty()
db.class_schedule.find({$nor: [{"Teacher":"Hu"},{"Period":3}]}).pretty()
db.class_schedule.find({$not: {"Teacher":"Hu"}]}).pretty()
db.mycol.find({"likes": {$gt:10}, $or: [{"by": "tutorials point"},{"title": "MongoDB Overview"}]}).pretty()
db.empDetails.find( { "Age": { $not: { $gt: "25" } } } )

# Update
db.mycol.update({'title':'MongoDB Overview'},{$set:{'title':'New MongoDB Tutorial'}})
db.empDetails.updateOne(
	{First_Name: 'Radhika'},
	{ $set: { Age: '30',e_mail: 'radhika_newemail@gmail.com'}}
)
db.empDetails.updateMany(
	{Age:{ $gt: "25" }},
	{ $set: { Age: '00'}}
)
db.mycol.save(
   {
      "_id" : ObjectId("507f191e810c19729de860ea"), 
		"title":"Tutorials Point New Topic",
      "by":"Tutorials Point"
   }
)
db.empDetails.findOneAndUpdate(
	{First_Name: 'Radhika'},
	{ $set: { Age: '30',e_mail: 'radhika_newemail@gmail.com'}}
)

# Delete
db.mycol.remove({}) # remove all
db.mycol.remove({'title':'MongoDB Overview'})

# Functions
db.mycol.find({},{"title":1,_id:0}) # 1 == display, 0 == hide
db.mycol.find({},{"title":1,_id:0}).limit(2)
db.mycol.find({},{"title":1,_id:0}).limit(1).skip(1)
db.mycol.find({},{"title":1,_id:0}).sort({"title":-1})
db.mycol.createIndex({"title":1})
db.mycol.createIndex({"title":1,"description":-1}) # 1 ascending, -1 descending
db.mycol.dropIndex({"title":1})
db.mycol.dropIndexes({"title":1,"description":-1})
db.mycol.getIndexes()

# Statistics $avg, $min, $max, $push, $addToSet, $first, $last
db.mycol.aggregate([{$group : {_id : "$by_user", num_tutorial : {$sum : 1}}}])

# Pipeline Concept
To execute an operation on some input and use the output as the input for the next command
$project − Used to select some specific fields from a collection.
$match − This is a filtering operation and thus this can reduce the amount of documents that are given as input to the next stage.
$group − This does the actual aggregation as discussed above.
$sort − Sorts the documents.
$skip − With this, it is possible to skip forward in the list of documents for a given amount of documents.
$limit − This limits the amount of documents to look at, by the given number starting from the current positions.
$unwind − This is used to unwind document that are using arrays.
```