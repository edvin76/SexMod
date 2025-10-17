using HarmonyLib;
using Kingmaker;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Cheats;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Events;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.DLC;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.GameCommands;
using Kingmaker.Localization;
using Kingmaker.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityModManagerNet;

namespace SexMod
{
    //-:cnd:noEmit
#if DEBUG
    [EnableReloading]
#endif
	//+:cnd:noEmit

	static class Main
    {




//#if DEBUG
		[HarmonyPatch(typeof(BlueprintDlcReward), "IsAvailable", MethodType.Getter)]
		internal static class BlueprintDlcReward_IsAvailable_Patch
		{

			private static bool Prefix(BlueprintDlcReward __instance, ref bool __result)
			{
				//if (!Main.enabled)
					//return true;
				try
				{
					
					//Log(__instance.name);
					//Log(__instance.Description);


					__result = Main.settings.dev;
					return false;

				}
				catch (Exception ex)
				{
					Log(ex.ToString() + "\n" + ex.StackTrace);
					return true;
				}
			}
		}
//#endif

		public static Dictionary<string, string> loc = new Dictionary<string, string>();

		public static string GetLocFilePath(string Locale = null)
		{
			if (Locale == null)
			{
				if (File.Exists(Path.Combine(UnityModManager.ModsPath, Main.modName, /*"Localization",*/ SettingsRoot.Game.Main.Localization.m_CurrentValue.ToString() + ".json")))
					return Path.Combine(UnityModManager.ModsPath, Main.modName, /*"Localization",*/ SettingsRoot.Game.Main.Localization.m_CurrentValue.ToString() + ".json");
				else
					return Path.Combine(UnityModManager.ModsPath, Main.modName, /*"Localization",*/ "enGB.json");
			}
			else
				return Path.Combine(UnityModManager.ModsPath, Main.modName, /*"Localization",*/ Locale + ".json");
		}

		public static string ReadLocResource()
		{
			Main.Log(JsonUtility.FromJson<string>(File.ReadAllText(GetLocFilePath())));
				return JsonUtility.FromJson<string>(File.ReadAllText(GetLocFilePath()));
		}

