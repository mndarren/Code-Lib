package abstractFactory;

public class ServerFactory implements ComputerAbstractFactory{
	private String ram;
	private String hdd;
	private String cpu;
	
	public ServerFactory(String ram, String hdd, String cpu){
		this.ram=ram;
		this.hdd=hdd;
		this.cpu=cpu;
	}

	public ServerFactory(Builder builder) {
		this.ram=builder.ram;
		this.hdd=builder.hdd;
		this.cpu=builder.cpu;
	}

	@Override
	public Computer createComputer() {
		return new Server(ram,hdd,cpu);
	}
	
	public static class Builder{
		private String ram;
		private String hdd;
		private String cpu;
		
		public Builder ram(String ram){this.ram=ram; return this;}
		public Builder hdd(String hdd){this.hdd=hdd; return this;}
		public Builder cpu(String cpu){this.cpu=cpu; return this;}
		public ServerFactory build(){return new ServerFactory(this);};
	}

}
