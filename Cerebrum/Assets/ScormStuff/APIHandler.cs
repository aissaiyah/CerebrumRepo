using Scorm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class APIHandler : MonoBehaviour
{
	IScormService scormService;
	public ScormHandler scormHandler;


	string nameID;
	string learnerID;

	//string url = "https://ll.gamestrax.com/data/xAPI/statements";
	string url = "https://ll.sg-tap.com/data/xAPI/statements";

	string jsonData;

	#region DataJSON
	[HideInInspector]
	public DataJSONEndStage dataListEndStage;
	[HideInInspector]
	public DataJSONStartStage dataListStartStage;
	[HideInInspector]
	public DataJSONOptions dataListOptions;
	[HideInInspector]
	public DataJSONOptionChose dataListOptionChose;
	[HideInInspector]
	public DataJSONSBAR dataListSBAR;
	[HideInInspector]
	public DataJSONSBARChose dataListSBARChose;
	[HideInInspector]
	public DataJSONSBARSubmit dataListSBARSubmit;
	[HideInInspector]
	public DataJSONSBARReset dataListSBARReset;
	[HideInInspector]
	public DataJSONMessage dataListMessage;
	[HideInInspector]
	public DataJSONOptionAlternate dataListOptionsAlternate;
	[HideInInspector]
	public DataJSONOptionChoseNoResult dataListOptionChoseNoResult;
	[HideInInspector]
	public DataJSONScore dataListScore;

	[HideInInspector]
	public DataJSONRaw dataListRaw;
	#endregion

	[Header("For API format")]
	[Header("8 for option appear (many to one), 9 for SBARreset, 10 for (3noresult)")]
	[Header("4 for SBAR, 5 for SBARchose, 6 for SBARsubmit, 7 for message")]
	[Header("1 for starting stage, 2 for options appear (many), 3 for clicking option,")]
	public int typeAPI;

	[Header("API Data")]
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataStage contextStage;
	public ContextDataOptions contextOptions;
	public ContextDataOptionsAlternate contextOptionsAlternate;
	public ContextDataSBAR contextSBAR;
	public ContextDataSBARChose contextSBARChose;
	public ContextDataSBARSubmit contextSBARSubmit;
	public ResultData resultData;
	public ResultMessage resultMessage;
	public ResultScore resultScore;
	public ResultRaw resultRaw;

	void Start()
	{
//		scormHandler = GameObject.Find("Singleton - Result Handler").GetComponent<ScormHandler>();
		scormHandler = GameObject.FindGameObjectWithTag("ScormHandler").GetComponent<ScormHandler>();
		#if UNITY_EDITOR
		scormService = new ScormPlayerPrefsService(); // PlayerPrefs implementation (for editor testing)
		#else
		scormService = new ScormService(); // Real implementation
		#endif

//			bool result = scormService.Initialize(Scorm.Version.Scorm_1_2);

			
		if (!scormHandler.scormstart)
		{
			bool result = scormService.Initialize(Scorm.Version.Scorm_2004);

			if (result)
			{
				scormHandler.scormstart = true;

				nameID = scormService.GetLearnerName();
				learnerID = scormService.GetLearnerId();

				scormHandler.nameID = nameID;
				scormHandler.learnerID = learnerID;

				Debug.Log("Initialization success");
			}
			else
			{
				Debug.Log("Initialization error");
				nameID = "testUser";
				learnerID = "testUserID";
			}
		}
		else
		{
//			nameID = "test@eag.com"; //resultHandler.nameID;
//			learnerID = "test@eag.com"; //resultHandler.learnerID;
			nameID = scormHandler.nameID;
			learnerID = scormHandler.learnerID;
		}
	}

	public void FinishScorm()
	{
		scormService.Finish();
	}

	public void SendAPI()
	{
		StartCoroutine(DoSendAPI());
	}

	IEnumerator DoSendAPI()
	{
		switch (typeAPI)
		{
			case 0:
				dataListEndStage.actor = actor;
				dataListEndStage.verb = verb;
				dataListEndStage.objectData = objectData;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListEndStage, true);

				break;
			case 1:
				dataListStartStage.actor = actor;
				dataListStartStage.verb = verb;
				dataListStartStage.objectData = objectData;
				dataListStartStage.context = contextStage;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListStartStage, true);

				break;
			case 2:
				dataListOptions.actor = actor;
				dataListOptions.verb = verb;
				dataListOptions.objectData = objectData;
				dataListOptions.context = contextOptions;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListOptions, true);

				break;
			case 3:
				dataListOptionChose.actor = actor;
				dataListOptionChose.verb = verb;
				dataListOptionChose.objectData = objectData;
				dataListOptionChose.result = resultData;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListOptionChose, true);

				break;
			case 4:
				dataListSBAR.actor = actor;
				dataListSBAR.verb = verb;
				dataListSBAR.objectData = objectData;
				dataListSBAR.context = contextSBAR;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListSBAR, true);

				break;
			case 5:
				dataListSBARChose.actor = actor;
				dataListSBARChose.verb = verb;
				dataListSBARChose.objectData = objectData;
				dataListSBARChose.context = contextSBARChose;
				dataListSBARChose.result = resultData;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListSBARChose, true);
				break;
			case 6:
				dataListSBARSubmit.actor = actor;
				dataListSBARSubmit.verb = verb;
				dataListSBARSubmit.objectData = objectData;
				dataListSBARSubmit.context = contextSBARSubmit;
				dataListSBARSubmit.result = resultData;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListSBARSubmit, true);
				break;
			case 7:
				dataListMessage.actor = actor;
				dataListMessage.verb = verb;
				dataListMessage.objectData = objectData;
				dataListMessage.result = resultMessage;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListMessage, true);
				break;
			case 8:
				dataListOptionsAlternate.actor = actor;
				dataListOptionsAlternate.verb = verb;
				dataListOptionsAlternate.objectData = objectData;
				dataListOptionsAlternate.context = contextOptionsAlternate;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListOptionsAlternate, true);

				break;
			case 9:
				dataListSBARReset.actor = actor;
				dataListSBARReset.verb = verb;
				dataListSBARReset.objectData = objectData;
				dataListSBARReset.context = contextSBARSubmit;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListSBARReset, true);
				break;
			case 10:
				dataListOptionChoseNoResult.actor = actor;
				dataListOptionChoseNoResult.verb = verb;
				dataListOptionChoseNoResult.objectData = objectData;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListOptionChoseNoResult, true);

				break;
			case 11:
				dataListScore.actor = actor;
				dataListScore.verb = verb;
				dataListScore.objectData = objectData;
				dataListScore.resultScore = resultScore;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListScore, true);

				break;
			case 12:
				dataListRaw.actor = actor;
				dataListRaw.verb = verb;
				dataListRaw.objectData = objectData;
				dataListRaw.result = resultRaw;

				GetTimestamp();

				jsonData = JsonUtility.ToJson(dataListRaw, true);

				break;

		}

		EditJSON(false);

