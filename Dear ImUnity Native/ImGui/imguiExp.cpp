#include "imgui.h"
#include "imgui_internal.h"
#define LabelLeft(label) {float indent = ImGui::GetCursorPosX(); std::string shownLabel(label, ImGui::FindRenderedTextEnd(label));if (!shownLabel.empty()) { ImGui::Text(shownLabel.c_str()); ImGui::SameLine(); ImGui::SetCursorPosX(indent + 150); }}//(ImGui::GetWindowWidth() < 384 ? 96 : ImGui::GetWindowWidth() * .25f)); }}
#define HideLabel(label) (std::string("##") + label).c_str()
#define FillWidth ImGui::PushItemWidth((ImGui::GetWindowContentRegionMax().x - ImGui::GetCursorPosX()) - 10)

extern "C"
{
__declspec(dllexport) ImGuiIO& __stdcall ImGui_GetIO300()
{
	return ImGui::GetIO();
}
__declspec(dllexport) ImGuiStyle& __stdcall ImGui_GetStyle301()
{
	return ImGui::GetStyle();
}
__declspec(dllexport) void __stdcall ImGui_NewFrame302()
{
	ImGui::NewFrame();
}
__declspec(dllexport) void __stdcall ImGui_EndFrame303()
{
	ImGui::EndFrame();
}
__declspec(dllexport) void __stdcall ImGui_Render304()
{
	ImGui::Render();
}
__declspec(dllexport) ImDrawData* __stdcall ImGui_GetDrawData305()
{
	return ImGui::GetDrawData();
}
__declspec(dllexport) void __stdcall ImGui_ShowDemoWindow308(bool* p_open)
{
	ImGui::ShowDemoWindow(p_open);
}
__declspec(dllexport) void __stdcall ImGui_ShowMetricsWindow309(bool* p_open)
{
	ImGui::ShowMetricsWindow(p_open);
}
__declspec(dllexport) void __stdcall ImGui_ShowDebugLogWindow310(bool* p_open)
{
	ImGui::ShowDebugLogWindow(p_open);
}
__declspec(dllexport) void __stdcall ImGui_ShowStackToolWindow311(bool* p_open)
{
	ImGui::ShowStackToolWindow(p_open);
}
__declspec(dllexport) void __stdcall ImGui_ShowAboutWindow312(bool* p_open)
{
	ImGui::ShowAboutWindow(p_open);
}
__declspec(dllexport) void __stdcall ImGui_ShowStyleEditor313(ImGuiStyle* ref)
{
	ImGui::ShowStyleEditor(ref);
}
__declspec(dllexport) bool __stdcall ImGui_ShowStyleSelector314(const char* label)
{
	return ImGui::ShowStyleSelector(label);
}
__declspec(dllexport) void __stdcall ImGui_ShowUserGuide316()
{
	ImGui::ShowUserGuide();
}
__declspec(dllexport) const char* __stdcall ImGui_GetVersion317()
{
	return ImGui::GetVersion();
}
__declspec(dllexport) void __stdcall ImGui_StyleColorsDark320(ImGuiStyle* dst)
{
	ImGui::StyleColorsDark(dst);
}
__declspec(dllexport) void __stdcall ImGui_StyleColorsLight321(ImGuiStyle* dst)
{
	ImGui::StyleColorsLight(dst);
}
__declspec(dllexport) void __stdcall ImGui_StyleColorsClassic322(ImGuiStyle* dst)
{
	ImGui::StyleColorsClassic(dst);
}
__declspec(dllexport) bool __stdcall ImGui_Begin336(const char* name, bool* p_open, ImGuiWindowFlags flags)
{
	return ImGui::Begin(name, p_open, flags);
}
__declspec(dllexport) void __stdcall ImGui_End337()
{
	ImGui::End();
}
__declspec(dllexport) bool __stdcall ImGui_BeginChild347(const char* str_id, const ImVec2& size, bool border, ImGuiWindowFlags flags)
{
	return ImGui::BeginChild(str_id, size, border, flags);
}
__declspec(dllexport) bool __stdcall ImGui_BeginChild348(ImGuiID id, const ImVec2& size, bool border, ImGuiWindowFlags flags)
{
	return ImGui::BeginChild(id, size, border, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndChild349()
{
	ImGui::EndChild();
}
__declspec(dllexport) bool __stdcall ImGui_IsWindowAppearing353()
{
	return ImGui::IsWindowAppearing();
}
__declspec(dllexport) bool __stdcall ImGui_IsWindowCollapsed354()
{
	return ImGui::IsWindowCollapsed();
}
__declspec(dllexport) bool __stdcall ImGui_IsWindowFocused355(ImGuiFocusedFlags flags)
{
	return ImGui::IsWindowFocused(flags);
}
__declspec(dllexport) bool __stdcall ImGui_IsWindowHovered356(ImGuiHoveredFlags flags)
{
	return ImGui::IsWindowHovered(flags);
}
__declspec(dllexport) ImDrawList* __stdcall ImGui_GetWindowDrawList357()
{
	return ImGui::GetWindowDrawList();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetWindowPos358()
{
	return ImGui::GetWindowPos();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetWindowSize359()
{
	return ImGui::GetWindowSize();
}
__declspec(dllexport) float __stdcall ImGui_GetWindowWidth360()
{
	return ImGui::GetWindowWidth();
}
__declspec(dllexport) float __stdcall ImGui_GetWindowHeight361()
{
	return ImGui::GetWindowHeight();
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowPos365(const ImVec2& pos, ImGuiCond cond, const ImVec2& pivot)
{
	ImGui::SetNextWindowPos(pos, cond, pivot);
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowSize366(const ImVec2& size, ImGuiCond cond)
{
	ImGui::SetNextWindowSize(size, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowSizeConstraints367(const ImVec2& size_min, const ImVec2& size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data)
{
	ImGui::SetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowContentSize368(const ImVec2& size)
{
	ImGui::SetNextWindowContentSize(size);
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowCollapsed369(bool collapsed, ImGuiCond cond)
{
	ImGui::SetNextWindowCollapsed(collapsed, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowFocus370()
{
	ImGui::SetNextWindowFocus();
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowScroll371(const ImVec2& scroll)
{
	ImGui::SetNextWindowScroll(scroll);
}
__declspec(dllexport) void __stdcall ImGui_SetNextWindowBgAlpha372(float alpha)
{
	ImGui::SetNextWindowBgAlpha(alpha);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowPos373(const ImVec2& pos, ImGuiCond cond)
{
	ImGui::SetWindowPos(pos, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowSize374(const ImVec2& size, ImGuiCond cond)
{
	ImGui::SetWindowSize(size, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowCollapsed375(bool collapsed, ImGuiCond cond)
{
	ImGui::SetWindowCollapsed(collapsed, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowFocus376()
{
	ImGui::SetWindowFocus();
}
__declspec(dllexport) void __stdcall ImGui_SetWindowPos378(const char* name, const ImVec2& pos, ImGuiCond cond)
{
	ImGui::SetWindowPos(name, pos, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowSize379(const char* name, const ImVec2& size, ImGuiCond cond)
{
	ImGui::SetWindowSize(name, size, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowCollapsed380(const char* name, bool collapsed, ImGuiCond cond)
{
	ImGui::SetWindowCollapsed(name, collapsed, cond);
}
__declspec(dllexport) void __stdcall ImGui_SetWindowFocus381(const char* name)
{
	ImGui::SetWindowFocus(name);
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetContentRegionAvail386()
{
	return ImGui::GetContentRegionAvail();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetContentRegionMax387()
{
	return ImGui::GetContentRegionMax();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetWindowContentRegionMin388()
{
	return ImGui::GetWindowContentRegionMin();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetWindowContentRegionMax389()
{
	return ImGui::GetWindowContentRegionMax();
}
__declspec(dllexport) float __stdcall ImGui_GetScrollX394()
{
	return ImGui::GetScrollX();
}
__declspec(dllexport) float __stdcall ImGui_GetScrollY395()
{
	return ImGui::GetScrollY();
}
__declspec(dllexport) void __stdcall ImGui_SetScrollX396(float scroll_x)
{
	ImGui::SetScrollX(scroll_x);
}
__declspec(dllexport) void __stdcall ImGui_SetScrollY397(float scroll_y)
{
	ImGui::SetScrollY(scroll_y);
}
__declspec(dllexport) float __stdcall ImGui_GetScrollMaxX398()
{
	return ImGui::GetScrollMaxX();
}
__declspec(dllexport) float __stdcall ImGui_GetScrollMaxY399()
{
	return ImGui::GetScrollMaxY();
}
__declspec(dllexport) void __stdcall ImGui_SetScrollHereX400(float center_x_ratio)
{
	ImGui::SetScrollHereX(center_x_ratio);
}
__declspec(dllexport) void __stdcall ImGui_SetScrollHereY401(float center_y_ratio)
{
	ImGui::SetScrollHereY(center_y_ratio);
}
__declspec(dllexport) void __stdcall ImGui_SetScrollFromPosX402(float local_x, float center_x_ratio)
{
	ImGui::SetScrollFromPosX(local_x, center_x_ratio);
}
__declspec(dllexport) void __stdcall ImGui_SetScrollFromPosY403(float local_y, float center_y_ratio)
{
	ImGui::SetScrollFromPosY(local_y, center_y_ratio);
}
__declspec(dllexport) void __stdcall ImGui_PushStyleColor408(ImGuiCol idx, ImU32 col)
{
	ImGui::PushStyleColor(idx, col);
}
__declspec(dllexport) void __stdcall ImGui_PushStyleColor409(ImGuiCol idx, const ImVec4& col)
{
	ImGui::PushStyleColor(idx, col);
}
__declspec(dllexport) void __stdcall ImGui_PopStyleColor410(int count)
{
	ImGui::PopStyleColor(count);
}
__declspec(dllexport) void __stdcall ImGui_PushStyleVar411(ImGuiStyleVar idx, float val)
{
	ImGui::PushStyleVar(idx, val);
}
__declspec(dllexport) void __stdcall ImGui_PushStyleVar412(ImGuiStyleVar idx, const ImVec2& val)
{
	ImGui::PushStyleVar(idx, val);
}
__declspec(dllexport) void __stdcall ImGui_PopStyleVar413(int count)
{
	ImGui::PopStyleVar(count);
}
__declspec(dllexport) void __stdcall ImGui_PushTabStop414(bool tab_stop)
{
	ImGui::PushTabStop(tab_stop);
}
__declspec(dllexport) void __stdcall ImGui_PopTabStop415()
{
	ImGui::PopTabStop();
}
__declspec(dllexport) void __stdcall ImGui_PushButtonRepeat416(bool repeat)
{
	ImGui::PushButtonRepeat(repeat);
}
__declspec(dllexport) void __stdcall ImGui_PopButtonRepeat417()
{
	ImGui::PopButtonRepeat();
}
__declspec(dllexport) void __stdcall ImGui_PushItemWidth420(float item_width)
{
	ImGui::PushItemWidth(item_width);
}
__declspec(dllexport) void __stdcall ImGui_PopItemWidth421()
{
	ImGui::PopItemWidth();
}
__declspec(dllexport) void __stdcall ImGui_SetNextItemWidth422(float item_width)
{
	ImGui::SetNextItemWidth(item_width);
}
__declspec(dllexport) float __stdcall ImGui_CalcItemWidth423()
{
	return ImGui::CalcItemWidth();
}
__declspec(dllexport) void __stdcall ImGui_PushTextWrapPos424(float wrap_local_pos_x)
{
	ImGui::PushTextWrapPos(wrap_local_pos_x);
}
__declspec(dllexport) void __stdcall ImGui_PopTextWrapPos425()
{
	ImGui::PopTextWrapPos();
}
__declspec(dllexport) ImU32 __stdcall ImGui_GetColorU32432(ImGuiCol idx, float alpha_mul)
{
	return ImGui::GetColorU32(idx, alpha_mul);
}
__declspec(dllexport) ImU32 __stdcall ImGui_GetColorU32433(const ImVec4& col)
{
	return ImGui::GetColorU32(col);
}
__declspec(dllexport) ImU32 __stdcall ImGui_GetColorU32434(ImU32 col)
{
	return ImGui::GetColorU32(col);
}
__declspec(dllexport) const ImVec4& __stdcall ImGui_GetStyleColorVec4435(ImGuiCol idx)
{
	return ImGui::GetStyleColorVec4(idx);
}
__declspec(dllexport) void __stdcall ImGui_Separator444()
{
	ImGui::Separator();
}
__declspec(dllexport) void __stdcall ImGui_SameLine445(float offset_from_start_x, float spacing)
{
	ImGui::SameLine(offset_from_start_x, spacing);
}
__declspec(dllexport) void __stdcall ImGui_NewLine446()
{
	ImGui::NewLine();
}
__declspec(dllexport) void __stdcall ImGui_Spacing447()
{
	ImGui::Spacing();
}
__declspec(dllexport) void __stdcall ImGui_Dummy448(const ImVec2& size)
{
	ImGui::Dummy(size);
}
__declspec(dllexport) void __stdcall ImGui_Indent449(float indent_w)
{
	ImGui::Indent(indent_w);
}
__declspec(dllexport) void __stdcall ImGui_Unindent450(float indent_w)
{
	ImGui::Unindent(indent_w);
}
__declspec(dllexport) void __stdcall ImGui_BeginGroup451()
{
	ImGui::BeginGroup();
}
__declspec(dllexport) void __stdcall ImGui_EndGroup452()
{
	ImGui::EndGroup();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetCursorPos453()
{
	return ImGui::GetCursorPos();
}
__declspec(dllexport) float __stdcall ImGui_GetCursorPosX454()
{
	return ImGui::GetCursorPosX();
}
__declspec(dllexport) float __stdcall ImGui_GetCursorPosY455()
{
	return ImGui::GetCursorPosY();
}
__declspec(dllexport) void __stdcall ImGui_SetCursorPos456(const ImVec2& local_pos)
{
	ImGui::SetCursorPos(local_pos);
}
__declspec(dllexport) void __stdcall ImGui_SetCursorPosX457(float local_x)
{
	ImGui::SetCursorPosX(local_x);
}
__declspec(dllexport) void __stdcall ImGui_SetCursorPosY458(float local_y)
{
	ImGui::SetCursorPosY(local_y);
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetCursorStartPos459()
{
	return ImGui::GetCursorStartPos();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetCursorScreenPos460()
{
	return ImGui::GetCursorScreenPos();
}
__declspec(dllexport) void __stdcall ImGui_SetCursorScreenPos461(const ImVec2& pos)
{
	ImGui::SetCursorScreenPos(pos);
}
__declspec(dllexport) void __stdcall ImGui_AlignTextToFramePadding462()
{
	ImGui::AlignTextToFramePadding();
}
__declspec(dllexport) float __stdcall ImGui_GetTextLineHeight463()
{
	return ImGui::GetTextLineHeight();
}
__declspec(dllexport) float __stdcall ImGui_GetTextLineHeightWithSpacing464()
{
	return ImGui::GetTextLineHeightWithSpacing();
}
__declspec(dllexport) float __stdcall ImGui_GetFrameHeight465()
{
	return ImGui::GetFrameHeight();
}
__declspec(dllexport) float __stdcall ImGui_GetFrameHeightWithSpacing466()
{
	return ImGui::GetFrameHeightWithSpacing();
}
__declspec(dllexport) void __stdcall ImGui_PushID479(const char* str_id)
{
	ImGui::PushID(str_id);
}
__declspec(dllexport) void __stdcall ImGui_PushID480(const char* str_id_begin, const char* str_id_end)
{
	ImGui::PushID(str_id_begin, str_id_end);
}
__declspec(dllexport) void __stdcall ImGui_PushID481(const void* ptr_id)
{
	ImGui::PushID(ptr_id);
}
__declspec(dllexport) void __stdcall ImGui_PushID482(int int_id)
{
	ImGui::PushID(int_id);
}
__declspec(dllexport) void __stdcall ImGui_PopID483()
{
	ImGui::PopID();
}
__declspec(dllexport) ImGuiID __stdcall ImGui_GetID484(const char* str_id)
{
	return ImGui::GetID(str_id);
}
__declspec(dllexport) ImGuiID __stdcall ImGui_GetID485(const char* str_id_begin, const char* str_id_end)
{
	return ImGui::GetID(str_id_begin, str_id_end);
}
__declspec(dllexport) ImGuiID __stdcall ImGui_GetID486(const void* ptr_id)
{
	return ImGui::GetID(ptr_id);
}
__declspec(dllexport) void __stdcall ImGui_TextUnformatted489(const char* text, const char* text_end)
{
	ImGui::TextUnformatted(text, text_end);
}
__declspec(dllexport) void __stdcall ImGui_SeparatorText502(const char* label)
{
	ImGui::SeparatorText(label);
}
__declspec(dllexport) bool __stdcall ImGui_Button507(const char* label, const ImVec2& size)
{
	return ImGui::Button(label, size);
}
__declspec(dllexport) bool __stdcall ImGui_SmallButton508(const char* label)
{
	return ImGui::SmallButton(label);
}
__declspec(dllexport) bool __stdcall ImGui_InvisibleButton509(const char* str_id, const ImVec2& size, ImGuiButtonFlags flags)
{
	return ImGui::InvisibleButton(str_id, size, flags);
}
__declspec(dllexport) bool __stdcall ImGui_ArrowButton510(const char* str_id, ImGuiDir dir)
{
	return ImGui::ArrowButton(str_id, dir);
}
__declspec(dllexport) bool __stdcall ImGui_Checkbox511(const char* label, bool* v)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::Checkbox(HideLabel(label), v);
}
__declspec(dllexport) bool __stdcall ImGui_CheckboxFlags512(const char* label, int* flags, int flags_value)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::CheckboxFlags(HideLabel(label), flags, flags_value);
}
__declspec(dllexport) bool __stdcall ImGui_CheckboxFlags513(const char* label, unsigned int* flags, unsigned int flags_value)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::CheckboxFlags(HideLabel(label), flags, flags_value);
}
__declspec(dllexport) bool __stdcall ImGui_RadioButton514(const char* label, bool active)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::RadioButton(HideLabel(label), active);
}
__declspec(dllexport) bool __stdcall ImGui_RadioButton515(const char* label, int* v, int v_button)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::RadioButton(HideLabel(label), v, v_button);
}
__declspec(dllexport) void __stdcall ImGui_ProgressBar516(float fraction, const ImVec2& size_arg, const char* overlay)
{
	ImGui::ProgressBar(fraction, size_arg, overlay);
}
__declspec(dllexport) void __stdcall ImGui_Bullet517()
{
	ImGui::Bullet();
}
__declspec(dllexport) void __stdcall ImGui_Image521(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0, const ImVec2& uv1, const ImVec4& tint_col, const ImVec4& border_col)
{
	ImGui::Image(user_texture_id, size, uv0, uv1, tint_col, border_col);
}
__declspec(dllexport) bool __stdcall ImGui_ImageButton522(const char* str_id, ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0, const ImVec2& uv1, const ImVec4& bg_col, const ImVec4& tint_col)
{
	return ImGui::ImageButton(str_id, user_texture_id, size, uv0, uv1, bg_col, tint_col);
}
__declspec(dllexport) bool __stdcall ImGui_BeginCombo527(const char* label, const char* preview_value, ImGuiComboFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::BeginCombo(HideLabel(label), preview_value, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndCombo528()
{
	ImGui::EndCombo();
}
__declspec(dllexport) bool __stdcall ImGui_Combo529(const char* label, int* current_item, const char* const items[], int items_count, int popup_max_height_in_items)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::Combo(HideLabel(label), current_item, items, items_count, popup_max_height_in_items);
}
__declspec(dllexport) bool __stdcall ImGui_Combo530(const char* label, int* current_item, const char* items_separated_by_zeros, int popup_max_height_in_items)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::Combo(HideLabel(label), current_item, items_separated_by_zeros, popup_max_height_in_items);
}
__declspec(dllexport) bool __stdcall ImGui_DragFloat545(const char* label, float* v, float v_speed, float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragFloat(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragFloat2546(const char* label, float v[2], float v_speed, float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragFloat2(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragFloat3547(const char* label, float v[3], float v_speed, float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragFloat3(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragFloat4548(const char* label, float v[4], float v_speed, float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragFloat4(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragFloatRange2549(const char* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, const char* format, const char* format_max, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragFloatRange2(HideLabel(label), v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragInt550(const char* label, int* v, float v_speed, int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragInt(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragInt2551(const char* label, int v[2], float v_speed, int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragInt2(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragInt3552(const char* label, int v[3], float v_speed, int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragInt3(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragInt4553(const char* label, int v[4], float v_speed, int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragInt4(HideLabel(label), v, v_speed, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragIntRange2554(const char* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, const char* format, const char* format_max, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragIntRange2(HideLabel(label), v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragScalar555(const char* label, ImGuiDataType data_type, void* p_data, float v_speed, const void* p_min, const void* p_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragScalar(HideLabel(label), data_type, p_data, v_speed, p_min, p_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_DragScalarN556(const char* label, ImGuiDataType data_type, void* p_data, int components, float v_speed, const void* p_min, const void* p_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::DragScalarN(HideLabel(label), data_type, p_data, components, v_speed, p_min, p_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderFloat564(const char* label, float* v, float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderFloat(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderFloat2565(const char* label, float v[2], float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderFloat2(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderFloat3566(const char* label, float v[3], float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderFloat3(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderFloat4567(const char* label, float v[4], float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderFloat4(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderAngle568(const char* label, float* v_rad, float v_degrees_min, float v_degrees_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderAngle(HideLabel(label), v_rad, v_degrees_min, v_degrees_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderInt569(const char* label, int* v, int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderInt(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderInt2570(const char* label, int v[2], int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderInt2(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderInt3571(const char* label, int v[3], int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderInt3(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderInt4572(const char* label, int v[4], int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderInt4(HideLabel(label), v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderScalar573(const char* label, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderScalar(HideLabel(label), data_type, p_data, p_min, p_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_SliderScalarN574(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::SliderScalarN(HideLabel(label), data_type, p_data, components, p_min, p_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_VSliderFloat575(const char* label, const ImVec2& size, float* v, float v_min, float v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::VSliderFloat(HideLabel(label), size, v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_VSliderInt576(const char* label, const ImVec2& size, int* v, int v_min, int v_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::VSliderInt(HideLabel(label), size, v, v_min, v_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_VSliderScalar577(const char* label, const ImVec2& size, ImGuiDataType data_type, void* p_data, const void* p_min, const void* p_max, const char* format, ImGuiSliderFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::VSliderScalar(HideLabel(label), size, data_type, p_data, p_min, p_max, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputText582(const char* label, char* buf, size_t buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputText(HideLabel(label), buf, buf_size, flags, callback, user_data);
}
__declspec(dllexport) bool __stdcall ImGui_InputTextMultiline583(const char* label, char* buf, size_t buf_size, const ImVec2& size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputTextMultiline(HideLabel(label), buf, buf_size, size, flags, callback, user_data);
}
__declspec(dllexport) bool __stdcall ImGui_InputTextWithHint584(const char* label, const char* hint, char* buf, size_t buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputTextWithHint(HideLabel(label), hint, buf, buf_size, flags, callback, user_data);
}
__declspec(dllexport) bool __stdcall ImGui_InputFloat585(const char* label, float* v, float step, float step_fast, const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputFloat(HideLabel(label), v, step, step_fast, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputFloat2586(const char* label, float v[2], const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputFloat2(HideLabel(label), v, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputFloat3587(const char* label, float v[3], const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputFloat3(HideLabel(label), v, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputFloat4588(const char* label, float v[4], const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputFloat4(HideLabel(label), v, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputInt589(const char* label, int* v, int step, int step_fast, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputInt(HideLabel(label), v, step, step_fast, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputInt2590(const char* label, int v[2], ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputInt2(HideLabel(label), v, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputInt3591(const char* label, int v[3], ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputInt3(HideLabel(label), v, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputInt4592(const char* label, int v[4], ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputInt4(HideLabel(label), v, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputDouble593(const char* label, double* v, double step, double step_fast, const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputDouble(HideLabel(label), v, step, step_fast, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputScalar594(const char* label, ImGuiDataType data_type, void* p_data, const void* p_step, const void* p_step_fast, const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputScalar(HideLabel(label), data_type, p_data, p_step, p_step_fast, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_InputScalarN595(const char* label, ImGuiDataType data_type, void* p_data, int components, const void* p_step, const void* p_step_fast, const char* format, ImGuiInputTextFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::InputScalarN(HideLabel(label), data_type, p_data, components, p_step, p_step_fast, format, flags);
}
__declspec(dllexport) bool __stdcall ImGui_ColorEdit3600(const char* label, float col[3], ImGuiColorEditFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::ColorEdit3(HideLabel(label), col, flags);
}
__declspec(dllexport) bool __stdcall ImGui_ColorEdit4601(const char* label, float col[4], ImGuiColorEditFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::ColorEdit4(HideLabel(label), col, flags);
}
__declspec(dllexport) bool __stdcall ImGui_ColorPicker3602(const char* label, float col[3], ImGuiColorEditFlags flags)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::ColorPicker3(HideLabel(label), col, flags);
}
__declspec(dllexport) bool __stdcall ImGui_ColorPicker4603(const char* label, float col[4], ImGuiColorEditFlags flags, const float* ref_col)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::ColorPicker4(HideLabel(label), col, flags, ref_col);
}
__declspec(dllexport) bool __stdcall ImGui_ColorButton604(const char* desc_id, const ImVec4& col, ImGuiColorEditFlags flags, const ImVec2& size)
{
	return ImGui::ColorButton(desc_id, col, flags, size);
}
__declspec(dllexport) void __stdcall ImGui_SetColorEditOptions605(ImGuiColorEditFlags flags)
{
	ImGui::SetColorEditOptions(flags);
}
__declspec(dllexport) bool __stdcall ImGui_TreeNode609(const char* label)
{
	return ImGui::TreeNode(label);
}
__declspec(dllexport) bool __stdcall ImGui_TreeNodeEx614(const char* label, ImGuiTreeNodeFlags flags)
{
	return ImGui::TreeNodeEx(label, flags);
}
__declspec(dllexport) void __stdcall ImGui_TreePush619(const char* str_id)
{
	ImGui::TreePush(str_id);
}
__declspec(dllexport) void __stdcall ImGui_TreePush620(const void* ptr_id)
{
	ImGui::TreePush(ptr_id);
}
__declspec(dllexport) void __stdcall ImGui_TreePop621()
{
	ImGui::TreePop();
}
__declspec(dllexport) float __stdcall ImGui_GetTreeNodeToLabelSpacing622()
{
	return ImGui::GetTreeNodeToLabelSpacing();
}
__declspec(dllexport) bool __stdcall ImGui_CollapsingHeader623(const char* label, ImGuiTreeNodeFlags flags)
{
	return ImGui::CollapsingHeader(label, flags);
}
__declspec(dllexport) bool __stdcall ImGui_CollapsingHeader624(const char* label, bool* p_visible, ImGuiTreeNodeFlags flags)
{
	return ImGui::CollapsingHeader(label, p_visible, flags);
}
__declspec(dllexport) void __stdcall ImGui_SetNextItemOpen625(bool is_open, ImGuiCond cond)
{
	ImGui::SetNextItemOpen(is_open, cond);
}
__declspec(dllexport) bool __stdcall ImGui_Selectable630(const char* label, bool selected, ImGuiSelectableFlags flags, const ImVec2& size)
{
	return ImGui::Selectable(label, selected, flags, size);
}
__declspec(dllexport) bool __stdcall ImGui_Selectable631(const char* label, bool* p_selected, ImGuiSelectableFlags flags, const ImVec2& size)
{
	return ImGui::Selectable(label, p_selected, flags, size);
}
__declspec(dllexport) bool __stdcall ImGui_BeginListBox639(const char* label, const ImVec2& size)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::BeginListBox(HideLabel(label), size);
}
__declspec(dllexport) void __stdcall ImGui_EndListBox640()
{
	ImGui::EndListBox();
}
__declspec(dllexport) bool __stdcall ImGui_ListBox641(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items)
{
	LabelLeft(label);
	FillWidth;
	return ImGui::ListBox(HideLabel(label), current_item, items, items_count, height_in_items);
}
__declspec(dllexport) void __stdcall ImGui_PlotLines646(const char* label, const float* values, int values_count, int values_offset, const char* overlay_text, float scale_min, float scale_max, ImVec2& graph_size, int stride)
{
	LabelLeft(label);
	FillWidth;
	ImGui::PlotLines(HideLabel(label), values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
}
__declspec(dllexport) void __stdcall ImGui_PlotHistogram648(const char* label, const float* values, int values_count, int values_offset, const char* overlay_text, float scale_min, float scale_max, ImVec2& graph_size, int stride)
{
	LabelLeft(label);
	FillWidth;
	ImGui::PlotHistogram(HideLabel(label), values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
}
__declspec(dllexport) void __stdcall ImGui_Value653(const char* prefix, bool b)
{
	ImGui::Value(prefix, b);
}
__declspec(dllexport) void __stdcall ImGui_Value654(const char* prefix, int v)
{
	ImGui::Value(prefix, v);
}
__declspec(dllexport) void __stdcall ImGui_Value655(const char* prefix, unsigned int v)
{
	ImGui::Value(prefix, v);
}
__declspec(dllexport) void __stdcall ImGui_Value656(const char* prefix, float v, const char* float_format)
{
	ImGui::Value(prefix, v, float_format);
}
__declspec(dllexport) bool __stdcall ImGui_BeginMenuBar663()
{
	return ImGui::BeginMenuBar();
}
__declspec(dllexport) void __stdcall ImGui_EndMenuBar664()
{
	ImGui::EndMenuBar();
}
__declspec(dllexport) bool __stdcall ImGui_BeginMainMenuBar665()
{
	return ImGui::BeginMainMenuBar();
}
__declspec(dllexport) void __stdcall ImGui_EndMainMenuBar666()
{
	ImGui::EndMainMenuBar();
}
__declspec(dllexport) bool __stdcall ImGui_BeginMenu667(const char* label, bool enabled)
{
	return ImGui::BeginMenu(label, enabled);
}
__declspec(dllexport) void __stdcall ImGui_EndMenu668()
{
	ImGui::EndMenu();
}
__declspec(dllexport) bool __stdcall ImGui_MenuItem669(const char* label, const char* shortcut, bool selected, bool enabled)
{
	return ImGui::MenuItem(label, shortcut, selected, enabled);
}
__declspec(dllexport) bool __stdcall ImGui_MenuItem670(const char* label, const char* shortcut, bool* p_selected, bool enabled)
{
	return ImGui::MenuItem(label, shortcut, p_selected, enabled);
}
__declspec(dllexport) bool __stdcall ImGui_BeginTooltip675()
{
	return ImGui::BeginTooltip();
}
__declspec(dllexport) void __stdcall ImGui_EndTooltip676()
{
	ImGui::EndTooltip();
}
__declspec(dllexport) bool __stdcall ImGui_BeginItemTooltip684()
{
	return ImGui::BeginItemTooltip();
}
__declspec(dllexport) bool __stdcall ImGui_BeginPopup700(const char* str_id, ImGuiWindowFlags flags)
{
	return ImGui::BeginPopup(str_id, flags);
}
__declspec(dllexport) bool __stdcall ImGui_BeginPopupModal701(const char* name, bool* p_open, ImGuiWindowFlags flags)
{
	return ImGui::BeginPopupModal(name, p_open, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndPopup702()
{
	ImGui::EndPopup();
}
__declspec(dllexport) void __stdcall ImGui_OpenPopup712(const char* str_id, ImGuiPopupFlags popup_flags)
{
	ImGui::OpenPopup(str_id, popup_flags);
}
__declspec(dllexport) void __stdcall ImGui_OpenPopup713(ImGuiID id, ImGuiPopupFlags popup_flags)
{
	ImGui::OpenPopup(id, popup_flags);
}
__declspec(dllexport) void __stdcall ImGui_OpenPopupOnItemClick714(const char* str_id, ImGuiPopupFlags popup_flags)
{
	ImGui::OpenPopupOnItemClick(str_id, popup_flags);
}
__declspec(dllexport) void __stdcall ImGui_CloseCurrentPopup715()
{
	ImGui::CloseCurrentPopup();
}
__declspec(dllexport) bool __stdcall ImGui_BeginPopupContextItem722(const char* str_id, ImGuiPopupFlags popup_flags)
{
	return ImGui::BeginPopupContextItem(str_id, popup_flags);
}
__declspec(dllexport) bool __stdcall ImGui_BeginPopupContextWindow723(const char* str_id, ImGuiPopupFlags popup_flags)
{
	return ImGui::BeginPopupContextWindow(str_id, popup_flags);
}
__declspec(dllexport) bool __stdcall ImGui_BeginPopupContextVoid724(const char* str_id, ImGuiPopupFlags popup_flags)
{
	return ImGui::BeginPopupContextVoid(str_id, popup_flags);
}
__declspec(dllexport) bool __stdcall ImGui_IsPopupOpen730(const char* str_id, ImGuiPopupFlags flags)
{
	return ImGui::IsPopupOpen(str_id, flags);
}
__declspec(dllexport) bool __stdcall ImGui_BeginTable755(const char* str_id, int column, ImGuiTableFlags flags, const ImVec2& outer_size, float inner_width)
{
	return ImGui::BeginTable(str_id, column, flags, outer_size, inner_width);
}
__declspec(dllexport) void __stdcall ImGui_EndTable756()
{
	ImGui::EndTable();
}
__declspec(dllexport) void __stdcall ImGui_TableNextRow757(ImGuiTableRowFlags row_flags, float min_row_height)
{
	ImGui::TableNextRow(row_flags, min_row_height);
}
__declspec(dllexport) bool __stdcall ImGui_TableNextColumn758()
{
	return ImGui::TableNextColumn();
}
__declspec(dllexport) bool __stdcall ImGui_TableSetColumnIndex759(int column_n)
{
	return ImGui::TableSetColumnIndex(column_n);
}
__declspec(dllexport) void __stdcall ImGui_TableSetupColumn769(const char* label, ImGuiTableColumnFlags flags, float init_width_or_weight, ImGuiID user_id)
{
	ImGui::TableSetupColumn(label, flags, init_width_or_weight, user_id);
}
__declspec(dllexport) void __stdcall ImGui_TableSetupScrollFreeze770(int cols, int rows)
{
	ImGui::TableSetupScrollFreeze(cols, rows);
}
__declspec(dllexport) void __stdcall ImGui_TableHeadersRow771()
{
	ImGui::TableHeadersRow();
}
__declspec(dllexport) void __stdcall ImGui_TableHeader772(const char* label)
{
	ImGui::TableHeader(label);
}
__declspec(dllexport) ImGuiTableSortSpecs* __stdcall ImGui_TableGetSortSpecs780()
{
	return ImGui::TableGetSortSpecs();
}
__declspec(dllexport) int __stdcall ImGui_TableGetColumnCount781()
{
	return ImGui::TableGetColumnCount();
}
__declspec(dllexport) int __stdcall ImGui_TableGetColumnIndex782()
{
	return ImGui::TableGetColumnIndex();
}
__declspec(dllexport) int __stdcall ImGui_TableGetRowIndex783()
{
	return ImGui::TableGetRowIndex();
}
__declspec(dllexport) const char* __stdcall ImGui_TableGetColumnName784(int column_n)
{
	return ImGui::TableGetColumnName(column_n);
}
__declspec(dllexport) ImGuiTableColumnFlags __stdcall ImGui_TableGetColumnFlags785(int column_n)
{
	return ImGui::TableGetColumnFlags(column_n);
}
__declspec(dllexport) void __stdcall ImGui_TableSetColumnEnabled786(int column_n, bool v)
{
	ImGui::TableSetColumnEnabled(column_n, v);
}
__declspec(dllexport) void __stdcall ImGui_TableSetBgColor787(ImGuiTableBgTarget target, ImU32 color, int column_n)
{
	ImGui::TableSetBgColor(target, color, column_n);
}
__declspec(dllexport) void __stdcall ImGui_Columns791(int count, const char* id, bool border)
{
	ImGui::Columns(count, id, border);
}
__declspec(dllexport) void __stdcall ImGui_NextColumn792()
{
	ImGui::NextColumn();
}
__declspec(dllexport) int __stdcall ImGui_GetColumnIndex793()
{
	return ImGui::GetColumnIndex();
}
__declspec(dllexport) float __stdcall ImGui_GetColumnWidth794(int column_index)
{
	return ImGui::GetColumnWidth(column_index);
}
__declspec(dllexport) void __stdcall ImGui_SetColumnWidth795(int column_index, float width)
{
	ImGui::SetColumnWidth(column_index, width);
}
__declspec(dllexport) float __stdcall ImGui_GetColumnOffset796(int column_index)
{
	return ImGui::GetColumnOffset(column_index);
}
__declspec(dllexport) void __stdcall ImGui_SetColumnOffset797(int column_index, float offset_x)
{
	ImGui::SetColumnOffset(column_index, offset_x);
}
__declspec(dllexport) int __stdcall ImGui_GetColumnsCount798()
{
	return ImGui::GetColumnsCount();
}
__declspec(dllexport) bool __stdcall ImGui_BeginTabBar802(const char* str_id, ImGuiTabBarFlags flags)
{
	return ImGui::BeginTabBar(str_id, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndTabBar803()
{
	ImGui::EndTabBar();
}
__declspec(dllexport) bool __stdcall ImGui_BeginTabItem804(const char* label, bool* p_open, ImGuiTabItemFlags flags)
{
	return ImGui::BeginTabItem(label, p_open, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndTabItem805()
{
	ImGui::EndTabItem();
}
__declspec(dllexport) bool __stdcall ImGui_TabItemButton806(const char* label, ImGuiTabItemFlags flags)
{
	return ImGui::TabItemButton(label, flags);
}
__declspec(dllexport) void __stdcall ImGui_SetTabItemClosed807(const char* tab_or_docked_window_label)
{
	ImGui::SetTabItemClosed(tab_or_docked_window_label);
}
__declspec(dllexport) void __stdcall ImGui_LogToTTY811(int auto_open_depth)
{
	ImGui::LogToTTY(auto_open_depth);
}
__declspec(dllexport) void __stdcall ImGui_LogToFile812(int auto_open_depth, const char* filename)
{
	ImGui::LogToFile(auto_open_depth, filename);
}
__declspec(dllexport) void __stdcall ImGui_LogToClipboard813(int auto_open_depth)
{
	ImGui::LogToClipboard(auto_open_depth);
}
__declspec(dllexport) void __stdcall ImGui_LogFinish814()
{
	ImGui::LogFinish();
}
__declspec(dllexport) void __stdcall ImGui_LogButtons815()
{
	ImGui::LogButtons();
}
__declspec(dllexport) bool __stdcall ImGui_BeginDragDropSource824(ImGuiDragDropFlags flags)
{
	return ImGui::BeginDragDropSource(flags);
}
__declspec(dllexport) bool __stdcall ImGui_SetDragDropPayload825(const char* type, const void* data, size_t sz, ImGuiCond cond)
{
	return ImGui::SetDragDropPayload(type, data, sz, cond);
}
__declspec(dllexport) void __stdcall ImGui_EndDragDropSource826()
{
	ImGui::EndDragDropSource();
}
__declspec(dllexport) bool __stdcall ImGui_BeginDragDropTarget827()
{
	return ImGui::BeginDragDropTarget();
}
__declspec(dllexport) const ImGuiPayload* __stdcall ImGui_AcceptDragDropPayload828(const char* type, ImGuiDragDropFlags flags)
{
	return ImGui::AcceptDragDropPayload(type, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndDragDropTarget829()
{
	ImGui::EndDragDropTarget();
}
__declspec(dllexport) const ImGuiPayload* __stdcall ImGui_GetDragDropPayload830()
{
	return ImGui::GetDragDropPayload();
}
__declspec(dllexport) void __stdcall ImGui_BeginDisabled836(bool disabled)
{
	ImGui::BeginDisabled(disabled);
}
__declspec(dllexport) void __stdcall ImGui_EndDisabled837()
{
	ImGui::EndDisabled();
}
__declspec(dllexport) void __stdcall ImGui_PushClipRect841(const ImVec2& clip_rect_min, const ImVec2& clip_rect_max, bool intersect_with_current_clip_rect)
{
	ImGui::PushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
}
__declspec(dllexport) void __stdcall ImGui_PopClipRect842()
{
	ImGui::PopClipRect();
}
__declspec(dllexport) void __stdcall ImGui_SetItemDefaultFocus846()
{
	ImGui::SetItemDefaultFocus();
}
__declspec(dllexport) void __stdcall ImGui_SetKeyboardFocusHere847(int offset)
{
	ImGui::SetKeyboardFocusHere(offset);
}
__declspec(dllexport) void __stdcall ImGui_SetNextItemAllowOverlap850()
{
	ImGui::SetNextItemAllowOverlap();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemHovered855(ImGuiHoveredFlags flags)
{
	return ImGui::IsItemHovered(flags);
}
__declspec(dllexport) bool __stdcall ImGui_IsItemActive856()
{
	return ImGui::IsItemActive();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemFocused857()
{
	return ImGui::IsItemFocused();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemClicked858(ImGuiMouseButton mouse_button)
{
	return ImGui::IsItemClicked(mouse_button);
}
__declspec(dllexport) bool __stdcall ImGui_IsItemVisible859()
{
	return ImGui::IsItemVisible();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemEdited860()
{
	return ImGui::IsItemEdited();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemActivated861()
{
	return ImGui::IsItemActivated();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemDeactivated862()
{
	return ImGui::IsItemDeactivated();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemDeactivatedAfterEdit863()
{
	return ImGui::IsItemDeactivatedAfterEdit();
}
__declspec(dllexport) bool __stdcall ImGui_IsItemToggledOpen864()
{
	return ImGui::IsItemToggledOpen();
}
__declspec(dllexport) bool __stdcall ImGui_IsAnyItemHovered865()
{
	return ImGui::IsAnyItemHovered();
}
__declspec(dllexport) bool __stdcall ImGui_IsAnyItemActive866()
{
	return ImGui::IsAnyItemActive();
}
__declspec(dllexport) bool __stdcall ImGui_IsAnyItemFocused867()
{
	return ImGui::IsAnyItemFocused();
}
__declspec(dllexport) ImGuiID __stdcall ImGui_GetItemID868()
{
	return ImGui::GetItemID();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetItemRectMin869()
{
	return ImGui::GetItemRectMin();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetItemRectMax870()
{
	return ImGui::GetItemRectMax();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetItemRectSize871()
{
	return ImGui::GetItemRectSize();
}
__declspec(dllexport) ImGuiViewport* __stdcall ImGui_GetMainViewport877()
{
	return ImGui::GetMainViewport();
}
__declspec(dllexport) ImDrawList* __stdcall ImGui_GetBackgroundDrawList880()
{
	return ImGui::GetBackgroundDrawList();
}
__declspec(dllexport) ImDrawList* __stdcall ImGui_GetForegroundDrawList881()
{
	return ImGui::GetForegroundDrawList();
}
__declspec(dllexport) bool __stdcall ImGui_IsRectVisible884(const ImVec2& size)
{
	return ImGui::IsRectVisible(size);
}
__declspec(dllexport) bool __stdcall ImGui_IsRectVisible885(const ImVec2& rect_min, const ImVec2& rect_max)
{
	return ImGui::IsRectVisible(rect_min, rect_max);
}
__declspec(dllexport) double __stdcall ImGui_GetTime886()
{
	return ImGui::GetTime();
}
__declspec(dllexport) int __stdcall ImGui_GetFrameCount887()
{
	return ImGui::GetFrameCount();
}
__declspec(dllexport) ImDrawListSharedData* __stdcall ImGui_GetDrawListSharedData888()
{
	return ImGui::GetDrawListSharedData();
}
__declspec(dllexport) const char* __stdcall ImGui_GetStyleColorName889(ImGuiCol idx)
{
	return ImGui::GetStyleColorName(idx);
}
__declspec(dllexport) void __stdcall ImGui_SetStateStorage890(ImGuiStorage* storage)
{
	ImGui::SetStateStorage(storage);
}
__declspec(dllexport) ImGuiStorage* __stdcall ImGui_GetStateStorage891()
{
	return ImGui::GetStateStorage();
}
__declspec(dllexport) bool __stdcall ImGui_BeginChildFrame892(ImGuiID id, const ImVec2& size, ImGuiWindowFlags flags)
{
	return ImGui::BeginChildFrame(id, size, flags);
}
__declspec(dllexport) void __stdcall ImGui_EndChildFrame893()
{
	ImGui::EndChildFrame();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_CalcTextSize896(const char* text, const char* text_end, bool hide_text_after_double_hash, float wrap_width)
{
	return ImGui::CalcTextSize(text, text_end, hide_text_after_double_hash, wrap_width);
}
__declspec(dllexport) ImVec4& __stdcall ImGui_ColorConvertU32ToFloat4899(ImU32 in)
{
	return ImGui::ColorConvertU32ToFloat4(in);
}
__declspec(dllexport) ImU32 __stdcall ImGui_ColorConvertFloat4ToU32900(const ImVec4& in)
{
	return ImGui::ColorConvertFloat4ToU32(in);
}
__declspec(dllexport) void __stdcall ImGui_ColorConvertRGBtoHSV901(float r, float g, float b, float& out_h, float& out_s, float& out_v)
{
	ImGui::ColorConvertRGBtoHSV(r, g, b, out_h, out_s, out_v);
}
__declspec(dllexport) void __stdcall ImGui_ColorConvertHSVtoRGB902(float h, float s, float v, float& out_r, float& out_g, float& out_b)
{
	ImGui::ColorConvertHSVtoRGB(h, s, v, out_r, out_g, out_b);
}
__declspec(dllexport) bool __stdcall ImGui_IsKeyDown909(ImGuiKey key)
{
	return ImGui::IsKeyDown(key);
}
__declspec(dllexport) bool __stdcall ImGui_IsKeyPressed910(ImGuiKey key, bool repeat)
{
	return ImGui::IsKeyPressed(key, repeat);
}
__declspec(dllexport) bool __stdcall ImGui_IsKeyReleased911(ImGuiKey key)
{
	return ImGui::IsKeyReleased(key);
}
__declspec(dllexport) int __stdcall ImGui_GetKeyPressedAmount912(ImGuiKey key, float repeat_delay, float rate)
{
	return ImGui::GetKeyPressedAmount(key, repeat_delay, rate);
}
__declspec(dllexport) const char* __stdcall ImGui_GetKeyName913(ImGuiKey key)
{
	return ImGui::GetKeyName(key);
}
__declspec(dllexport) void __stdcall ImGui_SetNextFrameWantCaptureKeyboard914(bool want_capture_keyboard)
{
	ImGui::SetNextFrameWantCaptureKeyboard(want_capture_keyboard);
}
__declspec(dllexport) bool __stdcall ImGui_IsMouseDown920(ImGuiMouseButton button)
{
	return ImGui::IsMouseDown(button);
}
__declspec(dllexport) bool __stdcall ImGui_IsMouseClicked921(ImGuiMouseButton button, bool repeat)
{
	return ImGui::IsMouseClicked(button, repeat);
}
__declspec(dllexport) bool __stdcall ImGui_IsMouseReleased922(ImGuiMouseButton button)
{
	return ImGui::IsMouseReleased(button);
}
__declspec(dllexport) bool __stdcall ImGui_IsMouseDoubleClicked923(ImGuiMouseButton button)
{
	return ImGui::IsMouseDoubleClicked(button);
}
__declspec(dllexport) int __stdcall ImGui_GetMouseClickedCount924(ImGuiMouseButton button)
{
	return ImGui::GetMouseClickedCount(button);
}
__declspec(dllexport) bool __stdcall ImGui_IsMouseHoveringRect925(const ImVec2& r_min, const ImVec2& r_max, bool clip)
{
	return ImGui::IsMouseHoveringRect(r_min, r_max, clip);
}
__declspec(dllexport) bool __stdcall ImGui_IsMousePosValid926(const ImVec2* mouse_pos)
{
	return ImGui::IsMousePosValid(mouse_pos);
}
__declspec(dllexport) bool __stdcall ImGui_IsAnyMouseDown927()
{
	return ImGui::IsAnyMouseDown();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetMousePos928()
{
	return ImGui::GetMousePos();
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetMousePosOnOpeningCurrentPopup929()
{
	return ImGui::GetMousePosOnOpeningCurrentPopup();
}
__declspec(dllexport) bool __stdcall ImGui_IsMouseDragging930(ImGuiMouseButton button, float lock_threshold)
{
	return ImGui::IsMouseDragging(button, lock_threshold);
}
__declspec(dllexport) ImVec2& __stdcall ImGui_GetMouseDragDelta931(ImGuiMouseButton button, float lock_threshold)
{
	return ImGui::GetMouseDragDelta(button, lock_threshold);
}
__declspec(dllexport) void __stdcall ImGui_ResetMouseDragDelta932(ImGuiMouseButton button)
{
	ImGui::ResetMouseDragDelta(button);
}
__declspec(dllexport) ImGuiMouseCursor __stdcall ImGui_GetMouseCursor933()
{
	return ImGui::GetMouseCursor();
}
__declspec(dllexport) void __stdcall ImGui_SetMouseCursor934(ImGuiMouseCursor cursor_type)
{
	ImGui::SetMouseCursor(cursor_type);
}
__declspec(dllexport) void __stdcall ImGui_SetNextFrameWantCaptureMouse935(bool want_capture_mouse)
{
	ImGui::SetNextFrameWantCaptureMouse(want_capture_mouse);
}
__declspec(dllexport) const char* __stdcall ImGui_GetClipboardText939()
{
	return ImGui::GetClipboardText();
}
__declspec(dllexport) void __stdcall ImGui_SetClipboardText940(const char* text)
{
	ImGui::SetClipboardText(text);
}
__declspec(dllexport) void __stdcall ImGui_LoadIniSettingsFromDisk946(const char* ini_filename)
{
	ImGui::LoadIniSettingsFromDisk(ini_filename);
}
__declspec(dllexport) void __stdcall ImGui_LoadIniSettingsFromMemory947(const char* ini_data, size_t ini_size)
{
	ImGui::LoadIniSettingsFromMemory(ini_data, ini_size);
}
__declspec(dllexport) void __stdcall ImGui_SaveIniSettingsToDisk948(const char* ini_filename)
{
	ImGui::SaveIniSettingsToDisk(ini_filename);
}
__declspec(dllexport) const char* __stdcall ImGui_SaveIniSettingsToMemory949(size_t* out_ini_size)
{
	return ImGui::SaveIniSettingsToMemory(out_ini_size);
}
__declspec(dllexport) void __stdcall ImGui_DebugTextEncoding952(const char* text)
{
	ImGui::DebugTextEncoding(text);
}
__declspec(dllexport) bool __stdcall ImGui_DebugCheckVersionAndDataLayout953(const char* version_str, size_t sz_io, size_t sz_style, size_t sz_vec2, size_t sz_vec4, size_t sz_drawvert, size_t sz_drawidx)
{
	return ImGui::DebugCheckVersionAndDataLayout(version_str, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert, sz_drawidx);
}
__declspec(dllexport) void __stdcall ImGui_SetAllocatorFunctions959(ImGuiMemAllocFunc alloc_func, ImGuiMemFreeFunc free_func, void* user_data)
{
	ImGui::SetAllocatorFunctions(alloc_func, free_func, user_data);
}
__declspec(dllexport) void __stdcall ImGui_GetAllocatorFunctions960(ImGuiMemAllocFunc* p_alloc_func, ImGuiMemFreeFunc* p_free_func, void** p_user_data)
{
	ImGui::GetAllocatorFunctions(p_alloc_func, p_free_func, p_user_data);
}
__declspec(dllexport) void* __stdcall ImGui_MemAlloc961(size_t size)
{
	return ImGui::MemAlloc(size);
}
__declspec(dllexport) void __stdcall ImGui_MemFree962(void* ptr)
{
	ImGui::MemFree(ptr);
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_Alpha1886(ImGuiStyle* objectPtr)
{
	return objectPtr->Alpha;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_Alpha1886(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->Alpha = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_DisabledAlpha1887(ImGuiStyle* objectPtr)
{
	return objectPtr->DisabledAlpha;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_DisabledAlpha1887(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->DisabledAlpha = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_WindowPadding1888(ImGuiStyle* objectPtr)
{
	return objectPtr->WindowPadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_WindowPadding1888(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->WindowPadding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_WindowRounding1889(ImGuiStyle* objectPtr)
{
	return objectPtr->WindowRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_WindowRounding1889(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->WindowRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_WindowBorderSize1890(ImGuiStyle* objectPtr)
{
	return objectPtr->WindowBorderSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_WindowBorderSize1890(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->WindowBorderSize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_WindowMinSize1891(ImGuiStyle* objectPtr)
{
	return objectPtr->WindowMinSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_WindowMinSize1891(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->WindowMinSize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_WindowTitleAlign1892(ImGuiStyle* objectPtr)
{
	return objectPtr->WindowTitleAlign;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_WindowTitleAlign1892(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->WindowTitleAlign = value;
}
__declspec(dllexport) ImGuiDir __stdcall ImGuiStyle_Get_WindowMenuButtonPosition1893(ImGuiStyle* objectPtr)
{
	return objectPtr->WindowMenuButtonPosition;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_WindowMenuButtonPosition1893(ImGuiStyle* objectPtr, ImGuiDir  value)
{
	objectPtr->WindowMenuButtonPosition = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_ChildRounding1894(ImGuiStyle* objectPtr)
{
	return objectPtr->ChildRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ChildRounding1894(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->ChildRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_ChildBorderSize1895(ImGuiStyle* objectPtr)
{
	return objectPtr->ChildBorderSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ChildBorderSize1895(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->ChildBorderSize = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_PopupRounding1896(ImGuiStyle* objectPtr)
{
	return objectPtr->PopupRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_PopupRounding1896(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->PopupRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_PopupBorderSize1897(ImGuiStyle* objectPtr)
{
	return objectPtr->PopupBorderSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_PopupBorderSize1897(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->PopupBorderSize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_FramePadding1898(ImGuiStyle* objectPtr)
{
	return objectPtr->FramePadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_FramePadding1898(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->FramePadding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_FrameRounding1899(ImGuiStyle* objectPtr)
{
	return objectPtr->FrameRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_FrameRounding1899(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->FrameRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_FrameBorderSize1900(ImGuiStyle* objectPtr)
{
	return objectPtr->FrameBorderSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_FrameBorderSize1900(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->FrameBorderSize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_ItemSpacing1901(ImGuiStyle* objectPtr)
{
	return objectPtr->ItemSpacing;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ItemSpacing1901(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->ItemSpacing = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_ItemInnerSpacing1902(ImGuiStyle* objectPtr)
{
	return objectPtr->ItemInnerSpacing;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ItemInnerSpacing1902(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->ItemInnerSpacing = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_CellPadding1903(ImGuiStyle* objectPtr)
{
	return objectPtr->CellPadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_CellPadding1903(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->CellPadding = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_TouchExtraPadding1904(ImGuiStyle* objectPtr)
{
	return objectPtr->TouchExtraPadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_TouchExtraPadding1904(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->TouchExtraPadding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_IndentSpacing1905(ImGuiStyle* objectPtr)
{
	return objectPtr->IndentSpacing;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_IndentSpacing1905(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->IndentSpacing = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_ColumnsMinSpacing1906(ImGuiStyle* objectPtr)
{
	return objectPtr->ColumnsMinSpacing;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ColumnsMinSpacing1906(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->ColumnsMinSpacing = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_ScrollbarSize1907(ImGuiStyle* objectPtr)
{
	return objectPtr->ScrollbarSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ScrollbarSize1907(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->ScrollbarSize = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_ScrollbarRounding1908(ImGuiStyle* objectPtr)
{
	return objectPtr->ScrollbarRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ScrollbarRounding1908(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->ScrollbarRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_GrabMinSize1909(ImGuiStyle* objectPtr)
{
	return objectPtr->GrabMinSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_GrabMinSize1909(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->GrabMinSize = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_GrabRounding1910(ImGuiStyle* objectPtr)
{
	return objectPtr->GrabRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_GrabRounding1910(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->GrabRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_LogSliderDeadzone1911(ImGuiStyle* objectPtr)
{
	return objectPtr->LogSliderDeadzone;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_LogSliderDeadzone1911(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->LogSliderDeadzone = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_TabRounding1912(ImGuiStyle* objectPtr)
{
	return objectPtr->TabRounding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_TabRounding1912(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->TabRounding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_TabBorderSize1913(ImGuiStyle* objectPtr)
{
	return objectPtr->TabBorderSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_TabBorderSize1913(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->TabBorderSize = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_TabMinWidthForCloseButton1914(ImGuiStyle* objectPtr)
{
	return objectPtr->TabMinWidthForCloseButton;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_TabMinWidthForCloseButton1914(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->TabMinWidthForCloseButton = value;
}
__declspec(dllexport) ImGuiDir __stdcall ImGuiStyle_Get_ColorButtonPosition1915(ImGuiStyle* objectPtr)
{
	return objectPtr->ColorButtonPosition;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ColorButtonPosition1915(ImGuiStyle* objectPtr, ImGuiDir  value)
{
	objectPtr->ColorButtonPosition = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_ButtonTextAlign1916(ImGuiStyle* objectPtr)
{
	return objectPtr->ButtonTextAlign;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_ButtonTextAlign1916(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->ButtonTextAlign = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_SelectableTextAlign1917(ImGuiStyle* objectPtr)
{
	return objectPtr->SelectableTextAlign;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_SelectableTextAlign1917(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->SelectableTextAlign = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_SeparatorTextBorderSize1918(ImGuiStyle* objectPtr)
{
	return objectPtr->SeparatorTextBorderSize;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_SeparatorTextBorderSize1918(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->SeparatorTextBorderSize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_SeparatorTextAlign1919(ImGuiStyle* objectPtr)
{
	return objectPtr->SeparatorTextAlign;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_SeparatorTextAlign1919(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->SeparatorTextAlign = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_SeparatorTextPadding1920(ImGuiStyle* objectPtr)
{
	return objectPtr->SeparatorTextPadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_SeparatorTextPadding1920(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->SeparatorTextPadding = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_DisplayWindowPadding1921(ImGuiStyle* objectPtr)
{
	return objectPtr->DisplayWindowPadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_DisplayWindowPadding1921(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->DisplayWindowPadding = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiStyle_Get_DisplaySafeAreaPadding1922(ImGuiStyle* objectPtr)
{
	return objectPtr->DisplaySafeAreaPadding;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_DisplaySafeAreaPadding1922(ImGuiStyle* objectPtr, ImVec2&  value)
{
	objectPtr->DisplaySafeAreaPadding = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_MouseCursorScale1923(ImGuiStyle* objectPtr)
{
	return objectPtr->MouseCursorScale;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_MouseCursorScale1923(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->MouseCursorScale = value;
}
__declspec(dllexport) bool __stdcall ImGuiStyle_Get_AntiAliasedLines1924(ImGuiStyle* objectPtr)
{
	return objectPtr->AntiAliasedLines;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_AntiAliasedLines1924(ImGuiStyle* objectPtr, bool  value)
{
	objectPtr->AntiAliasedLines = value;
}
__declspec(dllexport) bool __stdcall ImGuiStyle_Get_AntiAliasedLinesUseTex1925(ImGuiStyle* objectPtr)
{
	return objectPtr->AntiAliasedLinesUseTex;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_AntiAliasedLinesUseTex1925(ImGuiStyle* objectPtr, bool  value)
{
	objectPtr->AntiAliasedLinesUseTex = value;
}
__declspec(dllexport) bool __stdcall ImGuiStyle_Get_AntiAliasedFill1926(ImGuiStyle* objectPtr)
{
	return objectPtr->AntiAliasedFill;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_AntiAliasedFill1926(ImGuiStyle* objectPtr, bool  value)
{
	objectPtr->AntiAliasedFill = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_CurveTessellationTol1927(ImGuiStyle* objectPtr)
{
	return objectPtr->CurveTessellationTol;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_CurveTessellationTol1927(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->CurveTessellationTol = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_CircleTessellationMaxError1928(ImGuiStyle* objectPtr)
{
	return objectPtr->CircleTessellationMaxError;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_CircleTessellationMaxError1928(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->CircleTessellationMaxError = value;
}
__declspec(dllexport) ImVec4* __stdcall ImGuiStyle_Get_Colors1929(ImGuiStyle* objectPtr)
{
	return objectPtr->Colors;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_HoverStationaryDelay1933(ImGuiStyle* objectPtr)
{
	return objectPtr->HoverStationaryDelay;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_HoverStationaryDelay1933(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->HoverStationaryDelay = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_HoverDelayShort1934(ImGuiStyle* objectPtr)
{
	return objectPtr->HoverDelayShort;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_HoverDelayShort1934(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->HoverDelayShort = value;
}
__declspec(dllexport) float __stdcall ImGuiStyle_Get_HoverDelayNormal1935(ImGuiStyle* objectPtr)
{
	return objectPtr->HoverDelayNormal;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_HoverDelayNormal1935(ImGuiStyle* objectPtr, float  value)
{
	objectPtr->HoverDelayNormal = value;
}
__declspec(dllexport) ImGuiHoveredFlags __stdcall ImGuiStyle_Get_HoverFlagsForTooltipMouse1936(ImGuiStyle* objectPtr)
{
	return objectPtr->HoverFlagsForTooltipMouse;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_HoverFlagsForTooltipMouse1936(ImGuiStyle* objectPtr, ImGuiHoveredFlags  value)
{
	objectPtr->HoverFlagsForTooltipMouse = value;
}
__declspec(dllexport) ImGuiHoveredFlags __stdcall ImGuiStyle_Get_HoverFlagsForTooltipNav1937(ImGuiStyle* objectPtr)
{
	return objectPtr->HoverFlagsForTooltipNav;
}
__declspec(dllexport) void __stdcall ImGuiStyle_Set_HoverFlagsForTooltipNav1937(ImGuiStyle* objectPtr, ImGuiHoveredFlags  value)
{
	objectPtr->HoverFlagsForTooltipNav = value;
}
__declspec(dllexport) ImGuiStyle* __stdcall ImGuiStyle_ImGuiStyle1939()
{
	return IM_NEW(ImGuiStyle)();
}
__declspec(dllexport) void __stdcall ImGuiStyle_ScaleAllSizes1940(ImGuiStyle* objectPtr, float scale_factor)
{
	objectPtr->ScaleAllSizes(scale_factor);
}
__declspec(dllexport) bool __stdcall ImGuiKeyData_Get_Down1954(ImGuiKeyData* objectPtr)
{
	return objectPtr->Down;
}
__declspec(dllexport) void __stdcall ImGuiKeyData_Set_Down1954(ImGuiKeyData* objectPtr, bool  value)
{
	objectPtr->Down = value;
}
__declspec(dllexport) float __stdcall ImGuiKeyData_Get_DownDuration1955(ImGuiKeyData* objectPtr)
{
	return objectPtr->DownDuration;
}
__declspec(dllexport) void __stdcall ImGuiKeyData_Set_DownDuration1955(ImGuiKeyData* objectPtr, float  value)
{
	objectPtr->DownDuration = value;
}
__declspec(dllexport) float __stdcall ImGuiKeyData_Get_DownDurationPrev1956(ImGuiKeyData* objectPtr)
{
	return objectPtr->DownDurationPrev;
}
__declspec(dllexport) void __stdcall ImGuiKeyData_Set_DownDurationPrev1956(ImGuiKeyData* objectPtr, float  value)
{
	objectPtr->DownDurationPrev = value;
}
__declspec(dllexport) float __stdcall ImGuiKeyData_Get_AnalogValue1957(ImGuiKeyData* objectPtr)
{
	return objectPtr->AnalogValue;
}
__declspec(dllexport) void __stdcall ImGuiKeyData_Set_AnalogValue1957(ImGuiKeyData* objectPtr, float  value)
{
	objectPtr->AnalogValue = value;
}
__declspec(dllexport) ImGuiConfigFlags __stdcall ImGuiIO_Get_ConfigFlags1966(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigFlags;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigFlags1966(ImGuiIO* objectPtr, ImGuiConfigFlags  value)
{
	objectPtr->ConfigFlags = value;
}
__declspec(dllexport) ImGuiBackendFlags __stdcall ImGuiIO_Get_BackendFlags1967(ImGuiIO* objectPtr)
{
	return objectPtr->BackendFlags;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendFlags1967(ImGuiIO* objectPtr, ImGuiBackendFlags  value)
{
	objectPtr->BackendFlags = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiIO_Get_DisplaySize1968(ImGuiIO* objectPtr)
{
	return objectPtr->DisplaySize;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_DisplaySize1968(ImGuiIO* objectPtr, ImVec2&  value)
{
	objectPtr->DisplaySize = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_DeltaTime1969(ImGuiIO* objectPtr)
{
	return objectPtr->DeltaTime;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_DeltaTime1969(ImGuiIO* objectPtr, float  value)
{
	objectPtr->DeltaTime = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_IniSavingRate1970(ImGuiIO* objectPtr)
{
	return objectPtr->IniSavingRate;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_IniSavingRate1970(ImGuiIO* objectPtr, float  value)
{
	objectPtr->IniSavingRate = value;
}
__declspec(dllexport) const char* __stdcall ImGuiIO_Get_IniFilename1971(ImGuiIO* objectPtr)
{
	return objectPtr->IniFilename;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_IniFilename1971(ImGuiIO* objectPtr, const char*  value)
{
	objectPtr->IniFilename = value;
}
__declspec(dllexport) const char* __stdcall ImGuiIO_Get_LogFilename1972(ImGuiIO* objectPtr)
{
	return objectPtr->LogFilename;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_LogFilename1972(ImGuiIO* objectPtr, const char*  value)
{
	objectPtr->LogFilename = value;
}
__declspec(dllexport) void* __stdcall ImGuiIO_Get_UserData1973(ImGuiIO* objectPtr)
{
	return objectPtr->UserData;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_UserData1973(ImGuiIO* objectPtr, void*  value)
{
	objectPtr->UserData = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiIO_Get_DisplayFramebufferScale1979(ImGuiIO* objectPtr)
{
	return objectPtr->DisplayFramebufferScale;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_DisplayFramebufferScale1979(ImGuiIO* objectPtr, ImVec2&  value)
{
	objectPtr->DisplayFramebufferScale = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_MouseDrawCursor1982(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDrawCursor;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseDrawCursor1982(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->MouseDrawCursor = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigMacOSXBehaviors1983(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigMacOSXBehaviors;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigMacOSXBehaviors1983(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigMacOSXBehaviors = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigInputTrickleEventQueue1984(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigInputTrickleEventQueue;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigInputTrickleEventQueue1984(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigInputTrickleEventQueue = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigInputTextCursorBlink1985(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigInputTextCursorBlink;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigInputTextCursorBlink1985(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigInputTextCursorBlink = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigInputTextEnterKeepActive1986(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigInputTextEnterKeepActive;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigInputTextEnterKeepActive1986(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigInputTextEnterKeepActive = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigDragClickToInputText1987(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigDragClickToInputText;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigDragClickToInputText1987(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigDragClickToInputText = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigWindowsResizeFromEdges1988(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigWindowsResizeFromEdges;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigWindowsResizeFromEdges1988(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigWindowsResizeFromEdges = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigWindowsMoveFromTitleBarOnly1989(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigWindowsMoveFromTitleBarOnly;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigWindowsMoveFromTitleBarOnly1989(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigWindowsMoveFromTitleBarOnly = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_ConfigMemoryCompactTimer1990(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigMemoryCompactTimer;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigMemoryCompactTimer1990(ImGuiIO* objectPtr, float  value)
{
	objectPtr->ConfigMemoryCompactTimer = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_MouseDoubleClickTime1994(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDoubleClickTime;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseDoubleClickTime1994(ImGuiIO* objectPtr, float  value)
{
	objectPtr->MouseDoubleClickTime = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_MouseDoubleClickMaxDist1995(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDoubleClickMaxDist;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseDoubleClickMaxDist1995(ImGuiIO* objectPtr, float  value)
{
	objectPtr->MouseDoubleClickMaxDist = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_MouseDragThreshold1996(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDragThreshold;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseDragThreshold1996(ImGuiIO* objectPtr, float  value)
{
	objectPtr->MouseDragThreshold = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_KeyRepeatDelay1997(ImGuiIO* objectPtr)
{
	return objectPtr->KeyRepeatDelay;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeyRepeatDelay1997(ImGuiIO* objectPtr, float  value)
{
	objectPtr->KeyRepeatDelay = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_KeyRepeatRate1998(ImGuiIO* objectPtr)
{
	return objectPtr->KeyRepeatRate;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeyRepeatRate1998(ImGuiIO* objectPtr, float  value)
{
	objectPtr->KeyRepeatRate = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigDebugBeginReturnValueOnce2008(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigDebugBeginReturnValueOnce;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigDebugBeginReturnValueOnce2008(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigDebugBeginReturnValueOnce = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigDebugBeginReturnValueLoop2009(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigDebugBeginReturnValueLoop;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigDebugBeginReturnValueLoop2009(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigDebugBeginReturnValueLoop = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigDebugIgnoreFocusLoss2014(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigDebugIgnoreFocusLoss;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigDebugIgnoreFocusLoss2014(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigDebugIgnoreFocusLoss = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_ConfigDebugIniSettings2017(ImGuiIO* objectPtr)
{
	return objectPtr->ConfigDebugIniSettings;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ConfigDebugIniSettings2017(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->ConfigDebugIniSettings = value;
}
__declspec(dllexport) const char* __stdcall ImGuiIO_Get_BackendPlatformName2025(ImGuiIO* objectPtr)
{
	return objectPtr->BackendPlatformName;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendPlatformName2025(ImGuiIO* objectPtr, const char*  value)
{
	objectPtr->BackendPlatformName = value;
}
__declspec(dllexport) const char* __stdcall ImGuiIO_Get_BackendRendererName2026(ImGuiIO* objectPtr)
{
	return objectPtr->BackendRendererName;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendRendererName2026(ImGuiIO* objectPtr, const char*  value)
{
	objectPtr->BackendRendererName = value;
}
__declspec(dllexport) void* __stdcall ImGuiIO_Get_BackendPlatformUserData2027(ImGuiIO* objectPtr)
{
	return objectPtr->BackendPlatformUserData;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendPlatformUserData2027(ImGuiIO* objectPtr, void*  value)
{
	objectPtr->BackendPlatformUserData = value;
}
__declspec(dllexport) void* __stdcall ImGuiIO_Get_BackendRendererUserData2028(ImGuiIO* objectPtr)
{
	return objectPtr->BackendRendererUserData;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendRendererUserData2028(ImGuiIO* objectPtr, void*  value)
{
	objectPtr->BackendRendererUserData = value;
}
__declspec(dllexport) void* __stdcall ImGuiIO_Get_BackendLanguageUserData2029(ImGuiIO* objectPtr)
{
	return objectPtr->BackendLanguageUserData;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendLanguageUserData2029(ImGuiIO* objectPtr, void*  value)
{
	objectPtr->BackendLanguageUserData = value;
}
__declspec(dllexport) void* __stdcall ImGuiIO_Get_ClipboardUserData2035(ImGuiIO* objectPtr)
{
	return objectPtr->ClipboardUserData;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ClipboardUserData2035(ImGuiIO* objectPtr, void*  value)
{
	objectPtr->ClipboardUserData = value;
}
__declspec(dllexport) void* __stdcall ImGuiIO_Get_ImeWindowHandle2041(ImGuiIO* objectPtr)
{
	return objectPtr->ImeWindowHandle;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_ImeWindowHandle2041(ImGuiIO* objectPtr, void*  value)
{
	objectPtr->ImeWindowHandle = value;
}
__declspec(dllexport) ImWchar __stdcall ImGuiIO_Get_PlatformLocaleDecimalPoint2047(ImGuiIO* objectPtr)
{
	return objectPtr->PlatformLocaleDecimalPoint;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_PlatformLocaleDecimalPoint2047(ImGuiIO* objectPtr, ImWchar  value)
{
	objectPtr->PlatformLocaleDecimalPoint = value;
}
__declspec(dllexport) void __stdcall ImGuiIO_AddKeyEvent2054(ImGuiIO* objectPtr, ImGuiKey key, bool down)
{
	objectPtr->AddKeyEvent(key, down);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddKeyAnalogEvent2055(ImGuiIO* objectPtr, ImGuiKey key, bool down, float v)
{
	objectPtr->AddKeyAnalogEvent(key, down, v);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddMousePosEvent2056(ImGuiIO* objectPtr, float x, float y)
{
	objectPtr->AddMousePosEvent(x, y);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddMouseButtonEvent2057(ImGuiIO* objectPtr, int button, bool down)
{
	objectPtr->AddMouseButtonEvent(button, down);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddMouseWheelEvent2058(ImGuiIO* objectPtr, float wheel_x, float wheel_y)
{
	objectPtr->AddMouseWheelEvent(wheel_x, wheel_y);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddMouseSourceEvent2059(ImGuiIO* objectPtr, ImGuiMouseSource source)
{
	objectPtr->AddMouseSourceEvent(source);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddFocusEvent2060(ImGuiIO* objectPtr, bool focused)
{
	objectPtr->AddFocusEvent(focused);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddInputCharacter2061(ImGuiIO* objectPtr, unsigned int c)
{
	objectPtr->AddInputCharacter(c);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddInputCharacterUTF162062(ImGuiIO* objectPtr, ImWchar16 c)
{
	objectPtr->AddInputCharacterUTF16(c);
}
__declspec(dllexport) void __stdcall ImGuiIO_AddInputCharactersUTF82063(ImGuiIO* objectPtr, const char* str)
{
	objectPtr->AddInputCharactersUTF8(str);
}
__declspec(dllexport) void __stdcall ImGuiIO_SetKeyEventNativeData2065(ImGuiIO* objectPtr, ImGuiKey key, int native_keycode, int native_scancode, int native_legacy_index)
{
	objectPtr->SetKeyEventNativeData(key, native_keycode, native_scancode, native_legacy_index);
}
__declspec(dllexport) void __stdcall ImGuiIO_SetAppAcceptingEvents2066(ImGuiIO* objectPtr, bool accepting_events)
{
	objectPtr->SetAppAcceptingEvents(accepting_events);
}
__declspec(dllexport) void __stdcall ImGuiIO_ClearEventsQueue2067(ImGuiIO* objectPtr)
{
	objectPtr->ClearEventsQueue();
}
__declspec(dllexport) void __stdcall ImGuiIO_ClearInputKeys2068(ImGuiIO* objectPtr)
{
	objectPtr->ClearInputKeys();
}
__declspec(dllexport) void __stdcall ImGuiIO_ClearInputCharacters2070(ImGuiIO* objectPtr)
{
	objectPtr->ClearInputCharacters();
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_WantCaptureMouse2079(ImGuiIO* objectPtr)
{
	return objectPtr->WantCaptureMouse;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_WantCaptureMouse2079(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->WantCaptureMouse = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_WantCaptureKeyboard2080(ImGuiIO* objectPtr)
{
	return objectPtr->WantCaptureKeyboard;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_WantCaptureKeyboard2080(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->WantCaptureKeyboard = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_WantTextInput2081(ImGuiIO* objectPtr)
{
	return objectPtr->WantTextInput;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_WantTextInput2081(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->WantTextInput = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_WantSetMousePos2082(ImGuiIO* objectPtr)
{
	return objectPtr->WantSetMousePos;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_WantSetMousePos2082(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->WantSetMousePos = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_WantSaveIniSettings2083(ImGuiIO* objectPtr)
{
	return objectPtr->WantSaveIniSettings;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_WantSaveIniSettings2083(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->WantSaveIniSettings = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_NavActive2084(ImGuiIO* objectPtr)
{
	return objectPtr->NavActive;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_NavActive2084(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->NavActive = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_NavVisible2085(ImGuiIO* objectPtr)
{
	return objectPtr->NavVisible;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_NavVisible2085(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->NavVisible = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_Framerate2086(ImGuiIO* objectPtr)
{
	return objectPtr->Framerate;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_Framerate2086(ImGuiIO* objectPtr, float  value)
{
	objectPtr->Framerate = value;
}
__declspec(dllexport) int __stdcall ImGuiIO_Get_MetricsRenderVertices2087(ImGuiIO* objectPtr)
{
	return objectPtr->MetricsRenderVertices;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MetricsRenderVertices2087(ImGuiIO* objectPtr, int  value)
{
	objectPtr->MetricsRenderVertices = value;
}
__declspec(dllexport) int __stdcall ImGuiIO_Get_MetricsRenderIndices2088(ImGuiIO* objectPtr)
{
	return objectPtr->MetricsRenderIndices;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MetricsRenderIndices2088(ImGuiIO* objectPtr, int  value)
{
	objectPtr->MetricsRenderIndices = value;
}
__declspec(dllexport) int __stdcall ImGuiIO_Get_MetricsRenderWindows2089(ImGuiIO* objectPtr)
{
	return objectPtr->MetricsRenderWindows;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MetricsRenderWindows2089(ImGuiIO* objectPtr, int  value)
{
	objectPtr->MetricsRenderWindows = value;
}
__declspec(dllexport) int __stdcall ImGuiIO_Get_MetricsActiveWindows2090(ImGuiIO* objectPtr)
{
	return objectPtr->MetricsActiveWindows;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MetricsActiveWindows2090(ImGuiIO* objectPtr, int  value)
{
	objectPtr->MetricsActiveWindows = value;
}
__declspec(dllexport) int __stdcall ImGuiIO_Get_MetricsActiveAllocations2091(ImGuiIO* objectPtr)
{
	return objectPtr->MetricsActiveAllocations;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MetricsActiveAllocations2091(ImGuiIO* objectPtr, int  value)
{
	objectPtr->MetricsActiveAllocations = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiIO_Get_MouseDelta2092(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDelta;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseDelta2092(ImGuiIO* objectPtr, ImVec2&  value)
{
	objectPtr->MouseDelta = value;
}
__declspec(dllexport) int* __stdcall ImGuiIO_Get_KeyMap2098(ImGuiIO* objectPtr)
{
	return objectPtr->KeyMap;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_KeysDown2099(ImGuiIO* objectPtr)
{
	return objectPtr->KeysDown;
}
__declspec(dllexport) float* __stdcall ImGuiIO_Get_NavInputs2100(ImGuiIO* objectPtr)
{
	return objectPtr->NavInputs;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiIO_Get_MousePos2112(ImGuiIO* objectPtr)
{
	return objectPtr->MousePos;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MousePos2112(ImGuiIO* objectPtr, ImVec2&  value)
{
	objectPtr->MousePos = value;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_MouseDown2113(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDown;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_MouseWheel2114(ImGuiIO* objectPtr)
{
	return objectPtr->MouseWheel;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseWheel2114(ImGuiIO* objectPtr, float  value)
{
	objectPtr->MouseWheel = value;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_MouseWheelH2115(ImGuiIO* objectPtr)
{
	return objectPtr->MouseWheelH;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseWheelH2115(ImGuiIO* objectPtr, float  value)
{
	objectPtr->MouseWheelH = value;
}
__declspec(dllexport) ImGuiMouseSource __stdcall ImGuiIO_Get_MouseSource2116(ImGuiIO* objectPtr)
{
	return objectPtr->MouseSource;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseSource2116(ImGuiIO* objectPtr, ImGuiMouseSource  value)
{
	objectPtr->MouseSource = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_KeyCtrl2117(ImGuiIO* objectPtr)
{
	return objectPtr->KeyCtrl;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeyCtrl2117(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->KeyCtrl = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_KeyShift2118(ImGuiIO* objectPtr)
{
	return objectPtr->KeyShift;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeyShift2118(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->KeyShift = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_KeyAlt2119(ImGuiIO* objectPtr)
{
	return objectPtr->KeyAlt;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeyAlt2119(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->KeyAlt = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_KeySuper2120(ImGuiIO* objectPtr)
{
	return objectPtr->KeySuper;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeySuper2120(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->KeySuper = value;
}
__declspec(dllexport) ImGuiKeyChord __stdcall ImGuiIO_Get_KeyMods2123(ImGuiIO* objectPtr)
{
	return objectPtr->KeyMods;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_KeyMods2123(ImGuiIO* objectPtr, ImGuiKeyChord  value)
{
	objectPtr->KeyMods = value;
}
__declspec(dllexport) ImGuiKeyData* __stdcall ImGuiIO_Get_KeysData2124(ImGuiIO* objectPtr)
{
	return objectPtr->KeysData;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_WantCaptureMouseUnlessPopupClose2125(ImGuiIO* objectPtr)
{
	return objectPtr->WantCaptureMouseUnlessPopupClose;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_WantCaptureMouseUnlessPopupClose2125(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->WantCaptureMouseUnlessPopupClose = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiIO_Get_MousePosPrev2126(ImGuiIO* objectPtr)
{
	return objectPtr->MousePosPrev;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MousePosPrev2126(ImGuiIO* objectPtr, ImVec2&  value)
{
	objectPtr->MousePosPrev = value;
}
__declspec(dllexport) ImVec2* __stdcall ImGuiIO_Get_MouseClickedPos2127(ImGuiIO* objectPtr)
{
	return objectPtr->MouseClickedPos;
}
__declspec(dllexport) double* __stdcall ImGuiIO_Get_MouseClickedTime2128(ImGuiIO* objectPtr)
{
	return objectPtr->MouseClickedTime;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_MouseClicked2129(ImGuiIO* objectPtr)
{
	return objectPtr->MouseClicked;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_MouseDoubleClicked2130(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDoubleClicked;
}
__declspec(dllexport) ImU16* __stdcall ImGuiIO_Get_MouseClickedCount2131(ImGuiIO* objectPtr)
{
	return objectPtr->MouseClickedCount;
}
__declspec(dllexport) ImU16* __stdcall ImGuiIO_Get_MouseClickedLastCount2132(ImGuiIO* objectPtr)
{
	return objectPtr->MouseClickedLastCount;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_MouseReleased2133(ImGuiIO* objectPtr)
{
	return objectPtr->MouseReleased;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_MouseDownOwned2134(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDownOwned;
}
__declspec(dllexport) bool* __stdcall ImGuiIO_Get_MouseDownOwnedUnlessPopupClose2135(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDownOwnedUnlessPopupClose;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_MouseWheelRequestAxisSwap2136(ImGuiIO* objectPtr)
{
	return objectPtr->MouseWheelRequestAxisSwap;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_MouseWheelRequestAxisSwap2136(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->MouseWheelRequestAxisSwap = value;
}
__declspec(dllexport) float* __stdcall ImGuiIO_Get_MouseDownDuration2137(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDownDuration;
}
__declspec(dllexport) float* __stdcall ImGuiIO_Get_MouseDownDurationPrev2138(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDownDurationPrev;
}
__declspec(dllexport) float* __stdcall ImGuiIO_Get_MouseDragMaxDistanceSqr2139(ImGuiIO* objectPtr)
{
	return objectPtr->MouseDragMaxDistanceSqr;
}
__declspec(dllexport) float __stdcall ImGuiIO_Get_PenPressure2140(ImGuiIO* objectPtr)
{
	return objectPtr->PenPressure;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_PenPressure2140(ImGuiIO* objectPtr, float  value)
{
	objectPtr->PenPressure = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_AppFocusLost2141(ImGuiIO* objectPtr)
{
	return objectPtr->AppFocusLost;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_AppFocusLost2141(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->AppFocusLost = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_AppAcceptingEvents2142(ImGuiIO* objectPtr)
{
	return objectPtr->AppAcceptingEvents;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_AppAcceptingEvents2142(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->AppAcceptingEvents = value;
}
__declspec(dllexport) ImS8 __stdcall ImGuiIO_Get_BackendUsingLegacyKeyArrays2143(ImGuiIO* objectPtr)
{
	return objectPtr->BackendUsingLegacyKeyArrays;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendUsingLegacyKeyArrays2143(ImGuiIO* objectPtr, ImS8  value)
{
	objectPtr->BackendUsingLegacyKeyArrays = value;
}
__declspec(dllexport) bool __stdcall ImGuiIO_Get_BackendUsingLegacyNavInputArray2144(ImGuiIO* objectPtr)
{
	return objectPtr->BackendUsingLegacyNavInputArray;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_BackendUsingLegacyNavInputArray2144(ImGuiIO* objectPtr, bool  value)
{
	objectPtr->BackendUsingLegacyNavInputArray = value;
}
__declspec(dllexport) ImWchar16 __stdcall ImGuiIO_Get_InputQueueSurrogate2145(ImGuiIO* objectPtr)
{
	return objectPtr->InputQueueSurrogate;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_InputQueueSurrogate2145(ImGuiIO* objectPtr, ImWchar16  value)
{
	objectPtr->InputQueueSurrogate = value;
}
__declspec(dllexport) ImVector<ImWchar> __stdcall ImGuiIO_Get_InputQueueCharacters2146(int* returnListSize, ImGuiIO* objectPtr)
{
	*returnListSize = objectPtr->InputQueueCharacters.size();
	return objectPtr->InputQueueCharacters;
}
__declspec(dllexport) void __stdcall ImGuiIO_Set_InputQueueCharacters2146(ImGuiIO* objectPtr, ImVector<ImWchar>  value)
{
	objectPtr->InputQueueCharacters = value;
}
__declspec(dllexport) ImGuiIO* __stdcall ImGuiIO_ImGuiIO2148()
{
	return IM_NEW(ImGuiIO)();
}
__declspec(dllexport) ImGuiInputTextFlags __stdcall ImGuiInputTextCallbackData_Get_EventFlag2167(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->EventFlag;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_EventFlag2167(ImGuiInputTextCallbackData* objectPtr, ImGuiInputTextFlags  value)
{
	objectPtr->EventFlag = value;
}
__declspec(dllexport) ImGuiInputTextFlags __stdcall ImGuiInputTextCallbackData_Get_Flags2168(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->Flags;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_Flags2168(ImGuiInputTextCallbackData* objectPtr, ImGuiInputTextFlags  value)
{
	objectPtr->Flags = value;
}
__declspec(dllexport) void* __stdcall ImGuiInputTextCallbackData_Get_UserData2169(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->UserData;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_UserData2169(ImGuiInputTextCallbackData* objectPtr, void*  value)
{
	objectPtr->UserData = value;
}
__declspec(dllexport) ImWchar __stdcall ImGuiInputTextCallbackData_Get_EventChar2174(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->EventChar;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_EventChar2174(ImGuiInputTextCallbackData* objectPtr, ImWchar  value)
{
	objectPtr->EventChar = value;
}
__declspec(dllexport) ImGuiKey __stdcall ImGuiInputTextCallbackData_Get_EventKey2175(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->EventKey;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_EventKey2175(ImGuiInputTextCallbackData* objectPtr, ImGuiKey  value)
{
	objectPtr->EventKey = value;
}
__declspec(dllexport) char* __stdcall ImGuiInputTextCallbackData_Get_Buf2176(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->Buf;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_Buf2176(ImGuiInputTextCallbackData* objectPtr, char*  value)
{
	objectPtr->Buf = value;
}
__declspec(dllexport) int __stdcall ImGuiInputTextCallbackData_Get_BufTextLen2177(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->BufTextLen;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_BufTextLen2177(ImGuiInputTextCallbackData* objectPtr, int  value)
{
	objectPtr->BufTextLen = value;
}
__declspec(dllexport) int __stdcall ImGuiInputTextCallbackData_Get_BufSize2178(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->BufSize;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_BufSize2178(ImGuiInputTextCallbackData* objectPtr, int  value)
{
	objectPtr->BufSize = value;
}
__declspec(dllexport) bool __stdcall ImGuiInputTextCallbackData_Get_BufDirty2179(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->BufDirty;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_BufDirty2179(ImGuiInputTextCallbackData* objectPtr, bool  value)
{
	objectPtr->BufDirty = value;
}
__declspec(dllexport) int __stdcall ImGuiInputTextCallbackData_Get_CursorPos2180(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->CursorPos;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_CursorPos2180(ImGuiInputTextCallbackData* objectPtr, int  value)
{
	objectPtr->CursorPos = value;
}
__declspec(dllexport) int __stdcall ImGuiInputTextCallbackData_Get_SelectionStart2181(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->SelectionStart;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_SelectionStart2181(ImGuiInputTextCallbackData* objectPtr, int  value)
{
	objectPtr->SelectionStart = value;
}
__declspec(dllexport) int __stdcall ImGuiInputTextCallbackData_Get_SelectionEnd2182(ImGuiInputTextCallbackData* objectPtr)
{
	return objectPtr->SelectionEnd;
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_Set_SelectionEnd2182(ImGuiInputTextCallbackData* objectPtr, int  value)
{
	objectPtr->SelectionEnd = value;
}
__declspec(dllexport) ImGuiInputTextCallbackData* __stdcall ImGuiInputTextCallbackData_ImGuiInputTextCallbackData2186()
{
	return IM_NEW(ImGuiInputTextCallbackData)();
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_DeleteChars2187(ImGuiInputTextCallbackData* objectPtr, int pos, int bytes_count)
{
	objectPtr->DeleteChars(pos, bytes_count);
}
__declspec(dllexport) void __stdcall ImGuiInputTextCallbackData_InsertChars2188(ImGuiInputTextCallbackData* objectPtr, int pos, const char* text, const char* text_end)
{
	objectPtr->InsertChars(pos, text, text_end);
}
__declspec(dllexport) void* __stdcall ImGuiSizeCallbackData_Get_UserData2198(ImGuiSizeCallbackData* objectPtr)
{
	return objectPtr->UserData;
}
__declspec(dllexport) void __stdcall ImGuiSizeCallbackData_Set_UserData2198(ImGuiSizeCallbackData* objectPtr, void*  value)
{
	objectPtr->UserData = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiSizeCallbackData_Get_Pos2199(ImGuiSizeCallbackData* objectPtr)
{
	return objectPtr->Pos;
}
__declspec(dllexport) void __stdcall ImGuiSizeCallbackData_Set_Pos2199(ImGuiSizeCallbackData* objectPtr, ImVec2&  value)
{
	objectPtr->Pos = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiSizeCallbackData_Get_CurrentSize2200(ImGuiSizeCallbackData* objectPtr)
{
	return objectPtr->CurrentSize;
}
__declspec(dllexport) void __stdcall ImGuiSizeCallbackData_Set_CurrentSize2200(ImGuiSizeCallbackData* objectPtr, ImVec2&  value)
{
	objectPtr->CurrentSize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiSizeCallbackData_Get_DesiredSize2201(ImGuiSizeCallbackData* objectPtr)
{
	return objectPtr->DesiredSize;
}
__declspec(dllexport) void __stdcall ImGuiSizeCallbackData_Set_DesiredSize2201(ImGuiSizeCallbackData* objectPtr, ImVec2&  value)
{
	objectPtr->DesiredSize = value;
}
__declspec(dllexport) void* __stdcall ImGuiPayload_Get_Data2208(ImGuiPayload* objectPtr)
{
	return objectPtr->Data;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_Data2208(ImGuiPayload* objectPtr, void*  value)
{
	objectPtr->Data = value;
}
__declspec(dllexport) int __stdcall ImGuiPayload_Get_DataSize2209(ImGuiPayload* objectPtr)
{
	return objectPtr->DataSize;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_DataSize2209(ImGuiPayload* objectPtr, int  value)
{
	objectPtr->DataSize = value;
}
__declspec(dllexport) ImGuiID __stdcall ImGuiPayload_Get_SourceId2212(ImGuiPayload* objectPtr)
{
	return objectPtr->SourceId;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_SourceId2212(ImGuiPayload* objectPtr, ImGuiID  value)
{
	objectPtr->SourceId = value;
}
__declspec(dllexport) ImGuiID __stdcall ImGuiPayload_Get_SourceParentId2213(ImGuiPayload* objectPtr)
{
	return objectPtr->SourceParentId;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_SourceParentId2213(ImGuiPayload* objectPtr, ImGuiID  value)
{
	objectPtr->SourceParentId = value;
}
__declspec(dllexport) int __stdcall ImGuiPayload_Get_DataFrameCount2214(ImGuiPayload* objectPtr)
{
	return objectPtr->DataFrameCount;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_DataFrameCount2214(ImGuiPayload* objectPtr, int  value)
{
	objectPtr->DataFrameCount = value;
}
__declspec(dllexport) char* __stdcall ImGuiPayload_Get_DataType2215(ImGuiPayload* objectPtr)
{
	return objectPtr->DataType;
}
__declspec(dllexport) bool __stdcall ImGuiPayload_Get_Preview2216(ImGuiPayload* objectPtr)
{
	return objectPtr->Preview;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_Preview2216(ImGuiPayload* objectPtr, bool  value)
{
	objectPtr->Preview = value;
}
__declspec(dllexport) bool __stdcall ImGuiPayload_Get_Delivery2217(ImGuiPayload* objectPtr)
{
	return objectPtr->Delivery;
}
__declspec(dllexport) void __stdcall ImGuiPayload_Set_Delivery2217(ImGuiPayload* objectPtr, bool  value)
{
	objectPtr->Delivery = value;
}
__declspec(dllexport) ImGuiID __stdcall ImGuiTableColumnSortSpecs_Get_ColumnUserID2229(ImGuiTableColumnSortSpecs* objectPtr)
{
	return objectPtr->ColumnUserID;
}
__declspec(dllexport) void __stdcall ImGuiTableColumnSortSpecs_Set_ColumnUserID2229(ImGuiTableColumnSortSpecs* objectPtr, ImGuiID  value)
{
	objectPtr->ColumnUserID = value;
}
__declspec(dllexport) ImS16 __stdcall ImGuiTableColumnSortSpecs_Get_ColumnIndex2230(ImGuiTableColumnSortSpecs* objectPtr)
{
	return objectPtr->ColumnIndex;
}
__declspec(dllexport) void __stdcall ImGuiTableColumnSortSpecs_Set_ColumnIndex2230(ImGuiTableColumnSortSpecs* objectPtr, ImS16  value)
{
	objectPtr->ColumnIndex = value;
}
__declspec(dllexport) ImS16 __stdcall ImGuiTableColumnSortSpecs_Get_SortOrder2231(ImGuiTableColumnSortSpecs* objectPtr)
{
	return objectPtr->SortOrder;
}
__declspec(dllexport) void __stdcall ImGuiTableColumnSortSpecs_Set_SortOrder2231(ImGuiTableColumnSortSpecs* objectPtr, ImS16  value)
{
	objectPtr->SortOrder = value;
}
__declspec(dllexport) ImGuiSortDirection __stdcall ImGuiTableColumnSortSpecs_Get_SortDirection2232(ImGuiTableColumnSortSpecs* objectPtr)
{
	return objectPtr->SortDirection;
}
__declspec(dllexport) void __stdcall ImGuiTableColumnSortSpecs_Set_SortDirection2232(ImGuiTableColumnSortSpecs* objectPtr, ImGuiSortDirection  value)
{
	objectPtr->SortDirection = value;
}
__declspec(dllexport) const ImGuiTableColumnSortSpecs* __stdcall ImGuiTableSortSpecs_Get_Specs2243(ImGuiTableSortSpecs* objectPtr)
{
	return objectPtr->Specs;
}
__declspec(dllexport) void __stdcall ImGuiTableSortSpecs_Set_Specs2243(ImGuiTableSortSpecs* objectPtr, const ImGuiTableColumnSortSpecs*  value)
{
	objectPtr->Specs = value;
}
__declspec(dllexport) int __stdcall ImGuiTableSortSpecs_Get_SpecsCount2244(ImGuiTableSortSpecs* objectPtr)
{
	return objectPtr->SpecsCount;
}
__declspec(dllexport) void __stdcall ImGuiTableSortSpecs_Set_SpecsCount2244(ImGuiTableSortSpecs* objectPtr, int  value)
{
	objectPtr->SpecsCount = value;
}
__declspec(dllexport) bool __stdcall ImGuiTableSortSpecs_Get_SpecsDirty2245(ImGuiTableSortSpecs* objectPtr)
{
	return objectPtr->SpecsDirty;
}
__declspec(dllexport) void __stdcall ImGuiTableSortSpecs_Set_SpecsDirty2245(ImGuiTableSortSpecs* objectPtr, bool  value)
{
	objectPtr->SpecsDirty = value;
}
__declspec(dllexport) void __stdcall ImGuiOnceUponAFrame_Set_RefFrame2267(ImGuiOnceUponAFrame* objectPtr, mutable int  value)
{
	objectPtr->RefFrame = value;
}
__declspec(dllexport) ImGuiTextFilter* __stdcall ImGuiTextFilter_ImGuiTextFilter2274(const char* default_filter)
{
	return IM_NEW(ImGuiTextFilter)(default_filter);
}
__declspec(dllexport) bool __stdcall ImGuiTextFilter_Draw2275(ImGuiTextFilter* objectPtr, const char* label, float width)
{
	return objectPtr->Draw(label, width);
}
__declspec(dllexport) bool __stdcall ImGuiTextFilter_PassFilter2276(ImGuiTextFilter* objectPtr, const char* text, const char* text_end)
{
	return objectPtr->PassFilter(text, text_end);
}
__declspec(dllexport) void __stdcall ImGuiTextFilter_Build2277(ImGuiTextFilter* objectPtr)
{
	objectPtr->Build();
}
__declspec(dllexport) int __stdcall ImGuiTextFilter_Get_CountGrep2294(ImGuiTextFilter* objectPtr)
{
	return objectPtr->CountGrep;
}
__declspec(dllexport) void __stdcall ImGuiTextFilter_Set_CountGrep2294(ImGuiTextFilter* objectPtr, int  value)
{
	objectPtr->CountGrep = value;
}
__declspec(dllexport) ImVector<char> __stdcall ImGuiTextBuffer_Get_Buf2301(int* returnListSize, ImGuiTextBuffer* objectPtr)
{
	*returnListSize = objectPtr->Buf.size();
	return objectPtr->Buf;
}
__declspec(dllexport) void __stdcall ImGuiTextBuffer_Set_Buf2301(ImGuiTextBuffer* objectPtr, ImVector<char>  value)
{
	objectPtr->Buf = value;
}
__declspec(dllexport) char* __stdcall ImGuiTextBuffer_Get_EmptyString2302(ImGuiTextBuffer* objectPtr)
{
	return objectPtr->EmptyString;
}
__declspec(dllexport) void __stdcall ImGuiTextBuffer_append2313(ImGuiTextBuffer* objectPtr, const char* str, const char* str_end)
{
	objectPtr->append(str, str_end);
}
__declspec(dllexport) int __stdcall ImGuiStorage_GetInt2344(ImGuiStorage* objectPtr, ImGuiID key, int default_val)
{
	return objectPtr->GetInt(key, default_val);
}
__declspec(dllexport) void __stdcall ImGuiStorage_SetInt2345(ImGuiStorage* objectPtr, ImGuiID key, int val)
{
	objectPtr->SetInt(key, val);
}
__declspec(dllexport) bool __stdcall ImGuiStorage_GetBool2346(ImGuiStorage* objectPtr, ImGuiID key, bool default_val)
{
	return objectPtr->GetBool(key, default_val);
}
__declspec(dllexport) void __stdcall ImGuiStorage_SetBool2347(ImGuiStorage* objectPtr, ImGuiID key, bool val)
{
	objectPtr->SetBool(key, val);
}
__declspec(dllexport) float __stdcall ImGuiStorage_GetFloat2348(ImGuiStorage* objectPtr, ImGuiID key, float default_val)
{
	return objectPtr->GetFloat(key, default_val);
}
__declspec(dllexport) void __stdcall ImGuiStorage_SetFloat2349(ImGuiStorage* objectPtr, ImGuiID key, float val)
{
	objectPtr->SetFloat(key, val);
}
__declspec(dllexport) void* __stdcall ImGuiStorage_GetVoidPtr2350(ImGuiStorage* objectPtr, ImGuiID key)
{
	return objectPtr->GetVoidPtr(key);
}
__declspec(dllexport) void __stdcall ImGuiStorage_SetVoidPtr2351(ImGuiStorage* objectPtr, ImGuiID key, void* val)
{
	objectPtr->SetVoidPtr(key, val);
}
__declspec(dllexport) int* __stdcall ImGuiStorage_GetIntRef2357(ImGuiStorage* objectPtr, ImGuiID key, int default_val)
{
	return objectPtr->GetIntRef(key, default_val);
}
__declspec(dllexport) bool* __stdcall ImGuiStorage_GetBoolRef2358(ImGuiStorage* objectPtr, ImGuiID key, bool default_val)
{
	return objectPtr->GetBoolRef(key, default_val);
}
__declspec(dllexport) float* __stdcall ImGuiStorage_GetFloatRef2359(ImGuiStorage* objectPtr, ImGuiID key, float default_val)
{
	return objectPtr->GetFloatRef(key, default_val);
}
__declspec(dllexport) void** __stdcall ImGuiStorage_GetVoidPtrRef2360(ImGuiStorage* objectPtr, ImGuiID key, void* default_val)
{
	return objectPtr->GetVoidPtrRef(key, default_val);
}
__declspec(dllexport) void __stdcall ImGuiStorage_SetAllInt2363(ImGuiStorage* objectPtr, int val)
{
	objectPtr->SetAllInt(val);
}
__declspec(dllexport) void __stdcall ImGuiStorage_BuildSortByKey2366(ImGuiStorage* objectPtr)
{
	objectPtr->BuildSortByKey();
}
__declspec(dllexport) int __stdcall ImGuiListClipper_Get_DisplayStart2392(ImGuiListClipper* objectPtr)
{
	return objectPtr->DisplayStart;
}
__declspec(dllexport) void __stdcall ImGuiListClipper_Set_DisplayStart2392(ImGuiListClipper* objectPtr, int  value)
{
	objectPtr->DisplayStart = value;
}
__declspec(dllexport) int __stdcall ImGuiListClipper_Get_DisplayEnd2393(ImGuiListClipper* objectPtr)
{
	return objectPtr->DisplayEnd;
}
__declspec(dllexport) void __stdcall ImGuiListClipper_Set_DisplayEnd2393(ImGuiListClipper* objectPtr, int  value)
{
	objectPtr->DisplayEnd = value;
}
__declspec(dllexport) ImGuiListClipper* __stdcall ImGuiListClipper_ImGuiListClipper2401()
{
	return IM_NEW(ImGuiListClipper)();
}
__declspec(dllexport) void __stdcall ImGuiListClipper_DeleteImGuiListClipper2402(ImGuiListClipper* objectPtr)
{
	return IM_DELETE(objectPtr);
}
__declspec(dllexport) void __stdcall ImGuiListClipper_Begin2403(ImGuiListClipper* objectPtr, int items_count, float items_height)
{
	objectPtr->Begin(items_count, items_height);
}
__declspec(dllexport) void __stdcall ImGuiListClipper_End2404(ImGuiListClipper* objectPtr)
{
	objectPtr->End();
}
__declspec(dllexport) bool __stdcall ImGuiListClipper_Step2405(ImGuiListClipper* objectPtr)
{
	return objectPtr->Step();
}
__declspec(dllexport) void __stdcall ImGuiListClipper_IncludeItemsByIndex2410(ImGuiListClipper* objectPtr, int item_begin, int item_end)
{
	objectPtr->IncludeItemsByIndex(item_begin, item_end);
}
__declspec(dllexport) ImVec4& __stdcall ImColor_Get_Value2473(ImColor* objectPtr)
{
	return objectPtr->Value;
}
__declspec(dllexport) void __stdcall ImColor_Set_Value2473(ImColor* objectPtr, ImVec4&  value)
{
	objectPtr->Value = value;
}
__declspec(dllexport) ImVec4& __stdcall ImDrawCmd_Get_ClipRect2522(ImDrawCmd* objectPtr)
{
	return objectPtr->ClipRect;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_ClipRect2522(ImDrawCmd* objectPtr, ImVec4&  value)
{
	objectPtr->ClipRect = value;
}
__declspec(dllexport) ImTextureID __stdcall ImDrawCmd_Get_TextureId2523(ImDrawCmd* objectPtr)
{
	return objectPtr->TextureId;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_TextureId2523(ImDrawCmd* objectPtr, ImTextureID  value)
{
	objectPtr->TextureId = value;
}
__declspec(dllexport) unsigned int __stdcall ImDrawCmd_Get_VtxOffset2524(ImDrawCmd* objectPtr)
{
	return objectPtr->VtxOffset;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_VtxOffset2524(ImDrawCmd* objectPtr, unsigned int  value)
{
	objectPtr->VtxOffset = value;
}
__declspec(dllexport) unsigned int __stdcall ImDrawCmd_Get_IdxOffset2525(ImDrawCmd* objectPtr)
{
	return objectPtr->IdxOffset;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_IdxOffset2525(ImDrawCmd* objectPtr, unsigned int  value)
{
	objectPtr->IdxOffset = value;
}
__declspec(dllexport) unsigned int __stdcall ImDrawCmd_Get_ElemCount2526(ImDrawCmd* objectPtr)
{
	return objectPtr->ElemCount;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_ElemCount2526(ImDrawCmd* objectPtr, unsigned int  value)
{
	objectPtr->ElemCount = value;
}
__declspec(dllexport) ImDrawCallback __stdcall ImDrawCmd_Get_UserCallback2527(ImDrawCmd* objectPtr)
{
	return objectPtr->UserCallback;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_UserCallback2527(ImDrawCmd* objectPtr, ImDrawCallback  value)
{
	objectPtr->UserCallback = value;
}
__declspec(dllexport) void* __stdcall ImDrawCmd_Get_UserCallbackData2528(ImDrawCmd* objectPtr)
{
	return objectPtr->UserCallbackData;
}
__declspec(dllexport) void __stdcall ImDrawCmd_Set_UserCallbackData2528(ImDrawCmd* objectPtr, void*  value)
{
	objectPtr->UserCallbackData = value;
}
__declspec(dllexport) ImVec2& __stdcall ImDrawVert_Get_pos2540(ImDrawVert* objectPtr)
{
	return objectPtr->pos;
}
__declspec(dllexport) void __stdcall ImDrawVert_Set_pos2540(ImDrawVert* objectPtr, ImVec2&  value)
{
	objectPtr->pos = value;
}
__declspec(dllexport) ImVec2& __stdcall ImDrawVert_Get_uv2541(ImDrawVert* objectPtr)
{
	return objectPtr->uv;
}
__declspec(dllexport) void __stdcall ImDrawVert_Set_uv2541(ImDrawVert* objectPtr, ImVec2&  value)
{
	objectPtr->uv = value;
}
__declspec(dllexport) ImU32 __stdcall ImDrawVert_Get_col2542(ImDrawVert* objectPtr)
{
	return objectPtr->col;
}
__declspec(dllexport) void __stdcall ImDrawVert_Set_col2542(ImDrawVert* objectPtr, ImU32  value)
{
	objectPtr->col = value;
}
__declspec(dllexport) ImVec4& __stdcall ImDrawCmdHeader_Get_ClipRect2555(ImDrawCmdHeader* objectPtr)
{
	return objectPtr->ClipRect;
}
__declspec(dllexport) void __stdcall ImDrawCmdHeader_Set_ClipRect2555(ImDrawCmdHeader* objectPtr, ImVec4&  value)
{
	objectPtr->ClipRect = value;
}
__declspec(dllexport) ImTextureID __stdcall ImDrawCmdHeader_Get_TextureId2556(ImDrawCmdHeader* objectPtr)
{
	return objectPtr->TextureId;
}
__declspec(dllexport) void __stdcall ImDrawCmdHeader_Set_TextureId2556(ImDrawCmdHeader* objectPtr, ImTextureID  value)
{
	objectPtr->TextureId = value;
}
__declspec(dllexport) unsigned int __stdcall ImDrawCmdHeader_Get_VtxOffset2557(ImDrawCmdHeader* objectPtr)
{
	return objectPtr->VtxOffset;
}
__declspec(dllexport) void __stdcall ImDrawCmdHeader_Set_VtxOffset2557(ImDrawCmdHeader* objectPtr, unsigned int  value)
{
	objectPtr->VtxOffset = value;
}
__declspec(dllexport) ImVector<ImDrawCmd> __stdcall ImDrawChannel_Get__CmdBuffer2563(int* returnListSize, ImDrawChannel* objectPtr)
{
	*returnListSize = objectPtr->_CmdBuffer.size();
	return objectPtr->_CmdBuffer;
}
__declspec(dllexport) void __stdcall ImDrawChannel_Set__CmdBuffer2563(ImDrawChannel* objectPtr, ImVector<ImDrawCmd>  value)
{
	objectPtr->_CmdBuffer = value;
}
__declspec(dllexport) ImVector<ImDrawIdx> __stdcall ImDrawChannel_Get__IdxBuffer2564(int* returnListSize, ImDrawChannel* objectPtr)
{
	*returnListSize = objectPtr->_IdxBuffer.size();
	return objectPtr->_IdxBuffer;
}
__declspec(dllexport) void __stdcall ImDrawChannel_Set__IdxBuffer2564(ImDrawChannel* objectPtr, ImVector<ImDrawIdx>  value)
{
	objectPtr->_IdxBuffer = value;
}
__declspec(dllexport) int __stdcall ImDrawListSplitter_Get__Current2572(ImDrawListSplitter* objectPtr)
{
	return objectPtr->_Current;
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_Set__Current2572(ImDrawListSplitter* objectPtr, int  value)
{
	objectPtr->_Current = value;
}
__declspec(dllexport) int __stdcall ImDrawListSplitter_Get__Count2573(ImDrawListSplitter* objectPtr)
{
	return objectPtr->_Count;
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_Set__Count2573(ImDrawListSplitter* objectPtr, int  value)
{
	objectPtr->_Count = value;
}
__declspec(dllexport) ImVector<ImDrawChannel> __stdcall ImDrawListSplitter_Get__Channels2574(int* returnListSize, ImDrawListSplitter* objectPtr)
{
	*returnListSize = objectPtr->_Channels.size();
	return objectPtr->_Channels;
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_Set__Channels2574(ImDrawListSplitter* objectPtr, ImVector<ImDrawChannel>  value)
{
	objectPtr->_Channels = value;
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_ClearFreeMemory2579(ImDrawListSplitter* objectPtr)
{
	objectPtr->ClearFreeMemory();
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_Split2580(ImDrawListSplitter* objectPtr, ImDrawList* draw_list, int count)
{
	objectPtr->Split(draw_list, count);
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_Merge2581(ImDrawListSplitter* objectPtr, ImDrawList* draw_list)
{
	objectPtr->Merge(draw_list);
}
__declspec(dllexport) void __stdcall ImDrawListSplitter_SetCurrentChannel2582(ImDrawListSplitter* objectPtr, ImDrawList* draw_list, int channel_idx)
{
	objectPtr->SetCurrentChannel(draw_list, channel_idx);
}
__declspec(dllexport) ImVector<ImDrawCmd> __stdcall ImDrawList_Get_CmdBuffer2628(int* returnListSize, ImDrawList* objectPtr)
{
	*returnListSize = objectPtr->CmdBuffer.size();
	return objectPtr->CmdBuffer;
}
__declspec(dllexport) void __stdcall ImDrawList_Set_CmdBuffer2628(ImDrawList* objectPtr, ImVector<ImDrawCmd>  value)
{
	objectPtr->CmdBuffer = value;
}
__declspec(dllexport) ImVector<ImDrawIdx> __stdcall ImDrawList_Get_IdxBuffer2629(int* returnListSize, ImDrawList* objectPtr)
{
	*returnListSize = objectPtr->IdxBuffer.size();
	return objectPtr->IdxBuffer;
}
__declspec(dllexport) void __stdcall ImDrawList_Set_IdxBuffer2629(ImDrawList* objectPtr, ImVector<ImDrawIdx>  value)
{
	objectPtr->IdxBuffer = value;
}
__declspec(dllexport) ImVector<ImDrawVert> __stdcall ImDrawList_Get_VtxBuffer2630(int* returnListSize, ImDrawList* objectPtr)
{
	*returnListSize = objectPtr->VtxBuffer.size();
	return objectPtr->VtxBuffer;
}
__declspec(dllexport) void __stdcall ImDrawList_Set_VtxBuffer2630(ImDrawList* objectPtr, ImVector<ImDrawVert>  value)
{
	objectPtr->VtxBuffer = value;
}
__declspec(dllexport) ImDrawListFlags __stdcall ImDrawList_Get_Flags2631(ImDrawList* objectPtr)
{
	return objectPtr->Flags;
}
__declspec(dllexport) void __stdcall ImDrawList_Set_Flags2631(ImDrawList* objectPtr, ImDrawListFlags  value)
{
	objectPtr->Flags = value;
}
__declspec(dllexport) ImDrawListSharedData* __stdcall ImDrawList_Get__Data2635(ImDrawList* objectPtr)
{
	return objectPtr->_Data;
}
__declspec(dllexport) void __stdcall ImDrawList_Set__Data2635(ImDrawList* objectPtr, ImDrawListSharedData*  value)
{
	objectPtr->_Data = value;
}
__declspec(dllexport) const char* __stdcall ImDrawList_Get__OwnerName2636(ImDrawList* objectPtr)
{
	return objectPtr->_OwnerName;
}
__declspec(dllexport) void __stdcall ImDrawList_Set__OwnerName2636(ImDrawList* objectPtr, const char*  value)
{
	objectPtr->_OwnerName = value;
}
__declspec(dllexport) void __stdcall ImDrawList_PushClipRect2650(ImDrawList* objectPtr, const ImVec2& clip_rect_min, const ImVec2& clip_rect_max, bool intersect_with_current_clip_rect)
{
	objectPtr->PushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
}
__declspec(dllexport) void __stdcall ImDrawList_PushClipRectFullScreen2651(ImDrawList* objectPtr)
{
	objectPtr->PushClipRectFullScreen();
}
__declspec(dllexport) void __stdcall ImDrawList_PopClipRect2652(ImDrawList* objectPtr)
{
	objectPtr->PopClipRect();
}
__declspec(dllexport) void __stdcall ImDrawList_PushTextureID2653(ImDrawList* objectPtr, ImTextureID texture_id)
{
	objectPtr->PushTextureID(texture_id);
}
__declspec(dllexport) void __stdcall ImDrawList_PopTextureID2654(ImDrawList* objectPtr)
{
	objectPtr->PopTextureID();
}
__declspec(dllexport) void __stdcall ImDrawList_AddLine2665(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, ImU32 col, float thickness)
{
	objectPtr->AddLine(p1, p2, col, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddRect2666(ImDrawList* objectPtr, const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding, ImDrawFlags flags, float thickness)
{
	objectPtr->AddRect(p_min, p_max, col, rounding, flags, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddRectFilled2667(ImDrawList* objectPtr, const ImVec2& p_min, const ImVec2& p_max, ImU32 col, float rounding, ImDrawFlags flags)
{
	objectPtr->AddRectFilled(p_min, p_max, col, rounding, flags);
}
__declspec(dllexport) void __stdcall ImDrawList_AddRectFilledMultiColor2668(ImDrawList* objectPtr, const ImVec2& p_min, const ImVec2& p_max, ImU32 col_upr_left, ImU32 col_upr_right, ImU32 col_bot_right, ImU32 col_bot_left)
{
	objectPtr->AddRectFilledMultiColor(p_min, p_max, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
}
__declspec(dllexport) void __stdcall ImDrawList_AddQuad2669(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness)
{
	objectPtr->AddQuad(p1, p2, p3, p4, col, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddQuadFilled2670(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col)
{
	objectPtr->AddQuadFilled(p1, p2, p3, p4, col);
}
__declspec(dllexport) void __stdcall ImDrawList_AddTriangle2671(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness)
{
	objectPtr->AddTriangle(p1, p2, p3, col, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddTriangleFilled2672(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col)
{
	objectPtr->AddTriangleFilled(p1, p2, p3, col);
}
__declspec(dllexport) void __stdcall ImDrawList_AddCircle2673(ImDrawList* objectPtr, const ImVec2& center, float radius, ImU32 col, int num_segments, float thickness)
{
	objectPtr->AddCircle(center, radius, col, num_segments, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddCircleFilled2674(ImDrawList* objectPtr, const ImVec2& center, float radius, ImU32 col, int num_segments)
{
	objectPtr->AddCircleFilled(center, radius, col, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_AddNgon2675(ImDrawList* objectPtr, const ImVec2& center, float radius, ImU32 col, int num_segments, float thickness)
{
	objectPtr->AddNgon(center, radius, col, num_segments, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddNgonFilled2676(ImDrawList* objectPtr, const ImVec2& center, float radius, ImU32 col, int num_segments)
{
	objectPtr->AddNgonFilled(center, radius, col, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_AddText2677(ImDrawList* objectPtr, const ImVec2& pos, ImU32 col, const char* text_begin, const char* text_end)
{
	objectPtr->AddText(pos, col, text_begin, text_end);
}
__declspec(dllexport) void __stdcall ImDrawList_AddPolyline2679(ImDrawList* objectPtr, const ImVec2* points, int num_points, ImU32 col, ImDrawFlags flags, float thickness)
{
	objectPtr->AddPolyline(points, num_points, col, flags, thickness);
}
__declspec(dllexport) void __stdcall ImDrawList_AddConvexPolyFilled2680(ImDrawList* objectPtr, const ImVec2* points, int num_points, ImU32 col)
{
	objectPtr->AddConvexPolyFilled(points, num_points, col);
}
__declspec(dllexport) void __stdcall ImDrawList_AddBezierCubic2681(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, ImU32 col, float thickness, int num_segments)
{
	objectPtr->AddBezierCubic(p1, p2, p3, p4, col, thickness, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_AddBezierQuadratic2682(ImDrawList* objectPtr, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, ImU32 col, float thickness, int num_segments)
{
	objectPtr->AddBezierQuadratic(p1, p2, p3, col, thickness, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_AddImage2688(ImDrawList* objectPtr, ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min, const ImVec2& uv_max, ImU32 col)
{
	objectPtr->AddImage(user_texture_id, p_min, p_max, uv_min, uv_max, col);
}
__declspec(dllexport) void __stdcall ImDrawList_AddImageQuad2689(ImDrawList* objectPtr, ImTextureID user_texture_id, const ImVec2& p1, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, const ImVec2& uv1, const ImVec2& uv2, const ImVec2& uv3, const ImVec2& uv4, ImU32 col)
{
	objectPtr->AddImageQuad(user_texture_id, p1, p2, p3, p4, uv1, uv2, uv3, uv4, col);
}
__declspec(dllexport) void __stdcall ImDrawList_AddImageRounded2690(ImDrawList* objectPtr, ImTextureID user_texture_id, const ImVec2& p_min, const ImVec2& p_max, const ImVec2& uv_min, const ImVec2& uv_max, ImU32 col, float rounding, ImDrawFlags flags)
{
	objectPtr->AddImageRounded(user_texture_id, p_min, p_max, uv_min, uv_max, col, rounding, flags);
}
__declspec(dllexport) void __stdcall ImDrawList_PathArcTo2699(ImDrawList* objectPtr, const ImVec2& center, float radius, float a_min, float a_max, int num_segments)
{
	objectPtr->PathArcTo(center, radius, a_min, a_max, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_PathArcToFast2700(ImDrawList* objectPtr, const ImVec2& center, float radius, int a_min_of_12, int a_max_of_12)
{
	objectPtr->PathArcToFast(center, radius, a_min_of_12, a_max_of_12);
}
__declspec(dllexport) void __stdcall ImDrawList_PathBezierCubicCurveTo2701(ImDrawList* objectPtr, const ImVec2& p2, const ImVec2& p3, const ImVec2& p4, int num_segments)
{
	objectPtr->PathBezierCubicCurveTo(p2, p3, p4, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_PathBezierQuadraticCurveTo2702(ImDrawList* objectPtr, const ImVec2& p2, const ImVec2& p3, int num_segments)
{
	objectPtr->PathBezierQuadraticCurveTo(p2, p3, num_segments);
}
__declspec(dllexport) void __stdcall ImDrawList_PathRect2703(ImDrawList* objectPtr, const ImVec2& rect_min, const ImVec2& rect_max, float rounding, ImDrawFlags flags)
{
	objectPtr->PathRect(rect_min, rect_max, rounding, flags);
}
__declspec(dllexport) void __stdcall ImDrawList_AddCallback2706(ImDrawList* objectPtr, ImDrawCallback callback, void* callback_data)
{
	objectPtr->AddCallback(callback, callback_data);
}
__declspec(dllexport) void __stdcall ImDrawList_AddDrawCmd2707(ImDrawList* objectPtr)
{
	objectPtr->AddDrawCmd();
}
__declspec(dllexport) ImDrawList* __stdcall ImDrawList_CloneOutput2708(ImDrawList* objectPtr)
{
	return objectPtr->CloneOutput();
}
__declspec(dllexport) void __stdcall ImDrawList_PrimReserve2723(ImDrawList* objectPtr, int idx_count, int vtx_count)
{
	objectPtr->PrimReserve(idx_count, vtx_count);
}
__declspec(dllexport) void __stdcall ImDrawList_PrimUnreserve2724(ImDrawList* objectPtr, int idx_count, int vtx_count)
{
	objectPtr->PrimUnreserve(idx_count, vtx_count);
}
__declspec(dllexport) void __stdcall ImDrawList_PrimRect2725(ImDrawList* objectPtr, const ImVec2& a, const ImVec2& b, ImU32 col)
{
	objectPtr->PrimRect(a, b, col);
}
__declspec(dllexport) void __stdcall ImDrawList_PrimRectUV2726(ImDrawList* objectPtr, const ImVec2& a, const ImVec2& b, const ImVec2& uv_a, const ImVec2& uv_b, ImU32 col)
{
	objectPtr->PrimRectUV(a, b, uv_a, uv_b, col);
}
__declspec(dllexport) void __stdcall ImDrawList_PrimQuadUV2727(ImDrawList* objectPtr, const ImVec2& a, const ImVec2& b, const ImVec2& c, const ImVec2& d, const ImVec2& uv_a, const ImVec2& uv_b, const ImVec2& uv_c, const ImVec2& uv_d, ImU32 col)
{
	objectPtr->PrimQuadUV(a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
}
__declspec(dllexport) void __stdcall ImDrawList__ResetForNewFrame2737(ImDrawList* objectPtr)
{
	objectPtr->_ResetForNewFrame();
}
__declspec(dllexport) void __stdcall ImDrawList__ClearFreeMemory2738(ImDrawList* objectPtr)
{
	objectPtr->_ClearFreeMemory();
}
__declspec(dllexport) void __stdcall ImDrawList__TryMergeDrawCmds2740(ImDrawList* objectPtr)
{
	objectPtr->_TryMergeDrawCmds();
}
__declspec(dllexport) void __stdcall ImDrawList__OnChangedClipRect2741(ImDrawList* objectPtr)
{
	objectPtr->_OnChangedClipRect();
}
__declspec(dllexport) void __stdcall ImDrawList__OnChangedTextureID2742(ImDrawList* objectPtr)
{
	objectPtr->_OnChangedTextureID();
}
__declspec(dllexport) void __stdcall ImDrawList__OnChangedVtxOffset2743(ImDrawList* objectPtr)
{
	objectPtr->_OnChangedVtxOffset();
}
__declspec(dllexport) int __stdcall ImDrawList__CalcCircleAutoSegmentCount2744(ImDrawList* objectPtr, float radius)
{
	return objectPtr->_CalcCircleAutoSegmentCount(radius);
}
__declspec(dllexport) void __stdcall ImDrawList__PathArcToFastEx2745(ImDrawList* objectPtr, const ImVec2& center, float radius, int a_min_sample, int a_max_sample, int a_step)
{
	objectPtr->_PathArcToFastEx(center, radius, a_min_sample, a_max_sample, a_step);
}
__declspec(dllexport) void __stdcall ImDrawList__PathArcToN2746(ImDrawList* objectPtr, const ImVec2& center, float radius, float a_min, float a_max, int num_segments)
{
	objectPtr->_PathArcToN(center, radius, a_min, a_max, num_segments);
}
__declspec(dllexport) bool __stdcall ImDrawData_Get_Valid2754(ImDrawData* objectPtr)
{
	return objectPtr->Valid;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_Valid2754(ImDrawData* objectPtr, bool  value)
{
	objectPtr->Valid = value;
}
__declspec(dllexport) int __stdcall ImDrawData_Get_CmdListsCount2755(ImDrawData* objectPtr)
{
	return objectPtr->CmdListsCount;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_CmdListsCount2755(ImDrawData* objectPtr, int  value)
{
	objectPtr->CmdListsCount = value;
}
__declspec(dllexport) int __stdcall ImDrawData_Get_TotalIdxCount2756(ImDrawData* objectPtr)
{
	return objectPtr->TotalIdxCount;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_TotalIdxCount2756(ImDrawData* objectPtr, int  value)
{
	objectPtr->TotalIdxCount = value;
}
__declspec(dllexport) int __stdcall ImDrawData_Get_TotalVtxCount2757(ImDrawData* objectPtr)
{
	return objectPtr->TotalVtxCount;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_TotalVtxCount2757(ImDrawData* objectPtr, int  value)
{
	objectPtr->TotalVtxCount = value;
}
__declspec(dllexport) ImVector<ImDrawList*> __stdcall ImDrawData_Get_CmdLists2758(ImDrawData* objectPtr)
{
	return objectPtr->CmdLists;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_CmdLists2758(ImDrawData* objectPtr, ImVector<ImDrawList*>  value)
{
	objectPtr->CmdLists = value;
}
__declspec(dllexport) ImVec2& __stdcall ImDrawData_Get_DisplayPos2759(ImDrawData* objectPtr)
{
	return objectPtr->DisplayPos;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_DisplayPos2759(ImDrawData* objectPtr, ImVec2&  value)
{
	objectPtr->DisplayPos = value;
}
__declspec(dllexport) ImVec2& __stdcall ImDrawData_Get_DisplaySize2760(ImDrawData* objectPtr)
{
	return objectPtr->DisplaySize;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_DisplaySize2760(ImDrawData* objectPtr, ImVec2&  value)
{
	objectPtr->DisplaySize = value;
}
__declspec(dllexport) ImVec2& __stdcall ImDrawData_Get_FramebufferScale2761(ImDrawData* objectPtr)
{
	return objectPtr->FramebufferScale;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_FramebufferScale2761(ImDrawData* objectPtr, ImVec2&  value)
{
	objectPtr->FramebufferScale = value;
}
__declspec(dllexport) ImGuiViewport* __stdcall ImDrawData_Get_OwnerViewport2762(ImDrawData* objectPtr)
{
	return objectPtr->OwnerViewport;
}
__declspec(dllexport) void __stdcall ImDrawData_Set_OwnerViewport2762(ImDrawData* objectPtr, ImGuiViewport*  value)
{
	objectPtr->OwnerViewport = value;
}
__declspec(dllexport) void __stdcall ImDrawData_Clear2766(ImDrawData* objectPtr)
{
	objectPtr->Clear();
}
__declspec(dllexport) void __stdcall ImDrawData_AddDrawList2767(ImDrawData* objectPtr, ImDrawList* draw_list)
{
	objectPtr->AddDrawList(draw_list);
}
__declspec(dllexport) void __stdcall ImDrawData_DeIndexAllBuffers2768(ImDrawData* objectPtr)
{
	objectPtr->DeIndexAllBuffers();
}
__declspec(dllexport) void __stdcall ImDrawData_ScaleClipRects2769(ImDrawData* objectPtr, const ImVec2& fb_scale)
{
	objectPtr->ScaleClipRects(fb_scale);
}
__declspec(dllexport) ImGuiViewportFlags __stdcall ImGuiViewport_Get_Flags3049(ImGuiViewport* objectPtr)
{
	return objectPtr->Flags;
}
__declspec(dllexport) void __stdcall ImGuiViewport_Set_Flags3049(ImGuiViewport* objectPtr, ImGuiViewportFlags  value)
{
	objectPtr->Flags = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiViewport_Get_Pos3050(ImGuiViewport* objectPtr)
{
	return objectPtr->Pos;
}
__declspec(dllexport) void __stdcall ImGuiViewport_Set_Pos3050(ImGuiViewport* objectPtr, ImVec2&  value)
{
	objectPtr->Pos = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiViewport_Get_Size3051(ImGuiViewport* objectPtr)
{
	return objectPtr->Size;
}
__declspec(dllexport) void __stdcall ImGuiViewport_Set_Size3051(ImGuiViewport* objectPtr, ImVec2&  value)
{
	objectPtr->Size = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiViewport_Get_WorkPos3052(ImGuiViewport* objectPtr)
{
	return objectPtr->WorkPos;
}
__declspec(dllexport) void __stdcall ImGuiViewport_Set_WorkPos3052(ImGuiViewport* objectPtr, ImVec2&  value)
{
	objectPtr->WorkPos = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiViewport_Get_WorkSize3053(ImGuiViewport* objectPtr)
{
	return objectPtr->WorkSize;
}
__declspec(dllexport) void __stdcall ImGuiViewport_Set_WorkSize3053(ImGuiViewport* objectPtr, ImVec2&  value)
{
	objectPtr->WorkSize = value;
}
__declspec(dllexport) void* __stdcall ImGuiViewport_Get_PlatformHandleRaw3056(ImGuiViewport* objectPtr)
{
	return objectPtr->PlatformHandleRaw;
}
__declspec(dllexport) void __stdcall ImGuiViewport_Set_PlatformHandleRaw3056(ImGuiViewport* objectPtr, void*  value)
{
	objectPtr->PlatformHandleRaw = value;
}
__declspec(dllexport) bool __stdcall ImGuiPlatformImeData_Get_WantVisible3072(ImGuiPlatformImeData* objectPtr)
{
	return objectPtr->WantVisible;
}
__declspec(dllexport) void __stdcall ImGuiPlatformImeData_Set_WantVisible3072(ImGuiPlatformImeData* objectPtr, bool  value)
{
	objectPtr->WantVisible = value;
}
__declspec(dllexport) ImVec2& __stdcall ImGuiPlatformImeData_Get_InputPos3073(ImGuiPlatformImeData* objectPtr)
{
	return objectPtr->InputPos;
}
__declspec(dllexport) void __stdcall ImGuiPlatformImeData_Set_InputPos3073(ImGuiPlatformImeData* objectPtr, ImVec2&  value)
{
	objectPtr->InputPos = value;
}
__declspec(dllexport) float __stdcall ImGuiPlatformImeData_Get_InputLineHeight3074(ImGuiPlatformImeData* objectPtr)
{
	return objectPtr->InputLineHeight;
}
__declspec(dllexport) void __stdcall ImGuiPlatformImeData_Set_InputLineHeight3074(ImGuiPlatformImeData* objectPtr, float  value)
{
	objectPtr->InputLineHeight = value;
}
__declspec(dllexport) ImGuiKey __stdcall ImGui_GetKeyIndex3088(ImGuiKey key)
{
	return ImGui::GetKeyIndex(key);
}
__declspec(dllexport) void __stdcall ImGui_SetItemAllowOverlap3098()
{
	ImGui::SetItemAllowOverlap();
}
__declspec(dllexport) bool __stdcall ImGui_ImageButton3103(ImTextureID user_texture_id, const ImVec2& size, const ImVec2& uv0, const ImVec2& uv1, int frame_padding, const ImVec4& bg_col, const ImVec4& tint_col)
{
	return ImGui::ImageButton(user_texture_id, size, uv0, uv1, frame_padding, bg_col, tint_col);
}
__declspec(dllexport) void __stdcall ImGui_CalcListClipping3108(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end)
{
	ImGui::CalcListClipping(items_count, items_height, out_items_display_start, out_items_display_end);
}
}