using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Engine.Core.World.Features.Yield;
using TestGameEnv;
using UnityEngine;
using Util;

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
    }

    private static bool hasInit = false;
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
        var biomeTileMap = new BetterTileMap("Biome", GameEnv.WFeatureBiome);
        var landformTileMap = new BetterTileMap("Landform", GameEnv.WFeatureLandform);
        //var vegetationTileMap = new BetterTileMap("Vegetation", GameEnv.WFeatureVegetation);
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
