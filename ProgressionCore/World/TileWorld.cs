using System.Collections.Specialized;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.World
{
    public class TileWorld
    {
        public int Height { get; }
        public int Width { get; }
        public readonly FeatureWorld FeatureWorld;
        private readonly BitVector32[,,] _worldData;
        private ScheduleUpdate _updates;
        public readonly byte WorldType;
        public readonly IWorldHolder Holder;


        public TileWorld(FeatureWorld features, int height, int width) 
            : this(features, height, width, WorldHolder.BaseWorld) { }

        public TileWorld(FeatureWorld features, int height, int width, IWorldHolder holder)
        {
            Height = height;
            Width = width;
            FeatureWorld = features;
            WorldType = holder.WorldType;
            Holder = holder;
            _worldData = new BitVector32[height, width, features.GetTileSize(WorldType)];
        }

        public int this[Coordinate c, DataIdentifier identifier] {
            get => _worldData[c.X, c.Y, identifier[WorldType].Index][identifier[WorldType].Section];
            set => _worldData[c.X, c.Y, identifier[WorldType].Index][identifier[WorldType].Section] = value;
        }

        public bool this[int x, int y, IFeature feature] {
            get => feature.HasFeature(this[x, y]);
            set {
                if (value) feature.AddFeature(this[x, y]);
                else feature.RemoveFeature(this[x, y]);
            }
        }

        public bool this[Coordinate coord, IFeature feature] {
            get => feature.HasFeature(this[coord]);
            set {
                if (value) feature.AddFeature(this[coord]);
                else feature.RemoveFeature(this[coord]);
            }
        }


        public Tile this[ushort x, ushort y] => GetTile(x, y);
        public Tile this[int x, int y] => GetTile(x, y);
        public Tile this[Coordinate coord] => GetTile(coord);

        public Tile GetTile(ushort x, ushort y)
        {
            return new Tile(x, y, this);
        }

        public Tile GetTile(int x, int y)
        {
            return new Tile((ushort) x, (ushort) y, this);
        }

        public Tile GetTile(Coordinate coord)
        {
            return new Tile(coord, this);
        }

        public bool HasFeature<T>(int x, int y, T feature) where T : class, IFeature<T>
        {
            return feature.HasFeature(GetTile(x, y));
        }

        public void AddFeature<T>(int x, int y, T feature) where T : class, IFeature<T>
        {
            feature.AddFeature(GetTile(x, y));
        }

        public void RemoveFeature<T>(int x, int y, T feature) where T : class, IFeature<T>
        {
            feature.RemoveFeature(GetTile(x, y));
        }

        public double CalcYield(int x, int y, YieldType type)
        {
            return type.Manager.CalcYield(type, GetTile(x, y));
        }

        public void RegisterUpdate(ScheduleUpdate updateHandler)
        {
            if (_updates == null) {
                _updates = updateHandler;
            } else {
                _updates += updateHandler;
            }
        }

        public void ScheduleUpdate(WorldUpdateBase update)
        {
            _updates?.Invoke(update);
        }
    }

    public delegate void ScheduleUpdate(WorldUpdateBase update);
}