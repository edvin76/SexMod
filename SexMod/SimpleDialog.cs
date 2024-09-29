//
// Hambeard's SimpleDialog classes for creating dialog for NPC's that have bark
// The only thing special that is needed is a publicized version of Assembly-CSharp_public
// Follow the Wiki: https://github.com/WittleWolfie/OwlcatModdingWiki/wiki/Publicize-Assemblies on how to accomplish generating your own. 
//

//using HarmonyLib;
using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Dialog;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SexMod.Helpers;
//using CustomNpcPortraits.Simpledialogs;

namespace SexMod
{




    [HarmonyPatch(typeof(StartDialog), "Dialogue", MethodType.Getter)]
    public static class DialogOnClick_Constructor_Patch
    {

        //   [HarmonyPriority(Priority.First)]
        //   [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static bool Prefix(StartDialog __instance, ref BlueprintDialog __result, /*UnitEvaluator ___DialogueOwner,*/ BlueprintEvaluator ___DialogEvaluator)
        {
            /*Main.DebugLog("startd pre");

            if (___DialogueOwner != null)
                Main.DebugLog("(getter) "+___DialogueOwner.GetValue().Proxy.Id);
            else
                Main.DebugLog("(getter) " + __instance.Owner.name);


          */
            return true;
        }

        public static void Postfix(StartDialog __instance)
        {
            /*
            if (__instance.DialogueOwner != null)
                Main.DebugLog("3 const post: " + __instance.DialogueOwner.GetValue().Proxy.Id);
            else
                Main.DebugLog("3 const post no");


            if (__instance.DialogEvaluator != null)
                Main.DebugLog("3 const post: " + __instance.DialogEvaluator.Owner.name);
            else
                Main.DebugLog("3 const post no 2");
            */


        }


    }
    

    [HarmonyPatch(typeof(StartDialog), "RunAction", MethodType.Normal)]
    public static class DialogOnClick_Patch
    {

        //   [HarmonyPriority(Priority.First)]
        //   [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Postfix(StartDialog __instance)
        {
            /*
            UIUtility.SendWarning(Game.Instance.CursorController.m_HoveredUnitData.CharacterName);


            if (__instance.DialogueOwner != null)
                Main.DebugLog("2 const post: " + __instance.DialogueOwner.GetValue().Proxy.Id);
            else
                Main.DebugLog("2 const post no");


            if (__instance.DialogEvaluator != null)
                Main.DebugLog("2 const post: " + __instance.DialogEvaluator.Owner.name);
            else
                Main.DebugLog("2 const post no 2");
         */
        }

        public static bool Prefix(StartDialog __instance,/* UnitEvaluator ___DialogueOwner, */BlueprintEvaluator ___DialogEvaluator)
        {
            /*
            if (__instance.DialogueOwner != null)
                Main.DebugLog("const post: " + __instance.DialogueOwner.GetValue().Proxy.Id);
            else
                Main.DebugLog("const post no");


            if (__instance.DialogEvaluator != null)
                Main.DebugLog("const post: " + __instance.DialogEvaluator.Owner.name);
            else
                Main.DebugLog("const post no 2");
            */

            return true;

        }
    }



    //
    // Just some helpers to ensure there are no null objects in the dialog trees
    //
    public static class Empties
    {
        public static readonly ActionList Actions = new ActionList() { Actions = new GameAction[0] };
        public static readonly ConditionsChecker Conditions = new ConditionsChecker() { Conditions = new Condition[0] };
        public static readonly ContextDiceValue DiceValue = new ContextDiceValue()
        {
            DiceType = DiceType.Zero,
            DiceCountValue = 0,
            BonusValue = 0
        };
        public static readonly LocalizedString String = new LocalizedString();
        public static readonly PrefabLink PrefabLink = new PrefabLink();
        public static readonly ShowCheck ShowCheck = new ShowCheck();
        public static readonly CueSelection CueSelection = new CueSelection();
        public static readonly CharacterSelection CharacterSelection = new CharacterSelection();
        public static readonly DialogSpeaker DialogSpeaker = new DialogSpeaker() { NoSpeaker = true };
    }

