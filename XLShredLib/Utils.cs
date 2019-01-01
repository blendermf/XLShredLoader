using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using UnityModManagerNet;

namespace XLShredLib {

    public class XLShredLogger {
        private string lastMessage;
        private int repeatMessageCount;
        private static XLShredLogger instance;

        static XLShredLogger() {
            instance = new XLShredLogger();
        }


        public static void Log(string message, Type type = null) {
            string newMessage = (type == null) ? $"MOD_LOG: {message}": $"MOD_LOG {type.Name}: {message}";

            if (instance.lastMessage == newMessage) {
                instance.repeatMessageCount++;
            } else {
                instance.repeatMessageCount = 0;
                instance.lastMessage = newMessage;
            }

            if (instance.repeatMessageCount < 10) {
                Console.WriteLine(newMessage);
                return;
            }

            newMessage = $"{newMessage} ({instance.repeatMessageCount})";

            if (instance.repeatMessageCount <= 100 && instance.repeatMessageCount % 10 == 0) {
                Console.WriteLine(newMessage);
            } else if (instance.repeatMessageCount <= 1000 && instance.repeatMessageCount % 100 == 0) {
                Console.WriteLine(newMessage);
            } else if (instance.repeatMessageCount <= 10000 && instance.repeatMessageCount % 1000 == 0) {
                Console.WriteLine(newMessage);
            }
        }
    }

    public static class ModUtils {

        public static Assembly GetModAssembly(string modID) {
            var mod = UnityModManager.FindMod(modID);

            return (mod != null) ? GetModAssembly(mod) : null;
        }

        public static Assembly GetModAssembly(this UnityModManager.ModEntry modEntry) {
            if (modEntry == null) return null;

            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ReplayModLoader");
        }

        public static Type GetModType(string modID, string typeName) {
            var modEntry = UnityModManager.FindMod(modID);

            if (modEntry == null) return null;

            return modEntry.GetModType(typeName);
        }

        public static Type GetModType(this UnityModManager.ModEntry modEntry, string typeName) {
            if (modEntry == null) return null;

            Assembly assembly = GetModAssembly(modEntry);
            if (assembly == null) return null;

            return assembly.GetType(typeName, false);
        }

        public static object GetModObject(this UnityModManager.ModEntry modEntry, string typeName, Func<Type, object> getValue) {
            Type type = modEntry.GetModType(typeName);
            
            return (type == null) ? null : getValue.Invoke(type);
        }

        public static object GetModObject(string modID, string typeName, Func<Type, object> getValue) {
            var modEntry = UnityModManager.FindMod(modID);

            return modEntry?.GetModObject(typeName, getValue);
        }

        public static object GetModObject(string modID, string typeName, out UnityModManager.ModEntry modEntry, Func<Type, object> getValue) {
            modEntry = UnityModManager.FindMod(modID);

            return modEntry?.GetModObject(typeName, getValue);
        }
    }
}
