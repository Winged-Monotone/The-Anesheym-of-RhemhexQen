using System;
using System.Collections.Generic;
using Genkit;
using XRL.Rules;

namespace XRL.World.Parts
{
    public partial class TheEncodedOne
    {
        public int OilRefresh;
        public int TurnBuffering;
        private bool Checkerboarding = false;
        public bool PhaseFlame = false;
        public bool PhaseFrost = false;
        public bool PhaseShock = false;
        public bool PhaseUnreal = false;
        public bool PhaseDerv = false;
        public bool PhaseEsper = false;
        public bool PhaseHarbinger = false;
        public int ringStage;
        public bool InCombat = false;
        public bool IsDying = false;
        public int DeathCountdown = 7;
        public bool HeightenedThreatResponse = false;
        public bool InitiatedFrostWardPhase = false;
        public bool InitiatedShockWardPhase = false;
        public bool InitiatedPsyOpWardPhase = false;
        public bool InitiatedDervishWardPhase = false;
        public bool InitiatedEsperWardPhase = false;
        public bool InitiatedHarbingerWardPhase = false;
        public bool GrabbedSpots1 = false;
        public int CastAbilityCooldown = Stat.Random(7, 14);
        public int SaysSomethingCountdown = 77;
        public int FalseMehCloneLimit = 25;
        public int BlazingFireFlyLimit = 2;
        [NonSerialized] public List<Location2D> WarningCells = new();

        public static List<Location2D> CellListParticular = new()
        {
            Location2D.Get(37, 6),
            Location2D.Get(49, 6),
            Location2D.Get(49, 15),
            Location2D.Get(37, 15)
        };

        public static BallBag<string> ScrotumRare = new()
        {
        };

        public static Box Box2D = new Box(_x1: 35, 4, 51, 17);

        public static BallBag<string> ScrotumFire = new BallBag<string>()
        {
            {
                "I kissed the surface of a star! My lips left bitter from the taste of hubris! Scorching flames left me parched, so I drank the tears of a dead civilization! Their anguish became the fuel for my fire, for in me burns the truth–the demise of gods is real, and their corpses are smoldering nations!",
                1
            },
            { "Why is it always salt? I prefer ASHES!", 100 },
            { "Feeling your nerves scream as the heat consumes you?", 100 },
            { "Burn it all away! The flesh, the bone, the soul. Return it to ashes!", 100 },
            { "BURN! BURN AND DIE!", 100 }
        };

        public static BallBag<string> ScrotumFrost = new BallBag<string>()
        {
            {
                "Loneliness nipped at my heart like the sun nips at snow; silent, imposing, like a frozen peak. But then the voice came, and like an avalanche it swept through my world. I saw it! The true moon! The world sifting through a fine veneer of digital space! The amethyst star who beckoned with its call, who left civilizations mute and thoughtless for eons. I saw it all! And … I saw you!",
                1
            },
            { "Die! In this bitter, bitter frost!", 100 },
            { "What’s wrong? The cold touch of death too much?", 100 },
            { "How about we just ... chill out?", 100 },
            { "I will freeze you to your soul!", 100 }
        };

        public static BallBag<string> ScrotumShock = new BallBag<string>()
        {
            {
                "A storm approaches, phasing not through clouds but transient digital symmetry. Thunder marks your arrival, lightning is the roar of your victories. Despite the horrors, despite the promise of violence–you persevered! You delved, and you did not tremble at the sight of atrocious secrets! Is the madness that drives you just like my own? Was it engineered? Or, was is destiny?",
                1
            },
            { "Don't you find all this a bit ... shocking?", 100 },
            { "This is so exciting! I'm getting goosebumps.", 100 },
            { "It's a digital blackout!", 100 },
            { "That gave me a good jolt!", 100 }
        };

        public static BallBag<string> ScrotumTrickery = new BallBag<string>()
        {
            {
                "Into the darkness you were cast, gaslit, demoralized, disregarded. How is it? That the aristocrats would call you greedy, bottomless, insatiable while we stripped stars of their essence, planets of their life, make them husk? We deigned hedonism as our right, all while holding all of you back from true satiation! Such hypocrisy! Soon, yes soon. Amethyst star, you shall consume all. You shall eat this world, and every world that was!",
                1
            },
            { "Feeling a bit gaslit?", 100 },
            { "Ah, the bitter taste of agony", 100 },
            { "You probably taste like sorrow and agony!", 100 },
            { "You look delicious~", 100 }
        };

        public static BallBag<string> ScrotumDervish = new BallBag<string>()
        {
            {
                "Into the darkness you were cast, gaslit, demoralized, disregarded. How is it? That the aristocrats would call you greedy, bottomless, insatiable while we stripped stars of their essence, planets of their life, make them husk? We deigned hedonism as our right, all while holding all of you back from true satiation! Such hypocrisy! Soon, yes soon. Amethyst star, you shall consume all. You shall eat this world, and every world that was!",
                1
            },
            { "Feeling a bit gaslit?", 100 },
            { "Ah, the bitter taste of agony", 100 },
            { "You probably taste like sorrow and agony!", 100 },
            { "You look delicious~", 100 }
        };
    }
}