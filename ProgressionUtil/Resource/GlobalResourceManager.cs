namespace Progression.Resource
{
    public static class GlobalResourceManager
    {
        private static volatile IResourceManager _instance;
        private static object mutex = new object();

        public static T GetInstance<T>() where T:IResourceManager
        {
            return (T) Instance;
        }


        public static IResourceManager Instance {
            get {
                if (_instance == null) {
                    lock (mutex) {
                        if (_instance == null) {
                            return null;
                        }
                    }
                }
                return _instance;
            }
            set {
                lock (mutex) {
                    _instance = value;
                }
            }
        }
    }
}