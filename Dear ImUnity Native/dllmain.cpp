// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#define IMGUI_DEFINE_MATH_OPERATORS
#include "ImGui/imgui.h"
#include "ImGui/backends/imgui_impl_unity.h"
#include <vector>
#include <unordered_map>
#include <mutex>

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

struct WindowContext;
WindowContext* CurrentWindow;
std::mutex ContextMutex = {};
struct WindowContext
{
    ImGuiContext* Context;
    void Set()
    {
        IM_ASSERT(CurrentWindow == NULL && "Execution order issue! Previous window didn't get to render.");
        GlobalImGuiContext::SaveDefaultContext();
        GlobalImGuiContext::OverrideContext(Context);
        ImGui::SetCurrentContext(Context);
        CurrentWindow = this;
        ImGui_StyleColorsUnityShadow();
    }
    void Save()
    {
        GlobalImGuiContext::SaveContext(Context);
        GlobalImGuiContext::OverrideDefaultContext();
        ImGui::SetCurrentContext(GlobalImGuiContext::DefaultContext);
        CurrentWindow = nullptr;
    }
    bool IsOpen = true;
};
std::unordered_map<intptr_t, WindowContext> UnityWindows;


extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_NewUnityFrame(intptr_t handle)
{
    ImGui_ImplUnity_NewFrame(handle);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_SetWindowContext(intptr_t handle)
{
    ContextMutex.lock();
    if(UnityWindows.count(handle) == 0)
    {
        Debug::Log("Created new window.");
        UnityWindows[handle] = {};
        UnityWindows[handle].Context = ImGui::CreateContext();
        ImGui_ImplUnity_NewWindow(UnityWindows[handle].Context, handle);
    }
    UnityWindows[handle].Set();
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_SaveWindowContext(intptr_t handle)
{
    IM_ASSERT(ImGui::GetCurrentContext() != nullptr && "Context does not exist!");
    ImGuiContext& g = *ImGui::GetCurrentContext();
    UnityWindows[handle].Save();
    if(!UnityWindows[handle].IsOpen)
    {
        Debug::Log("Closed window");
        //ImGui_ImplUnity_CloseWindow(UnityWindows[handle].Context);
        UnityWindows.erase(handle);
    }
    ContextMutex.unlock();
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_RenderUnityFrame(intptr_t handle)
{
    ContextMutex.lock();
    if(UnityWindows.count(handle) == 0)
    {
        ContextMutex.unlock();
        return;
    }
    UnityWindows[handle].Set();
    ImGui_ImplUnity_RenderFrame(ImGui::GetDrawData());
    UnityWindows[handle].Save();
    ContextMutex.unlock();
}


extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_SetNextWindowPos(ImVec2 aPos)
{
    ImGui::SetNextWindowPos(aPos);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_SetNextWindowSize(ImVec2 aSize)
{
    ImGui::SetNextWindowSize(aSize);
}