		[HarmonyPriority(Priority.First)]
		[HarmonyPatch(typeof(BlueprintsCache), "Init")]
		public static class BlueprintsCache_Init_Patch
		{
			//[HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
			private static void Postfix()
			{


				if (!Main.isModEnabled)
					return;

				if (isRTLoaded)
					return;

				isRTLoaded = true;

			//	loc = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GetLocFilePath()));

				//DialogCreate.CreateYrlietDialog();

				//DialogCreate.CreateYrlietDialog();

				DialogCreate.ReplaceYrlietMeditationActionHolder();

				ConditionsHolder conditionsHolder2 = ResourcesLibrary.TryGetScriptable<ConditionsHolder>("2bbe7368-a55b-4652-9147-c4a2742f3c3b");
				if (conditionsHolder2 == null)
				{
					conditionsHolder2 = Helpers.CreateAndAddESO<ConditionsHolder>("cond", "2bbe7368-a55b-4652-9147-c4a2742f3c3b");
				}
				var currentAreaPartIsNot = Helpers.CreateElement<CurrentAreaPartIs>(conditionsHolder2);
				var rtCabin = ResourcesLibrary.TryGetBlueprint<BlueprintAreaPart>("9fd386ae7b34415792b03c174931980f");
				currentAreaPartIsNot.m_AreaPart = rtCabin.ToReference<BlueprintAreaPartReference>();
				currentAreaPartIsNot.Not = true;

				/*
				
				Cutscene cs = ResourcesLibrary.TryGetBlueprint<Cutscene>("fc46d3e602eb4ec186bafe969cc3ddec");
				if (cs.m_Tracks.Count() == 4)
				{
					cs.m_Tracks.RemoveAt(2);
					cs.m_Tracks.RemoveAt(2);
				}


				cs.OnStopped.Actions = cs.OnStopped.Actions.Where((value, index) => index != 0).ToArray();


				BlueprintEtude be = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>("7ff8d4be5722449980d29c2b0c14e4ae");
				//cs.OnStopped.Actions = null;
				((DeactivateTrigger)be.Components[0]).Actions.Actions = ((DeactivateTrigger)be.Components[0]).Actions.Actions.Where((value, index) => index != 2).ToArray();



				BlueprintEtude be2 = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>("c7c76f31f6574c7f9efa5d4ec5b7f846");

				((EtudePlayTrigger)be2.Components[0]).Actions.Actions = ((EtudePlayTrigger)be2.Components[0]).Actions.Actions.Where((value, index) => index != 1).ToArray();
				((DeactivateTrigger)be2.Components[1]).Actions.Actions = ((DeactivateTrigger)be2.Components[1]).Actions.Actions.Where((value, index) => index != 0).ToArray();



				*/

				BlueprintAnswer ba1 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("222d7429892b4864a40c38f8a061034e");
				ba1.ShowConditions.Conditions = ba1.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba2 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("8fa0a37925ef45b7a94cffcfbdb44b53");
				ba2.ShowConditions.Conditions = ba2.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba3 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("da1841442dca42658fcfa3be3b41cbdf");
				ba3.ShowConditions.Conditions = ba3.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba4 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("c1fd5dc8a9d64d59816d3edb583c040a");
				ba4.ShowConditions.Conditions = ba4.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba5 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("b5682f9b453e438bbcb72901499d476d");
				ba5.ShowConditions.Conditions = ba5.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba6 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("f8cd89c9a4b7451e9367de70ebc8f062");
				ba6.ShowConditions.Conditions = ba6.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba7 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("59bda0866896455ea82ac40717da978f");
				ba7.ShowConditions.Conditions = ba7.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba8 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("577baa13e21541dcb9477f9858c72c68");
				ba8.ShowConditions.Conditions = ba8.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba9 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("68b49fb941774e09b37cb83c6f1a405c");
				ba9.ShowConditions.Conditions = ba9.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba10 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("f9ceb8f5fc4740b6b9713c81fa317b10");
				ba10.ShowConditions.Conditions = ba10.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba11 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("49787910d0f9409a8efd3771d6bf8d16");
				ba11.ShowConditions.Conditions = ba11.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();

				BlueprintAnswer ba12 = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("7f6ddcf63d864b34852df312878869e7");
				ba12.ShowConditions.Conditions = ba12.ShowConditions.Conditions.Append<Condition>(currentAreaPartIsNot).ToArray();




				//string dir = Path.GetFullPath(Path.Combine(CustomPortraitsManager.PortraitsRootFolderPath, @"..\"));

				//dir = Path.Combine(dir, "Portraits - Npc");

				//Directory.CreateDirectory(dir);

				//DirectoryInfo target = new DirectoryInfo(dir);

				//DirectoryInfo source = new DirectoryInfo(Helpers.GetPortraitsDir());
				//CopyFilesRecursively(source, target);



				//loc = JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadLocResource());





				var bp = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("20c5ce9f1e2bcf9448a7a0fd0850f5d2");

				YrlietName = bp.CharacterName;

			}
		}

		public static string YrlietName = "Yrliet";

		[HarmonyPatch(typeof(Player), "OnAreaLoaded")]