    //
    // This is main class that will assit in building a very simple dialog tree. Expand as you see fit.
    //
    public class SimpleDialogBuilder
    {
        // This method must be called first to start building the dialog tree. The other methods will create the other parts of the tree where you need them. 
        public static BlueprintCue baseCue;


        public static BlueprintDialogReference CreateDialog(string name, string guid, List<BlueprintCueBaseReference> firstCue)
        {
            var dialog = Helpers.CreateAndAdd<BlueprintDialog>(name, guid);
            dialog.FirstCue = new CueSelection()
            {
                Cues = firstCue,
                Strategy = Strategy.First
            };

            dialog.Conditions = Empties.Conditions;
            dialog.StartActions = Empties.Actions;
            dialog.FinishActions = Empties.Actions;
            dialog.ReplaceActions = Empties.Actions;

            return dialog.ToReference<BlueprintDialogReference>();
        }

        public static BlueprintAnswerBaseReference CreateAnswer(string name, string guid, string text, List<BlueprintCueBaseReference> nextCues, ConditionsChecker conditionsChecker = null, ActionList onSelect = null, bool showOnce = false)
        {
            var answer = Helpers.CreateAndAdd<BlueprintAnswer>(name, guid);
            answer.Text = CreateString(name, text);
            answer.NextCue = new CueSelection()
            {
                Cues = nextCues,
                Strategy = Strategy.First
            };

            answer.ShowCheck = Empties.ShowCheck;
            if (conditionsChecker == null)
                answer.ShowConditions = Empties.Conditions;
            else
                answer.ShowConditions = conditionsChecker;
            answer.SelectConditions = Empties.Conditions;
            if (onSelect == null)
                answer.OnSelect = Empties.Actions;
            else
                answer.OnSelect = onSelect;
            answer.FakeChecks = new CheckData[0];
            answer.CharacterSelection = Empties.CharacterSelection;

            answer.ShowOnce = showOnce;


            return answer.ToReference<BlueprintAnswerBaseReference>();
        }

        public static BlueprintCueBaseReference CreateCue(string name, string guid, BlueprintUnitReference speaker, string text, List<BlueprintAnswerBaseReference> answerList = null, CueSelection cueSelection = null, ActionList onStop = null)
        {
            var cue = CreateAndAdd<BlueprintCue>(name, guid);
            cue.Text = CreateString(name, text);

            cue.Speaker = new DialogSpeaker()
            {
                m_Blueprint = speaker,
                MoveCamera = true
            };

            cue.Answers = answerList;
            cue.Continue = cueSelection;

            if (cue.Text is null)
                cue.Text = Empties.String;
            if (cue.Speaker is null)
                cue.Speaker = Empties.DialogSpeaker;
            if (cue.Answers is null)
                cue.Answers = new List<BlueprintAnswerBaseReference>();
            if (cue.Continue is null)
                cue.Continue = new CueSelection();

            cue.Conditions = Empties.Conditions;
            cue.m_Listener = Activator.CreateInstance<BlueprintUnitReference>();
            cue.OnShow = Empties.Actions;
            if (onStop == null)
                cue.OnStop = Empties.Actions;
            else
                cue.OnStop = onStop;

            return cue.ToReference<BlueprintCueBaseReference>();
        }

        public static BlueprintAnswerBaseReference CreateAnswerList(string name, string guid, List<BlueprintAnswerBaseReference> answers)
        {
            var answerList = Helpers.CreateAndAdd<BlueprintAnswersList>(name, guid);
            answerList.Answers = answers;

            if (answerList.Answers is null)
                answerList.Answers = new List<BlueprintAnswerBaseReference>();

            answerList.Conditions = Empties.Conditions;

            return answerList.ToReference<BlueprintAnswerBaseReference>();
        }

        //
        // Creates a new blueprint T and adds it to the games BlueprintCache. Each blueprint needs a unique AssetGuid. I just use the Tools->Create GUID tool included in Visual Studio
        // to generate the guid
        //

        private static T Create<T>(string name, string guid) where T : SimpleBlueprint, new()
        {
            T asset = new T()
            {
                name = name,
                AssetGuid = guid.ToGUID()
            };

            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(guid.ToGUID(), asset);

            return asset;
        }




    }

}
