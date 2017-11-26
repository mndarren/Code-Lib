package prototype;

import java.util.List;

/**
 * @benefit: If the object cloning was not provided, every time we need to make database
 *           call to fetch the employee list and then do the manipulations that would have
 *           been resource and time consuming.
 * @author Darren
 *
 */
public class TestPrototype {

	public static void main(String[] args) throws CloneNotSupportedException{

		Employees emps = new Employees();
		emps.loadData();
		
		//Use the clone method to get the Employee object
		Employees empsNew = (Employees) emps.clone();
		Employees empsNew1 = (Employees) emps.clone();
		List<String> list = empsNew.getEmpList();
		list.add("John");
		List<String> list1 = empsNew1.getEmpList();
		list1.remove("Pankaj");
		System.out.println("emps List: "+emps.getEmpList());
		System.out.println("empsNew List: "+list);
		System.out.println("empsNew1 List: "+list1);

	}

}
