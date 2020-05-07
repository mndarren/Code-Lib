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

# Insert document
db.movie.insert({"name":"tutorials point"})

# Create Collection (auto create collection)
db.createCollection("mycollection")
db.createCollection("mycol", { capped : true, autoIndexID : true, size : 6142800, max : 10000 } )
# insert a document will automatically create collection
db.mycol.insert({"name": "Darren Xie"})


```