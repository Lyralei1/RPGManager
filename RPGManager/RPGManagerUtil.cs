using System;
using System.Text;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using System.Collections.Generic;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Objects.RabbitHoles;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Abstracts;
using Sims3.UI.GameEntry;
using Sims3.Gameplay.Core;
using Sims3.SimIFace.CustomContent;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.PetSystems;

//Template Created by Battery

namespace Sims3.Gameplay.Lyralei.RPGManager
{
	public class RPGManagerUtil
	{
		[Tunable] static bool init;
		[Tunable] public static int kHowManyEntriesCanBeShownOnNotification = 10;


		// Tunables for active sims friendships
		[Tunable] public static int kChanceOfGettingBestFriendForever = 10;
		[Tunable] public static int kChanceOfGettingBestFriend = 20;
		[Tunable] public static int kChanceOfGettingGoodFriend = 40;
		[Tunable] public static int kChanceOfGettingOldFriend = 40;
		[Tunable] public static int kChanceOfGettingFriend = 60;
		[Tunable] public static int kChanceOfGettingDistantFriend = 60;


		// Tunables for active sims Relationships
		[Tunable] public static int kChanceOfGettingEx = 25;
		[Tunable] public static int kChanceOfGettingExSpouse = 10;
		[Tunable] public static int kChanceOfGettingFiancee = 10;
		[Tunable] public static int kChanceOfGettingPartner = 40;
		[Tunable] public static int kChanceOfGettingRomanticInterest = 60;
		[Tunable] public static int kChanceOfGettingSpouse = 10;


		// Tunables for active sims Relationships
		[Tunable] public static int kChanceOfGettingDisliked = 60;
		[Tunable] public static int kChanceOfGettingEnemy = 10;
		[Tunable] public static int kChanceOfGettingOldEnemies = 10;


		// Tunables for active sims Known Neighbor
		[Tunable] public static int kChanceOfGettingAcquaintance = 60;
		[Tunable] public static int kChanceOfGettingStranger = 30;

		[Tunable]
		public static int[] kMoneyPerOptionToBeAddedOrSubstracted = new int[] { 500, 1250, 25000, 40000, 100000, 99999999 };

		[PersistableStatic] static bool mHasRunBefore = false;
		[PersistableStatic] static float mLastKnownHouseholdId = 0;
		[PersistableStatic] static int mLastCountBinContent = 0;

		private static EventListener sBoughtObjectLister = null;
		private static EventListener sSubStateListener = null;

		public static Sim[] simsInWorld = null;

		static RPGManagerUtil()
		{
			World.sOnWorldLoadFinishedEventHandler += OnWorldLoaded;
			//World.sOnWorldLoadStartedEventHandler += new EventHandler(OnWorldLoadStarted);
			World.sOnWorldQuitEventHandler += new EventHandler(OnWorldQuit);

		}

		//public static void OnWorldQuit(object sender, EventArgs e)
		//      {
		//	if (InWorldState.InWorldSubStateChanging != null)
		//          {
		//		InWorldState.InWorldSubStateChanging -= OnGameStateChanging;
		//	}
		//	//Responder.Instance.GameStateChanging -= OnGameStateChanging;
		//}
		//public static void OnWorldLoadStarted(object sender, EventArgs e)
		//      {
		//	if (InWorldState.InWorldSubStateChanging != null)
		//	{
		//		InWorldState.InWorldSubStateChanging += OnGameStateChanging;
		//	}
		//	//Responder.Instance.GameStateChanging += OnGameStateChanging;
		//}

		private static int CurrentSubStateID = 0;

		public static ListenerAction OnGameStateChanging(Event e)
		{
			try
			{
				InWorldSubStateEvent inWorldSubStateEvent = e as InWorldSubStateEvent;
				if (inWorldSubStateEvent == null)
				{
					return ListenerAction.Keep;
				}

				if (BinModel.Singleton == null) { return ListenerAction.Keep; }
				CurrentSubStateID = inWorldSubStateEvent.State.StateId;

				Simulator.AddObject(new OneShotFunctionTask(HandleRPGManagerInEditTown));

			}
			catch (Exception ex)
			{
				printException(ex);
				return ListenerAction.Keep;
			}
			return ListenerAction.Keep;

		}

