using System.Collections.Generic;
using System.IO;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Util.Keys;
using UnityEngine;

namespace Util
{
    public static class TextureHelper
    {
        
        public static Texture2D FontAtlas;
        public static Texture2D TextureAtlas;
        private static List<IKeyNameable> _texturedFeatures = new List<IKeyNameable>();
        public const int TextureSize = 64;
        public const int FontSize = 16;
        public static readonly AttachmentKey<TextureLocation> TextureLocation = new AttachmentKey<TextureLocation>();
        
        public static void InitTexture()
        {
            FontAtlas = (Texture2D) Resources.Load("Font", typeof(Texture2D));

            foreach (var item in _texturedFeatures) {
                if (!TextureLocation.Applicable(item.Key.Flavour)) {
                    TextureLocation.AddFlavour(item.Key.Flavour);
                }
            }
            TextureLocation.Register();
            TextureLocation.Freeze();
            
            
            byte widthAmount = 8;
            byte heightAmount = (byte) (_texturedFeatures.Count / widthAmount + 1);
            
            
            TextureAtlas = new Texture2D(TextureSize * widthAmount, TextureSize * heightAmount, TextureFormat.ARGB32, false);
            
            for (byte i = 0; i < widthAmount; i++) {
                for (byte j = 0; j < heightAmount; j++) {
                    var index = i * heightAmount + j ;
                    Debug.Log($"{i} {j} {index} {_texturedFeatures.Count}");
                    if (index == _texturedFeatures.Count)  goto breakLocation;
                    IKeyNameable item = _texturedFeatures[index];
                    createFeatureTexture(item, TextureAtlas, i * TextureSize, j * TextureSize);
                    item.Key.Set(TextureLocation, new TextureLocation(i*TextureSize/(float)TextureAtlas.width, (i+1)*TextureSize/(float)TextureAtlas.width, j*TextureSize/(float)TextureAtlas.height, (j+1)*TextureSize/(float)TextureAtlas.height));
                }
            }
            breakLocation: ;

            TextureAtlas.wrapMode = TextureWrapMode.Clamp;
            TextureAtlas.anisoLevel = 0;
            TextureAtlas.filterMode = FilterMode.Point;
            
            TextureAtlas.Apply();
            /*byte[] bytes = TextureAtlas.EncodeToPNG();
            var rnd = new System.Random();
            File.WriteAllBytes("C:\\Users\\sirati97\\Desktop\\" + rnd.Next() + ".png", bytes);/**/
            
            
            Debug.Log ("Done creating atlas");

        }

        private static void createFeatureTexture(IKeyNameable item, Texture2D to, int x, int y)
        {
            Color background = Color.gray;
            var textY = y + TextureSize;
            if (item is TerrainBiome) {
                textY -= FontSize;
            } else if (item is TerrainLandform) {
                textY -= FontSize*2;
                background=new Color(0,0,0,0);
            } else if (item is TerrainVegetation) {
                textY -= FontSize*3;
                background=new Color(0,0,0,0);
            }

            for (int i = 0; i < TextureSize; i++) {
                for (int j = 0; j < TextureSize; j++) {
                    to.SetPixel(x + j, y + i, background);
                }
            }
            
            //to.SetPixel(x + TextureSize - 1, y , Color.green);
            //to.SetPixel(x, y + TextureSize - 1 , Color.magenta);
            int maxI = item.Name.Length<TextureSize/FontSize-1?item.Name.Length:TextureSize/FontSize-1;
            for (int i = 0; i < maxI; i++) {
                copyLetter(FontAtlas, to, x + FontSize * i, textY, item.Name[i], Color.red);
            }
        }

        private static void copyLetter(Texture2D font, Texture2D to, int x, int y, char c, Color color)
        {
            const int fontMaxY = FontSize;
            const int fontMaxX = FontSize;
            int cIndex = (int) c;
            cIndex = cIndex > 255 ? 255 : cIndex;
            int fontX = cIndex % fontMaxY;

            int fontY = fontMaxY - cIndex / fontMaxY - 1;
            var colors = font.GetPixels(fontX * FontSize, fontY * FontSize, FontSize, FontSize);
            //Debug.Log($"x={x} y={y} colors.Length={colors.Length} to.width={to.width} to.height={to.height} {fontX} {fontY} {c}");
            for (int i = 0; i < fontMaxY; i++) {
                for (int j = 0; j < fontMaxX; j++) {
                    int index = i * fontMaxX + j;
                    if (colors[index] == Color.white) {
                        to.SetPixel(x + j, y + i, color);
                    }
                }
            }
        }

        public static void TerrainHook(IKeyNameable item)
        {
            _texturedFeatures.Add(item);
        }

    }
}