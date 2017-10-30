package precipitationFrequencyGet;


public class PFGetTest {
	public static void main(final String[] args) throws Exception{
		String fp = "e:/rainfallInput.txt";
		PFGet.INSTANCE.setFilePath(fp);
		PFGet.INSTANCE.processInput();
		
		String realValue = PFGet.INSTANCE.getResult();
		System.out.println("the real value = "+realValue);
		/*PFGet pfGet = PFGet.getInstance();
		String fp = "e:/rainfallInput.txt";
		pfGet.setFilePath(fp);
		pfGet.processInput();
		
		String realValue = pfGet.getResult();
		System.out.println("the real value = "+realValue);*/
    }
}
