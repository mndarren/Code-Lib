package abstractFactory;

public class TestAbstractFactory {
	public static void main(String[] args){
		testAbstractFactory();
	}

	private static void testAbstractFactory() {
		Computer pc = ComputerFactory.INSTANCE.getComputer(new PCFactory.Builder()
		                                                                .ram("2 GB")
																		.hdd("500 GB")
																		.cpu("2.4 GHz")
																		.build());
		Computer server = ComputerFactory.INSTANCE.getComputer(new ServerFactory.Builder()
		                                                                        .ram("16 GB")
																				.hdd("1 TB")
																				.cpu("294 GHz")
																				.build());
		System.out.println("AbstractFactory PC Config: " + pc);
		System.out.println("AbstractFactory Server Config: " + server);
	}

}
