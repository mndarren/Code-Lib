package abstractFactory;

public enum ComputerFactory {
	INSTANCE;
	
	public Computer getComputer(ComputerAbstractFactory factory){
		return factory.createComputer();
	}

}