		class Player_OnAreaLoaded_Patch
		{
			private static void Postfix(BaseUnitEntity __instance)
			{
				if (!Main.isModEnabled)
					return;

				Main.settings.dev = false;

				// Log("Scenname: " + Game.Instance.CurrentScene.Blueprint.name);


				// Log("mfane isloaded: " + SceneManager.GetSceneByName("MidnightFane_Caves_Mechanics").isLoaded);
				/*
                if (SceneManager.GetSceneByName("TenThousandDelights_Mechanics").isLoaded)
                {

                    foreach (UnitEntityData unit in Game.Instance.State.Units)
                    {

                        if (File.Exists(Path.Combine(GetPortraitsDir(), unit.CharacterName, unit.Blueprint.name, "Medium.png")))
                        {
                            //  Log("FOUND - " + unit.Blueprint.name);

                            BlueprintPortrait bp = new BlueprintPortrait();

                            bp.Data = new PortraitData(Path.Combine(GetPortraitsDir(), unit.CharacterName, unit.Blueprint.name));

                            if (unit.Blueprint.Name.Equals("TTD_Succubus1a"))
                                bp.AssetGuid = "04bf33e0-ff02-438a-935f-4fd32c3569ae".ToGUID();
                            else
                                bp.AssetGuid = new BlueprintGuid(Guid.NewGuid());

                            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(bp.AssetGuid, bp as SimpleBlueprint);

                            unit.Blueprint.m_Portrait = bp.ToReference<BlueprintPortraitReference>();

                            unit.UISettings.SetPortrait(bp);

                        }
                    }
                }
                */

				Log("OnAreaLoaded Start");




				BlueprintCue Cue_86 = ResourcesLibrary.TryGetBlueprint<BlueprintCue>("75832b318c534005beff4ee87e71815d");
				
				Log("OnAreaLoaded 1");


				//Cue_86.Speaker.m_Blueprint = bref;
				//Cue_86.Speaker.m_SpeakerPortrait = bref;
				if (Cue_86 != null)
					Log("found cue_86");
				else
					Log("not found cue_86");

				ActionsHolder ActionsHolder0 = ResourcesLibrary.TryGetScriptable<ActionsHolder>("CF8C05ED-20E6-40E7-B6ED-F68FF3D94B31");
				if (ActionsHolder0 == null)
				{
					//$PlayCutscene$
					ActionsHolder0 = Helpers.CreateAndAddESO<ActionsHolder>("Start_Meditate", "CF8C05ED-20E6-40E7-B6ED-F68FF3D94B31");

					Log("AH created name: " + ActionsHolder0.name);
				}
				//   Log("ActionName: "+ActionName);
				var startMeditate = Helpers.CreateElement<DialogCreate.StartMeditate>(ActionsHolder0);


				Cue_86.OnStop = new ActionList()
				{
					Actions = new GameAction[]
													{
													startMeditate
													}
				};

				foreach (GameAction ga in Cue_86.OnStop.Actions)
				{
				
					Log("GameAction: " + ga.name + " - " + ga.GetType().Name);
				
				}
				/*
				if (//Game.Instance.CurrentlyLoadedAreaPart.name.Equals("RTCabin") 
					SceneManager.GetSceneByName("VoidshipBridge_RTCabin_Mechanics").isLoaded
					)
				{

					Log("FOUND - VoidshipBridge_RTCabin_Mechanics");

					DialogCreate.CreateYrlietDialog();

				

				}*/

				Log("OnAreaLoaded 2");


				DialogCreate.CreateYrlietDialog();


				foreach (KeyValuePair<BlueprintUnlockableFlag, int> unlockedFlag in Game.Instance.Player.UnlockableFlags.UnlockedFlags)
				{
					if (unlockedFlag.Key.name.Equals("Meditation"))
					{
						Log("OnAreaLoaded 3");

						if (Game.Instance.Player.UnlockableFlags.GetFlagValue(unlockedFlag.Key) < 0)
							Game.Instance.Player.UnlockableFlags.SetFlagValue(unlockedFlag.Key, 0);

						Log("OnAreaLoaded 4");

					}
				}

				Log("OnAreaLoaded 5");
			}
		}
		internal static Harmony HarmonyInstance;
        public static UnityModManager.ModEntry.ModLogger log;

		//public static UnityModManager.ModEntry.ModLogger logger;

		public static void Log(string str)
		{
#if DEBUG

			Main.log.Log(str);

			Main.LogStr = Main.LogStr + str + ", ";
#endif
		}
		public static string modName;

		static bool Load(UnityModManager.ModEntry modEntry)
        {
            log = modEntry.Logger;
            //-:cnd:noEmit
#if DEBUG
            modEntry.OnUnload = OnUnload;
#endif
            //+:cnd:noEmit
            modEntry.OnGUI = OnGUI;

			modEntry.OnToggle = new Func<UnityModManager.ModEntry, bool, bool>(Main.OnToggle);

			modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(Main.OnSaveGUI);

			modEntry.OnHideGUI = new Action<UnityModManager.ModEntry>(Main.OnHideGUI);

            modEntry.OnShowGUI = new Action<UnityModManager.ModEntry>(Main.OnShowGUI);



            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

			var methods = HarmonyInstance.GetPatchedMethods();

			modName = modEntry.Info.Id;

			Main.settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

            Main.settings.dev = false;


            /*
			foreach (var m in methods)
                {

				Log(m.Name);
            }*/

            loc = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GetLocFilePath()));

