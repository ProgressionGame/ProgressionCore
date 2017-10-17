using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Progression.Engine.Core.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public interface IFeatureResolver<T> : IFeatureResolver, IEnumerable<T> where T : class, IFeature<T>
    {
        bool IsFeatureOnTile(Tile tile, T feature);
        void AddFeature(Tile tile, T feature);
        void RemoveFeature(Tile tile, T feature);
    }
    public interface IFeatureResolver : IEnumerable
    {
        bool HasFeature(Tile tile);
        void LockRegistration(FeatureWorld fw);
        Key FeatureTypeKey { get; }
        int Count { get; }
        FeatureWorld FeatureWorld { get; }
        
        bool IsFeatureOnTile(Tile tile, IFeature feature);
        void AddFeature(Tile tile, IFeature feature);
        void RemoveFeature(Tile tile, IFeature feature);
        DataIdentifier[] GenerateIdentifiers();
        DataIdentifier GetIdentifier(int index);
    }
}