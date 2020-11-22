using System;

namespace Runtime.Logic.Core {
    public class SingletonBase<T> where T : class {
        public static T Instance { get { return sInstance.Value; } }
        
        private static readonly Lazy<T> sInstance = new Lazy<T>(() => CreateInstanceOfT());

        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}