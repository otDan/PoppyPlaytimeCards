using System.Collections;
using ClassesManagerReborn;
using PoppyPlaytimeCards.Card;

namespace PoppyPlaytimeCards.Class
{
    public class PoppyPlaytimeClass : ClassHandler
    {
        internal static string name = "Poppy\nPlaytime";

        public override IEnumerator Init()
        {
            UnityEngine.Debug.Log("Registering: " + name.Replace("\n", ""));
            while (!(GrabPackCard.Card 
                     && BunzoBunnyCard.Card 
                     && HuggyWuggyCard.Card 
                     && KissyMissyCard.Card 
                     && MiniHuggiesCard.Card 
                     && MommyLongLegsCard.Card)) 
                yield return null;

            ClassesRegistry.Register(GrabPackCard.Card, CardType.Entry);
            ClassesRegistry.Register(BunzoBunnyCard.Card, CardType.Card, GrabPackCard.Card);
            ClassesRegistry.Register(HuggyWuggyCard.Card, CardType.Card, GrabPackCard.Card);
            ClassesRegistry.Register(KissyMissyCard.Card, CardType.Card, GrabPackCard.Card);
            ClassesRegistry.Register(MiniHuggiesCard.Card, CardType.Card, GrabPackCard.Card);
            ClassesRegistry.Register(MommyLongLegsCard.Card, CardType.Gate, GrabPackCard.Card);
        }

        public override IEnumerator PostInit()
        {
            yield break;
        }
    }
}
