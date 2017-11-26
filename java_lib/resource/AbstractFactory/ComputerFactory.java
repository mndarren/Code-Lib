package abstractFactory;
//using enum Singleton for ComputerFactory class here.
//if needing multiple factory, don't use singleton.
public enum ComputerFactory {
	INSTANCE;
	
	public Computer getComputer(ComputerAbstractFactory factory){
		return factory.createComputer();
	}

}
