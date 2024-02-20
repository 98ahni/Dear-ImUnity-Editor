using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// This file is NOT auto generated.
/// Add structs for template types and C-typedefs.
/// TODO: Add conditionals for editor and standalone.

[InitializeOnLoad]
public class StartupWarning
{
    static StartupWarning()
    {
        if (!EditorPrefs.HasKey("Seen ImUnity startup warning"))
        {
            EditorPrefs.SetBool("Seen ImUnity startup warning", true);
            EditorUtility.DisplayDialog("Attention!",
                "The Dear ImGui plug-in is currently only meant as a proof of concept. " +
                "This means that stability is not guaranteed and many things are untested or unfinnished. " +
                "\n\nIf you'd like to test things yourself, please look at \"TestWindow.cs\" and \"SceneGuiTest.cs\" as a reference. " +
                "\n\n*Please note* that you are required by license to contribute any changes made " +
                "to https://github.com/98ahni/Dear-ImUnity-Editor.", "I understand");
        }
    }
}

namespace Dear
{
    public struct ImVec2
    {
        public float x, y;
        public ImVec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static implicit operator ImVec2(Vector2 vec)
        {
            return new ImVec2 { x = vec.x, y = vec.y };
        }
        public static implicit operator Vector2(ImVec2 vec)
        {
            return new Vector2 { x = vec.x, y = vec.y };
        }
    }
    public struct ImVec4
    {
        public float x, y, z, w;
        public ImVec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public static implicit operator ImVec4(Vector4 vec)
        {
            return new ImVec4 { x = vec.x, y = vec.y, z = vec.z, w = vec.w };
        }
        public static implicit operator Vector4(ImVec4 vec)
        {
            return new Vector4 { x = vec.x, y = vec.y, z = vec.z, w = vec.w };
        }
    }
    public struct ImTextureID
    {
        System.IntPtr _ptr;
        public static explicit operator ImTextureID(Texture tPtr)
        {
            return new ImTextureID { _ptr = tPtr.GetNativeTexturePtr() };
        }
    }
    public struct ImGuiID
    {
        System.UInt32 _id;
        public ImGuiID(uint id)
        {
            _id = id;
        }
        public static implicit operator ImGuiID(uint id)
        {
            return new ImGuiID(id);
        }
        public static implicit operator uint(ImGuiID id)
        {
            return id._id;
        }
    }
    public struct ImDrawIdx
    {
        System.UInt16 _id;
        public static explicit operator ImDrawIdx(ushort id)
        {
            return new ImDrawIdx { _id = id };
        }
    }
    public struct ImCol
    {
        System.UInt32 _col;
        public ushort A
        {
            get { return (ushort)(_col >> 24); }
            set { _col = (_col | 0xFF000000) & ((((uint)value) << 24) | 0x00FFFFFF); }
        }
        public ushort R
        {
            get { return (ushort)(_col); }
            set { _col = (_col | 0x000000FF) & (((uint)value) | 0xFFFFFF00); }
        }
        public ushort G
        {
            get { return (ushort)(_col >> 8); }
            set { _col = (_col | 0x0000FF00) & ((((uint)value) << 8) | 0xFFFF00FF); }
        }
        public ushort B
        {
            get { return (ushort)(_col >> 16); }
            set { _col = (_col | 0x00FF0000) & ((((uint)value) << 16) | 0xFF00FFFF); }
        }
        public ImCol(uint col)
        {
            _col = col;
        }
        public ImCol(Color col)
        {
            _col = (((uint)(col.a * 255) << 24) | ((uint)(col.b * 255) << 16) | ((uint)(col.g * 255) << 8) | ((uint)(col.r * 255) << 0));
        }
        public ImCol(float a, float r, float g, float b)
        {
            _col = ((uint)(a * 255) << 24) | ((uint)(b * 255) << 16) | ((uint)(g * 255) << 8) | ((uint)(r * 255) << 0);
        }
        public static implicit operator ImCol(uint col)
        {
            return new ImCol(col);
        }
        public static implicit operator uint(ImCol col)
        {
            return col._col;
        }
        public static implicit operator ImCol(Color col)
        {
            return new ImCol(col);
        }
        public static implicit operator Color(ImCol col)
        {
            return new Color(col.R, col.G, col.B, col.A);
        }
    }

