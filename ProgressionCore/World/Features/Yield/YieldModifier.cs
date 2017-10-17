using System;

namespace Progression.Engine.Core.World.Features.Yield
{
    public abstract class YieldModifier : IYieldModifer
    {
        protected readonly double[] _modifiers;
        
        protected YieldModifier(YieldManager manager, int defaultValue)
        {
            _modifiers = new double[manager.Count];
            for (var i = 0; i < _modifiers.Length; i++) {
                _modifiers[i] = defaultValue;
            }
        }
        
        protected YieldModifier(YieldManager manager, double[] modifiers)
        {
            if (manager.Count != modifiers.Length) {
                throw new ArgumentException("Modifiers length does not match amount of yield types");   
            }
            _modifiers = new double[manager.Count];
            Array.Copy(modifiers, _modifiers, modifiers.Length);
        }

        public void setModifier(YieldType type, double newValue)
        {
            _modifiers[type.Index] = newValue;
        }
        
        
        public abstract double Modify(YieldType type, Tile tile, double input);
    }
}