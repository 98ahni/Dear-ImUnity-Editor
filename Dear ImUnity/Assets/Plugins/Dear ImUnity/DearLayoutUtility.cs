using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.UIElements;

namespace Dear
{
    public static partial class ImGui
    {
        public static class LayoutUtility
        {
            public static void WriteDearStyleToUnityStyle()
            {

            }
            public static void WriteUnityStyleToDearStyle()
            {

            }
            /// <summary> Currently NOT working! </summary>
            public static T InsertUnityGUI<T>(Func<Rect, T> GUIFunc, T Default)
            {
                if(Event.current.type == EventType.Repaint)
                {
                    Rect rect = GetUnityRect();
                    //ImVec4 cData = new ImVec4(rect.x, rect.y, rect.width, rect.height);
                    //IntPtr dataPtr = Marshal.AllocHGlobal(sizeof(float) * 4);
                    //Marshal.StructureToPtr(cData, dataPtr, false);
                    //GCHandle rectHandle = GCHandle.Alloc(rect, GCHandleType.Pinned);
                    Func<T> func = () => GUIFunc(rect);
                    GCHandle funcHandle = GCHandle.Alloc(func, GCHandleType.Pinned);
                    GetWindowDrawList().AddCallback((ref ImDrawList parent_list, ref ImDrawCmd cmd) =>
                    {
                        //GCHandle handle = GCHandle.Alloc(cmd, GCHandleType.Pinned);
                        ImDrawCmd cmdPtr = new ImDrawCmd((IntPtr)(object)cmd);
                        //GUIFunc(Marshal.PtrToStructure<Rect>(cmdPtr.UserCallbackData));
                        ((Func<T>)GCHandle.FromIntPtr(cmdPtr.UserCallbackData).Target)();
                        //Marshal.FreeHGlobal(cmdPtr.UserCallbackData);
                        GCHandle.FromIntPtr(cmdPtr.UserCallbackData).Free();
                    }, GCHandle.ToIntPtr(funcHandle));
                    return Default;
                }
                return GUIFunc(GetUnityRect());
            }
            /// <summary>Reserves a Rect in Unity and Dear.</summary>
            public static Rect GetUnityRect()
            {
                //Rect output = new Rect(GetCursorPosX() - 2, GetCursorPosY() - 24, GetWindowWidth() - GetCursorPosX(), 18);
                Rect output = new Rect(GetCursorPosX() - 2, GetCursorPosY() - 24, CalcItemWidth() /*GetWindowWidth() - GetCursorPosX()*/, 18);
                //Vector2 dummy = new Vector2(GetWindowWidth() - GetCursorPosX(), 18);
                Vector2 dummy = new Vector2(CalcItemWidth(), 18);
                Dummy(out dummy);
                return output;
            }
        }
    }
}