		private static void HandleRPGManagerInEditTown()
		{
			int amountHouseholds = 0;

			// Edit town
			if (CurrentSubStateID == 14)
			{
				// Let's check if our households count is the same.
				try
				{
					// This is most likely the case. The code triggers a bit quicker than the UI does, thus it hasn't yet been populated properly when our code starts.
					if (BinModel.Singleton.mExportBin.Count == 0)
					{
						BinModel.Singleton.PopulateExportBin();
						//print("Populated: " + BinModel.Singleton.mExportBin.Count);
					}

					//ProgressDialog.Show("RPG Manager Is thinking...");
					foreach (ExportBinContents info in BinModel.Singleton.mExportBin)
					{
						if (info.mExportBinType == ExportBinType.Household)
						{
							//bool flag = info.IsLoaded();
							//if (!flag)
							//{
							//	info.Import(false);
							//}
							amountHouseholds++;
						}
					}
					//ProgressDialog.Close();
				}
				catch (Exception ex)
				{
					WriteErrorXMLFile("RPG_Manager_BinData_Error", ex, "");
				}
				//finally
				//{
				//	ProgressDialog.Close();
				//}

				//print("amountHouseholds: " + amountHouseholds.ToString() + " and mLastCountBinContent:" + mLastCountBinContent.ToString());

				// If it returns the same amount of households, then we return.
				if (mLastCountBinContent == amountHouseholds) { return; }
				bool wantsToEditStuff = TwoButtonDialog.Show("RPG Manager detected that you've added a new household to the Household Bin! Would you like to pick one or more bin households to edit?", "Yes please!", "No");

				if (wantsToEditStuff)
				{
					DoRPGManagerEDitTownThing();
				}
				mLastCountBinContent = amountHouseholds;

			}
			//else if (CurrentSubStateID == 15) // Playflow
			//{
			//	// This is most likely the case. The code triggers a bit quicker than the UI does, thus it hasn't yet been populated properly when our code starts.
			//	//if (BinModel.Singleton.mExportBin.Count == 0)
			//	//{
			//	//	BinModel.Singleton.PopulateExportBin();
			//	//	print("Populated: " + BinModel.Singleton.mExportBin.Count);
			//	//}

			//	// Let's check if our households count is the same.
			//	try
			//	{
			//		// This is most likely the case. The code triggers a bit quicker than the UI does, thus it hasn't yet been populated properly when our code starts.
			//		if (BinModel.Singleton.mExportBin.Count == 0)
			//		{
			//			BinModel.Singleton.PopulateExportBin();
			//			//print("Populated: " + BinModel.Singleton.mExportBin.Count);
			//		}

			//		ProgressDialog.Show("RPG Manager Is thinking...");
			//		foreach (ExportBinContents info in BinModel.Singleton.mExportBin)
			//		{
			//			if (info.mExportBinType == ExportBinType.Household)
			//			{
			//				//bool flag = info.IsLoaded();
			//				//if (!flag)
			//				//{
			//				//	info.Import(false);
			//				//}
			//				amountHouseholds++;
			//			}
			//		}
			//		ProgressDialog.Close();
			//	}
			//	catch (Exception ex)
			//	{
			//		WriteErrorXMLFile("RPG_Manager_BinData_Error", ex, "");
			//	}
			//	finally
			//	{
			//		ProgressDialog.Close();
			//	}

			//	print("amountHouseholds: " + amountHouseholds.ToString() + " and mLastCountBinContent:" + mLastCountBinContent.ToString());

			//	// If it returns the same amount of households, then we return.
			//	if (mLastCountBinContent == amountHouseholds) { return; }

			//	bool wantsToEditStuff = TwoButtonDialog.Show("RPG Manager detected that you've added a new household to the Household Bin! Would you like to pick one or more bin households to edit?", "Yes please!", "No");

			//	if (wantsToEditStuff)
			//	{
			//		DoRPGManagerEDitTownThing();
			//	}
			//}


			// Went from Edit Town into Livemode
			if (!GameStates.IsCurrentlySwitchingSubStates && GameStates.PrevInWorldStateId == InWorldState.SubState.EditTown && CurrentSubStateID == 0)
			{
				if (mLastKnownHouseholdId != Household.ActiveHousehold.HouseholdId || mLastKnownHouseholdId == 0)
				{
					AlarmManager.Global.AddAlarm(10f, TimeUnit.Minutes, new AlarmTimerCallback(RunActiveHouseholdRPGFixer), "Lyralei's RPGManagerUtil", AlarmType.NeverPersisted, null);
				}
			}

			if (GameStates.GetCurrentSubState() == InWorldState.SubState.LiveMode)
			{
				AlarmManager.Global.AddAlarm(1f, TimeUnit.Minutes, DoPregnancyFix, "RPG Manager pregnancy Fix for bin sims", AlarmType.NeverPersisted, null);
			}
		}

		public static void DoRPGManagerEDitTownThing()
		{
			try
			{
				if (BinModel.Singleton != null && BinModel.Singleton.mExportBin != null)
				{
					if (BinModel.Singleton.mExportBin.Count > 0)
					{
						List<ExportBinContents> households = BinManagerRPG.HouseholdPicker(BinModel.Singleton.mExportBin, "Pick any household(s)", -1);

						if (households != null && households.Count > 0)
						{
							for (int i = 0; i < households.Count; i++)
							{
								if (households[i] != null && households[i].Household != null)
								{
									for (int j = 0; j < households[i].Household.AllSimDescriptions.Count; j++)
									{
										ActiveSimsManagerRPG.GoThroughQuestionare(households[i].Household.AllSimDescriptions[j], true, households[i].ContentId);
									}
								}
							}

							// Update Export bin with the right info..
							for (int i = 0; i < BinModel.Singleton.mExportBin.Count; i++)
							{
								for (int j = 0; j < households.Count; j++)
								{
									if (BinModel.Singleton.mExportBin[i].HouseholdId == households[j].HouseholdId)
									{
										BinModel.Singleton.mExportBin[i].mHouseholdContents.mHousehold = households[j].Household;
									}
								}
							}
						}
						BinModel.Singleton.RefreshExportBinUI();

						if (PlayFlowBinPanel.Singleton != null)
						{
							PlayFlowBinPanel.Singleton.BinNeedsUpdate();
							PlayFlowBinPanel.Singleton.RefreshGrid();
						}

						if (EditTownLibraryPanel.Instance != null)
						{
							EditTownLibraryPanel.Instance.mGrid.Clear();
							EditTownLibraryPanel.Instance.PopulateGrid();

							EditTownModel editTownModel = Responder.Instance.EditTownModel as EditTownModel;

							if (editTownModel != null)
							{
								editTownModel.mGameBin = null;
								//editTownModel.GetExportBinInfoList();
								//editTownModel.GetGameBinInfoList();
								if (editTownModel.GameBinChanged != null)
								{
									editTownModel.GameBinChanged(BinModel.Singleton, null);
								}
							}
						}
					}

					//foreach (ExportBinContents content in BinModel.Singleton.mExportBin)
					//{
					//	if (content.mExportBinType == ExportBinType.Household)
					//	{
					//		bool flag = content.IsLoaded();
					//		if (!flag)
					//		{
					//			content.Import(false);
					//		}
					//	}
					//}
				}
			}
			catch (Exception ex)
			{
				printException(ex);
			}
		}

