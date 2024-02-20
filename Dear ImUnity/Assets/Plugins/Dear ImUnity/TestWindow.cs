using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Dear;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;

public class HandleTest : EditorWindow
{
    [MenuItem("Dear ImGui/HandleTestWindow")]
    public static void Init()
    {
        HandleTest wnd1 = CreateWindow<HandleTest>("HandleTest1");
        wnd1.Handle = wnd1.GetNativeID().ToString();
        //HandleTest wnd2 = CreateWindow<HandleTest>("HandleTest2");
        //wnd2.Handle = wnd2.GetNativeID().ToString();
    }

    string Handle = "";

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Handle: " + this.GetNativeHandle().ToString());
        EditorGUILayout.LabelField("CHWND: " + Handle);
        EditorGUILayout.LabelField("HWND: " + this.GetNativeID().ToString());
        EditorGUILayout.LabelField("WndPtr: " + this.GetNativeWindowPtr().ToString());
        if (Event.current.type == EventType.Layout)
        {
            GUI.backgroundColor = Color.red;
            GUILayout.Button("  Red  ");
            Debug.Log("Layout" + titleContent.text);
        }
        if (Event.current.type == EventType.Repaint)
        {
            GUI.backgroundColor = Color.green;
            GUILayout.Button(" Green ");
            Debug.Log("Repaint" + titleContent.text);
        }
        GUILayout.Button(" ");
        GUI.backgroundColor = Color.white;
    }
}

public class TestWindow2 : ImGui.EditorWindow
{
    Vector2 _imTestVector;
    bool _imTestValue;
    bool _uniTestValue;
    [MenuItem("Dear ImGui/TestWindow")]
    public static void Init()
    {
        TestWindow2 wnd = CreateWindow<TestWindow2>("Test");
    }
    void OnGUI()
    {
        if (!ImGui_BeginWindow()) return;
        ImGui.Begin("Test", out _, ImGuiWindowFlags.NoResize);// 0);// 
        Dear.ImGui.Checkbox("This is an ImGui checkbox", out _imTestValue);
        _uniTestValue = EditorGUI.Toggle(ImGui.LayoutUtility.GetUnityRect(), "This is a Unity checkbox", _uniTestValue);
        Dear.ImGui.Checkbox("This is another ImGui checkbox", out _imTestValue);
        _imTestVector.x = EditorGUI.FloatField(ImGui.LayoutUtility.GetUnityRect(), "Mixed Vector", _imTestVector.x);
        ImGui.SameLine();
        ImGui.DragFloat("##vectory", out _imTestVector.y);
        ImGui.DragFloat2("Imvector", out _imTestVector.x);
        ImGui.DragFloat("Mixed Vector##vectorx", out _imTestVector.x);
        ImGui.SameLine();
        _imTestVector.y = EditorGUI.FloatField(ImGui.LayoutUtility.GetUnityRect(), "", _imTestVector.y);
        //Vector2 rectMin = new Vector2(40, 40);
        //Vector2 rectMax = new Vector2(80, 80);
        //ImGui.GetWindowDrawList().AddRectFilled(out rectMin, out rectMax, new ImCol(1, 0, 0, 0));
        //EditorGUI.DrawRect(new Rect(rectMin.x, rectMin.y, rectMin.x, rectMin.y), Color.white);
        ImGui.End();
        ImGui_EndWindow();
    }
}

public class DemoWindow : ImGui.EditorWindow
{
    [MenuItem("Dear ImGui/Demo Window")]
    public static void Init()
    {
        CreateWindow<DemoWindow>("Demo");
    }

    void OnGUI()
    {
        if (!ImGui_BeginWindow()) return;
        Vector2 pos = Vector2.zero;
        ImGui.SetWindowPos("Dear ImGui Demo", out pos);
        Vector2 size = position.size;
        size.y += 22;           // We set the window position/size like this because...
        ImGui.SetWindowSize("Dear ImGui Demo", out size);
        ImGui.ShowDemoWindow(); // This window originally sets its own position and size at line 341-342 in imgui_demo.cpp.
        ImGui_EndWindow();
    }
}

public class LogWindow : ImGui.EditorWindow
{
    [MenuItem("Dear ImGui/Log Window")]
    public static void Init()
    {
        CreateWindow<LogWindow>("Dear Log");
    }

    void OnGUI()
    {
        if (!ImGui_BeginWindow()) return;
        ImGui.ShowDebugLogWindow();
        ImGui_EndWindow();
    }
}
