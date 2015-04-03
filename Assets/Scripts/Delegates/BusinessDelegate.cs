using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MiniJSON;

public class BusinessDelegate{

	private BusinessLookUp lookupService = new BusinessLookUp();
	private BusinessService businessService;
	private string serviceType;
	private UserDTO user;
	private ResultDTO result;
	private GameObject context;
	
	public void setServiceType(string serviceType, UserDTO user, ResultDTO result, GameObject context){
		this.serviceType = serviceType;
		this.user = user;
		this.result = result;
		this.context = context;
	}
	
	public IEnumerator doTask(){
		businessService = lookupService.getBusinessService(serviceType, context,user,result);
		return businessService.doProcessing();		
	}
}
