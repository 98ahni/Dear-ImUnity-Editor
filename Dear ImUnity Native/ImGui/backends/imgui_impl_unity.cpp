#pragma once
#include "imgui_impl_unity.h"
#include "imgui_impl_unity_event.h"

static void UNITY_INTERFACE_API OnGraphicsDeviceEvent(UnityGfxDeviceEventType eventType)
{
	switch(eventType)
	{
		case kUnityGfxDeviceEventInitialize:
		{
			s_RendererType = s_Graphics->GetRenderer();
			switch(s_RendererType)
			{
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
				case kUnityGfxRendererD3D11:
					s_GraphicsBackend = (IUnityGraphics*)s_UnityInterfaces->Get<IUnityGraphicsD3D11>();
					break;
				case kUnityGfxRendererD3D12:
					s_GraphicsBackend = (IUnityGraphics*)s_UnityInterfaces->Get<IUnityGraphicsD3D12v7>();
					break;
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what pipelines Unity on Linux uses... 
#elif __OBJC__
				case kUnityGfxRendererMetal:
					s_GraphicsBackend = (IUnityGraphics*)s_UnityInterfaces->Get<IUnityGraphicsMetal>();
					break;
#endif
				case kUnityGfxRendererOpenGLES20:
				case kUnityGfxRendererOpenGLES30:
				case kUnityGfxRendererOpenGLCore:
					// Get OpenGL
				case kUnityGfxRendererVulkan:
					// Get Vulcan
				default:
					IM_ASSERT(false && "No Graphics Available!");
					break;
			}
			//TODO: user initialization code
			Debug::Log("Dear ImGui kUnityGfxDeviceEventInitialize!", LogInfo);

			// Setup Dear ImGui context
			IMGUI_CHECKVERSION();

			GlobalImGuiContext::OverrideDefaultContext();
			ImGui_StyleColorsUnityShadow();
			IM_ASSERT(GlobalImGuiContext::DefaultContext != NULL && "Context could not be created!");
			GlobalImGuiContext::SaveDefaultContext();

			break;
		}
		case kUnityGfxDeviceEventShutdown:
		{
			s_RendererType = kUnityGfxRendererNull;
			Debug::Log("Dear ImGui kUnityGfxDeviceEventShutdown!", LogInfo);
			//TODO: user shutdown code
			//ImGui_ImplDX11_Shutdown();
			break;
		}
		case kUnityGfxDeviceEventBeforeReset:
		{
			Debug::Log("Dear ImGui kUnityGfxDeviceEventBeforeReset!", LogInfo);
			//TODO: user Direct3D 9 code
			break;
		}
		case kUnityGfxDeviceEventAfterReset:
		{
			Debug::Log("Dear ImGui kUnityGfxDeviceEventAfterReset!", LogInfo);
			//TODO: user Direct3D 9 code
			break;
		}
	};
}

// Unity plugin load event
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(IUnityInterfaces * unityInterfaces)
{
	s_UnityInterfaces = unityInterfaces;
	s_Graphics = unityInterfaces->Get<IUnityGraphics>();
	Debug::Init(unityInterfaces->Get<IUnityLog>());
	Debug::Log("Dear ImGui Initialized!", LogInfo);

	s_Graphics->RegisterDeviceEventCallback(OnGraphicsDeviceEvent);
}
// Unity plugin unload event
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload()
{
	Debug::LogWarning("Dear ImGui Uninitialized!", LogInfo);
	s_Graphics->UnregisterDeviceEventCallback(OnGraphicsDeviceEvent);
}

void ImGui_ImplUnity_NewFrame(intptr_t handle)
{
	ImGui_ImplWin32_NewFrame();
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
	ImGui_ImplWin32_NewFrame();
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what backend Unity on Linux uses... 
#elif __OBJC__
	ImGui_ImplOSX_NewFrame((NSView*)handle);
#endif
	switch(s_RendererType)
	{
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
		case kUnityGfxRendererD3D11:
			ImGui_ImplDX11_NewFrame();
			break;
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what backend Unity on Linux uses... 
#elif __OBJC__
		case kUnityGfxRendererMetal:
			ImGui_ImplMetal_NewFrame(((IUnityGraphicsMetalV1*)s_GraphicsBackend)->CurrentRenderPassDescriptor());
			break;
#endif
		default:
			IM_ASSERT(false && "No Graphics Available!");
			break;
	}
}

void ImGui_ImplUnity_RenderFrame(ImDrawData* drawData)
{
	if(drawData == NULL) return;
	switch(s_RendererType)
	{
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
		case kUnityGfxRendererD3D11:
			ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());
			break;
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what backend Unity on Linux uses... 
#elif __OBJC__
		case kUnityGfxRendererMetal:
			ImGui_ImplMetal_RenderDrawData(ImGui::GetDrawData(), ((IUnityGraphicsMetalV1*)s_GraphicsBackend)->CurrentCommandBuffer(), ((IUnityGraphicsMetalV1*)s_GraphicsBackend)->CurrentCommandEncoder());
			break;
#endif
		default:
			IM_ASSERT(false && "No Graphics Available!");
			break;
	}
}

