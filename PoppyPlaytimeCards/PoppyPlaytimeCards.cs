using System.Collections;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using PoppyPlaytimeCards.Card;
using PoppyPlaytimeCards.Card.Base;
using PoppyPlaytimeCards.Util;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;

namespace PoppyPlaytimeCards
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInDependency("pykess.rounds.plugins.pickncards")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInDependency("pykess.rounds.plugins.playerjumppatch")]
    [BepInDependency("pykess.rounds.plugins.legraycasterspatch")]
    [BepInDependency("root.classes.manager.reborn")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class PoppyPlaytimeCards : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.poppyplaytimecards";
        private const string ModName = "Poppy Playtime Cards";
        public const string Version = "1.0.1";
        public const string ModInitials = "PPC";
        private const string CompatibilityModName = "PoppyPlaytimeCards";
        public static PoppyPlaytimeCards Instance { get; private set; }
        private const bool DEBUG = false;

        public static List<GameObject> cobwebs = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
            
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            ThemeManager.LoadThemes();
        }

        private void Start()
        {
            CustomCard.BuildCard<GrabPackCard>(card => GrabPackCard.Card = card);

            CustomCard.BuildCard<BunzoBunnyCard>(card => BunzoBunnyCard.Card = card);
            CustomCard.BuildCard<HuggyWuggyCard>(card => HuggyWuggyCard.Card = card);
            CustomCard.BuildCard<KissyMissyCard>(card => KissyMissyCard.Card = card);
            CustomCard.BuildCard<MiniHuggiesCard>(card => MiniHuggiesCard.Card = card);
            CustomCard.BuildCard<MommyLongLegsCard>(card => MommyLongLegsCard.Card = card);

            ModdingUtils.Utils.Cards.instance.AddOnRemoveCallback(MinionBaseCard.OnRemoveCallback);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, MinionBaseCard.WaitForAIs);

            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PickEnd);
        }
        
        public void Log(string debug)
        {
            if (DEBUG) UnityEngine.Debug.Log(debug);
        }

        public static IEnumerator PickEnd(IGameModeHandler gm)
        {
            foreach (var cobweb in cobwebs)
            {
                Destroy(cobweb);
            }
            cobwebs = new List<GameObject>();
            yield return null;
        }
    }
}