		private static void DoPregnancyFix()
		{
			if (simsToMakePregnant.Count > 0)
			{
				Sim[] simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();
				List<Sim> listSimsInWorld = new List<Sim>(simsInWorld);
				List<string> momsToDelete = new List<string>();

				print("RPG Manager will quickly fix any sims that should be pregnant. You'll get prompted whether you'd like to tweak their pregnancy where possible.");

				Sim mom = null;
				Sim dad = null;
				foreach (KeyValuePair<string, string> kpv in simsToMakePregnant)
				{
					mom = null;
					dad = null;
					foreach (Sim sim in listSimsInWorld)
					{
						if (sim.SimDescription.FullName == kpv.Key)
						{
							mom = sim;
						}
						if (sim.SimDescription.FullName == kpv.Value)
						{
							dad = sim;
						}
					}

					if (mom != null && dad != null)
					{
						if (Pregnancy.Start(mom, dad))
						{
							momsToDelete.Add(mom.SimDescription.FullName);
							bool PregnancyLength = TwoButtonDialog.Show("[Pregnancy Manager]" + '\n' + '\n' + "Would you like to manually set how far along they are?", "Yes please!", "No, next!");

							if (PregnancyLength)
							{
								string Parsedpercentage = StringInputDialog.Show("RPG Manager", "Hours of how far along your sim is. (" + Pregnancy.kHourToStartContractions.ToString() + " hours == Starting contractions)", "10", true);
								float result = 10;

								if (!float.TryParse(Parsedpercentage, out result))
								{
									SimpleMessageDialog.Show("RPG Manager", "Wasn't a proper number! Will set it to the default of 10 hours...");
								}
								mom.SimDescription.Pregnancy.SetHourOfPregnancy(Math.Min((int)result, Pregnancy.kHourToStartContractions - 1));
								mom.SimDescription.Pregnancy.SetPregoBlendShape();
								mom.SimDescription.Pregnancy.HourlyCallback();

								mom.SimDescription.Pregnancy.TryToGiveLeave();
							}
						}
					}

				}

				// Delete the sims that are now pregnant.
				foreach (string moms in momsToDelete)
				{
					if (simsToMakePregnant.ContainsKey(moms))
					{
						simsToMakePregnant.Remove(moms);
					}
				}
			}
		}
		public static void OnWorldLoaded(object sender, EventArgs e)
		{
			//Attempting to get some pets for the pet manager... which partially works so yay! :p
			PetPoolManager.AlarmCallBack();

			Commands.sGameCommands.Register("RPGManagerRunBin", "RPG Manager will re-run Bin sims manager [on-off]", Commands.CommandType.General, OnRPGManagerRunBinCommand);
			Commands.sGameCommands.Register("ForceBinToPopulate", "If Bin Manager comes back with no bin sims, run this cheat. [on-off]", Commands.CommandType.General, OnForceBinPopulate);

			// Get all sims in town
			sSimIsInActiveFamily = EventTracker.AddListener(EventTypeId.kHouseholdSelected, new ProcessEventDelegate(OnHouseholdSelected));

			foreach (CityHall cityHall in Sims3.Gameplay.Queries.GetObjects<CityHall>())
			{
				if (cityHall != null)
				{
					AddInteractionsObjects(cityHall);
				}
			}

			sBoughtObjectLister = EventTracker.AddListener(EventTypeId.kBoughtObject, new ProcessEventDelegate(OnObjectBought));
			sBoughtObjectLister = EventTracker.AddListener(EventTypeId.kObjectStateChanged, new ProcessEventDelegate(OnObjectBought));
			sSubStateListener = EventTracker.AddListener(EventTypeId.kEnterInWorldSubState, new ProcessEventDelegate(OnGameStateChanging));

			AlarmManager.Global.AddAlarm(10f, TimeUnit.Minutes, new AlarmTimerCallback(StartQuestionare), "Lyralei's RPGManagerUtil", AlarmType.NeverPersisted, null);
		}

		public static void OnWorldQuit(object sender, EventArgs e)
		{
			Commands.sGameCommands.Unregister("RPGManagerRunBin");
			Commands.sGameCommands.Unregister("ForceBinToPopulate");

		}

