namespace Util
{
    static public class LayerManager
    {
        private static float last = 0;
        private static float increment = -0.0001f;

        public static float getNextLayer()
        {
            return last += increment;
        } 

    }
}