//		yield return null;
		//Send API

		Debug.Log ("URL="+ url + ", JSON = " +jsonData);
		
		using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
		{
			//Set header
			request.method = UnityWebRequest.kHttpVerbPOST;
			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("charset", "utf-8");
			request.SetRequestHeader("X-Experience-API-Version", "1.0.3");
			//request.SetRequestHeader("Authorization", "YmI0YTJiYmYyMWFjNzE4YTI1MWNjM2QzYTdmODhiMjdiZTA3ODBmZDpmZmQ1OTIxZjZiY2M3Mzk5ZTcxMzQzOTQ3NzYyNjY4MTU3MTg4ODM0");
			//request.SetRequestHeader("Authorization", "Basic ZGQ1OGY4ODM1Njk0M2FiZjkyMzMyODhhODMwODFkMWY3NDkwMTg2ZTo0NDI5NzBkNjM3NzFmNmRlNGM2M2UyYzY5MzFhYTU4NGRmNjBmNGZm");
			request.SetRequestHeader("Authorization", "Basic OTdkODYyNjE3OTViN2NmYTc1ZDEzNGVmMGU0YzUzZmRmYjNhYzAxMjowZDYxM2E4MmY5MjZmMDdhNjQyY2EzMjRmZjdiMzA2ZWMzMDZmMmM2");

			yield return request.SendWebRequest();
			if (request != null)
			{
				if (request.isNetworkError || request.isHttpError)
				{
					Debug.Log(request.responseCode + " Authentication Error for "+ jsonData);
				}
				else
				{
					Debug.Log("Log completed for "+ jsonData);
				}
			}
		}
		
		
	}

	void GetTimestamp()
	{
		 TimeZone localZone = TimeZone.CurrentTimeZone;
        DateTime currentDate = DateTime.Now;
      	TimeSpan currentOffset = 
        localZone.GetUtcOffset( currentDate );
 		//print("localZoneOffset = "+ currentOffset);

			TimeZoneInfo infos = TimeZoneInfo.Local;
			var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, infos);