			return true;
        }

		public static string s = "undefined, ";

		public static string LogStr = "undefined, ";

		public static Settings settings;


		static void OnGUI(UnityModManager.ModEntry modEntry)
		{



			Main.settings.male = GUILayout.Toggle(Main.settings.male, "Male Captain", new GUILayoutOption[0]);
			Main.settings.female = GUILayout.Toggle(Main.settings.female, "Female Captain", new GUILayoutOption[0]);

			if (settings.male == false && settings.female == false)
			{
				settings.female = !settings.female;
				settings.male = !settings.male;
			}



            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			Main.settings.dev = GUILayout.Toggle(Main.settings.dev, "Developer Mode", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();



			if (Main.settings.dev)
			{

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Get answer", GUILayout.Width(200f), GUILayout.Height(20f)))
				{

					//d35f72efa8cd4b6a995e87ca9ded7d9e
					BlueprintAnswer ba = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("d35f72efa8cd4b6a995e87ca9ded7d9e");
					if (ba)
						Log(ba.name);
					else
						Log("no ba");
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("LOG: " + Main.LogStr);
				GUILayout.EndHorizontal();
			}

            #if DEBUG
            BlueprintCue Cue_86 = ResourcesLibrary.TryGetBlueprint<BlueprintCue>("75832b318c534005beff4ee87e71815d");
				foreach (GameAction ga in Cue_86.OnStop.Actions)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("GameAction: " + ga.name + " - " + ga.GetType().Name);
					GUILayout.EndHorizontal();
				}


				if (ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>("2e582cda6d9846b0ab10a338adadd945") != null)
				{
					BlueprintAnswersList answersList = ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>("2e582cda6d9846b0ab10a338adadd945");

					// BlueprintAnswer ba = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("d35f72efa8cd4b6a995e87ca9ded7d9e");

					foreach (BlueprintAnswer ba in answersList.Answers)
					{
						if (ba.name.Contains("simpledialog"))
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Label("Answer: " + ba.name + " - " + ba.AssetGuid + " - " + ba.ShowConditions.Conditions[0].GetType());
							GUILayout.EndHorizontal();
							if (ba.ShowConditions.Conditions.Count() > 1)
							{
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Label("2nd condition: " + ba.ShowConditions.Conditions[1].GetType());
								GUILayout.EndHorizontal();
							}
							else
							{
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Label("2nd condition: NONE");
								GUILayout.EndHorizontal();
							}

						}
					}
				}


				Cutscene cs = ResourcesLibrary.TryGetBlueprint<Cutscene>("fc46d3e602eb4ec186bafe969cc3ddec");

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Onstopped actions: " + cs.OnStopped.Actions.Length);
				GUILayout.EndHorizontal();

				if (cs.OnStopped.Actions != null)
				{
					foreach (GameAction ga in cs.OnStopped.Actions)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label("GameAction: " + ga.name + " - " + ga.GetType().Name);
						GUILayout.EndHorizontal();
					}
				}
				else
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("GameAction is null or zero length");
					GUILayout.EndHorizontal();
				}

				if (cs.m_Tracks.Count() > 0)
				{
					cs.m_Tracks.ForEach(item =>

					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label("m_Track item: " + item.m_Commands.ElementAt<CommandReference>(1));
						GUILayout.EndHorizontal();

					}

					);
				}


				BlueprintEtude be2 = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>("c7c76f31f6574c7f9efa5d4ec5b7f846");

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("actions: " + ((EtudePlayTrigger)be2.Components[0]).Actions.Actions.Length);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("actions 2: " + ((DeactivateTrigger)be2.Components[1]).Actions.Actions.Length);
				GUILayout.EndHorizontal();









				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				//GUILayout.Label("str: (StartSex.Run)" + DialogCreate.str);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				//GUILayout.Label("str2: (test()) " + DialogCreate.str2);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				//GUILayout.Label("CLAPn: " + Game.Instance.CurrentlyLoadedAreaPart.name);
				GUILayout.EndHorizontal();


				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Create Yrliet Dialog", GUILayout.Width(200f), GUILayout.Height(20f)))
				{
					DialogCreate.CreateYrlietDialog();
					//DialogCreate.ReplaceYrlietMeditationActionHolder();
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Meditation", GUILayout.Width(200f), GUILayout.Height(20f)))
				{
					Cutscene cs1 = ResourcesLibrary.TryGetBlueprint<Cutscene>("fe236c09e1d04d088a1c69973b2db010");
					CutscenePlayerView.Play(cs1);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Find sex answer", GUILayout.Width(200f), GUILayout.Height(20f)))
				{

					//BlueprintEtude et = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>("c7c76f31f6574c7f9efa5d4ec5b7f846");
					//Game.Instance.Player.EtudesSystem.StartEtude(et);

				}
				GUILayout.EndHorizontal();



				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("UNLOCKEDFLAGS:");
				GUILayout.EndHorizontal();
				foreach (KeyValuePair<BlueprintUnlockableFlag, int> unlockedFlag in Game.Instance.Player.UnlockableFlags.UnlockedFlags)
				{
					if (unlockedFlag.Key.name.Contains("Meditation"))
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label(unlockedFlag.Key.name + " : " + unlockedFlag.Value);
						GUILayout.EndHorizontal();
					}
				}

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Increase Meditation", GUILayout.Width(200f), GUILayout.Height(20f)))
				{
					foreach (KeyValuePair<BlueprintUnlockableFlag, int> unlockedFlag in Game.Instance.Player.UnlockableFlags.UnlockedFlags)
					{
						if (unlockedFlag.Key.name.Equals("Meditation"))
						{
							Game.Instance.Player.UnlockableFlags.SetFlagValue(unlockedFlag.Key, Game.Instance.Player.UnlockableFlags.GetFlagValue(unlockedFlag.Key) + 1); ;
						}
					}
				}

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("STOP CUTSCENE!!!!!!!", GUILayout.Width(200f), GUILayout.Height(20f)))
				{
					Game.Instance.GameCommandQueue.SkipCutscene();
				}

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Replace Yrliet bark", GUILayout.Width(200f), GUILayout.Height(20f)))
				{
					Cutscene cs2 = ResourcesLibrary.TryGetBlueprint<Cutscene>("fe236c09e1d04d088a1c69973b2db010");
					CutscenePlayerView.Play(cs2);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("s: " + s);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Save portraits", GUILayout.Width(200f), GUILayout.Height(20f)))
				{




					string dir = Path.Combine(CustomPortraitsManager.PortraitsRootFolderPath, "NpcPortrait - " + "Idira_Changed");

					Directory.CreateDirectory(dir);


					BlueprintPortrait bp = ResourcesLibrary.TryGetBlueprint<BlueprintPortrait>("7f06e09da991435fbe188cd9c38f39f3");

					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.FullLengthPortrait, Path.Combine(dir, "Fulllength.png")));
					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.HalfLengthPortrait, Path.Combine(dir, "Medium.png")));
					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.SmallPortrait, Path.Combine(dir, "Small.png")));



					dir = Path.Combine(CustomPortraitsManager.PortraitsRootFolderPath, "NpcPortrait - " + "Jae_Changed");

					Directory.CreateDirectory(dir);

					bp = ResourcesLibrary.TryGetBlueprint<BlueprintPortrait>("76169c62a9a848f8b4ac67a1cc19d6f1");

					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.FullLengthPortrait, Path.Combine(dir, "Fulllength.png")));
					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.HalfLengthPortrait, Path.Combine(dir, "Medium.png")));
					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.SmallPortrait, Path.Combine(dir, "Small.png")));


					dir = Path.Combine(CustomPortraitsManager.PortraitsRootFolderPath, "NpcPortrait - " + "Jae");

					Directory.CreateDirectory(dir);

					bp = ResourcesLibrary.TryGetBlueprint<BlueprintPortrait>("a0c6745df40a492391e848d090f4a861");

					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.FullLengthPortrait, Path.Combine(dir, "Fulllength.png")));
					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.HalfLengthPortrait, Path.Combine(dir, "Medium.png")));
					LoadingProcess.Instance.StartCoroutine(Helpers.ExportSprite(bp.Data.SmallPortrait, Path.Combine(dir, "Small.png")));


				}
				GUILayout.EndHorizontal();
		#endif
			
		}


        /*
foreach (BaseUnitEntity bue in Game.Instance.Player.AllCharacters)
{
}
*/
        /*
		foreach (Quest quest in Game.Instance.Player.QuestBook.Quests)
		{
			foreach (BlueprintQuestObjective objective in quest.Blueprint.AllObjectives)
			{

				if (objective.name.ToLower().Contains("aeldari"))
				{
					QuestObjectiveState objectiveState = Game.Instance.Player.QuestBook.GetObjectiveState(objective);
					if (objectiveState == QuestObjectiveState.Started)
					{
						Game.Instance.Player.QuestBook.GiveObjective(objective);
						Game.Instance.Player.QuestBook.CompleteObjective(objective);

						CheatsUnlock.CompleteObjective(objective);
					}
				}
			}
		}
		*/




        //-:cnd:noEmit
