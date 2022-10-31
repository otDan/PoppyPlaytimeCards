using UnityEngine;

namespace PoppyPlaytimeCards.Asset
{
    public static class AssetManager
    {
        private static readonly AssetBundle PoppyPlaytimeAssetsBundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("poppyplaytime_assets", typeof(PoppyPlaytimeCards).Assembly);

        public static GameObject BunzoBunnyCard = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("BunzoBunnyCard");
        public static GameObject GrabPackCard = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("GrabPackCard");
        public static GameObject HuggyWuggyCard = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("HuggyWuggyCard");
        public static GameObject KissyMissyCard = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("KissyMissyCard");
        public static GameObject MiniHuggiesCard = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("MiniHuggiesCard");
        public static GameObject MommyLongLegsCard = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("MommyLongLegsCard");

        // public static GameObject JumpScarePlayer = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("JumpScarePlayer");
        //
        // public static VideoClip BunzoBunny = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("BunzoBunny");
        // public static VideoClip HuggyWuggy = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("HuggyWuggy");
        // public static VideoClip KissyMissy = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("KissyMissy");
        // public static VideoClip MiniHuggiesGreen = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("MiniHuggieGreen");
        // public static VideoClip MiniHuggiesRed = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("MiniHuggieRed");
        // public static VideoClip MiniHuggiesYellow = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("MiniHuggieYellow");
        // public static VideoClip MommyLongLegs = PoppyPlaytimeAssetsBundle.LoadAsset<VideoClip>("MommyLongLegs");

        public static Sprite MiniHuggyFace = PoppyPlaytimeAssetsBundle.LoadAsset<Sprite>("MiniHuggyFace");

        public static GameObject BunzoBunnyEffect = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("BunzoBunnyEffect");
        public static GameObject KissyMissyEffect = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("KissyMissyEffect");
        public static GameObject MommyLongLegsEffect = PoppyPlaytimeAssetsBundle.LoadAsset<GameObject>("MommyLongLegsEffect");

        public static AudioClip BunzoBunnySound = PoppyPlaytimeAssetsBundle.LoadAsset<AudioClip>("bunzo_hit");
        public static AudioClip KissyMissySound = PoppyPlaytimeAssetsBundle.LoadAsset<AudioClip>("kissy_kiss");
    }
}
