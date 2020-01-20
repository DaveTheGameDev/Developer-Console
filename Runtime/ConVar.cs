using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeveloperConsole
{
    
    public class ConVar<T>
    {
        public readonly string command;

        public ConVar(string command)
        {
            this.command = command;

            ConVar<string> stringConVar = this as ConVar<string>;
            if (stringConVar != null)
            {
                ConsoleSystem.registeredStringConVars.Add(command, stringConVar);
                return;
            }
            
            ConVar<int> intConVar = this as ConVar<int>;
            if (intConVar != null)
            {
                ConsoleSystem.registeredIntConVars.Add(command, intConVar);
                return;
            }

            ConVar<float> floatConVar = this as ConVar<float>;
            if (floatConVar != null)
            {
                ConsoleSystem.registeredFloatConVars.Add(command, floatConVar);
                return;
            }

            ConVar<bool> boolConVar = this as ConVar<bool>;
            if (boolConVar != null)
            {
                ConsoleSystem.registeredBoolConVars.Add(command, boolConVar);
            }
        }

        public void Print()
        {
            switch (typeof(T).Name.ToLower())
            {
                case "string":
                    ConsoleSystem.Log($"{command} = {stringValue}");
                    break;
                case "boolean":
                    ConsoleSystem.Log($"{command} = {boolValue}");
                    break;
                case "int32":
                    ConsoleSystem.Log($"{command} = {intValue}");
                    break;
                case "single":
                    ConsoleSystem.Log($"{command} = {floatValue}");
                    break;
            }
        }
        
        private string stringValue;
        public string StringValue
        {
            get => stringValue;
            
            set
            {
                stringValue = value;

                foreach (Action<ConVar<T>> hook in onUpdateHooks)
                {
                    hook?.Invoke(this);
                }
            }
        }
        
        private int intValue;
        public int IntValue
        {
            get => intValue;
            
            set
            {
                intValue = value;

                foreach (Action<ConVar<T>> hook in onUpdateHooks)
                {
                    hook?.Invoke(this);
                }
            }
        }
        
        private float floatValue;
        public float FloatValue
        {
            get => floatValue;
            
            set
            {
                floatValue = value;

                foreach (Action<ConVar<T>> hook in onUpdateHooks)
                {
                    hook?.Invoke(this);
                }
            }
        }
        
        private bool boolValue;
        public bool BoolValue
        {
            get => boolValue;
            
            set
            {
                boolValue = value;

                foreach (Action<ConVar<T>> hook in onUpdateHooks)
                {
                    hook?.Invoke(this);
                }
            }
        }

        private readonly List<Action<ConVar<T>>> onUpdateHooks =  new List<Action<ConVar<T>>>();
        
        public void AddListener(Action<ConVar<T>> action)
        {
            onUpdateHooks.Add(action);
        }
        
        public void RemoveListener(Action<ConVar<T>> action)
        {
            onUpdateHooks.Remove(action);
        }
    }

    public static class Test
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            TestConVar.AddListener(OnUpdate);
        }
        
        public static ConVar<int> TestConVar = new ConVar<int>("debug.test");
        
        private static void OnUpdate(ConVar<int> conVar)
        {
            Debug.Log("TeSTs");
        }
    }
}