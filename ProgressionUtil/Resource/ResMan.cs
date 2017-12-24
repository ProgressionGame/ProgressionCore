namespace Progression.Resource
{
    public static class ResMan
    {
        private static volatile IResourceManager _instance;
        private static object mutex = new object();


        public static void SetInstance(IResourceManager value)
        {
            lock (mutex) {
                _instance = value;
            }
        }

        public static T GetInstance<T>() where T:class,IResourceManager
        {
            return (T) GetInstance();
        }
        
        

        public static IResourceManager GetInstance()
        {
            if (_instance == null) {
                lock (mutex) {
                    if (_instance == null) {
                        return null;
                    }
                }
            }
            return _instance;
        }
    }
}