		public static int OnForceBinPopulate(object[] parameters)
		{

			try
			{
				if (parameters.Length == 0)
				{
					return -1;
				}

				bool flag = true;
				if (Commands.ParseParamAsBool(parameters, out flag, true))
				{
					if (flag)
					{
						Simulator.AddObject(new OneShotFunctionTask(ActuallyHandleCommand));
					}
					return 1;
				}
				return -1;
			}
			catch (Exception ex)
			{
				//WriteErrorXMLFile("Cheat error ", ex, null);
				return -1;
			}
		}

		public static int OnRPGManagerRunBinCommand(object[] parameters)
		{

			try
			{
				if (parameters.Length == 0)
				{
					return -1;
				}

				bool flag = true;
				if (Commands.ParseParamAsBool(parameters, out flag, true))
				{
					if (flag)
					{
						Simulator.AddObject(new OneShotFunctionTask(ActuallyHandleCommand));
					}
					return 1;
				}
				return -1;
			}
			catch (Exception ex)
			{
				//WriteErrorXMLFile("Cheat error ", ex, null);
				return -1;
			}
		}

		private static void PopulateBinForce()
		{
			if (CurrentSubStateID != 14)
			{
				print("Cannot force populate bin whilst not being in Edit Town!");
				return;
			}
			BinModel.Singleton.PopulateExportBin();

		}

		private static void ActuallyHandleCommand()
		{
			if (CurrentSubStateID != 14)
			{
				print("Can only trigger the Bin Manager when in edit town! Either quickly choose an household and go into edit Town, or go to Settings > Edit Town (if it's there, that's not always the case)");
				return;
			}

			bool wantsToEditStuff = TwoButtonDialog.Show("RPG Manager detected that you've added a new household to the Household Bin! Would you like to pick one or more bin households to edit?", "Yes please!", "No");
			if (wantsToEditStuff)
			{
				DoRPGManagerEDitTownThing();
			}
		}

		public static void AddInteractionsObjects(CityHall gameObj)
        {
			if(gameObj == null) { return; }
			foreach (InteractionObjectPair interaction in gameObj.Interactions)
			{
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunTownieRPG.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsCareer.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsDegrees.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsDoPregnancy.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsEnemies.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsFriendships.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsKnownNeighbors.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsLovers.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsMoney.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsRPG.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunActorsSkills.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RunTownieCareer.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RunTownieDegree.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RunTownieSkills.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RerunSexualOrientation.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RunTowniePetAdoption.Singleton.GetType())
				{
					return;
				}
				if (interaction.InteractionDefinition.GetType() == RPGManagerInteractions.RunTownieGenderPreference.Singleton.GetType())
				{
					return;
				}
			}

			gameObj.AddInteraction(RPGManagerInteractions.RerunTownieRPG.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RunTownieSkills.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RunTownieDegree.Singleton) ;
			gameObj.AddInteraction(RPGManagerInteractions.RunTownieCareer.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsSkills.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsRPG.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsMoney.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsLovers.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsKnownNeighbors.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsFriendships.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsEnemies.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsDoPregnancy.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsDegrees.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunActorsCareer.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RerunSexualOrientation.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RunTowniePetAdoption.Singleton);
			gameObj.AddInteraction(RPGManagerInteractions.RunTownieGenderPreference.Singleton);
		}

		protected static ListenerAction OnObjectBought(Event e)
		{
			try
			{
				if (e.TargetObject == null) { return ListenerAction.Keep; }
				CityHall obj = e.TargetObject as CityHall;
				if(e.TargetObject == null) { return ListenerAction.Keep; }

				AddInteractionsObjects(obj);
			}
			catch (Exception)
			{
			}
			return ListenerAction.Keep;
		}


		public static EventListener sSimIsInActiveFamily = null;

		public static void StartQuestionare()
		{
			try
			{
				if (mHasRunBefore) { return; }
				mHasRunBefore = true;

				bool userChosen = TwoButtonDialog.Show("Thank you for downloading Lyralei's RPG Manager! This save hasn't yet had RPG Manager run over it. Would you like to go through the setup wizard?", "Yes, please!", "Not today");

				if (userChosen)
				{
					RunTownieRPGFixer();

					RunActiveHouseholdRPGFixer();

					SimpleMessageDialog.Show("Lyralei's RPG Manager", "That's it for the RPG Manager then! Even though it will lean back and do nothing for now, if you ever would like to rerun the process (or parts of it), simply click on the City Hall and search for the RPG Manager's name!");
					return;

					//CustomizableNotification.Show("Hello World!");
				}
			}
			catch(Exception ex)
            {
				print(ex.ToString());
			}
		}

		public static void print(string text)
		{
			SimpleMessageDialog.Show("Lyralei's RPG Manager", text);
		}

		public static void DoProgressDialog()
        {
				ProgressDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObject/progress:main"));
		}

		public static void DoCloseProgressDialog()
		{
			ProgressDialog.Close();
		}

