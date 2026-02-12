using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataJSONEndStage //0
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public string timestamp;
}

[Serializable]
public class DataJSONStartStage //1 - start stage
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataStage context;
	public string timestamp;
}

[Serializable]
public class DataJSONOptions //2 - many choices and more than one answer
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataOptions context;
	public string timestamp;
}

[Serializable]
public class DataJSONOptionChose //3 - MCQ, select and deselect
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ResultData result;
	public string timestamp;
}

[Serializable]
public class DataJSONSBAR //4
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataSBAR context;
	public string timestamp;
}

[Serializable]
public class DataJSONSBARChose //5
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataSBARChose context;
	public ResultData result;
	public string timestamp;
}

[Serializable]
public class DataJSONSBARSubmit //6 - Submit MCQ and SBAR
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataSBARSubmit context;
	public ResultData result;
	public string timestamp;
}

[Serializable]
public class DataJSONMessage //7 - message prompt, currently not working!!
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ResultMessage result;
	public string timestamp;
}

[Serializable]
public class DataJSONOptionAlternate //8 - many choices and one answer
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataOptionsAlternate context;
	public string timestamp;
}

[Serializable]
public class DataJSONSBARReset //9
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ContextDataSBARSubmit context;
	public string timestamp;
}

[Serializable]
public class DataJSONOptionChoseNoResult //10 - only one choice since initialized
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public string timestamp;
}

[Serializable]
public class DataJSONScore //11 - only one choice since initialized
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ResultScore resultScore;
	public string timestamp;
}

[Serializable]
public class DataJSONRaw //12 - Stage score
{
	public ActorData actor;
	public VerbData verb;
	public ObjectData objectData;
	public ResultRaw result;
	public string timestamp;
}


#region ACTOR

[Serializable]
public class ActorData
{
	public string name;
	public string mbox;
}

#endregion

#region VERB

[Serializable]
public class VerbData
{
	public string id;
}

#endregion

#region OBJECT

[Serializable]
public class ObjectData
{
	public string id;
	public Definition definition;
}

[Serializable]
public class Definition
{
	public string type;
}

#endregion

#region CONTEXTS

[Serializable]
public class ContextDataStage
{
	public ExtensionContextStage extensions;
}

[Serializable]
public class ContextDataOptions
{
	public ExtensionContextOptions extensions;
}

[Serializable]
public class ContextDataOptionsAlternate
{
	public ExtensionContextOptionsAlternate extensions;
}

[Serializable]
public class ContextDataSBAR
{
	public ExtensionContextSBAR extensions;
}

[Serializable]
public class ContextDataSBARChose
{
	public ExtensionContextSBARChose extensions;
}

[Serializable]
public class ContextDataSBARSubmit
{
	public ExtensionContextSBARSubmit extensions;
}

#endregion

#region RESULT

[Serializable]
public class ResultData
{
	public bool success;
}

[Serializable]
public class ResultMessage
{
	public ExtensionResultMessage Extensions;
}

[Serializable]
public class ResultRaw
{
	public bool success;
	public ExtensionResultRaw extensions;
}
[Serializable]
public class ResultScore
{
	public ScoreData score;
	public bool completion;
}

#endregion

#region EXTENSIONS

[Serializable]
public class ExtensionContextStage
{
	//for CONTEXT starting stage only
	public string game_title;
	public string game_id;
	public string game_version;
}

[Serializable]
public class ExtensionContextOptions
{
	//for CONTEXT anything else
	public string[] options;
	public string[] correct_option;
}

[Serializable]
public class ExtensionContextOptionsAlternate
{
	//for CONTEXT anything else (many to one)
	public string[] options;
	public string correct_option;
}

[Serializable]
public class ExtensionContextSBAR
{
	//for CONTEXT SBAR
	public string[] fieldSBAR;
	public string[] options;
	public ExtensionCorrect_OptionSBAR[] correct_option;
}

[Serializable]
public class ExtensionCorrect_OptionSBAR
{
	public string field;
	public string option;
}

[Serializable]
public class ExtensionContextSBARChose
{
	//for CONTEXT SBAR when choosing
	public string fieldSBAR;
}

[Serializable]
public class ExtensionContextSBARSubmit
{
	//for CONTEXT SBAR when submitting
	public string[] options;
}

[Serializable]
public class ExtensionResultMessage
{
	public string message;
}

[Serializable]
public class V2
{
	public int[] v = new int[2] {5,6};
}


[Serializable]
public class ExtensionResultRaw
{
	public string raw_data;
	//	public V2 [] raw_data;  
//	public [] int intA1 = new int[] {1,2,3,4,5};
//	List<> listA1 = new List<int>()
//	public int [][] raw_data = new int [2][] {new []{0,1},new []{2,3}};
}

[Serializable]
public class ScoreData
{
	public int raw;
	public int min;
	public int max;
}

#endregion