CREATE_UNITY_CALLBACK_FUNCTION(intptr_t, ImGui_ImplUnity_CreateWindow)
void ImGui_ImplUnity_NewWindow(ImGuiContext* Context, intptr_t handle)
{
	IM_ASSERT(Context && "No context was passed!");
	ImGuiContext* current = ImGui::GetCurrentContext();
	ImGui::SetCurrentContext(Context);

#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
	IM_ASSERT(ImGui_ImplWin32_Init((HWND)handle) && "Could not init platform!");
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what backend Unity on Linux uses... 
#elif __OBJC__
	IM_ASSERT(ImGui_ImplOSX_Init((NSView*)handle) && "Could not init platform!");
#endif

	switch(s_RendererType)
	{
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
		case kUnityGfxRendererD3D11:
			ID3D11DeviceContext* context;
			((IUnityGraphicsD3D11*)s_GraphicsBackend)->GetDevice()->GetImmediateContext(&context);
			IM_ASSERT(ImGui_ImplDX11_Init(((IUnityGraphicsD3D11*)s_GraphicsBackend)->GetDevice(), context) && "Could not init renderer!");
			break;
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what graphics Unity on Linux uses... 
#elif __OBJC__
		case kUnityGfxRendererMetal:
			IM_ASSERT(ImGui_ImplMetal_Init(((IUnityGraphicsMetalV1*)s_GraphicsBackend)->MetalDevice()) && "Could not init renderer!");
			break;
#endif
		default:
			IM_ASSERT(false && "No Graphics Available!");
			break;
	}


	ImGui::SetCurrentContext(current);
}

CREATE_UNITY_CALLBACK_FUNCTION(void, ImGui_ImplUnity_DestroyWindow, intptr_t)
void ImGui_ImplUnity_CloseWindow(ImGuiContext* Context)
{
	if(!s_ImGui_ImplUnity_DestroyWindowCallbackExists) return;
	ImGuiContext* current = ImGui::GetCurrentContext();
	ImGui::SetCurrentContext(Context);

	s_ImGui_ImplUnity_DestroyWindowCallback((intptr_t)ImGui::GetMainViewport()->PlatformHandleRaw);
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
	ImGui_ImplWin32_Shutdown();
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what backend Unity on Linux uses... 
#elif __OBJC__
	ImGui_ImplOSX_Shutdown()
#endif

	switch(s_RendererType)
	{
#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
		case kUnityGfxRendererD3D11:
			ImGui_ImplDX11_Shutdown();
			break;
#elif defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#error I don't know what backend Unity on Linux uses... 
#elif __OBJC__
		case kUnityGfxRendererMetal:
			ImGui_ImplMetal_Shutdown();
			break;
#endif
		default:
			IM_ASSERT(false && "No Graphics Available!");
			break;
	}

	ImGui::SetCurrentContext(current);
}

