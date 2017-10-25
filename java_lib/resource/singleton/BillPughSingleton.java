package singleton;
/**
 * thread-safe
 * no synchronized cost
 * lazy initialization
 * @author Bill Pugh
 */
public class BillPughSingleton {
	//other variables
	private BillPughSingleton(){};
	private static class SingletonHelper{
		private static final BillPughSingleton INSTANCE = new BillPughSingleton();
	}
	public static BillPughSingleton getInstance(){
		return SingletonHelper.INSTANCE;
	}
	//other methods

}
