// dear imgui: Complete Backend for Unity
// This Backend will choose its Platform and Renderer based on what Unity is using.
//       ! DO NOT USE OTHER BACKENDS DIRECTLY, THEY WILL BREAK !

// Implemented features:
//  [x] Input Events
//  [ ] Focus Events
//  [ ] Only plugin can move/close window
//  [ ] Unity Docking / Window changes
//  [ ] Tooltips
//  [ ] Popups
//  [ ] Child/Sub windows
//  [ ] Drag/Drop - Dear ImGui <-> Dear ImGui
//  [ ] Drag/Drop - Dear ImGui <-> Unity
//  [ ] Textures from Unity
//  [ ] Correcly mixing Gui elements from Unity and Dear ImGui
// 
//  [x] Platform: Win32
//  [x] Renderer: DirectX11
//  [ ] Renderer: DirectX12
//  [ ] Platform: macOS
//  [ ] Platform: Metal

// Desired execution order
// Unity Init plugin
// ImGui Init backend
// Unity OnGUI()
// Unity - Get global hid-Input
// ImGui - Save global hid-Input from Unity
// ImGui - NewFrame() if first for this UnityEditorWindow
// ImGui - - create new ImContext
// ImGui - else
// ImGui - - switch to associated context
// ImGui - proccess inter-window data and hid-input form global context
// Unity - if UnityEditorWindow was closed/transformed - tell ImGui
// Unity - OnGui() <- User defined drawing
// ImGui - EndFrame()
// ImGui - copy inter-window data to global context
// ImGui - if NginEditorWindow was closed/transformed - tell Unity
// ImGui - Render()
// Unity Exit

// You can use unmodified imgui_impl_* files in your project. See examples/ folder for examples of using this.
// Prefer including the entire imgui/ repository into your project (either as a copy or as a submodule), and only build the backends you need.
// Learn about Dear ImGui:
// - FAQ                  n/a
// - Getting Started      n/a
// - Documentation        n/a
// - Introduction, links and more at the top of imgui.cpp

#pragma once
#include "../imgui.h"      // IMGUI_IMPL_API
#include "../imgui_internal.h"
#include "../../UnityAPI/IUnityInterface.h"
#include "../../UnityAPI/IUnityGraphics.h"
#include "../../UnityAPI/IUnityRenderingExtensions.h"
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
#include <d3d11.h>
#include <d3d12.h>
#include "../../UnityAPI/IUnityGraphicsD3D11.h"
#include "../../UnityAPI/IUnityGraphicsD3D12.h"
#include "imgui_impl_win32.h"
#include "imgui_impl_dx11.h"
#include "imgui_impl_dx12.h"
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what includes Unity on Linux uses... 
#elif __OBJC__
#import <Cocoa/Cocoa.h>
#import <Metal/Metal.h>
#import <MetalKit/MetalKit.h>
#include "../../UnityAPI/IUnityGraphicsMetal.h"
#include "imgui_impl_osx.h"
#include "imgui_impl_metal.h"
#endif
#ifndef IMGUI_DISABLE

// C++ s_{func_name}Callback(...)
// C # Set{func_name}Callback(UnityCallback_{func_name})
#define CREATE_UNITY_CALLBACK_FUNCTION(return_type, func_name, ...)typedef return_type (UNITY_INTERFACE_API* UnityCallback_##func_name)(__VA_ARGS__);static inline UnityCallback_##func_name s_##func_name##Callback;static inline bool s_##func_name##CallbackExists = false;extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API Set##func_name##Callback(UnityCallback_##func_name CallbackFunc){s_##func_name##Callback = CallbackFunc;s_##func_name##CallbackExists = true;}

static IUnityInterfaces* s_UnityInterfaces = NULL;
static IUnityGraphics* s_Graphics = NULL;
static IUnityGraphics* s_GraphicsBackend = NULL;
static UnityGfxRenderer s_RendererType = kUnityGfxRendererNull;

// Unity plugin load event
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(IUnityInterfaces * unityInterfaces);
// Unity plugin unload event
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload();

void ImGui_ImplUnity_NewFrame(intptr_t handle);
void ImGui_ImplUnity_RenderFrame(ImDrawData* drawData);
void ImGui_ImplUnity_NewWindow(ImGuiContext* Context, intptr_t handle);
void ImGui_ImplUnity_CloseWindow(ImGuiContext* Context);