    public static partial class ImGui
    {
        public class EditorWindow : UnityEditor.EditorWindow
        {
            IntPtr _InitHandle;
            IntPtr _Handle;
            Vector2 _LastPosition;
            bool _IsVisible = true;
            public EditorWindow()
            {
                _InitHandle = this.GetNativeID();
                _Handle = this.GetNativeID();
                wantsMouseMove = true;
                wantsMouseEnterLeaveWindow = true;
            }
            public void OnBecameVisible()
            {
                _IsVisible = true;
            }
            public void OnBecameInvisible()
            {
                _IsVisible = false;
            }
            public bool ImGui_BeginWindow()
            {
                if(!_IsVisible)
                {
                    return false;
                }
                if(_LastPosition != position.position)
                {
                    _LastPosition = position.position;
                    _Handle = this.GetNativeID();
                }
                if(Event.current.type == EventType.Repaint) GL.IssuePluginEvent(Marshal.GetFunctionPointerForDelegate<Action<IntPtr>>(ImGui_RenderUnityFrame), (int)_Handle);
                ImGui_SetWindowContext(_Handle);
                ImGui_NewUnityFrame(_Handle);
                RelayEvent.InputEvent(Event.current);
                ImGui.NewFrame();
                ImGui_SetNextWindowPos(Vector2.zero);
                Vector2 size = position.size;
                //size.x += 2;
                size.y += 22;
                ImGui_SetNextWindowSize(size);
                return true;
            }
            public void ImGui_EndWindow()
            {
                ImGui.Render();
                ImGui_SaveWindowContext(_Handle);
                if (Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint)
                {
                    Repaint();
                }
            }
        }
        [DllImport("Dear ImGui")]
        public static extern void ImGui_NewUnityFrame(IntPtr Handle);
        [DllImport("Dear ImGui")]
        public static extern void ImGui_SetWindowContext(IntPtr Handle);
        [DllImport("Dear ImGui")]
        public static extern void ImGui_SaveWindowContext(IntPtr Handle);
        [DllImport("Dear ImGui")]
        public static extern void ImGui_RenderUnityFrame(IntPtr Handle);
        [DllImport("Dear ImGui")]
        public static extern void ImGui_SetNextWindowPos(Vector2 Position);
        [DllImport("Dear ImGui")]
        public static extern void ImGui_SetNextWindowSize(Vector2 Size);

        public static class RelayEvent
        {
            public static void InputEvent(Event Current)
            {
                if (Current.isMouse && (Current.type == EventType.MouseUp || Current.type == EventType.MouseDown))
                {
                    ImGui_ImplUnity_AddMouseButtonEvent(Current.button, Current.type == EventType.MouseDown);
                }
                else if (Current.isKey && (Current.type == EventType.KeyUp || Current.type == EventType.KeyDown))
                {
                    ImGui_ImplUnity_AddKeyEvent(Current.keyCode, Current.type == EventType.KeyDown);
                }
                else if (Current.isMouse)
                {
                    Vector2 adjustedPos = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
                    adjustedPos.y = Screen.height - adjustedPos.y;
                    ImGui_ImplUnity_AddMousePosEvent(adjustedPos);
                }
                if (Current.isScrollWheel)
                {
                    ImGui_ImplUnity_AddMouseWheelEvent(Current.delta);
                }
            }
            public static void InputUpdate()
            {
                bool isMouse = false;
                Vector2 adjustedPos = /*GUIUtility.ScreenToGUIPoint*/(Input.mousePosition);
                adjustedPos.y = Screen.height - adjustedPos.y;
                ImGui_ImplUnity_AddMousePosEvent(adjustedPos);
                ImGui_ImplUnity_AddMouseWheelEvent(Input.mouseScrollDelta);
                for (int b = 0; b < 5; b++)
                {
                    if (Input.GetMouseButtonDown(b))
                    {
                        ImGui_ImplUnity_AddMouseButtonEvent(b, true);
                        isMouse = true;
                    }
                    else if (Input.GetMouseButtonUp(b))
                    {
                        ImGui_ImplUnity_AddMouseButtonEvent(b, false);
                        isMouse = true;
                    }
                }
                if (!isMouse) foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(key))
                        {
                            ImGui_ImplUnity_AddKeyEvent(key, true);
                        }
                        else if (Input.GetKeyUp(key))
                        {
                            ImGui_ImplUnity_AddKeyEvent(key, false);
                        }
                    }
            }
            [DllImport("Dear ImGui")]
            private static extern void ImGui_ImplUnity_AddKeyEvent(KeyCode Key, [MarshalAs(UnmanagedType.I1)] bool Down);
            [DllImport("Dear ImGui")]
            private static extern void ImGui_ImplUnity_AddMouseButtonEvent(int Button, [MarshalAs(UnmanagedType.I1)] bool Down);
            [DllImport("Dear ImGui")]
            private static extern void ImGui_ImplUnity_AddMousePosEvent(Vector2 Position);
            [DllImport("Dear ImGui")]
            private static extern void ImGui_ImplUnity_AddMouseWheelEvent(Vector2 Position);
            [DllImport("Dear ImGui")]
            private static extern void ImGui_ImplUnity_AddFocusEvent([MarshalAs(UnmanagedType.I1)] bool IsFocused);
        }
    }

    // Callback and functions types
    /// <summary>Callback function for ImGui::InputText()</summary>
    public delegate int ImGuiInputTextCallback(ImGuiInputTextCallbackData data);
    /// <summary>Callback function for ImGui::SetNextWindowSizeConstraints()</summary>
    public delegate void ImGuiSizeCallback(ImGuiSizeCallbackData data);
    /// <summary>Function signature for ImGui::SetAllocatorFunctions()</summary>
    public delegate IntPtr ImGuiMemAllocFunc(long sz, IntPtr user_data);
    /// <summary>Function signature for ImGui::SetAllocatorFunctions()</summary>
    public delegate void ImGuiMemFreeFunc(IntPtr ptr, IntPtr user_data);
    /// <summary>ImDrawCallback: Draw callbacks for advanced uses [configurable type: override in imconfig.h]
    /// NB: You most likely do NOT need to use draw callbacks just to create your own widget or customized UI rendering,
    /// you can poke into the draw list for that!</summary>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ImDrawCallback(ref ImDrawList parent_list, ref ImDrawCmd cmd);
}