void ImGui_StyleColorsUnityShadow(ImGuiStyle* dst)
{
	ImGuiStyle* style = dst ? dst : &ImGui::GetStyle();
	ImVec4* colors = style->Colors;

	//style->WindowPadding = ImVec2(0.00f, 0.00f);
	//style->FramePadding = ImVec2(5.00f, 4.00f);
	//style->CellPadding = ImVec2(6.00f, 6.00f);
	style->FrameRounding = 2;
	style->PopupRounding = 1;
	style->ScrollbarRounding = 2;

	ImVec4 text = ImVec4(1.f, 1.f, 1.f, 1.00f);
	ImVec4 bg = ImVec4(0.2f, 0.2f, 0.22f, 1.f);
	ImVec4 darkbg = ImVec4(0.09f, 0.08f, 0.12f, 1.f);
	ImVec4 lightItembg = ImVec4(0.3f, 0.3f, 0.3f, 1.f);
	ImVec4 itembg = ImVec4(0.1f, 0.1f, 0.1f, 1.f);
	ImVec4 lightItemHover = ImVec4(0.37f, 0.35f, 0.4f, 1.f);
	ImVec4 itemHover = ImVec4(0.15f, 0.13f, 0.2f, 1.f);
	ImVec4 lightItemActive = ImVec4(0.37f, 0.35f, 0.4f, 1.f);
	ImVec4 itemActive = ImVec4(0.15f, 0.15f, 0.2f, 1.f);

	//ImVec4 text = ImVec4(1.f, 1.f, 1.f, 1.00f);
	//ImVec4 bg = ImVec4(0.05f, 0.05f, 0.06f, 1.f);
	//ImVec4 darkbg = ImVec4(0.04f, 0.035f, 0.05f, 1.f);
	//ImVec4 lightItembg = ImVec4(0.2f, 0.2f, 0.2f, 1.f);
	//ImVec4 itembg = ImVec4(0.02f, 0.02f, 0.03f, 1.f);
	//ImVec4 lightItemHover = ImVec4(0.25f, 0.23f, 0.3f, 1.f);
	//ImVec4 itemHover = ImVec4(0.1f, 0.08f, 0.11f, 1.f);
	//ImVec4 lightItemActive = ImVec4(0.27f, 0.25f, 0.3f, 1.f);
	//ImVec4 itemActive = ImVec4(0.1f, 0.1f, 0.15f, 1.f);

	colors[ImGuiCol_Text] = text;
	colors[ImGuiCol_TextDisabled] = ImVec4(0.60f, 0.60f, 0.60f, 1.00f);
	colors[ImGuiCol_WindowBg] = bg;
	colors[ImGuiCol_ChildBg] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_PopupBg] = darkbg;
	colors[ImGuiCol_Border] = darkbg;
	colors[ImGuiCol_BorderShadow] = ImVec4(0.00f, 0.00f, 0.00f, 0.30f);
	colors[ImGuiCol_FrameBg] = itembg;
	colors[ImGuiCol_FrameBgHovered] = itemHover;
	colors[ImGuiCol_FrameBgActive] = itemActive;
	colors[ImGuiCol_TitleBg] = darkbg;
	colors[ImGuiCol_TitleBgActive] = colors[ImGuiCol_TitleBg];
	colors[ImGuiCol_TitleBgCollapsed] = colors[ImGuiCol_TitleBg];
	colors[ImGuiCol_MenuBarBg] = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
	colors[ImGuiCol_ScrollbarBg] = darkbg;
	colors[ImGuiCol_ScrollbarGrab] = lightItembg;
	colors[ImGuiCol_ScrollbarGrabHovered] = lightItemHover;
	colors[ImGuiCol_ScrollbarGrabActive] = lightItemActive;
	colors[ImGuiCol_CheckMark] = text;
	colors[ImGuiCol_SliderGrab] = lightItembg;
	colors[ImGuiCol_SliderGrabActive] = lightItemActive;
	colors[ImGuiCol_Button] = lightItembg;
	colors[ImGuiCol_ButtonHovered] = lightItemHover;
	colors[ImGuiCol_ButtonActive] = lightItemActive;
	colors[ImGuiCol_Header] = itembg;
	colors[ImGuiCol_HeaderHovered] = itemHover;
	colors[ImGuiCol_HeaderActive] = itemActive;
	colors[ImGuiCol_Separator] = colors[ImGuiCol_Border];
	colors[ImGuiCol_SeparatorHovered] = ImVec4(0.10f, 0.40f, 0.75f, 0.78f);
	colors[ImGuiCol_SeparatorActive] = ImVec4(0.10f, 0.40f, 0.75f, 1.00f);
	colors[ImGuiCol_ResizeGrip] = ImVec4(0.26f, 0.59f, 0.98f, 0.20f);
	colors[ImGuiCol_ResizeGripHovered] = ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
	colors[ImGuiCol_ResizeGripActive] = ImVec4(0.26f, 0.59f, 0.98f, 0.95f);
	colors[ImGuiCol_Tab] = ImLerp(colors[ImGuiCol_Header], colors[ImGuiCol_TitleBgActive], 0.80f);
	colors[ImGuiCol_TabHovered] = colors[ImGuiCol_HeaderHovered];
	colors[ImGuiCol_TabActive] = ImLerp(colors[ImGuiCol_HeaderActive], colors[ImGuiCol_TitleBgActive], 0.60f);
	colors[ImGuiCol_TabUnfocused] = ImLerp(colors[ImGuiCol_Tab], colors[ImGuiCol_TitleBg], 0.80f);
	colors[ImGuiCol_TabUnfocusedActive] = ImLerp(colors[ImGuiCol_TabActive], colors[ImGuiCol_TitleBg], 0.40f);
	//colors[ImGuiCol_DockingPreview] = colors[ImGuiCol_HeaderActive] * ImVec4(1.0f, 1.0f, 1.0f, 0.7f);
	//colors[ImGuiCol_DockingEmptyBg] = ImVec4(0.20f, 0.20f, 0.20f, 1.00f);
	colors[ImGuiCol_PlotLines] = ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
	colors[ImGuiCol_PlotLinesHovered] = ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
	colors[ImGuiCol_PlotHistogram] = ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
	colors[ImGuiCol_PlotHistogramHovered] = ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
	colors[ImGuiCol_TableHeaderBg] = ImVec4(0.19f, 0.19f, 0.20f, 1.00f);
	colors[ImGuiCol_TableBorderStrong] = ImVec4(0.31f, 0.31f, 0.35f, 1.00f);   // Prefer using Alpha=1.0 here
	colors[ImGuiCol_TableBorderLight] = ImVec4(0.23f, 0.23f, 0.25f, 1.00f);   // Prefer using Alpha=1.0 here
	colors[ImGuiCol_TableRowBg] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_TableRowBgAlt] = ImVec4(1.00f, 1.00f, 1.00f, 0.06f);
	colors[ImGuiCol_TextSelectedBg] = ImVec4(0.26f, 0.59f, 0.98f, 0.35f);
	colors[ImGuiCol_DragDropTarget] = lightItemActive;
	colors[ImGuiCol_NavHighlight] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
	colors[ImGuiCol_NavWindowingHighlight] = ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
	colors[ImGuiCol_NavWindowingDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
	colors[ImGuiCol_ModalWindowDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.35f);
}