void ImGui_StyleColorsUnityShadow(ImGuiStyle* dst = nullptr);


struct GlobalImGuiContext
{
	static inline ImGuiContext*			DefaultContext;				// Set at Unity start - should be set when no windows are active.
	static inline ImGuiStyle              Style;

	// Inputs
	static inline ImVector<ImGuiInputEvent> InputEventsQueue;                 // Input events which will be trickled/written into IO structure.
	static inline ImVector<ImGuiInputEvent> InputEventsTrail;                 // Past input events processed in NewFrame(). This is to allow domain-specific application to access e.g mouse/pen trail.
	static inline ImGuiMouseSource		  InputEventsNextMouseSource;

	// Shared stacks
	static inline ImVector<ImGuiColorMod>     ColorStack;                     // Stack for PushStyleColor()/PopStyleColor() - inherited by Begin()
	static inline ImVector<ImGuiStyleMod>     StyleVarStack;                  // Stack for PushStyleVar()/PopStyleVar() - inherited by Begin()
	static inline ImVector<ImFont*>           FontStack;                      // Stack for PushFont()/PopFont() - inherited by Begin()
	static inline ImVector<ImGuiID>           FocusScopeStack;                // Stack for PushFocusScope()/PopFocusScope() - inherited by BeginChild(), pushed into by Begin()
	static inline ImVector<ImGuiItemFlags>    ItemFlagsStack;                 // Stack for PushItemFlag()/PopItemFlag() - inherited by Begin()
	static inline ImVector<ImGuiGroupData>    GroupStack;                     // Stack for BeginGroup()/EndGroup() - not inherited by Begin()
	static inline ImVector<ImGuiPopupData>    OpenPopupStack;                 // Which popups are open (persistent)
	static inline ImVector<ImGuiPopupData>    BeginPopupStack;                // Which level of BeginPopup() we are in (reset every frame)
	static inline ImVector<ImGuiNavTreeNodeData> NavTreeNodeStack;            // Stack for TreeNode() when a NavLeft requested is emitted.

	// Drag and Drop
	static inline bool                    DragDropActive;
	static inline bool                    DragDropWithinSource;               // Set when within a BeginDragDropXXX/EndDragDropXXX block for a drag source.
	static inline bool                    DragDropWithinTarget;               // Set when within a BeginDragDropXXX/EndDragDropXXX block for a drag target.
	static inline ImGuiDragDropFlags      DragDropSourceFlags;
	static inline int                     DragDropSourceFrameCount;
	static inline int                     DragDropMouseButton;
	static inline ImGuiPayload            DragDropPayload;
	static inline ImRect                  DragDropTargetRect;                 // Store rectangle of current target candidate (we favor small targets when overlapping)
	static inline ImGuiID                 DragDropTargetId;
	static inline ImGuiDragDropFlags      DragDropAcceptFlags;
	static inline float                   DragDropAcceptIdCurrRectSurface;    // Target item surface (we resolve overlapping targets by prioritizing the smaller surface)
	static inline ImGuiID                 DragDropAcceptIdCurr;               // Target item id (set at the time of accepting the payload)
	static inline ImGuiID                 DragDropAcceptIdPrev;               // Target item id from previous frame (we need to store this to allow for overlapping drag and drop targets)
	static inline int                     DragDropAcceptFrameCount;           // Last time a target expressed a desire to accept the source
	static inline ImGuiID                 DragDropHoldJustPressedId;          // Set when holding a payload just made ButtonBehavior() return a press.
	static inline ImVector<unsigned char> DragDropPayloadBufHeap;             // We don't expose the ImVector<> directly, ImGuiPayload only holds pointer+size
	static inline unsigned char           DragDropPayloadBufLocal[16];        // Local buffer for small payloads

	// Next window/item data
	static inline ImGuiNextWindowData     NextWindowData;                     // Storage for SetNextWindow** functions


