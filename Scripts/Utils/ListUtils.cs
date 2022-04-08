using System;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Utils
{
    public static class ListUtils
    {
        public static void CloneList<T>(List<T> Target, List<T> WhatToClone)
        {
            if(Target != null)
            {
                Target.Clear();
            }
            else
            {
                Target = new List<T>();
            }

            for (int L = 0; L < WhatToClone.Count; L++)
            {
                Target.Add(WhatToClone[L]);
            }
        }

        public static List<T> RandomItemsFromList<T>(List<T> Items, int Amount)
        {
            List<T> Result = new List<T>();
            for (int I = 0; I < Amount; I++)
            {
                int Index = UnityEngine.Random.Range(0, Amount + 1);
                Result.Add(Items[Index]);
                Items.RemoveAt(Index);
            }

            return Result;
        }

        public static int IndexOfItem<T>(List<T> Items, T Item)
        {
            return BinarySearch<T>(Items, Item, 0, Items.Count);
        }
        static int BinarySearch<T>(List<T> Items, T Target, int Start, int End)
        {
            if(Start > End)
            {
                Debug.LogError("Invalid Start Index!");
                return 1;
            }

            int Middle = Mathf.FloorToInt((Start + End) / 2);

            if(Target.Equals(Items[Middle]))
            {
                return Middle;
            }
            if(Items.IndexOf(Target) > Middle)
            {
                return BinarySearch<T>(Items, Target, Start, Middle - 1);
            }
            if(Items.IndexOf(Target) < Middle)
            {
                return BinarySearch<T>(Items, Target, Middle + 1, End);
            }
            return 0;
        }

        public static bool ValidIndex<T>(List<T> Items, int Index)
        {
            return (Index > -1 && Index < Items.Count);
        }
    }
}