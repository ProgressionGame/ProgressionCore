using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Resource;
using Progression.Util.Keys;

namespace TestGameEnv
{
    public class GameEnv
    {
        public RootKey KeysRoot;
        public YieldType YieldFood;
        public YieldType YieldProduction;
        public YieldType YieldCommerce;
        public YieldModifyingSSFR<TerrainBiome> WFeatureBiome;
        public YieldModifyingSMFR<TerrainVegetation> WFeatureVegetation;
        public YieldModifyingSSFR<TerrainLandform> WFeatureLandform;
        public TerrainBiome WFeatureGlassland;
        public TerrainBiome WFeaturePlains;
        public TerrainBiome WFeatureDesert;
        public TerrainBiome WFeatureTundra;
        public TerrainBiome WFeatureIce;
        public TerrainVegetation WFeatureJungle;
        public TerrainVegetation WFeatureForest;
        public TerrainVegetation WFeaturePineForest;
        public TerrainLandform WFeatureFlatland;
        public TerrainLandform WFeatureHills;
        public TerrainLandform WFeatureMountains;
        public TerrainLandform WFeatureHighMountains;
        public CivilizationManager CivilizationManager;
        public Civilization CivilizationRome;
        public Civilization CivilizationEgypt;
        public YieldManager YieldManager;
        public FeatureWorld FeatureWorld;

        private void InitWorldContent()
        {
            
            //game data
            KeysRoot = new RootKey("root");

            YieldManager = new YieldManagerImpl(new Key(KeysRoot, "yield"));

            WFeatureBiome = new YieldModifyingSSFR<TerrainBiome>(WorldType.World, new Key(KeysRoot, "tb"), false, TileYieldModifierPriority.Terrain);
            WFeatureVegetation = new YieldModifyingSMFR<TerrainVegetation>(WorldType.World, new Key(KeysRoot, "tv"), TileYieldModifierPriority.Terrain);
            WFeatureLandform = new YieldModifyingSSFR<TerrainLandform>(WorldType.World, new Key(KeysRoot, "tl"), false, TileYieldModifierPriority.Terrain);

            //civs
            CivilizationManager = new CivilizationManager(new Key(KeysRoot, "civs"), 4);



        }
        private void LoadWorldContent()
        {
            
            //game data

            YieldFood = new YieldType("food", YieldManager);
            YieldProduction = new YieldType("production", YieldManager);
            YieldCommerce = new YieldType("commerce", YieldManager);

            WFeatureGlassland = new TerrainBiome("grassland", WFeatureBiome, YieldManager, YieldModifierType.Addition, new double[] {2, 0, 0});
            WFeaturePlains = new TerrainBiome("plains", WFeatureBiome, YieldManager, YieldModifierType.Addition, new double[] {1, 1, 0});
            WFeatureDesert = new TerrainBiome("desert", WFeatureBiome, YieldManager, YieldModifierType.Addition, new double[] {0, 1, 0});
            WFeatureTundra = new TerrainBiome("tundra", WFeatureBiome, YieldManager, YieldModifierType.Addition, new double[] {1, 0, 0});
            WFeatureIce = new TerrainBiome("ice", WFeatureBiome, YieldManager, YieldModifierType.Addition, new double[] {0, 0, 0});
            
            WFeatureJungle = new TerrainVegetation("jungle", WFeatureVegetation, YieldManager, YieldModifierType.Addition, new double[] {0, -1, -1});
            WFeatureForest = new TerrainVegetation("forest", WFeatureVegetation, YieldManager, YieldModifierType.Addition, new double[] {0, 1, 0});
            WFeaturePineForest = new TerrainVegetation("pine forest", WFeatureVegetation, YieldManager, YieldModifierType.Addition, new double[] {0, 1, 0});
            
            WFeatureFlatland = new TerrainLandform("flatland", WFeatureLandform, YieldManager, YieldModifierType.Addition, new double[] {0, 0, 0});
            WFeatureHills = new TerrainLandform("hills", WFeatureLandform, YieldManager, YieldModifierType.Addition, new double[] {-1, 1, 0});
            WFeatureMountains = new TerrainLandform("mountains", WFeatureLandform, YieldManager, YieldModifierType.Addition, new double[] {-2, 1, 0});
            WFeatureHighMountains = new TerrainLandform("high mountains", WFeatureLandform, YieldManager, YieldModifierType.Addition, new double[] {-2, 0, -1});

            //civs
            CivilizationRome = new Civilization("Rome", CivilizationManager);
            CivilizationEgypt = new Civilization("Egypt", CivilizationManager);



        }

        private void LockFeatureWorld()
        {
            //init
            FeatureWorld = new FeatureWorld(2, YieldManager);
            FeatureWorld.Register(WFeatureBiome);
            FeatureWorld.Register(WFeatureVegetation);
            FeatureWorld.Register(WFeatureLandform);
            FeatureWorld.Register(CivilizationManager.Resolver);
            FeatureWorld.Lock();
            
        }

        private void AddResourceHooks()
        {
            ResourceManager.Instance.AddHook(WFeatureBiome, TerrainHook);
            ResourceManager.Instance.AddHook(WFeatureLandform, TerrainHook);
            ResourceManager.Instance.AddHook(WFeatureVegetation, TerrainHook);
            ResourceManager.Instance.AddHook(CivilizationManager.Resolver, TerrainHook);
            
            //Textures
            ResourceManager.Instance.AddHook(WFeatureBiome, Util.TextureHelper.TerrainHook);
            ResourceManager.Instance.AddHook(WFeatureLandform, Util.TextureHelper.TerrainHook);
            ResourceManager.Instance.AddHook(WFeatureVegetation, Util.TextureHelper.TerrainHook);
        }

        private void TerrainHook(IKeyNameable item)
        {
            //ResourceManager.Instance.LoadResource()
        }

        private void InitResourceMan()
        {
            ResourceManager.Init(ResourceDomain.All); //All used for test purposes
        }


        public void Init()
        {
            InitResourceMan();
            InitWorldContent();
        }


        public void Load()
        {
            LoadWorldContent();
            AddResourceHooks();
            LockFeatureWorld();
        }

    }
}