	static void OverrideContext(ImGuiContext* context)
	{
		IM_ASSERT(context && "No context given!");
		if(!context) return;
		//context->IO = IO;
		context->Style = Style;
		context->InputEventsNextMouseSource = InputEventsNextMouseSource;
		context->InputEventsTrail = InputEventsTrail;
		context->InputEventsQueue = InputEventsQueue;
		context->ColorStack = ColorStack;
		context->StyleVarStack = StyleVarStack;
		context->FontStack = FontStack;
		context->FocusScopeStack = FocusScopeStack;
		context->ItemFlagsStack = ItemFlagsStack;
		context->GroupStack = GroupStack;
		context->OpenPopupStack = OpenPopupStack;
		context->BeginPopupStack = BeginPopupStack;
		context->NavTreeNodeStack = NavTreeNodeStack;
		context->DragDropActive = DragDropActive;
		context->DragDropWithinSource = DragDropWithinSource;
		context->DragDropWithinTarget = DragDropWithinTarget;
		context->DragDropSourceFlags = DragDropSourceFlags;
		context->DragDropSourceFrameCount = DragDropSourceFrameCount;
		context->DragDropMouseButton = DragDropMouseButton;
		context->DragDropPayload = DragDropPayload;
		context->DragDropTargetRect = DragDropTargetRect;
		context->DragDropTargetId = DragDropTargetId;
		context->DragDropAcceptFlags = DragDropAcceptFlags;
		context->DragDropAcceptIdCurrRectSurface = DragDropAcceptIdCurrRectSurface;
		context->DragDropAcceptIdCurr = DragDropAcceptIdCurr;
		context->DragDropAcceptIdPrev = DragDropAcceptIdPrev;
		context->DragDropAcceptFrameCount = DragDropAcceptFrameCount;
		context->DragDropHoldJustPressedId = DragDropHoldJustPressedId;
		context->DragDropPayloadBufHeap = DragDropPayloadBufHeap;
		memcpy(context->DragDropPayloadBufLocal, DragDropPayloadBufLocal, 16);
		context->NextWindowData = NextWindowData;
	}

	static void SaveContext(ImGuiContext* context)
	{
		IM_ASSERT(context && "No context given!");
		if(!context) return;
		//IO = context->IO;
		Style = context->Style;
		InputEventsNextMouseSource = context->InputEventsNextMouseSource;
		InputEventsTrail = context->InputEventsTrail;
		InputEventsQueue = context->InputEventsQueue;
		ColorStack = context->ColorStack;
		StyleVarStack = context->StyleVarStack;
		FontStack = context->FontStack;
		FocusScopeStack = context->FocusScopeStack;
		ItemFlagsStack = context->ItemFlagsStack;
		GroupStack = context->GroupStack;
		OpenPopupStack = context->OpenPopupStack;
		BeginPopupStack = context->BeginPopupStack;
		NavTreeNodeStack = context->NavTreeNodeStack;
		DragDropActive = context->DragDropActive;
		DragDropWithinSource = context->DragDropWithinSource;
		DragDropWithinTarget = context->DragDropWithinTarget;
		DragDropSourceFlags = context->DragDropSourceFlags;
		DragDropSourceFrameCount = context->DragDropSourceFrameCount;
		DragDropMouseButton = context->DragDropMouseButton;
		DragDropPayload = context->DragDropPayload;
		DragDropTargetRect = context->DragDropTargetRect;
		DragDropTargetId = context->DragDropTargetId;
		DragDropAcceptFlags = context->DragDropAcceptFlags;
		DragDropAcceptIdCurrRectSurface = context->DragDropAcceptIdCurrRectSurface;
		DragDropAcceptIdCurr = context->DragDropAcceptIdCurr;
		DragDropAcceptIdPrev = context->DragDropAcceptIdPrev;
		DragDropAcceptFrameCount = context->DragDropAcceptFrameCount;
		DragDropHoldJustPressedId = context->DragDropHoldJustPressedId;
		DragDropPayloadBufHeap = context->DragDropPayloadBufHeap;
		memcpy(DragDropPayloadBufLocal, context->DragDropPayloadBufLocal, 16);
		NextWindowData = context->NextWindowData;
	}
	static void OverrideDefaultContext()
	{
		if(!DefaultContext) 
		{
			Debug::LogWarning("No DefaultContext! Creating new one.");
			DefaultContext = ImGui::CreateContext();
		}
		OverrideContext(DefaultContext);
	}
	static void SaveDefaultContext()
	{
		if(!DefaultContext)
		{
			OverrideDefaultContext();
		}
		SaveContext(DefaultContext);
	}
};

#endif // #ifndef IMGUI_DISABLE