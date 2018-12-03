using System;
using System.Linq;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using UnityEngine;
using Random = System.Random;

namespace Util
{
    public class BetterTileMap
    {
        public GameObject GameObject { get; }
        public MeshFilter MeshFilter { get; }
        public MeshRenderer MeshRenderer { get; }
        public ISingleFeatureResolver Resolver { get; }
        
        private float LayerZ;
        private Mesh _mesh;
        private const int TileSize = StartUp.TileSize;
        
        public BetterTileMap(string name, ISingleFeatureResolver resolver)
        {
            Resolver = resolver;
            GameObject = new GameObject(name);
            MeshFilter = GameObject.AddComponent<MeshFilter>();
            MeshRenderer = GameObject.AddComponent<MeshRenderer>();

            LayerZ = LayerManager.getNextLayer();
            
            BuildMesh();
        }
    

        public void BuildMesh()
	    {
            StartUp.Init();
            
            int sizeX = StartUp.World.Height;
            int sizeY = StartUp.World.Width;
            
            
            int numTiles = sizeX * sizeY;
            
            // Generate the mesh data
            
            Vector3[] vertices = new Vector3[ numTiles * 4 ];
            Vector3[] normals = new Vector3[numTiles * 4];
            Vector2[] uv = new Vector2[numTiles * 4];
            int[] triangles = new int[ numTiles * 6 ];
            
            
            for(int y=0; y < sizeY; y++) {
                for(int x=0; x < sizeX; x++) {
                    int squareIndex = y * sizeX + x;
                    int triOffset = squareIndex * 6;
                    int vertOffset = squareIndex * 4;
                    
                    vertices[vertOffset + 0] = new Vector3( x*TileSize, y*TileSize, LayerZ); //A
                    vertices[vertOffset + 1] = new Vector3( (x+1)*TileSize, y*TileSize, LayerZ); //B
                    vertices[vertOffset + 2] = new Vector3( (x+1)*TileSize, (y+1)*TileSize, LayerZ); //C
                    vertices[vertOffset + 3] = new Vector3( x*TileSize, (y+1)*TileSize, LayerZ); //D
    
                    triangles[triOffset + 0] = vertOffset + 2; //C
                    triangles[triOffset + 1] = vertOffset + 1; //B
                    triangles[triOffset + 2] = vertOffset + 0; //A
                    
                    triangles[triOffset + 3] = vertOffset + 3; //D
                    triangles[triOffset + 4] = vertOffset + 2; //C
                    triangles[triOffset + 5] = vertOffset + 0; //A
    
                    var feature = Resolver.GetFeature(StartUp.World[x, y]);
                    var loc = feature.Key.Get(TextureHelper.TextureLocation);
                    
                    
                    uv[vertOffset + 0] = new Vector2(loc.StartX, loc.StartY);
                    uv[vertOffset + 1] = new Vector2(loc.EndX, loc.StartY);
                    uv[vertOffset + 2] = new Vector2(loc.EndX, loc.EndY);
                    uv[vertOffset + 3] = new Vector2(loc.StartX, loc.EndY);
                }
            }
            
            Debug.Log ("Done Triangles!");
            
            // Create a new Mesh and populate with the data
            _mesh = new Mesh();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            //mesh.normals = normals;
            _mesh.uv = uv;
                
            
            MeshFilter.mesh = _mesh;
            Debug.Log ("Done Mesh!");
    
            MeshRenderer.material = new Material (Shader.Find("Unlit/Transparent"));
                MeshRenderer.material.mainTexture = TextureHelper.TextureAtlas;
            //mesh_renderer.material.shader.
            //mesh_renderer.
            //mesh_renderer.material.SetTextureScale("_MainTex", new Vector2(100,100));
            
            //mesh_renderer.sharedMaterial.mainTexture = TextureHelper.TextureAtlas;
            Debug.Log ("Added texture");
	    }


        public void UpdateTile(Tile tile)
        {
            int squareIndex = tile.Coordinate.Y * tile.World.Width + tile.Coordinate.X;
            int vertOffset = squareIndex * 4;
            
            var feature = Resolver.GetFeature(tile);
            var loc = feature.Key.Get(TextureHelper.TextureLocation);
            //Debug.Log ($"applying update {feature.Name} {loc.StartX} {loc.EndX}");
            _mesh = MeshFilter.mesh;
            Vector2[] uv = _mesh.uv;
            uv[vertOffset + 0] = new Vector2(loc.StartX, loc.StartY);
            uv[vertOffset + 1] = new Vector2(loc.EndX, loc.StartY);
            uv[vertOffset + 2] = new Vector2(loc.EndX, loc.EndY);
            uv[vertOffset + 3] = new Vector2(loc.StartX, loc.EndY);
            _mesh.uv = uv;

            /*var rnd = new Random();
            Vector3[] vertices = _mesh.vertices;
            for (int i = 0; i < vertices.Length; i++) {
                var vertex = vertices[i];
                vertex.x += rnd.Next(-10, 10);
                vertex.y += rnd.Next(-10, 10);
                vertex.z += rnd.Next(-10, 10);
                vertices[i] = vertex;
            }

            _mesh.vertices = vertices;/**/
            _mesh.UploadMeshData(false);
        }
    }
}