using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace Jam4JokeEntry
{
    public class Jam4JokeEntry : ModBehaviour
    {
        public static Jam4JokeEntry Instance;
        public INewHorizons NewHorizons;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(Jam4JokeEntry)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);
            NewHorizons.GetStarSystemLoadedEvent().AddListener(s =>
            {
                if (s == "MegaPiggy.Jam4JokeEntry")
                {
                    var room = NewHorizons.GetPlanet("The Box").transform.Find("Sector/Room");
                    var door = room.transform.Find("Door");
                    var interact = room.transform.Find("Button/Interact");
                    interact.gameObject.AddComponent<Button>().door = door;
                }
            });

            new Harmony("MegaPiggy.Jam4JokeEntry").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
        }
    }

}