		public static void RunTownieRPGFixer()
		{
			try
			{
				simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();

				bool TownieChecker = TwoButtonDialog.Show("[Skill Manager]" + '\n' + '\n' + "Would you Like RPG Manager run over all sims in town and assign any skills (Service and Homeless sims will be ignored.)", "Yep!", "No, next!");
				if (TownieChecker)
				{
					DoProgressDialog();
					for (int i = 0; i < simsInWorld.Length; i++)
					{
						Sim sim = simsInWorld[i];
						if (sim != null || sim.SimDescription != null || sim.SimDescription.CreatedSim != null)
						{
							if (sim.InWorld && !sim.IsPet && !sim.IsPerformingAService && sim.Household != null && sim.Household.LotHome != null)
							{
								// We need to skip our active household sims.
								if (sim.IsInActiveHousehold) { continue; }
								TownieManagerRPG.TownieHobbyFixer(sim);
							}
						}
					}

					DoCloseProgressDialog();

					if (TownieManagerRPG.assignedSKills.Count > 0)
					{
						ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sims as Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");

						switch (XmlWriter)
						{
							case ThreeButtonDialog.ButtonPressed.FirstButton:
								WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Skills] Log", null, TownieManagerRPG.GetTownieNamesAndSkillsInStringFormat(true));
								SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
								break;
							case ThreeButtonDialog.ButtonPressed.SecondButton:
								string TownieData = TownieManagerRPG.GetTownieNamesAndSkillsInStringFormat(false);

								string[] subs = TownieData.Split(new string[] { "[CUT]" }, StringSplitOptions.None);

								foreach (string split in subs)
								{
									StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
									StyledNotification.Show(format);
								}
								break;
							case ThreeButtonDialog.ButtonPressed.ThirdButton:
								break;
						}
						TownieManagerRPG.assignedSKills.Clear();
					}
					else
					{
						SimpleMessageDialog.Show("Lyralei's RPG Manager", "[Skill Manager]" + '\n' + '\n' + "The Townie manager of the mod tried to find any townies that need skills assigned, but couldn't find any good candidates! (or, there are no sims in your world!)");
					}
				}

				bool TownieJobChecker = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + "Would you Like RPG Manager run over all sims in town and check if unemployed sims can have a good job, depending on their traits/skills?", "Yes please!", "No, next!");

				if (TownieJobChecker)
				{
					TownieManagerRPG.UserCanConfirmJob = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + "Before the RPG manager begins, there is a chance that townies are qualified for multiple jobs. Do you want to manually choose the jobs in those cases?", "Yep!", "Nah, assign randomly");
					if (!TownieManagerRPG.UserCanConfirmJob) { DoProgressDialog(); }

					for (int i = 0; i < simsInWorld.Length; i++)
					{
						Sim sim = simsInWorld[i];
						if (sim != null || sim.SimDescription != null || sim.SimDescription.CreatedSim != null)
						{
							if (sim.InWorld && !sim.IsPet && !sim.IsPerformingAService && sim.Household != null && sim.Household.LotHome != null)
							{
								// We need to skip our active household sims.
								if (sim.IsInActiveHousehold) { continue; }
								TownieManagerRPG.TownieJobFixer(sim);
							}
						}
					}
					if (!TownieManagerRPG.UserCanConfirmJob) { 	DoCloseProgressDialog(); }