//			string tz = infos.DisplayName;
			string tz = currentOffset.ToString();

//			tz = tz.Substring(4, 6);
			tz = tz.Remove(tz.Length - 3); //remove seconds ":00"
			if (tz[0] != '-'  && tz[0] != '+')
				tz = '+' + tz;

			print("timezone = "+ tz);


			switch (typeAPI)
			{
				case 0:
					dataListEndStage.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 1:
					dataListStartStage.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 2:
					dataListOptions.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 3:
					dataListOptionChose.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 4:
					dataListSBAR.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 5:
					dataListSBARChose.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 6:
					dataListSBARSubmit.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 7:
					dataListMessage.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 8:
					dataListOptionsAlternate.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 9:
					dataListSBARReset.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 10:
					dataListOptionChoseNoResult.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;				
				case 11:
					dataListScore.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
				case 12:
					dataListRaw.timestamp = date.ToString("yyyy-MM-ddTHH:mm:ss").ToString() + tz;
					break;
			}
	}

	void EditJSON(bool array)
	{
			//jsonData = jsonData.Replace("mailto:default@mail.com", "mailto:test@fxmweb.com");
			//jsonData = jsonData.Replace("Default", "test@fxmweb.com");
			if (learnerID!=null && learnerID.Contains("@")){
				jsonData = jsonData.Replace("mailto:default@mail.com", "mailto:"+learnerID);
			}else{
				jsonData = jsonData.Replace("mailto:default@mail.com", "mailto:"+learnerID+"@sg-tap.com");
			}
			jsonData = jsonData.Replace("Default", nameID);

			jsonData = jsonData.Replace("\"QUOTEBEFORE", "");
			jsonData = jsonData.Replace("QUOTEAFTER\"", "");
			jsonData = jsonData.Replace("objectData", "object");
			jsonData = jsonData.Replace("resultScore", "result");

			jsonData = jsonData.Replace("game_title", "http://gamestrax.com/extensions/game_title");
			jsonData = jsonData.Replace("game_id", "http://gamestrax.com/extensions/game_id");
			jsonData = jsonData.Replace("game_version", "http://gamestrax.com/extensions/game_version");
			jsonData = jsonData.Replace("raw_data", "http://gamestrax.com/extensions/raw");

			jsonData = jsonData.Replace("fieldSBAR", "http://gamestrax.com/extensions/field");
			if (typeAPI != 3)
			{
				jsonData = jsonData.Replace("options", "http://gamestrax.com/extensions/options");
			}
			jsonData = jsonData = jsonData.Replace("SBARoption", "options");
			jsonData = jsonData.Replace("correct_option", "http://gamestrax.com/extensions/correct_options");

			jsonData = jsonData.Replace("message", "http://gamestrax.com/extensions/message");

			if (array)
			{
				jsonData = jsonData.Remove(0, 9);
				jsonData = jsonData.Remove(jsonData.Length - 1);
			}
			else
			{
				jsonData = "[" + jsonData + "]";
			}

			print(jsonData);
	}
}
