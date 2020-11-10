using System.Collections.Generic;
using JetBrains.Annotations;

namespace Runtime.Logic.Core.EventBus {
    public interface IEvent { }

    public interface IEventReceiverBase { }

    public interface IEventReceiver<T> : IEventReceiverBase where T : struct, IEvent {
        void OnEvent(T e);
    }

    public static class EventBus<T> where T : struct, IEvent {
        private static IEventReceiver<T>[] _buffer;
        private static int _count;
        private static readonly int blocksize = 256;

        private static HashSet<IEventReceiver<T>> hash;

        static EventBus() {
            hash = new HashSet<IEventReceiver<T>>();
            _buffer = new IEventReceiver<T>[0];
        }

        [UsedImplicitly]
        public static void Register(IEventReceiverBase handler) {
            _count++;
            hash.Add(handler as IEventReceiver<T>);
            if (_buffer.Length < _count) {
                _buffer = new IEventReceiver<T>[_count + blocksize];
            }


            hash.CopyTo(_buffer);
        }

        [UsedImplicitly]
        public static void UnRegister(IEventReceiverBase handler) {
            hash.Remove(handler as IEventReceiver<T>);
            hash.CopyTo(_buffer);
            _count--;
        }

        public static void Raise(T e) {
            for (int i = 0; i < _count; i++) {
                _buffer[i].OnEvent(e);
            }
        }

        public static void RaiseAsInterface(IEvent e) {
            Raise((T) e);
        }

        public static void Clear() {
            hash.Clear();
        }
    }
}