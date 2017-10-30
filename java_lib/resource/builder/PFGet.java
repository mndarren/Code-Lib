package precipitationFrequencyGet;

import java.io.BufferedReader;

import java.io.File;
import java.io.FileReader;
import java.util.StringTokenizer;

import javax.swing.JOptionPane;

/**
 * @purpose this class is to get the precipitation frequency info of specific location 
 *          as Rainfall input for the whole software.
 * @author Zhao Xie
 * @date 12/16/2015
 * @fileName PFGet.java
 */
public enum PFGet {
	INSTANCE;
	private PFManager pfManager;
	private String filePath;
	
	private PFGet(){
		filePath = GUI.Utility.defaultRainFilePath;
	}
	
	public void setFilePath(String fp){
		filePath = fp;
	}
	public String getResult(){
		return pfManager.getResultValue();
	}
	public void processInput() throws Exception{
		String line=null;
		try {
			File file = new File(filePath);
			BufferedReader br = new BufferedReader(new FileReader(file));
			line = br.readLine();
			br.close();
		} catch (Exception e) {
			JOptionPane.showMessageDialog(null, "Rainfall input file Error!");
			e.printStackTrace();
		}
		StringTokenizer st = new StringTokenizer(line.toString());
		String[] param = new String[4];
		for(int i=0;i<4;i++){
			param[i] = st.nextToken();
		}
		GUI.Utility.inputCheck(param[0],param[1],param[2],param[3]);
		pfManager = new PFManager.Builder(param[0],param[1]).duration(param[2]).
			      interval(param[3]).build(); 
	}
	public String toString(){
		return pfManager.toString();
	}
}
