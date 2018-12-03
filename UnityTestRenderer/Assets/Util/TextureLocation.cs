using UnityEngine;

namespace Util
{
    public struct TextureLocation
    {
        public readonly float StartX;
        public readonly float EndX;
        public readonly float StartY;
        public readonly float EndY;
        public TextureLocation(float startX, float endX, float startY, float endY)
        {
            StartX = startX;
            EndX = endX;
            StartY = startY;
            EndY = endY;
            Debug.Log($"{startX} {endX} {startY} {endY}");
        }
    }
}