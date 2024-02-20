using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public static class NativeCalls
{
    public static IntPtr GetNativeHandle(this EditorWindow Window)
    {
        _EditorWindow wnd = new _EditorWindow(Window);
        _HostView view = new _HostView(wnd.m_Parent);
        return (IntPtr)view.nativeHandle;
    }
    public static IntPtr GetNativeID(this EditorWindow Window)
    {
        Window.Focus();
#if UNITY_EDITOR_WIN
        return GetActiveWindow();
#elif UNITY_EDITOR_OSX
        //AppKit.NSWindow.FromWindowRef(GetNativeWindowPtr(Window));
        return GetNativeHandle(Window);             // Maybe..? I can't find a way to get an NSWindow pointer...
#endif
    }
    public static IntPtr GetNativeWindowPtr(this EditorWindow Window)
    {
        _EditorWindow wnd = new _EditorWindow(Window);
        _HostView view = new _HostView(wnd.m_Parent);
        _ContainerWindow cwd = new _ContainerWindow(view.window);
        _MonoReloadableIntPtr ptr = new _MonoReloadableIntPtr(cwd.m_WindowPtr);
        return ptr.m_IntPtr;
    }

    public static EditorWindow GetCurrentUnityWindow(this EditorWindow Window)
    {
        _EditorWindow wnd = new _EditorWindow(Window);
        _HostView view = new _HostView(wnd.m_Parent);
        _HostView currView = new _HostView(view.current);
        return currView.m_ActualView;
    }


    [DllImport("user32")]
    public static extern IntPtr GetActiveWindow();

    private class _EditorWindow
    {
        private EditorWindow instance;
        private Type type;

        public _EditorWindow(EditorWindow instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        public void RepaintImmediately()
        {
            var method = type.GetMethod("RepaintImmediately", BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(instance, null);
        }

        public object m_Parent
        {
            get
            {
                var field = type.GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic);
                return field.GetValue(instance);
            }
        }
    }

    private class _HostView
    {
        private object instance;
        private Type type;

        public _HostView(object instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        // ContainerWindow
        public object window
        {
            get
            {
                var property = type.GetProperty("window", BindingFlags.Instance | BindingFlags.Public);
                return property.GetValue(instance, null);
            }
        }

        // GUIView as HostView
        public object current
        { // UnityEditor.GUIView
            get
            {
                var property = type.GetProperty("current", BindingFlags.Static | BindingFlags.Public);
                return Convert.ChangeType(property.GetValue(instance, null), type);
            }
        }

        // EditorWindow
        public EditorWindow m_ActualView
        { // UnityEditor.Hostview
            get
            {
                var field = type.GetField("m_ActualView", BindingFlags.Instance | BindingFlags.NonPublic);
                return (EditorWindow)field.GetValue(instance);
            }
        }

        // ContainerWindow
        public object m_Window
        { // UnityEditor.View
            get
            {
                var field = type.GetField("m_Window", BindingFlags.Instance | BindingFlags.NonPublic);
                return field.GetValue(instance);
            }
        }

        // MonoReloadableIntPtr
        public object m_ViewPtr
        { // UnityEditor.View
            get
            {
                var field = type.GetField("m_ViewPtr", BindingFlags.Instance | BindingFlags.NonPublic);
                return field.GetValue(instance);
            }
        }

        public object nativeHandle
        {
            get
            {
                //var property = typeof(EditorWindow).Assembly.GetType("UnityEditor.GUIView").GetProperty("nativeHandle", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic);
                var property = type.GetProperty("nativeHandle", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic);
                return property.GetValue(instance, null);
            }
        }
    }

    private class _ContainerWindow
    {
        private object instance;
        private Type type;

        public _ContainerWindow(object instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        public object m_WindowPtr
        {
            get
            {
                var field = type.GetField("m_WindowPtr", BindingFlags.Instance | BindingFlags.NonPublic);
                return field.GetValue(instance);
            }
        }
    }

    private class _MonoReloadableIntPtr
    {
        private object instance;
        private Type type;

        public _MonoReloadableIntPtr(object instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        public IntPtr m_IntPtr
        {
            get
            {
                var field = type.GetField("m_IntPtr", BindingFlags.Instance | BindingFlags.NonPublic);
                return (IntPtr)field.GetValue(instance);
            }
        }
    }
}
