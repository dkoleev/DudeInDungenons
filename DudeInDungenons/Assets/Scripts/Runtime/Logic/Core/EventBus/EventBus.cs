﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Runtime.Logic.Core.EventBus {
    public static class EventBus {
        private static readonly Dictionary<Type, ClassMap> class_register_map = new Dictionary<Type, ClassMap>();
        private static readonly Dictionary<Type, Action<IEvent>> cached_raise = new Dictionary<Type, Action<IEvent>>();

        private class BusMap {
            public Action<IEventReceiverBase> register;
            public Action<IEventReceiverBase> unregister;
        }

        private class ClassMap {
            public BusMap[] buses;
        }

        public static void Initialize() { }

        static EventBus() {
            var bus_register_map = new Dictionary<Type, BusMap>();

            Type delegateType = typeof(Action<>);
            Type delegategenericregister = delegateType.MakeGenericType(typeof(IEventReceiverBase));
            Type delegategenericraise = delegateType.MakeGenericType(typeof(IEvent));

            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var t in types) {
                if (t != typeof(IEvent) && typeof(IEvent).IsAssignableFrom(t)) {
                    var eventhubtype = typeof(EventBus<>);
                    var genMyClass = eventhubtype.MakeGenericType(t);
                    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(genMyClass.TypeHandle);

                    var busmap = new BusMap() {
                        register =
                            Delegate.CreateDelegate(delegategenericregister, genMyClass.GetMethod("Register")) as
                                Action<IEventReceiverBase>,
                        unregister =
                            Delegate.CreateDelegate(delegategenericregister, genMyClass.GetMethod("UnRegister")) as
                                Action<IEventReceiverBase>
                    };

                    bus_register_map.Add(t, busmap);

                    var method = genMyClass.GetMethod("RaiseAsInterface");
                    cached_raise.Add(t, (Action<IEvent>) Delegate.CreateDelegate(delegategenericraise, method));
                }
            }

            foreach (var t in types) {
                if (typeof(IEventReceiverBase).IsAssignableFrom(t) && !t.IsInterface) {
                    Type[] interfaces = t.GetInterfaces().Where(x =>
                        x != typeof(IEventReceiverBase) && typeof(IEventReceiverBase).IsAssignableFrom(x)).ToArray();

                    ClassMap map = new ClassMap() {
                        buses = new BusMap[interfaces.Length]
                    };

                    for (int i = 0; i < interfaces.Length; i++) {
                        var arg = interfaces[i].GetGenericArguments()[0];
                        map.buses[i] = bus_register_map[arg];
                    }

                    class_register_map.Add(t, map);
                }
            }
        }

        public static void Register(IEventReceiverBase target) {
            var t = target.GetType();
            var map = class_register_map[t];

            foreach (var busmap in map.buses) {
                busmap.register(target);
            }
        }

        [UsedImplicitly]
        public static void UnRegister(IEventReceiverBase target) {
            var t = target.GetType();
            var map = class_register_map[t];

            foreach (var busmap in map.buses) {
                busmap.unregister(target);
            }
        }
        
        [UsedImplicitly]
        public static void Raise(IEvent ev) {
            cached_raise[ev.GetType()](ev);
        }
    }
}