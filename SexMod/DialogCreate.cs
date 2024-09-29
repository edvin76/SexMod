using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Code.UI.MVVM.VM.Fade;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SexMod
{
    class DialogCreate
    {
        private static IEnumerator FadeOutCoroutine()
        {
            FadeCanvas.Fadeout(true);

            yield return new WaitForSeconds((float)2.5);

            FadeCanvas.Fadeout(false);

            Game.Instance.TimeController.AdvanceGameTime(new TimeSpan(2, 0, 0));

            yield return null;
            
        }





        public class StartSex : GameAction
        {

            public override void RunAction()
            {
                //Game.Instance.RootUiContext.Fadeout(true);

                //LoadingProcess.Instance.StartLoadingProcess(FadeOutCoroutine(), () => test(), LoadingProcessTag.TeleportParty);
                
                LoadingProcess.Instance.StartCoroutine(FadeOutCoroutine());


                // Game.ReloadAreaMechanic(false, null);
                /*
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);
                Game.ReloadAreaMechanic(false, null);*/

                // UIUtility.SendWarning(Game.Instance.Player.GameTime.ToString());
                // UIUtility.SendWarning(Game.Instance.Player.GameTime.ToString());
        
            }

            public override string GetCaption()
            {

                return "";
            }
        }

        public class YrlietSexPortrait : GameAction
        {

            public override void RunAction()
            {
                //BlueprintUnit bpUnit = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("20c5ce9f1e2bcf9448a7a0fd0850f5d2");


                //BlueprintUnit newBpUnit = Helpers.CopyAndAdd(bpUnit, "YrlietHot", "5c2f4bd5-2259-4a05-9080-1aba05bfb13e");

                foreach (BaseUnitEntity unit in Game.Instance.State.AllBaseAwakeUnits)
                {
                    if (unit.CharacterName.Equals(Main.YrlietName))
                        if (File.Exists(Path.Combine(Helpers.GetPortraitsDir(), "Yrliet", "Medium.png")))
                        {
                            //  Log("FOUND - " + unit.Blueprint.name);

                            BlueprintPortrait bp = new BlueprintPortrait();

                            bp.Data = new PortraitData(Path.Combine(Helpers.GetPortraitsDir(), "Yrliet"));


                            bp.AssetGuid = "6bb0625b-959f-4077-914a-7add6fee0aca".ToGUID();

                            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(bp.AssetGuid, bp as SimpleBlueprint);

                            //bpUnit.m_Portrait = bp.ToReference<BlueprintPortraitReference>();

                            unit.UISettings.SetPortrait(bp);

                        }
                }
            }

            public override string GetCaption()
            {

                return "";
            }
        }

        public class YrlietNormalPortrait : GameAction
        {

            public override void RunAction()
            {
                //BlueprintUnit bpUnit = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("20c5ce9f1e2bcf9448a7a0fd0850f5d2");

                foreach (BaseUnitEntity unit in Game.Instance.State.AllBaseAwakeUnits)
                {
                    if (unit.CharacterName.Equals(Main.YrlietName))
                    {
                        BlueprintPortrait bp = ResourcesLibrary.TryGetBlueprint<BlueprintPortrait>("b57e63a38dd04b59ba3e9356d6b2448b");

                        if (bp != null)
                        {

                            //bpUnit.m_Portrait = bp.ToReference<BlueprintPortraitReference>();

                            unit.UISettings.SetPortrait(bp);

                        }
                    }
                }
            }

            public override string GetCaption()
            {

                return "";
            }
        }

        public static void CreateYrlietDialog_old()
        {

            //{mf|Master|Mistress}
            BlueprintDialogReference dialog;

            BlueprintUnitReference bref = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("20c5ce9f1e2bcf9448a7a0fd0850f5d2").ToReference<BlueprintUnitReference>();



            if (ResourcesLibrary.TryGetBlueprint<BlueprintDialog>("3180a5aeae0c484a8bfe95acf459878c") != null)
            {
                dialog = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>("3180a5aeae0c484a8bfe95acf459878c").ToReference<BlueprintDialogReference>();
                //Update
            }
            else
            {
                ActionsHolder ActionsHolder = ResourcesLibrary.TryGetScriptable<ActionsHolder>("aac2a168-0bbb-4058-ba39-a2a636cf6b03");
                if (ActionsHolder == null)
                {
                    ActionsHolder = Helpers.CreateAndAddESO<ActionsHolder>("Sex_Yrliet", "aac2a168-0bbb-4058-ba39-a2a636cf6b03");
                }
                //   Log("ActionName: "+ActionName);
                var startSex = Helpers.CreateElement<StartSex>(ActionsHolder);


                ActionsHolder ActionsHolder2 = ResourcesLibrary.TryGetScriptable<ActionsHolder>("632c63d9-3655-4649-b1da-e3c0d90fdf72");
                if (ActionsHolder2 == null)
                {
                    ActionsHolder2 = Helpers.CreateAndAddESO<ActionsHolder>("SexPortrait_Yrliet", "632c63d9-3655-4649-b1da-e3c0d90fdf72");
                }
                //   Log("ActionName: "+ActionName);
                var yrlietSexPortrait = Helpers.CreateElement<YrlietSexPortrait>(ActionsHolder2);


                ActionsHolder ActionsHolder3 = ResourcesLibrary.TryGetScriptable<ActionsHolder>("c7c6d538-5d5f-4393-ada3-6263ae5073f7");
                if (ActionsHolder3 == null)
                {
                    ActionsHolder3 = Helpers.CreateAndAddESO<ActionsHolder>("NormalPortrait_Yrliet", "c7c6d538-5d5f-4393-ada3-6263ae5073f7");
                }
                //   Log("ActionName: "+ActionName);
                var yrlietNormalPortrait = Helpers.CreateElement<YrlietNormalPortrait>(ActionsHolder3);


                dialog = SimpleDialogBuilder.CreateDialog
                (
                    name: "simpledialog.yrliet.base",
                    guid: "3180a5ae-ae0c-484a-8bfe-95acf459878c",
                    firstCue: new List<BlueprintCueBaseReference>()
                    {
                        SimpleDialogBuilder.CreateCue
                        (
                            name: "simpledialog.yrliet.greet",
                            guid: "0a5b9ac7-217c-4db6-bfb7-766cb7fa8e37", // this guid is referenced below
                            speaker: bref,
                            text: "\"Elantach?\"",
                            answerList: new List<BlueprintAnswerBaseReference>()
                            {
                                SimpleDialogBuilder.CreateAnswerList
                                (
                                    name: "simpledialog.yrliet.greet.answerlist",
                                    guid: "19AF2283-3563-460D-BE36-D4E0D582B7FC",
                                    answers: new List<BlueprintAnswerBaseReference>()
                                    {

                                        SimpleDialogBuilder.CreateAnswer
                                        (
                                            name: "simpldialog.yrliet.greet.answer.sex",
                                            guid: "d35f72ef-a8cd-4b6a-995e-87ca9ded7d9e",
                                            text: "\"Yrliet, I can't meditate, because I can't clear my mind. I have all these sexual thoughts about you and they fill my mind.\"",
                                            onSelect: new ActionList()
                                            {
                                                Actions = new GameAction[]
                                                {
                                                    yrlietSexPortrait
                                                }
                                            },
                                            nextCues: new List<BlueprintCueBaseReference>()
                                            {
                                                SimpleDialogBuilder.CreateCue
                                                (
                                                    name: "simpledialog.yrliet.loop.sex",
                                                    guid: "874e6a3e-3184-4457-aebb-7b2984ddf16f",
                                                    speaker: bref,
                                                    text: "\"I could give you a quick blowjob Elantach, every time before we meditate together, but I hope you realize that in the case of any other mon-keigh that would be practically zoophilia from my part. But in your case, you being an evolved being, I don't necessary mind it at all, if you don't mind it either. In fact I would feel honored to allow the eruption of your seminal fluids to grace my taste buds with their presence. Wouldn't you know in some Aeldari circles, such delicacy (and the traditional way of aquiring it) is considered to be an exquisite culinary treat.\"",
                                                    answerList: new List<BlueprintAnswerBaseReference>()
                                                    {
                                                        SimpleDialogBuilder.CreateAnswerList
                                                        (
                                                            name: "simpledialog.yrliet.greet.answerlist",
                                                            guid: "d806156c-4b82-4979-8e77-ecf325a295bc",
                                                            answers: new List<BlueprintAnswerBaseReference>()
                                                            {
                                                                SimpleDialogBuilder.CreateAnswer
                                                                (
                                                                    name: "simpledialog.yrliet.greet.answer.sex",
                                                                    guid: "1ec21e87-6f9f-4cc1-946d-9ea5c63af96d",
                                                                    text: "\"Sweet! I don't mind at all!\"",
                                                                    onSelect: new ActionList()
                                                                    {
                                                                        Actions = new GameAction[]
                                                                        {
                                                                            startSex
                                                                        }
                                                                    },
                                                                    nextCues: new List<BlueprintCueBaseReference>()
                                                                    {
                                                                        SimpleDialogBuilder.CreateCue
                                                                        (
                                                                            name: "simpledialog.yrliet.loop.1",
                                                                            guid: "C2F218D7-AB52-4313-A582-C39986CC4450",
                                                                            speaker: bref,
                                                                            text: "what is here?",
                                                                            cueSelection: new Kingmaker.DialogSystem.CueSelection()
                                                                            {
                                                                                Cues = new List<BlueprintCueBaseReference>()
                                                                                {
                                                                                    new BlueprintCueBaseReference()
                                                                                    {
                                                                                        guid = "0A5B9AC7-217C-4DB6-BFB7-766CB7FA8E37".ToGUID() // loop back to the beginning.
                                                                                    }
                                                                                },
                                                                                Strategy = Strategy.First
                                                                            }
                                                                        )
                                                                    }
                                                                ),
                                                                SimpleDialogBuilder.CreateAnswer
                                                                (
                                                                    name: "simpledialog.yrliet.exit",
                                                                    guid: "52e84e46-8144-4a82-b850-c7b562a5ce21",
                                                                    text: "\"See you!\"",
                                                                    onSelect: new ActionList()
                                                                    {
                                                                        Actions = new GameAction[]
                                                                        {
                                                                            yrlietNormalPortrait
                                                                        }
                                                                    },
                                                                    nextCues: new List<BlueprintCueBaseReference>()
                                                                    {
                                                                        SimpleDialogBuilder.CreateCue
                                                                        (
                                                                            name: "simpledialog.yrliet.exit.response",
                                                                            guid: "defe9f7c-684e-4c71-9e67-c00fd2b72115",
                                                                            speaker: bref,
                                                                            text: "\"Namárië, Elantach!\""
                                                                        )
                                                                    }
                                                                )
                                                            }
                                                        )
                                                    }
                                                )
                                            }
                                        )
                                    }
                                )
                            }
                        )
                    }
                );
            }



        }


        public static void CreateYrlietDialog()
        {

            //{mf|Master|Mistress}
            BlueprintDialogReference dialog;

            BlueprintAnswerBaseReference bAR;

            BlueprintUnitReference bref = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("20c5ce9f1e2bcf9448a7a0fd0850f5d2").ToReference<BlueprintUnitReference>();


            

            BlueprintCue Cue_32 = ResourcesLibrary.TryGetBlueprint<BlueprintCue>("f1b4fe7d985b4cc480193cf3449b1b67");
            Cue_32.Speaker.m_Blueprint = bref;

            Cue_32.Speaker.m_SpeakerPortrait = bref;


            if (ResourcesLibrary.TryGetBlueprint<BlueprintDialog>("3180a5aeae0c484a8bfe95acf459878c") != null)
            {
                dialog = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>("3180a5aeae0c484a8bfe95acf459878c").ToReference<BlueprintDialogReference>();
                //Update
            }
            else
            {
                ActionsHolder ActionsHolder = ResourcesLibrary.TryGetScriptable<ActionsHolder>("aac2a168-0bbb-4058-ba39-a2a636cf6b03");
                if (ActionsHolder == null)
                {
                    ActionsHolder = Helpers.CreateAndAddESO<ActionsHolder>("Sex_Yrliet", "aac2a168-0bbb-4058-ba39-a2a636cf6b03");
                }
                //   Log("ActionName: "+ActionName);
                var startSex = Helpers.CreateElement<StartSex>(ActionsHolder);


                ActionsHolder ActionsHolder2 = ResourcesLibrary.TryGetScriptable<ActionsHolder>("632c63d9-3655-4649-b1da-e3c0d90fdf72");
                if (ActionsHolder2 == null)
                {
                    ActionsHolder2 = Helpers.CreateAndAddESO<ActionsHolder>("SexPortrait_Yrliet", "632c63d9-3655-4649-b1da-e3c0d90fdf72");
                }
                //   Log("ActionName: "+ActionName);
                var yrlietSexPortrait = Helpers.CreateElement<YrlietSexPortrait>(ActionsHolder2);


                ActionsHolder ActionsHolder3 = ResourcesLibrary.TryGetScriptable<ActionsHolder>("c7c6d538-5d5f-4393-ada3-6263ae5073f7");
                if (ActionsHolder3 == null)
                {
                    ActionsHolder3 = Helpers.CreateAndAddESO<ActionsHolder>("NormalPortrait_Yrliet", "c7c6d538-5d5f-4393-ada3-6263ae5073f7");
                }
                //   Log("ActionName: "+ActionName);
                var yrlietNormalPortrait = Helpers.CreateElement<YrlietNormalPortrait>(ActionsHolder3);


                ConditionsHolder conditionsHolder = ResourcesLibrary.TryGetScriptable<ConditionsHolder>("64ff1c09-802c-4944-820e-51172b5e6bc1");
                if (conditionsHolder == null)
                {
                    conditionsHolder = Helpers.CreateAndAddESO<ConditionsHolder>("SexYrliet", "64ff1c09-802c-4944-820e-51172b5e6bc1");
                }
                var currentAreaPartIs = Helpers.CreateElement<CurrentAreaPartIs>(conditionsHolder);
                var rtCabin = ResourcesLibrary.TryGetBlueprint<BlueprintAreaPart>("9fd386ae7b34415792b03c174931980f");
                currentAreaPartIs.m_AreaPart = rtCabin.ToReference<BlueprintAreaPartReference>();


                ConditionsHolder conditionsHolder3 = ResourcesLibrary.TryGetScriptable<ConditionsHolder>("dfb41c35-b930-4d6b-9d27-f34b44f403fc");
                if (conditionsHolder3 == null)
                {
                    conditionsHolder3 = Helpers.CreateAndAddESO<ConditionsHolder>("SexYrliet", "dfb41c35-b930-4d6b-9d27-f34b44f403fc");
                }
                var pcMale = Helpers.CreateElement<PcMale>(conditionsHolder3);


                bAR = SimpleDialogBuilder.CreateAnswer
                 (
                     name: "simpldialog.yrliet.greet.answer.sex",
                     guid: "d35f72ef-a8cd-4b6a-995e-87ca9ded7d9e",
                     text: "\"Yrliet, I can't meditate, because I can't clear my mind. I have all these sexual thoughts about you and they fill my mind.\"",

                     conditionsChecker: new ConditionsChecker()
                     {
                        Conditions = new Condition[]
                        {
                            currentAreaPartIs, pcMale
                        }
                     },
                     nextCues: new List<BlueprintCueBaseReference>()
                     {
                        SimpleDialogBuilder.CreateCue
                        (
                            name: "simpledialog.yrliet.loop.sex",
                            guid: "874e6a3e-3184-4457-aebb-7b2984ddf16f",
                            speaker: bref,
                            text: "{n}Yrliet slightly raises one of her eyebrows, then slowly starts talking, as if she were talking about a secret cooking recipe only known to a select few. {/n}\"I could give you a long and thorough blowjob, Elantach, every time before we meditate together, but I hope you realize that in the case of any other mon-keigh that would be practically zoophilia on my part. But in your case, being an evolved being, I don't necessarily mind it at all, if you don't mind it either. I would feel honored to allow the eruption of your seminal fluids to grace my taste buds with their presence. Wouldn't you know that in some Aeldari circles, such a delicacy (and the traditional way of acquiring it) is considered an exquisite culinary treat?\"",
                            answerList: new List<BlueprintAnswerBaseReference>()
                            {
                                SimpleDialogBuilder.CreateAnswerList
                                (
                                    name: "simpledialog.yrliet.greet.answerlist",
                                    guid: "d806156c-4b82-4979-8e77-ecf325a295bc",
                                    answers: new List<BlueprintAnswerBaseReference>()
                                    {
                                        SimpleDialogBuilder.CreateAnswer
                                        (
                                            name: "simpledialog.yrliet.greet.answer.sex",
                                            guid: "1ec21e87-6f9f-4cc1-946d-9ea5c63af96d",
                                            text: "\"Sweet! I don't mind it at all!\"",
                                            onSelect: new ActionList()
                                            {
                                                Actions = new GameAction[]
                                                {
                                                    startSex,
                                                    yrlietSexPortrait
                                                }
                                            },
                                                                    
                                            nextCues: new List<BlueprintCueBaseReference>()
                                            {
                                                SimpleDialogBuilder.CreateCue
                                                (
                                                    name: "simpledialog.yrliet.loop.1",
                                                    guid: "C2F218D7-AB52-4313-A582-C39986CC4450",
                                                    speaker: bref,
                                                    text: "{n}Most of the fluids filled up her mouth, but at some point, the forceful tension caused the virile manhood to pop out of the embrace of her lips and spray all over her face and chest. Still, one eye shut, closed by the viscous nature of the liquid, she used her long and elegant finger to scoop it off from wherever she could, then she licked it off, savoring it like it was some millennia-old wine. Finally, she sighs: {/n}\"That was exquisite, {name}...\"",
                                                                            
                                                    onStop: new ActionList()
                                                    {
                                                        Actions = new GameAction[]
                                                        {
                                                            yrlietNormalPortrait
                                                        }
                                                    },
                                                    cueSelection: new Kingmaker.DialogSystem.CueSelection()
                                                    {
                                                        Cues = new List<BlueprintCueBaseReference>()
                                                        {
                                                            new BlueprintCueBaseReference()
                                                            {
                                                                guid = "0A5B9AC7-217C-4DB6-BFB7-766CB7FA8E37".ToGUID() // loop back to the beginning.
                                                            }
                                                        },
                                                        Strategy = Strategy.First
                                                    }
                                                )
                                            }
                                        ),
                                        SimpleDialogBuilder.CreateAnswer
                                        (
                                            name: "simpledialog.yrliet.exit",
                                            guid: "52e84e46-8144-4a82-b850-c7b562a5ce21",
                                            text: "\"Maybe next time.\"",
                                            nextCues: new List<BlueprintCueBaseReference>()
                                            {
                                                SimpleDialogBuilder.CreateCue
                                                (
                                                    name: "simpledialog.yrliet.exit.response",
                                                    guid: "defe9f7c-684e-4c71-9e67-c00fd2b72115",
                                                    speaker: bref,
                                                    text: "\"Namárië, Elantach!\""
                                                )
                                            }
                                        )
                                    }
                                )
                            }
                        )
                     }
                 );


                if (ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>("2e582cda6d9846b0ab10a338adadd945") != null)
                {
                    BlueprintAnswersList answersList = ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>("2e582cda6d9846b0ab10a338adadd945");

                    answersList.Answers.Insert(0,

                                     bAR


                        );

                }





            }




        }






        public static void CreateDialog_test(string name)
        {


            BlueprintUnitReference bref = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("20c5ce9f1e2bcf9448a7a0fd0850f5d2").ToReference<BlueprintUnitReference>();


            var dialog = SimpleDialogBuilder.CreateDialog("simpledialog." + name + ".base", "3180a5aeae0c484a8bfe95acf459878c", new List<BlueprintCueBaseReference>()
            {
                SimpleDialogBuilder.CreateCue("simpledialog."+name+".greet", "c065641a-a953-47b3-8025-8fdc1077caa0", bref, "{mf|Master|Mistress}, I am "+name+" who had no dialog but I can speak now!", new List<BlueprintAnswerBaseReference>()
                {
                    SimpleDialogBuilder.CreateAnswerList("simpledialog."+name+".greet.answerlist", "60730705-a03d-4961-8888-f2733da0a7db", new List<BlueprintAnswerBaseReference>()
                    {
                        SimpleDialogBuilder.CreateAnswer("simpledialog."+name+".greet.answer.good", "4b8ed33a-9df5-47b2-b720-6f6b6f5a4068", "You sound great!", new List<BlueprintCueBaseReference>()
                        {
                            SimpleDialogBuilder.CreateCue("simpledialog."+name+".exit.good", "363c076c-0cf9-4835-add6-aa6e16300f6e", bref, "Thanks, I am off to tell the world!", new List<BlueprintAnswerBaseReference>())
                        }),
                        SimpleDialogBuilder.CreateAnswer("simpledialog."+name+".greet.answer.bad", "f987522a-f9f4-48af-aa36-2de9eb0a8f1d", "I liked you better when you couldn't talk.", new List<BlueprintCueBaseReference>()
                        {
                            SimpleDialogBuilder.CreateCue("simpledialog."+name+".exit.bad", "23dcc062-fbb6-4b32-86b1-26a31cb7f968", bref, "Maybe I should have another chat with you.", new List<BlueprintAnswerBaseReference>())
                        })
                    })
                })
            });


        }









        public static void ReplaceYrlietMeditationActionHolder()
		{
            // Log($"Creating ActionHolder ...");

            string dguid = "6527c7c824ef4053a71ad198edc4869c";


			string aguid = "9ad049c3e6ae4d48bbc3bbf0c05b1f00";


			ActionsHolder ActionsHolder = ResourcesLibrary.TryGetScriptable<ActionsHolder>(aguid);



			//BlueprintCue bpCue = ResourcesLibrary.TryGetBlueprint<BlueprintCue>("f1b4fe7d985b4cc480193cf3449b1b67");


			//bpCue.Speaker.m_Blueprint = bpUnit.ToReference<BlueprintUnitReference>();

			//EldarCompanionMeditationBark_Actions
			//ActionsHolder!!!
			//9ad049c3e6ae4d48bbc3bbf0c05b1f00

			//insert
			//Ranger_Companion_dialog
			//6527c7c824ef4053a71ad198edc4869c

			//PortraiBlueprintUnitt
			//b57e63a38dd04b59ba3e9356d6b2448b

			//BpUnit
			//20c5ce9f1e2bcf9448a7a0fd0850f5d2

			//CurrentCue
			//f1b4fe7d985b4cc480193cf3449b1b67

			if (ActionsHolder != null)
			{

				var startDialog = Helpers.CreateElement<StartDialog>(ActionsHolder);


				startDialog.m_Dialogue = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(dguid).ToReference<BlueprintDialogReference>();

				//startDialog.SpeakerName = Helpers.CreateString("yrliet.name","xYrlietx");

				ActionsHolder.Actions = new ActionList()
				{
					Actions = new GameAction[]
					{
						startDialog
					}
				};



			}


			/*
				if (ActionsHolder == null)
			{
				ActionsHolder = CreateAndAddESO<ActionsHolder>(ActionName, ActionGuid);

				//   Log("ActionName: "+ActionName);

				var startDialog = CreateElement<StartDialog>(ActionsHolder);


				startDialog.m_Dialogue = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(dguid).ToReference<BlueprintDialogReference>();

				// Log($"Added dialog..." + startDialog.m_Dialogue.NameSafe());


				ActionsHolder.Actions = new ActionList()
				{
					Actions = new GameAction[]
					{
						startDialog
					}
				};
			}
				*/

		}




	}
}
