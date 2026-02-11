namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class MCQGeneral : MonoBehaviour //IAction
	{
		public Stage2Handler handler2;
		public Stage3Handler handler3;
		public Stage4Handler handler4;
		public GameObject overlayButton;
		public bool chooseRight;
		public bool alreadyChosen;
		public int stageMCQ;

		[Header("For sending API")]
		public int _typeAPI = 3;
		public ActorData _actor;
		public VerbData _verb;
		public ObjectData _objectData;
		public ResultData _resultData;
		public string _SBAROption;

		public bool InstantExecute(GameObject target)//, IAction[] actions, int index)
		{
			switch (stageMCQ)
			{
				case 2:
					if (!alreadyChosen)
					{
						if (handler2.itemChosenMCQ != 1)
						{
							handler2.API_SBARoption[handler2.API_SBARchosen] = _SBAROption;
							handler2.API_SBARchosen++;

							handler2.apiHandler.resultData.success = true;
							SendAPI_SBAR2();

							handler2.itemChosenMCQ++;
							handler2.itemChosenMCQRight = chooseRight;

							handler2.SFX.clip = handler2.audioUI;
							handler2.SFX.Play();

							overlayButton.SetActive(true);
							alreadyChosen = true;
						}

					}
					else
					{
						handler2.itemChosenMCQRight = false;
						handler2.API_SBARchosen--;
						handler2.API_SBARoption[handler2.API_SBARchosen] = _SBAROption;

						handler2.apiHandler.resultData.success = false;
						SendAPI_SBAR2();

						handler2.itemChosenMCQ--;

						handler2.SFX.clip = handler2.audioUI;
						handler2.SFX.Play();

						overlayButton.SetActive(false);
						alreadyChosen = false;
					}

					break;

				case 3:
					if (!alreadyChosen)
					{
						if (handler3.itemChosenMCQ != 1)
						{
							handler3.API_SBARoption[handler3.API_SBARchosen] = _SBAROption;
							handler3.API_SBARchosen++;

							handler3.apiHandler.resultData.success = true;
							SendAPI_SBAR3();

							handler3.itemChosenMCQ++;
							handler3.itemChosenMCQRight = chooseRight;

							handler3.SFX.clip = handler3.audioUI;
							handler3.SFX.Play();

							overlayButton.SetActive(true);
							alreadyChosen = true;
						}

					}
					else
					{
						handler3.API_SBARchosen--;
						handler3.API_SBARoption[handler3.API_SBARchosen] = _SBAROption;

						handler3.apiHandler.resultData.success = false;
						SendAPI_SBAR3();

						handler3.itemChosenMCQ--;

						handler3.SFX.clip = handler3.audioUI;
						handler3.SFX.Play();

						overlayButton.SetActive(false);
						alreadyChosen = false;
					}

					break;
			}
			return true;
		}
		
		void SendAPI_SBAR2()
		{
			handler2.apiHandler.typeAPI = _typeAPI;
			handler2.apiHandler.actor = _actor;
			handler2.apiHandler.verb = _verb;
			handler2.apiHandler.objectData = _objectData;

			handler2.apiHandler.SendAPI();
		}

		void SendAPI_SBAR3()
		{
			handler3.apiHandler.typeAPI = _typeAPI;
			handler3.apiHandler.actor = _actor;
			handler3.apiHandler.verb = _verb;
			handler3.apiHandler.objectData = _objectData;

			handler3.apiHandler.SendAPI();
		}

#if UNITY_EDITOR
		public static new string NAME = "Custom/MCQGeneral";
		#endif
	}
}