					if (TownieManagerRPG.assignedJob.Count > 0)
					{
						ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sims gotten a job as a Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");

						switch (XmlWriter)
						{
							case ThreeButtonDialog.ButtonPressed.FirstButton:
								WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Jobs] Log", null, TownieManagerRPG.GetTownieNamesJobsInStringFormat(true));
								SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
								break;
							case ThreeButtonDialog.ButtonPressed.SecondButton:
								string TownieData = TownieManagerRPG.GetTownieNamesJobsInStringFormat(false);

								string[] subs = TownieData.Split(new string[] { "[CUT]" }, StringSplitOptions.None);

								foreach (string split in subs)
								{
									StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
									StyledNotification.Show(format);
								}
								break;
							case ThreeButtonDialog.ButtonPressed.ThirdButton:
								break;
						}
						TownieManagerRPG.assignedDegree.Clear();
					}
					else
					{
						SimpleMessageDialog.Show("Lyralei's RPG Manager", "[Job Manager]" + '\n' + '\n' + "The Townie manager of the mod tried to find any townies that need a degree, but couldn't find any good candidates! (or, there are no sims in your world!)");
					}
				}
				if (GameUtils.IsInstalled(ProductVersion.EP9))
				{
					bool TownieDegreeChecker = TwoButtonDialog.Show("[Degree Manager]" + '\n' + '\n' + "Would you Like RPG Manager run over all sims in town and give successful sims a University degree depending on their current job? (this will also add skills they could have been taught at said university course)", "Yep!", "No, next!");

					if (TownieDegreeChecker)
					{
						DoProgressDialog();

						for (int i = 0; i < simsInWorld.Length; i++)
						{
							Sim sim = simsInWorld[i];
							if (sim != null || sim.SimDescription != null || sim.SimDescription.CreatedSim != null)
							{
								if (sim.InWorld && !sim.IsPet && !sim.IsPerformingAService && sim.Household != null && sim.Household.LotHome != null)
								{
									// We need to skip our active household sims.
									if (sim.IsInActiveHousehold) { continue; }
									TownieManagerRPG.TownieDegreeFixer(sim, false);
								}
							}
						}
						DoCloseProgressDialog();

						if (TownieManagerRPG.assignedDegree.Count > 0)
						{
							ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sims as Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");

							switch (XmlWriter)
							{
								case ThreeButtonDialog.ButtonPressed.FirstButton:
									WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Degrees] Log", null, TownieManagerRPG.GetTownieNamesDegreesInStringFormat(true));
									SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
									break;
								case ThreeButtonDialog.ButtonPressed.SecondButton:
									string TownieData = TownieManagerRPG.GetTownieNamesDegreesInStringFormat(false);

									string[] subs = TownieData.Split(new string[] { "[CUT]" }, StringSplitOptions.None);

									foreach (string split in subs)
									{
										StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
										StyledNotification.Show(format);
									}
									break;
								case ThreeButtonDialog.ButtonPressed.ThirdButton:
									break;
							}
							TownieManagerRPG.assignedDegree.Clear();
						}
						else
						{
							SimpleMessageDialog.Show("Lyralei's RPG Manager", "[Degree Manager]" + '\n' + '\n' + "The Townie manager of the mod tried to find any townies that need a degree, but couldn't find any good candidates! (or, there are no sims in your world!)");
						}
					}
				}


				bool GenderPreferenceChecker = TwoButtonDialog.Show("[Gender Preference Manager]" + '\n' + '\n' + "Would You like The RPG Manager let you handle Gender Preferences in town?", "Yes please!", "No, next!");

				if (GenderPreferenceChecker)
				{
					bool GenderPreferenceCheckerRandom = TwoButtonDialog.Show("[Gender Preference Manager]" + '\n' + '\n' + "Great! Would you like to do this manually, or have RPG Manager handle it? (You'll be able to state the percentages of same-sex or straight preferences. Sims with a preference already won't be overridden.)", "Manually please", "Randomize it!");

					if (GenderPreferenceCheckerRandom)
					{
						List<Sim> townies = TownieManagerRPG.TowniePicker("Gender Preference", -1);
						if (townies != null && townies.Count > 0)
						{
							StringBuilder sb = new StringBuilder();

							for (int i = 0; i < townies.Count; i++)
							{
								if (townies[i] != null)
								{
									if (i == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
									{
										sb.AppendLine("[CUT]");
									}

									ActiveSimsManagerRPG.DoSexualOrientation(townies[i].SimDescription);

									sb.AppendLine(townies[i].SimDescription.FullName);

									sb.AppendLine("     Preference for Male: " + townies[i].SimDescription.mGenderPreferenceMale.ToString());
									sb.AppendLine("     Preference for Female: " + townies[i].SimDescription.mGenderPreferenceFemale.ToString());

									if (townies[i].SimDescription.mGenderPreferenceMale > townies[i].SimDescription.mGenderPreferenceFemale)
									{
										sb.AppendLine("			Story Progression's potential partner decision: Male");
									}
									else if (townies[i].SimDescription.mGenderPreferenceFemale > townies[i].SimDescription.mGenderPreferenceMale)
									{
										sb.AppendLine("			Story Progression's potential partner decision: Female");
									}
									else
									{
										sb.AppendLine("			Story Progression's potential partner decision: Both");
									}
									sb.AppendLine("");
									sb.AppendLine("------------");
									sb.AppendLine("");
								}
							}

							ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sim's Gender Preferences Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");
							string[] subs = sb.ToString().Split(new string[] { "[CUT]" }, StringSplitOptions.None);

							switch (XmlWriter)
							{
								case ThreeButtonDialog.ButtonPressed.FirstButton:

									string res = String.Concat(subs);

									WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Gender Preference] Log", null, res);
									SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
									break;
								case ThreeButtonDialog.ButtonPressed.SecondButton:
									foreach (string split in subs)
									{
										StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
										StyledNotification.Show(format);
									}
									break;
								case ThreeButtonDialog.ButtonPressed.ThirdButton:
									break;
							}
						}
					}
                    else
                    {
						int MaleSameSex = 10;
						string Level = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will have a male same-sex preference.", "10", true);
						
						if (!int.TryParse(Level, out MaleSameSex))
						{
							SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
						}

						int FemaleSameSex = 10;
						string Level2 = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will have a female same-sex preference.", "10", true);

						if (!int.TryParse(Level2, out FemaleSameSex))
						{
							SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
						}

						int Bisexual = 10;
						string Level3 = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will have a preference for both genders.", "10", true);

						if (!int.TryParse(Level3, out Bisexual))
						{
							SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
						}

						int Straight = 10;
						string Level4 = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will only have a preference for the opposite sex.", "10", true);

						if (!int.TryParse(Level4, out Straight))
						{
							SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
						}

						StringBuilder sb = new StringBuilder();
						int index = 0;

						Sim[] simsInWorld = Queries.GetObjects<Sim>();
						foreach (Sim friend in simsInWorld)
						{
							if (friend != null && friend.SimDescription != null && friend.SimDescription.CreatedSim != null)
							{
								if (friend.InWorld && !friend.IsPet && !friend.IsPerformingAService && friend.Household.LotHome != null && !friend.IsInActiveHousehold)
								{
                                    if (friend.SimDescription.ChildOrBelow) { continue; }

									if(friend.SimDescription.mGenderPreferenceMale == 0 && friend.SimDescription.mGenderPreferenceFemale == 0)
                                    {
										if (index == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
										{
											sb.AppendLine("[CUT]");
										}

										if (MaleSameSex != 0)
                                        {
											if(friend.IsMale && RandomUtil.RandomChance(MaleSameSex))
                                            {
												friend.SimDescription.mGenderPreferenceMale = 200;
											}
                                        }

										if (FemaleSameSex != 0)
										{
											if (friend.IsMale && RandomUtil.RandomChance(FemaleSameSex))
											{
												friend.SimDescription.mGenderPreferenceFemale = 200;
											}
										}

										if (Bisexual != 0)
										{
											if (RandomUtil.RandomChance(Bisexual))
											{
												friend.SimDescription.mGenderPreferenceFemale = 200;
												friend.SimDescription.mGenderPreferenceMale = 200;
											}
										}

										if (Straight != 0)
										{
											if (RandomUtil.RandomChance(Straight))
											{
												if(friend.IsMale)
                                                {
													friend.SimDescription.mGenderPreferenceFemale = 200;
												}
                                                else
                                                {
													friend.SimDescription.mGenderPreferenceMale = 200;
												}
											}
										}

										sb.AppendLine(friend.SimDescription.FullName);

										sb.AppendLine("     Preference for Male: " + friend.SimDescription.mGenderPreferenceMale.ToString());
										sb.AppendLine("     Preference for Female: " + friend.SimDescription.mGenderPreferenceFemale.ToString());

										if (friend.SimDescription.mGenderPreferenceMale > friend.SimDescription.mGenderPreferenceFemale)
										{
											sb.AppendLine("			Story Progression's potential partner decision: Male");
										}
										else if (friend.SimDescription.mGenderPreferenceFemale > friend.SimDescription.mGenderPreferenceMale)
										{
											sb.AppendLine("			Story Progression's potential partner decision: Female");
										}
										else
										{
											sb.AppendLine("			Story Progression's potential partner decision: Both");
										}
										sb.AppendLine("");
										sb.AppendLine("------------");
										sb.AppendLine("");

										index++;
									}
								}
							}
						}
						ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sim's Gender Preferences Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");
						string[] subs = sb.ToString().Split(new string[] { "[CUT]" }, StringSplitOptions.None);

						switch (XmlWriter)
						{
							case ThreeButtonDialog.ButtonPressed.FirstButton:

								string res = String.Concat(subs);

								WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Gender Preference] Log", null, res);
								SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
								break;
							case ThreeButtonDialog.ButtonPressed.SecondButton:
								foreach (string split in subs)
								{
									StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
									StyledNotification.Show(format);
								}
								break;
							case ThreeButtonDialog.ButtonPressed.ThirdButton:
								break;
						}

					}
				}

				bool PetsChecker = TwoButtonDialog.Show("[Pet Manager]" + '\n' + '\n' + "Would You like to automatically adopt an animal for certain townie households? You'll be able to choose which household AND which animal(s)", "Yes please!", "No, next!");

				if (PetsChecker)
				{
					List<Household> townies = TownieManagerRPG.TownieHouseholdPicker("Pet ", -1);
					if (townies != null && townies.Count > 0)
					{
						StringBuilder sb = new StringBuilder();

						for (int i = 0; i < townies.Count; i++)
						{
							if (townies[i] != null)
							{
								if (i == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
								{
									sb.AppendLine("[CUT]");
								}

								SimDescription simDescription = BinManagerRPG.ShowAdoptPetPicker();

								if (simDescription != null)
								{
									sb.AppendLine("The " + townies[i].Name + "'s Adopted: ");

									PetAdoption.GetPetOutOfPool(simDescription);

									townies[i].AddSilent(simDescription);
									simDescription.OnHouseholdChanged(townies[i], false);
									townies[i].OnMemberChanged(townies[i].Sims[0].SimDescription, simDescription.CreatedSim);

									simDescription.Instantiate(townies[i].LotHome);

									//townies[i].Add(simDescription);
									simDescription.IsNeverSelectable = false;

									sb.AppendLine("		- " + simDescription.FullName + " (" + simDescription.Species + ")");
									sb.AppendLine("");
									sb.AppendLine("");
								}
							}
						}

						ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of households adopted pets as a Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");
						string[] subs = sb.ToString().Split(new string[] { "[CUT]" }, StringSplitOptions.None);

						switch (XmlWriter)
						{
							case ThreeButtonDialog.ButtonPressed.FirstButton:

								string res = String.Concat(subs);

								WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Pet Adoption] Log", null, res);
								SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");

								break;
							case ThreeButtonDialog.ButtonPressed.SecondButton:
								foreach (string split in subs)
								{
									StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
									StyledNotification.Show(format);
								}
								break;
							case ThreeButtonDialog.ButtonPressed.ThirdButton:
								break;
						}
					}
				}
			}
			catch(Exception ex)
            {
				printException(ex);
            }
            finally
            {
				DoCloseProgressDialog();
			}
		}



		// Mom, dad
		public static Dictionary<string, string> simsToMakePregnant = new Dictionary<string, string>();
		public static void RunActiveHouseholdRPGFixer()
        {
			try
			{
				bool runActiveHousehold = TwoButtonDialog.Show("Let's tweak your current active Household!" + '\n'+ '\n' +" The RPG manager will first present you with a list of sims you'd like to individually edit. The questions you'll get asked are based off what Skills they may need, previous/current career, etc.", "Yep!", "No, next!");

				if (runActiveHousehold)
				{
					if(Sim.ActiveActor != null && Sim.ActiveActor.Household != null && Sim.ActiveActor.Household.SimDescriptions != null)
                    {
						SimpleMessageDialog.Show("RPG Manager", "You will now see a dialog with all your sims in the household. Please select the sims you want to edit (All or None of them is fine too!)");
						List<SimDescription> UserChosenSims = BuildSimPicker(Sim.ActiveActor.Household.SimDescriptions);
						if (UserChosenSims != null)
						{
							for (int i = 0; i < UserChosenSims.Count; i++)
							{
								ActiveSimsManagerRPG.GoThroughQuestionare(UserChosenSims[i], false, 0);
							}
						}
						mLastKnownHouseholdId = Sim.ActiveActor.Household.HouseholdId;
					}
					else
                    {
						SimpleMessageDialog.Show("RPG Manager", "The RPG Manager couldn't find your active sims! This can happen if you haven't yet moved your sims in and are still in the clipboard. If this is the case, simply click on the City hall > Lyralei's RPG Manager > Active Household... > Run Active Sim's RPG Manager Entirely");
					}
				}
				ActiveSimsManagerRPG.MoneySituationAlreadyRun = false;
				return;
			}
			catch(Exception ex)
            {
				printException(ex);
            }
		}

		//Kindly stolen from EA :p
		public static List<SimDescription> BuildSimPicker(List<SimDescription> simDescriptions)
		{
			int numSelectableRows = simDescriptions.Count;

			List<ObjectPicker.HeaderInfo> headers = new List<ObjectPicker.HeaderInfo>();
			List<ObjectPicker.TabInfo> listObjs = new List<ObjectPicker.TabInfo>();
			headers.Add(new ObjectPicker.HeaderInfo("Ui/Caption/ObjectPicker:Sim", "Ui/Tooltip/ObjectPicker:LastName", 230));
			ObjectPicker.TabInfo tabInfo = new ObjectPicker.TabInfo("shop_all_r2", Localization.LocalizeString("Ui/Caption/ObjectPicker:All"), new List<ObjectPicker.RowInfo>());
			if (simDescriptions != null)
			{
				foreach (SimDescription simDescription in simDescriptions)
				{
					ObjectPicker.RowInfo rowInfo = new ObjectPicker.RowInfo(simDescription, new List<ObjectPicker.ColumnInfo>());
					ThumbnailKey thumbnailKey = simDescription.GetThumbnailKey(ThumbnailSize.Medium, 0);
					rowInfo.ColumnInfo.Add(new ObjectPicker.ThumbAndTextColumn(thumbnailKey, simDescription.FullName));
					tabInfo.RowInfo.Add(rowInfo);
				}
			}
			listObjs.Add(tabInfo);

			try
			{
				// Return the Object the user selected.
				List<ObjectPicker.RowInfo> rowInfo1 = ObjectPickerDialog.Show(
					true,
					ModalDialog.PauseMode.PauseSimulator,
					"Select which sims in your household You'd like to edit:",
					Localization.LocalizeString("Ui/Caption/Global:Ok"),
					Localization.LocalizeString("Ui/Caption/Global:Cancel"),
					listObjs,
					headers,
					numSelectableRows
				);

				// Prepare the Sims List:
				if(rowInfo1.Count != 0)
                {
					List<SimDescription> sims = new List<SimDescription>();
					for (int i = 0; i < rowInfo1.Count; i++)
                    {
						SimDescription sim = rowInfo1[i].Item as SimDescription;
						if(sim != null)
                        {
							sims.Add(sim);
                        }
                    }

					if(sims.Count != 0)
                    {
						return sims;
                    }
                }
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static ListenerAction OnHouseholdSelected(Event e)
		{
			if (Household.ActiveHousehold != null)
			{
				mLastKnownHouseholdId = Household.ActiveHousehold.HouseholdId;
			}
			return ListenerAction.Keep;
		}

		public static void printException(Exception e)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("An exception (" + e.GetType().Name + ") occurred.");
			sb.AppendLine("   Message:\n" + e.Message);
			sb.AppendLine("   Stack Trace:\n   " + e.StackTrace);
			Exception ie = e.InnerException;
			if (ie != null)
			{
				sb.AppendLine("   The Inner Exception:");
				sb.AppendLine("      Exception Name: " + ie.GetType().Name);
				sb.AppendLine("      Message: " + ie.Message + "\n");
				sb.AppendLine("      Stack Trace:\n   " + ie.StackTrace + "\n");
			}

			if(sb.Length > 500)
			{
				WriteErrorXMLFile("Lyralei's RPG Manager_ERROR_FILE", e, null);
				SimpleMessageDialog.Show("Lyralei's RPG Manager Error:", "Wrote error to Documents/Electionica Arts/The Sims 3. If you see this, please report to Lyralei! Screenshot/link the text file and mention what you were doing.");
			}
            else
			{
				SimpleMessageDialog.Show("Lyralei's RPG Manager Error:", sb.ToString());
			}

		}
		public static void WriteErrorXMLFile(string fileName, Exception errorToPrint, string additionalinfo)
		{
			uint num = 0u;
			string s = Simulator.CreateExportFile(ref num, fileName);

			if (num != 0)
			{
				CustomXmlWriter customXmlWriter = new CustomXmlWriter(num);

				if (!String.IsNullOrEmpty(additionalinfo))
				{
					customXmlWriter.WriteToBuffer(additionalinfo);
				}
				if (errorToPrint != null)
				{
					customXmlWriter.WriteToBuffer(errorToPrint.ToString());
				}
				customXmlWriter.WriteEndDocument();
			}
		}

	}
}