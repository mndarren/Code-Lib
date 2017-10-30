package precipitationFrequencyGet;
import java.io.IOException;
import java.net.MalformedURLException;
import java.util.StringTokenizer;

import org.junit.Assert;

import com.gargoylesoftware.htmlunit.FailingHttpStatusCodeException;
import com.gargoylesoftware.htmlunit.WebClient;
import com.gargoylesoftware.htmlunit.html.HtmlElement;
import com.gargoylesoftware.htmlunit.html.HtmlPage;
import com.gargoylesoftware.htmlunit.html.HtmlSubmitInput;
/**
 * @purpose this class is to get the precipitation frequency info of specific location 
 *          as Rainfall input for the whole software.
 * @author Zhao Xie
 * @date 12/16/2015
 * @fileName PFManager.java
 */
public class PFManager {
	
	private String latitude;
	private String longitude;
	private String duration;
	private String interval;
	private String resultValue;
	
	public PFManager(Builder builder) throws Exception{
		latitude = builder.latitude;
	    longitude = builder.longitude;
	    duration = builder.duration;
	    interval = builder.interval;
	    calResultValue();
	}
	
	public String getLatitude(){return latitude;}
	public String getLlongitude(){return longitude;}
	public String getDuration(){return duration;}
	public String getInterval(){return interval;}
	public String getResultValue(){return resultValue;}
	
	private void calResultValue() throws Exception{ /*calculate the resultVaue*/
		final WebClient webClient = new WebClient();
		String id = GUI.Utility.DURATION.get(duration) + "-" + GUI.Utility.INTERVAL.get(interval);
		try {
            final HtmlPage page = webClient.getPage("http://hdsc.nws.noaa.gov/hdsc/pfds/pfds_map_cont.html?bkmrk=mn");

            /* set latitude and longitude in web site */
            final HtmlElement inputManLat = page.getElementByName("manLat");
            inputManLat.click();
            inputManLat.type(latitude);
            final HtmlElement inputManLon = page.getElementByName("manLon");
            inputManLon.click();
            inputManLon.type(longitude);
            /*submit the input and get a new web page*/
            final HtmlSubmitInput button = (HtmlSubmitInput)page.getElementsByTagName("input").get(7);
            Assert.assertTrue(button.asText().contains("submit"));
            final HtmlPage page2 = button.click();
            /* we have to wait about 5 seconds for the response of the web site server */
            Thread.sleep(5000);
            String temp = page2.getElementById(id).asText();
            StringTokenizer st = new StringTokenizer(temp);
            resultValue = st.nextToken();
		} catch (final FailingHttpStatusCodeException e) {
            e.printStackTrace();
        } catch (final MalformedURLException e) {
            e.printStackTrace();
        } catch (final IOException e) {
            e.printStackTrace();
        } catch (final Exception e) {
            e.printStackTrace();
        }
	}
	//Builder class
	public static class Builder{
		private final String latitude; //required
		private final String longitude;//required
		private String duration = null;//optional
		private String interval = null;//optional
		
		public Builder(String lat,String lon){
			latitude = lat;
			longitude = lon;
		}
		public Builder duration(String d){duration = d; return this;}
		public Builder interval(String in){interval = in; return this;}
		public PFManager build() throws Exception{return new PFManager(this);}
	}
	
	public String toString(){
		return "latitude = "+latitude + ", longitude = "+longitude + ", duration = "+
	           duration + ", interval = "+interval + ", value = "+resultValue;
	}
}