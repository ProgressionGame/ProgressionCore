using System;

namespace Progression.Engine.Core.World.Features.Yield
{
    public abstract class YieldModifier : IYieldModifer
    {
        protected readonly double[] Modifiers;
        
        protected YieldModifier(YieldManager manager, int defaultValue)
        {
            Modifiers = new double[manager.Count];
            for (var i = 0; i < Modifiers.Length; i++) {
                Modifiers[i] = defaultValue;
            }
        }
        
        protected YieldModifier(YieldManager manager, double[] modifiers)
        {
            if (manager.Count != modifiers.Length) {
                throw new ArgumentException("Modifiers length does not match amount of yield types");   
            }
            Modifiers = new double[manager.Count];
            Array.Copy(modifiers, Modifiers, modifiers.Length);
        }

        public void SetModifier(YieldType type, double newValue)
        {
            Modifiers[type.Index] = newValue;
        }
        
        
        public abstract double Modify(YieldType type, Tile tile, double input);
    }
}