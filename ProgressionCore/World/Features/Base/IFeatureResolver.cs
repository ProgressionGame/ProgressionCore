﻿using System.Collections;
using System.Collections.Generic;
using Progression.Resource;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public interface IFeatureResolver<T> : IFeatureResolver, IEnumerable<T>, IResourceable<T> where T : class, IFeature<T>
    {
//        bool IsFeatureOnTile(Tile tile, T feature);
//        void AddFeature(Tile tile, T feature);
//        void RemoveFeature(Tile tile, T feature);
        new T Get(int index);
    }
    public interface IFeatureResolver : IEnumerable, IResourceable, IKeyed
    {
//        bool HasFeature(Tile tile);
        void Freeze(FeatureWorld fw);
        int Count { get; }
        FeatureWorld FeatureWorld { get; }
        IFeature Get(int index);
        
//        bool IsFeatureOnTile(Tile tile, IFeature feature);
//        void AddFeature(Tile tile, IFeature feature);
//        void RemoveFeature(Tile tile, IFeature feature);
        DataIdentifier[] GenerateIdentifiers();
        DataIdentifier GetIdentifier(int index);
        
    }
}