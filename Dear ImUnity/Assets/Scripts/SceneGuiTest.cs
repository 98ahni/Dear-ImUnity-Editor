using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Dear;
using UnityEngine.Rendering;
using static Dear.ImGui;
using UnityEngine.UIElements;

public class SceneGuiTest : MonoBehaviour
{
    public IntPtr Handle;
    public bool _imTestValue;

    private void OnEnable()
    {
        Handle = NativeCalls.GetActiveWindow();
    }

    void OnGUI()
    {
        GL.IssuePluginEvent(Marshal.GetFunctionPointerForDelegate<Action<IntPtr>>(ImGui_RenderUnityFrame), (int)Handle);
    }

    void Update()
    {
        ImGui_SetWindowContext(Handle);
        ImGui_NewUnityFrame(Handle);
        RelayEvent.InputUpdate();
        ImGui.NewFrame();
        ImGui.Begin("Test", out _, 0);
        Dear.ImGui.Checkbox("This is an ImGui checkbox", out _imTestValue);
        Dear.ImGui.Checkbox("This is another ImGui checkbox", out _imTestValue);
        ImGui.End();
        ImGui.Render();
        ImGui_SaveWindowContext(Handle);
    }
}
