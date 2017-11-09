package abstractFactory;

public class PCFactory implements ComputerAbstractFactory{
	private String ram;
	private String hdd;
	private String cpu;
	
	public PCFactory(String ram, String hdd, String cpu){
		
		}

	public PCFactory(Builder builder) {
		this.ram=builder.ram;
		this.hdd=builder.hdd;
		this.cpu=builder.cpu;
	}

	@Override
	public Computer createComputer() {
		return new PC(ram,hdd,cpu);
	}
	
	public static class Builder{
		private String ram;
		private String hdd;
		private String cpu;
		
		public Builder ram(String ram){this.ram = ram;return this;}
		public Builder hdd(String hdd){this.hdd = hdd;return this;}
		public Builder cpu(String cpu){this.cpu = cpu;return this;}
		public PCFactory build(){return new PCFactory(this);}
	}

}
