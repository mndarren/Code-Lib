# Grails
=======================================
1. install grails
```
curl -s https://get.sdkman.io | bash
source "$HOME/.sdkman/bin/sdkman-init.sh"
sdk install grails
grails -version
```
2. create grails app
```
grails create-app com.vogella.grails.guestbook  # @ grails-app/conf/application.yml/ to speccify a default package
grails list-profiles  # list all profiles
grails create-app myApp -profile rest-api
grails create-app myApp -profile org.grails.profiles:react:1.0.2
grails profile-info plugin  # get detaied info about a profile
```
3. run grails app
```
./grailsw run-app  # without installed grails
grails run-app   # which installed grails
http://localhost:8080/com.vogella.grails.guestbook
# run app by interactive mode
./grailsw
run-app
# run app with Gradle (Grails built upon Spring Boot and Gradle)
./gradlew bootRun
http://localhost:8080  # check it out
# run app with env vars and environment type
POSTGRES_PASSWORD=postgresql POSTGRES_USERNAME=postgres  grails test run-app
```
4. change default port
```
# add the following in grails-app/conf/application.yml
server:
	port: 8090
# then run app with port
./grailsw run-app --port=8090
http://localhost:8090  # check it out
```
5. auto reloading
```
# in grails-app/views/application/index.gson

# modify something will auto reloading
```
6. MVC (Convention over Configuration)
```
M: domain model defined with Groovy classes under grails-app/domain
```
7. create a domain class (GORM)
```
./grailsw create-domain-class Vehicle
class Vehicle {

    String name 

    String make
    String model

    static constraints = { # 
        name maxSize: 255
        make inList: ['Ford', 'Chevrolet', 'Nissan']
        model nullable: true
    }
}
# Hibernate will be used to configure a datasource (by default an in-memory)\
http://localhost:8080/dbconsole
JDBC URL is: jdbc:h2:mem:devDb;MVCC=TRUE;LOCK_TIMEOUT=10000;DB_CLOSE_ON_EXIT=FALSE
# can be found in application.yml under environments development dataSource url
# update Domains
package org.grails.guides

@SuppressWarnings('GrailsDomainReservedSqlKeywordName')
class Vehicle {

    Integer year

    String name
    Model model
    Make make

    static constraints = {
        year min: 1900
        name maxSize: 255
    }
}

package org.grails.guides

class Model {

    String name

    static belongsTo = [ make: Make ]

    static constraints = {
    }

    String toString() {
        name
    }
}

package org.grails.guides

class Make {

    String name

    static constraints = {
    }

    String toString() {
        name
    }
}
```
8. Bootstrapping Data (Preload data in DB)
```
package org.grails.guides

class BootStrap {

    def init = { servletContext ->

        def nissan = new Make(name: 'Nissan').save()
        def ford = new Make(name: 'Ford').save()

        def titan = new Model(name: 'Titan', make: nissan).save()
        def leaf = new Model(name: 'Leaf', make: nissan).save()
        def windstar = new Model(name: 'Windstar', make: ford).save()

        new Vehicle(name: 'Pickup',  make: nissan, model: titan, year: 2012).save()
        new Vehicle(name: 'Economy', make: nissan, model: leaf, year: 2014).save()
        new Vehicle(name: 'Minivan', make: ford, model: windstar, year: 1990).save()
    }
    def destroy = {
    }
}
```
9. Configure MySQL (change default H2 which recreated every time)
```
# in build.gradle
dependencies {
    //...

    runtime 'mysql:mysql-connector-java:5.1.40'
}
# application.yml
dataSource:
    pooled: true
    jmxExport: true
    driverClassName: com.mysql.jdbc.Driver   
    dialect: org.hibernate.dialect.MySQL5InnoDBDialect
    username: sa
    password: testing
environments:
    development:
        dataSource:
            dbCreate: update
            url: jdbc:mysql://127.0.0.1:3306/myapp
```
10. Controllers
```
./grailsw create-controller org.grails.guides.Home  # Grails adds the *Controller suffix automatically.
# Actions are public methods in a controller, which can respond to requests.
# create a view in views/home/index.gsp   # gsp ~ html
1) URL Mapping
package org.grails.guides

class UrlMappings {

    static mappings = {
        "/$controller/$action?/$id?(.$format)?"{  
            constraints {
                // apply constraints here
            }
        }

        "/"(view:"/index")   
        "500"(view:'/error')
        "404"(view:'/notFound')
    }
}
2) Scaffolding
# dynamic scaffolding (VehicleController.groovy, the same to others)
package org.grails.guides

class VehicleController {
    static scaffold = Vehicle
}
# With the scaffold property set, Grails will now generate all necessary CRUD (Create, Read, Update, Delete) actions for the respective domain classes
# Static Scaffolding
./grailsw generate-views Vehicle
./grailsw generate-controller Vehicle
./grailsw generate-all Vehicle             # generate controller and view
./grailsw generate-all Vehicle -force      # override existing files
```