### Design Patterns
===================  
0. design patterns with 23 types from Gang of Four
   ```
   Creational               Structural           Behavioral
   +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
   Abstract Factory         Adapter              Chain of Responsibility
   Builder                  Bridge               Command
   Factory Method           Composite            Interpreter
   Prototype                Decorator            Iterator
   Singleton                Facade               Mediator
                            Flyweight            Observer
                            Proxy                State
                                                 Strategy
                                                 Template Method
                                                 Visitor
   ```
1. Singleton
   ```
   1) Private constructor to restrict instantiation of the class from other classes.
   2) Private static variable of the same class that is the only instance of the class.
   3) Public static method that returns the instance of the class, this is the global  
      access point for outer world to get the instance of the singleton class.
   4) Used for logging, driver objects, caching and thread pool.
      Used in other design patterns like Abstract Factory, Builder, Prototype, Facade etc.
      Used in core java classes also. for example java.lang.Runtime, java.awt.Desktop.
   5) Introduced 2 best versions (Bill Pugh, enum)
   ```
   [Bill Pugh Singleton](https://github.com/mndarren/Code-Lib/blob/master/java_lib/resource/singleton/BillPughSingleton.java)  
   [enum Singleton](https://github.com/mndarren/Code-Lib/blob/master/java_lib/resource/singleton/EnumSingleton.java)
2. Factory Method
   ```
   1) Used when we have a super class with multiple subclasses and based on input, we need to return one of the sub-class.
   2) Benefit: Removed instantiation of actual implementation classes from client code, making it more robust, less coupled and easy to extend.
   3) Example in JDK: java.util.Calendar, ResourceBundle, NumberFormat
   ```
   [Computer Simulation](https://github.com/mndarren/Code-Lib/tree/master/java_lib/resource/factory)
3. Builder
   ```
   ```