#if DEBUG
		static bool OnUnload(UnityModManager.ModEntry modEntry)
        {
            HarmonyInstance.UnpatchAll(modEntry.Info.Id);
            return true;
        }
#endif

        public static bool isModEnabled;
		public static bool isRTLoaded = false;

		private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
		{

			Main.isModEnabled = value;


			return true;
		}

		private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
		{
			Main.settings.Save(modEntry);
		}
		private static void OnShowGUI(UnityModManager.ModEntry modEntry)
		{
			Main.settings.dev = false;
		}
        private static void OnHideGUI(UnityModManager.ModEntry modEntry)
		{
			Main.settings.Save(modEntry);


			if (ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>("2e582cda6d9846b0ab10a338adadd945") != null)
			{
				BlueprintAnswersList answersList = ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>("2e582cda6d9846b0ab10a338adadd945");

				// BlueprintAnswer ba = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>("d35f72efa8cd4b6a995e87ca9ded7d9e");

				foreach (BlueprintAnswer ba in answersList.Answers)
				{
					if (ba.name.Contains("simpledialog"))
					{

						ConditionsHolder conditionsHolder3 = ResourcesLibrary.TryGetScriptable<ConditionsHolder>("dfb41c35-b930-4d6b-9d27-f34b44f403fc");
						if (conditionsHolder3 == null)
						{
							conditionsHolder3 = Helpers.CreateAndAddESO<ConditionsHolder>("SexYrliet", "dfb41c35-b930-4d6b-9d27-f34b44f403fc");
						}


						if (settings.male && settings.female)
						{
							var specificInstances = ba.ShowConditions.Conditions.OfType<PcMale>();
							var specificInstances2 = ba.ShowConditions.Conditions.OfType<PcFemale>();

							if (specificInstances.Count() > 0 || specificInstances2.Count() > 0)
                            {
								List<Condition> list = new List<Condition>(ba.ShowConditions.Conditions);

								Condition instanceToRemove = list.Find(item => item is PcFemale || item is PcMale);

								if (instanceToRemove != null)
									list.Remove(instanceToRemove);

								ba.ShowConditions.Conditions = list.ToArray();
							}

						}
						else if (settings.female)
						{
							var specificInstances = ba.ShowConditions.Conditions.OfType<PcFemale>();
						
							if (specificInstances.Count() == 0)
							{
								List<Condition> list = new List<Condition>(ba.ShowConditions.Conditions);

								Condition instanceToRemove = list.Find(item => item is PcMale);

								if (instanceToRemove != null)
									list.Remove(instanceToRemove);

								var sex = Helpers.CreateElement<PcFemale>(conditionsHolder3);

								list.Add(sex);

								ba.ShowConditions.Conditions = list.ToArray();
							}

						}
						else if (settings.male)
						{
							var specificInstances = ba.ShowConditions.Conditions.OfType<PcMale>();
						
							if (specificInstances.Count() == 0)
							{
								List<Condition> list = new List<Condition>(ba.ShowConditions.Conditions);

								Condition instanceToRemove = list.Find(item => item is PcFemale);

								if (instanceToRemove != null)
									list.Remove(instanceToRemove);

								var sex = Helpers.CreateElement<PcMale>(conditionsHolder3);

								list.Add(sex);

								ba.ShowConditions.Conditions = list.ToArray();
							}

						}

					}
				}
			}







		}

		//+:cnd:noEmit
	}

}