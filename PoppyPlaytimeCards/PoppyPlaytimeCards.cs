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
    [BepInDependency("root.cardtheme.lib")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class PoppyPlaytimeCards : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.poppyplaytimecards";
        private const string ModName = "Poppy Playtime Cards";
        public const string Version = "1.0.4";
        public const string ModInitials = "PPC";
        private const string CompatibilityModName = "PoppyPlaytimeCards";
        public static PoppyPlaytimeCards Instance { get; private set; }
        private const bool DEBUG = false;
        
        public static List<GameObject> Cobwebs = new();

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

            // Minion setup
            ModdingUtils.Utils.Cards.instance.AddOnRemoveCallback(MinionBaseCard.OnRemoveCallback);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, MinionBaseCard.WaitForAIs);

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickStart, ResetEffects);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, ResetEffects);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameEnd);
        }
        
        public void Log(string debug)
        {
            if (DEBUG) UnityEngine.Debug.Log(debug);
        }

        public static IEnumerator ResetEffects(IGameModeHandler gm)
        {
            ResetCobwebs();
            yield return null;
        }

        private static void ResetCobwebs()
        {
            foreach (var cobweb in Cobwebs)
            {
                Destroy(cobweb);
            }

            Cobwebs = new List<GameObject>();
        }

        public static IEnumerator GameEnd(IGameModeHandler gm)
        {
            yield return ResetEffects(gm);
        }
    }
}