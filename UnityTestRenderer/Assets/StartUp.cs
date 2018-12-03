using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Engine.Core.World.Features.Yield;
using TestGameEnv;
using UnityEngine;
using Util;
using Random = System.Random;

public class StartUp : MonoBehaviour
{
    [NotNull] public static readonly GameEnv GameEnv;
    [NotNull] public static readonly TileWorld World;
    public const int TileSize = 15;
    public static string DebugInfo = "Bootup";
    
    static StartUp()
    {
        GameEnv = new GameEnv();
        GameEnv.Init();
        GameEnv.Load();
        

        World = new TileWorld(GameEnv.FeatureWorld, 7, 5);
        GameEnv.WFeatureDesert.AddFeature(World[1,1]);
        Debug.Log($"DataRepresentation of Landform {GameEnv.WFeatureFlatland.DataRepresentation} {GameEnv.WFeatureHills.DataRepresentation} {GameEnv.WFeatureMountains.DataRepresentation} {GameEnv.WFeatureHighMountains.DataRepresentation} bits={GameEnv.WFeatureLandform.DataIdentifier.Bits}");
    }

    private static bool hasInit;
    private BetterTileMap _biomeTileMap;
    private BetterTileMap _landformTileMap;
    private WorldUpdateInterface _wui;

    public static void Init()
    {
        if (hasInit) return;
        hasInit = true;
        TextureHelper.InitTexture();
    }

    // Use this for initialization
	void Start ()
	{
	    //Application.targetFrameRate = -1;
	    Init();
	    createTileMap();
	    
	    
	}

    void createTileMap()
    {
        _biomeTileMap = new BetterTileMap("Biome", GameEnv.WFeatureBiome);
        _landformTileMap = new BetterTileMap("Landform", GameEnv.WFeatureLandform);
        //var vegetationTileMap = new BetterTileMap("Vegetation", GameEnv.WFeatureVegetation);
        
        _wui = new WorldUpdateInterface(World, GameEnv.WFeatureBiome, GameEnv.WFeatureLandform, _biomeTileMap, _landformTileMap);
        World.RegisterUpdate(_wui.ScheduleUpdate);
        
        Debug.Log("Done createTileMap");
    }

    private int i = 0;
    void Update()
    {
        if (!hasInit) {
            Debug.Log("skipped update because not initiased");
            return;
        }
        _wui.Execute();
        
        //if (++i < 10) return;
        i = 0;
        ISimpleFeature[] allFeatures = {
            GameEnv.WFeatureDesert, GameEnv.WFeatureHighMountains, GameEnv.WFeatureHills, GameEnv.WFeatureIce, GameEnv.WFeatureMountains, GameEnv.WFeaturePlains,
            GameEnv.WFeatureTundra/*, GameEnv.WFeatureFlatland, GameEnv.WFeatureGlassland*/
        };
        var rnd = new Random();
        int y = rnd.Next(World.Width);
        int x = rnd.Next(World.Height);
        var feat = allFeatures[rnd.Next(allFeatures.Length)];
        //Debug.Log($"Changing {x} {y} to {feat.Name}");
        feat.AddFeature(World[x, y]);

        
    }




    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
 
        GUIStyle style = new GUIStyle();
 
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.LowerRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
        GUI.Label(rect, DebugInfo, style);
    }
}
