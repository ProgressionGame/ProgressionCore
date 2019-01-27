using System;
using System.Collections.Generic;
using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Resource;
using Progression.Util.Keys;
using TestLauncher.Properties;
using static Progression.Engine.Core.World.Features.Yield.YieldModifierType;
using static Progression.Engine.Core.World.Features.Yield.TileYieldModifierPriority;

namespace TestLauncher
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
        public CivilizationRegister CivilizationRegister;
        public Civilization CivilizationRome;
        public Civilization CivilizationEgypt;
        public YieldManager YieldManager;

        public CivilizationRegister CivilisationRegister => CivilizationRegister;
        public FeatureWorld FeatureWorld;

        private void InitWorldContent()
        {
            
            //game data
            KeysRoot = new RootKey("root");

            YieldManager = new YieldManagerImpl(new Key(KeysRoot, "yield"));

            WFeatureBiome = new YieldModifyingSSFR<TerrainBiome>(WorldType.World, new Key(KeysRoot, "tb"), false, Terrain);
            WFeatureVegetation = new YieldModifyingSMFR<TerrainVegetation>(WorldType.World, new Key(KeysRoot, "tv"), Terrain);
            WFeatureLandform = new YieldModifyingSSFR<TerrainLandform>(WorldType.World, new Key(KeysRoot, "tl"), false, Terrain);

            //civs
            CivilizationRegister = new CivilizationRegister(new Key(KeysRoot, "civs"), 4, WorldType.Base, WorldType.World);



        }
        private void LoadWorldContent()
        {
            
            //game data

            YieldFood = new YieldType("food", YieldManager);
            YieldProduction = new YieldType("production", YieldManager);
            YieldCommerce = new YieldType("commerce", YieldManager);

            WFeatureGlassland = new TerrainBiome("grassland", WFeatureBiome, YieldManager, Addition, new double[] {2, 0, 0});
            WFeaturePlains = new TerrainBiome("plains", WFeatureBiome, YieldManager, Addition, new double[] {1, 1, 0});
            WFeatureDesert = new TerrainBiome("desert", WFeatureBiome, YieldManager, Addition, new double[] {0, 1, 0});
            WFeatureTundra = new TerrainBiome("tundra", WFeatureBiome, YieldManager, Addition, new double[] {1, 0, 0});
            WFeatureIce = new TerrainBiome("ice", WFeatureBiome, YieldManager, Addition, new double[] {0, 0, 0});
            
            WFeatureJungle = new TerrainVegetation("jungle", WFeatureVegetation, YieldManager, Addition, new double[] {0, -1, -1});
            WFeatureForest = new TerrainVegetation("forest", WFeatureVegetation, YieldManager, Addition, new double[] {0, 1, 0});
            WFeaturePineForest = new TerrainVegetation("pine forest", WFeatureVegetation, YieldManager, Addition, new double[] {0, 1, 0});
            
            WFeatureFlatland = new TerrainLandform("flatland", WFeatureLandform, YieldManager, Addition, new double[] {0, 0, 0});
            WFeatureHills = new TerrainLandform("hills", WFeatureLandform, YieldManager, Addition, new double[] {-1, 1, 0});
            WFeatureMountains = new TerrainLandform("mountains", WFeatureLandform, YieldManager, Addition, new double[] {-2, 1, 0});
            WFeatureHighMountains = new TerrainLandform("high mountains", WFeatureLandform, YieldManager, Addition, new double[] {-2, 0, -1});

            //civs
            CivilizationRome = new Civilization("Rome", CivilisationRegister);
            CivilizationEgypt = new Civilization("Egypt", CivilisationRegister);



        }

        private void LockFeatureWorld()
        {
            //init
            var fw = new FeatureWorld(2, YieldManager);
            fw.Register(WFeatureBiome);
            fw.Register(WFeatureVegetation);
            fw.Register(WFeatureLandform);
            fw.Register(CivilisationRegister.Resolver);
            fw.Lock();
            FeatureWorld = fw;
            
            InitTexture();
        }

        private void AddResourceHooks()
        {
            ResourceManager.Instance.AddHook(WFeatureBiome, TerrainHook);
            ResourceManager.Instance.AddHook(WFeatureLandform, TerrainHook);
            ResourceManager.Instance.AddHook(WFeatureVegetation, TerrainHook);
            ResourceManager.Instance.AddHook(CivilisationRegister.Resolver, TerrainHook);
        }
        
        private static List<IKeyNameable> _texturedFeatures = new List<IKeyNameable>();
        public static readonly AttachmentKey<object> TextureLocation = new AttachmentKey<object>();

        public static void InitTexture()
        {

            foreach (var item in _texturedFeatures) {
                if (!TextureLocation.Applicable(item.Key.Flavour)) {
                    TextureLocation.AddFlavour(item.Key.Flavour);
                }
            }

            TextureLocation.Register();
            TextureLocation.Freeze();


            foreach (var item in _texturedFeatures) {
                item.Key.Set(TextureLocation, new object());
            }
        }

        private void TerrainHook(IKeyNameable item)
        {
            _texturedFeatures.Add(item);
            Console.WriteLine(item.Name);
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