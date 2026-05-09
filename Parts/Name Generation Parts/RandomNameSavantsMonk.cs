using System;
using XRL.World;
using XRL.World.Parts.Mutation;
using System.Collections.Generic;
using XRL.Rules;
using System.Linq;
using XRL.World.Effects;
using XRL.Language;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomNameSavantsGenerator : IPart
    {
        public RandomNameSavantsGenerator()
        {
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            Object.RegisterPartEvent(this, "ObjectCreated");
            base.Register(Object, eventRegistrar);
        }

        private List<string> RanMainName = new List<string>()
        {
            "Lheiysha",
            "Kalshera",
            "Neydaleth",
            "Lan'us",
            "Vahygah",
            "Phora",
            "Egas-Yoahn",
            "Atamas",
            "Scleradae",
            "Envengalit",
            "Aughasta",
            "Yeingel'Ulfae",
            "Ulfarache",
            "Charcillara",
            "Padipeleya",
            "Abdomae",
            "Peydi",
            "Eteym",
            "Aeugis",
            "Quen",
            "Yettae",
            "Vespa",
            "Xeporadthaxia",
            "Odunara",
            "Arachynia",
            "Turanchila",
            "Vespiera",
            "Arashi'yora",
            "Nandurah",
            "Rae",
            "Mantisa",
            "Lactrodectura",
            "Eratigena",
            "Ampulexa",
            "Compressa",
            "Mantodea",
            "Formicidea",
            "Hymenoptera",
            "Nymphiditis",
            "Hymenoptera",
            "Rhopalocerae",
            "Hesperiidae",
            "Lepidoptera",
            "Diptera",
            "Coleopterae",
            "Coccinellidae",
            "Tipulidae",
            "Pholcidae",
            "Arctopernopa",
            "Limoniinae",
            "Chionea",
            "Limonia",
            "Hexetomini",
            "Symphyta",
            "Tenthredinoidea",
            "Apocrita",
            "Anaxyeloidea",
            "Orussiudea",
            "Pamphilloidae",
            "Xyeloidea",
            "Aculeata",
            "Coleopticae",
            "Xanthorius",
            "Echthrus",
            "Xanthopimpla",
            "Bug",
            "Chiras",
            "Sashi",
            "Chelon",
            "Chara",
            "Raxi",
            "Keriax",
            "Zyla",
            "Archive",
            "Hela",
            "Shishi",
            "Chorus",
            "Rhyssa",
            "Albicoxa",
            "Itoplexus",
            "Ichneumonidae",
            "Aculeata",
            "Heteroponerinae",
            "Ectatomminae",
            "Dolichoderinae",
            "Ponerinae",
            "Leptanillinae",
            "Sphecomyrminae",
            "Acaenitinae",
            "Adelognathinae",
            "Agriotypinae",
            "Banchinae",
            "Brachycyrtinae ",
            "Ctenopelmatinae",
            "Diacritinae",
            "Eucerotinae",
            "Labeninae",
            "Mesochorinae",
            "Ophioninae",
            "Phrudinae",
            "Tersilochinae",
            "Tryphoninae",
            "Novolandicus",
            "Xanthocryptus",
            "Xoridinae",
            "Chelicerata",
            "Palpigradi",
            "Acari",
            "Acarina",
            "Astigmatina",
            "Sphaerolichida",
            "Mesothelae",
            "Liphistiidae",
            "Heptathela",
            "Arthrolycosidae",
            "Setae",
            "Araneomorphae",
            "Gasteracantha",
            "Liphistiidae",
            "Periegops",
            "Evarcha",
            "Culicivora",
            "Parasteatoda",
            "Dysderidae",
            "Harpactea",
            "Crocata",
            "Kaemis",
            "Minotauria",
            "Stalagtia",
            "Speleoharpactea",
            "Stalitella",
            "Dysderella ",
            "Tedia",
        };

        private List<string> RanSurName = new List<string>(1)
        {
            "Bug",
            "Chiras",
            "Sashi",
            "Chelon",
            "Chara",
            "Raxi",
            "Keriax",
            "Zyla",
            "Ar'chive",
            "Hela",
            "Shishi",
            "Chorus",
            "Rhyssa",
        };

        public override bool FireEvent(Event E)
        {
            if (E.ID == "ObjectCreated")
            {
                ParentObject.DisplayName = "{{violet|Lunarlucid-}}" + RanMainName.GetRandomElement() +
                                           ", {{violet|Savant Monad}}";
            }

            return true;
        }
    }
}