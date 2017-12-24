using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Resource;
using Progression.Util.Keys;
using static Progression.Engine.Core.World.Features.Yield.YieldModifierType;
using static Progression.Engine.Core.World.Features.Yield.TileYieldModifierPriority;

namespace TestLauncher
{
    public class GameEnv
    {
        private RootKey _keysRoot;
        private YieldType _yieldFood;
        private YieldType _yieldProduction;
        private YieldType _yieldCommerce;
        private YieldModifyingSSFR<TerrainBiome> _wfeatureBiome;
        private YieldModifyingSMFR<TerrainVegetation> _wfeatureVegetation;
        private YieldModifyingSSFR<TerrainLandform> _wfeatureLandform;
        private TerrainBiome _wfeatureGlassland;
        private TerrainBiome _wfeaturePlains;
        private TerrainBiome _wfeatureDesert;
        private TerrainBiome _wfeatureTundra;
        private TerrainBiome _wfeatureIce;
        private TerrainVegetation _wfeatureJungle;
        private TerrainVegetation _wfeatureForest;
        private TerrainVegetation _wfeaturePineForest;
        private TerrainLandform _wfeatureFlatland;
        private TerrainLandform _wfeatureHills;
        private TerrainLandform _wfeatureMountains;
        private TerrainLandform _wfeatureHighMountains;
        private CivilizationManager _civilizationManager;
        private Civilization _civilizationRome;
        private Civilization _civilizationEgypt;
        private YieldManager _yieldManager;

        private void InitWorldContent()
        {
            
            //game data
            _keysRoot = new RootKey("root");

            _yieldManager = new YieldManagerImpl(new Key(_keysRoot, "yield"));
            _yieldFood = new YieldType("food", _yieldManager);
            _yieldProduction = new YieldType("production", _yieldManager);
            _yieldCommerce = new YieldType("commerce", _yieldManager);

            _wfeatureBiome = new YieldModifyingSSFR<TerrainBiome>(WorldType.World, new Key(_keysRoot, "tb"), false, Terrain);
            _wfeatureGlassland = new TerrainBiome("grassland", _wfeatureBiome, _yieldManager, Addition, new double[] {2, 0, 0});
            _wfeaturePlains = new TerrainBiome("plains", _wfeatureBiome, _yieldManager, Addition, new double[] {1, 1, 0});
            _wfeatureDesert = new TerrainBiome("desert", _wfeatureBiome, _yieldManager, Addition, new double[] {0, 1, 0});
            _wfeatureTundra = new TerrainBiome("tundra", _wfeatureBiome, _yieldManager, Addition, new double[] {1, 0, 0});
            _wfeatureIce = new TerrainBiome("ice", _wfeatureBiome, _yieldManager, Addition, new double[] {0, 0, 0});
            _wfeatureVegetation = new YieldModifyingSMFR<TerrainVegetation>(WorldType.World, new Key(_keysRoot, "tv"), Terrain);
            _wfeatureJungle = new TerrainVegetation("jungle", _wfeatureVegetation, _yieldManager, Addition, new double[] {0, -1, -1});
            _wfeatureForest = new TerrainVegetation("forest", _wfeatureVegetation, _yieldManager, Addition, new double[] {0, 1, 0});
            _wfeaturePineForest = new TerrainVegetation("pine forest", _wfeatureVegetation, _yieldManager, Addition, new double[] {0, 1, 0});
            _wfeatureLandform = new YieldModifyingSSFR<TerrainLandform>(WorldType.World, new Key(_keysRoot, "tl"), false, Terrain);
            _wfeatureFlatland = new TerrainLandform("flatland", _wfeatureLandform, _yieldManager, Addition, new double[] {0, 0, 0});
            _wfeatureHills = new TerrainLandform("hills", _wfeatureLandform, _yieldManager, Addition, new double[] {-1, 1, 0});
            _wfeatureMountains = new TerrainLandform("mountains", _wfeatureLandform, _yieldManager, Addition, new double[] {-2, 1, 0});
            _wfeatureHighMountains = new TerrainLandform("high mountains", _wfeatureLandform, _yieldManager, Addition, new double[] {-2, 0, -1});

            //civs
            _civilizationManager = new CivilizationManager(new Key(_keysRoot, "civs"), 4);
            _civilizationRome = new Civilization("Rome", _civilizationManager);
            _civilizationEgypt = new Civilization("Egypt", _civilizationManager);



        }

        private void LockFeatureWorld()
        {
            //init
            var fw = new FeatureWorld(2, _yieldManager);
            fw.Register(_wfeatureBiome);
            fw.Register(_wfeatureVegetation);
            fw.Register(_wfeatureLandform);
            fw.Register(_civilizationManager.Resolver);
            fw.Lock();
        }

        private void AddResourceHooks()
        {
            
        }

        private void InitResourceMan()
        {
            ResMan.SetInstance(new ResourceManager(ResourceDomain.All)); //All used for test purposes
        }

    }
}