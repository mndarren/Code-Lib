### Design Patterns
===================
1. Singleton
   ```
   1) Private constructor to restrict instantiation of the class from other classes.
   2) Private static variable of the same class that is the only instance of the class.
   3) Public static method that returns the instance of the class, this is the global  
      access point for outer world to get the instance of the singleton class.
   4) Used for logging, driver objects, caching and thread pool.
      Used in other design patterns like Abstract Factory, Builder, Prototype, Facade etc.
      Used in core java classes also. for example java.lang.Runtime, java.awt.Desktop.
   ```