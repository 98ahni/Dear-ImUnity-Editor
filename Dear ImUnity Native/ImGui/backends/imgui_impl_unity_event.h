#pragma once
#include "../imgui.h"      // IMGUI_IMPL_API
#include "../imgui_internal.h"
enum class UnityKeyCode
{
    None = 0,
    Backspace = 8,
    Delete = 0x7F,
    Tab = 9,
    Clear = 12,
    Return = 13,
    Pause = 19,
    Escape = 27,
    Space = 0x20,
    Keypad0 = 0x100,
    Keypad1 = 257,
    Keypad2 = 258,
    Keypad3 = 259,
    Keypad4 = 260,
    Keypad5 = 261,
    Keypad6 = 262,
    Keypad7 = 263,
    Keypad8 = 264,
    Keypad9 = 265,
    KeypadPeriod = 266,
    KeypadDivide = 267,
    KeypadMultiply = 268,
    KeypadMinus = 269,
    KeypadPlus = 270,
    KeypadEnter = 271,
    KeypadEquals = 272,
    UpArrow = 273,
    DownArrow = 274,
    RightArrow = 275,
    LeftArrow = 276,
    Insert = 277,
    Home = 278,
    End = 279,
    PageUp = 280,
    PageDown = 281,
    F1 = 282,
    F2 = 283,
    F3 = 284,
    F4 = 285,
    F5 = 286,
    F6 = 287,
    F7 = 288,
    F8 = 289,
    F9 = 290,
    F10 = 291,
    F11 = 292,
    F12 = 293,
    F13 = 294,
    F14 = 295,
    F15 = 296,
    Alpha0 = 48,
    Alpha1 = 49,
    Alpha2 = 50,
    Alpha3 = 51,
    Alpha4 = 52,
    Alpha5 = 53,
    Alpha6 = 54,
    Alpha7 = 55,
    Alpha8 = 56,
    Alpha9 = 57,
    Exclaim = 33,
    DoubleQuote = 34,
    Hash = 35,
    Dollar = 36,
    Percent = 37,
    Ampersand = 38,
    Quote = 39,
    LeftParen = 40,
    RightParen = 41,
    Asterisk = 42,
    Plus = 43,
    Comma = 44,
    Minus = 45,
    Period = 46,
    Slash = 47,
    Colon = 58,
    Semicolon = 59,
    Less = 60,
    Equals = 61,
    Greater = 62,
    Question = 0x3F,
    At = 0x40,
    LeftBracket = 91,
    Backslash = 92,
    RightBracket = 93,
    Caret = 94,
    Underscore = 95,
    BackQuote = 96,
    A = 97,
    B = 98,
    C = 99,
    D = 100,
    E = 101,
    F = 102,
    G = 103,
    H = 104,
    I = 105,
    J = 106,
    K = 107,
    L = 108,
    M = 109,
    N = 110,
    O = 111,
    P = 112,
    Q = 113,
    R = 114,
    S = 115,
    T = 116,
    U = 117,
    V = 118,
    W = 119,
    X = 120,
    Y = 121,
    Z = 122,
    LeftCurlyBracket = 123,
    Pipe = 124,
    RightCurlyBracket = 125,
    Tilde = 126,
    Numlock = 300,
    CapsLock = 301,
    ScrollLock = 302,
    RightShift = 303,
    LeftShift = 304,
    RightControl = 305,
    LeftControl = 306,
    RightAlt = 307,
    LeftAlt = 308,
    LeftMeta = 310,
    LeftCommand = 310,
    LeftApple = 310,
    LeftWindows = 311,
    RightMeta = 309,
    RightCommand = 309,
    RightApple = 309,
    RightWindows = 312,
    AltGr = 313,
    Help = 315,
    Print = 316,
    SysReq = 317,
    Break = 318,
    Menu = 319,
    Mouse0 = 323,
    Mouse1 = 324,
    Mouse2 = 325,
    Mouse3 = 326,
    Mouse4 = 327,
    Mouse5 = 328,
    Mouse6 = 329,
    JoystickButton0 = 330,
    JoystickButton1 = 331,
    JoystickButton2 = 332,
    JoystickButton3 = 333,
    JoystickButton4 = 334,
    JoystickButton5 = 335,
    JoystickButton6 = 336,
    JoystickButton7 = 337,
    JoystickButton8 = 338,
    JoystickButton9 = 339,
    JoystickButton10 = 340,
    JoystickButton11 = 341,
    JoystickButton12 = 342,
    JoystickButton13 = 343,
    JoystickButton14 = 344,
    JoystickButton15 = 345,
    JoystickButton16 = 346,
    JoystickButton17 = 347,
    JoystickButton18 = 348,
    JoystickButton19 = 349,
    Joystick1Button0 = 350,
    Joystick1Button1 = 351,
    Joystick1Button2 = 352,
    Joystick1Button3 = 353,
    Joystick1Button4 = 354,
    Joystick1Button5 = 355,
    Joystick1Button6 = 356,
    Joystick1Button7 = 357,
    Joystick1Button8 = 358,
    Joystick1Button9 = 359,
    Joystick1Button10 = 360,
    Joystick1Button11 = 361,
    Joystick1Button12 = 362,
    Joystick1Button13 = 363,
    Joystick1Button14 = 364,
    Joystick1Button15 = 365,
    Joystick1Button16 = 366,
    Joystick1Button17 = 367,
    Joystick1Button18 = 368,
    Joystick1Button19 = 369
};

// Map KeyCode.xxx to ImGuiKey_xxx.
static ImGuiKey ImGui_ImplUnity_KeyCodeToImGuiKey(UnityKeyCode KeyCode)
{
    switch(KeyCode)
    {
        case UnityKeyCode::None: return ImGuiKey_None;
        case UnityKeyCode::Backspace: return ImGuiKey_Backspace;
        case UnityKeyCode::Delete: return ImGuiKey_Delete;
        case UnityKeyCode::Tab: return ImGuiKey_Tab;
        case UnityKeyCode::Return: return ImGuiKey_Enter;
        case UnityKeyCode::Pause: return ImGuiKey_Pause;
        case UnityKeyCode::Escape: return ImGuiKey_Escape;
        case UnityKeyCode::Space: return ImGuiKey_Space;
        case UnityKeyCode::Keypad0: case UnityKeyCode::Keypad1: case UnityKeyCode::Keypad2: case UnityKeyCode::Keypad3: case UnityKeyCode::Keypad4: case UnityKeyCode::Keypad5: case UnityKeyCode::Keypad6: case UnityKeyCode::Keypad7: case UnityKeyCode::Keypad8: case UnityKeyCode::Keypad9:
            return (ImGuiKey)(((int)KeyCode - (int)UnityKeyCode::Keypad0) + ImGuiKey_Keypad0);
        case UnityKeyCode::KeypadPeriod: return ImGuiKey_KeypadDecimal;
        case UnityKeyCode::KeypadDivide: return ImGuiKey_KeypadDivide;
        case UnityKeyCode::KeypadMultiply: return ImGuiKey_KeypadMultiply;
        case UnityKeyCode::KeypadMinus: return ImGuiKey_KeypadSubtract;
        case UnityKeyCode::KeypadPlus: return ImGuiKey_KeypadAdd;
        case UnityKeyCode::KeypadEnter: return ImGuiKey_KeypadEnter;
        case UnityKeyCode::KeypadEquals: return ImGuiKey_KeypadEqual;
        case UnityKeyCode::UpArrow: return ImGuiKey_UpArrow;
        case UnityKeyCode::DownArrow: return ImGuiKey_DownArrow;
        case UnityKeyCode::RightArrow: return ImGuiKey_RightArrow;
        case UnityKeyCode::LeftArrow: return ImGuiKey_LeftArrow;
        case UnityKeyCode::Insert: return ImGuiKey_Insert;
        case UnityKeyCode::Home: return ImGuiKey_Home;
        case UnityKeyCode::End: return ImGuiKey_End;
        case UnityKeyCode::PageUp: return ImGuiKey_PageUp;
        case UnityKeyCode::PageDown: return ImGuiKey_PageDown;
        case UnityKeyCode::F1: case UnityKeyCode::F2: case UnityKeyCode::F3: case UnityKeyCode::F4: case UnityKeyCode::F5: case UnityKeyCode::F6: case UnityKeyCode::F7: case UnityKeyCode::F8: case UnityKeyCode::F9: case UnityKeyCode::F10: case UnityKeyCode::F11: case UnityKeyCode::F12: case UnityKeyCode::F13: case UnityKeyCode::F14: case UnityKeyCode::F15:
            return (ImGuiKey)(((int)KeyCode - (int)UnityKeyCode::F1) + ImGuiKey_F1);
        case UnityKeyCode::Alpha0: case UnityKeyCode::Alpha1: case UnityKeyCode::Alpha2: case UnityKeyCode::Alpha3: case UnityKeyCode::Alpha4: case UnityKeyCode::Alpha5: case UnityKeyCode::Alpha6: case UnityKeyCode::Alpha7: case UnityKeyCode::Alpha8: case UnityKeyCode::Alpha9:
            return (ImGuiKey)(((int)KeyCode - (int)UnityKeyCode::Alpha0) + ImGuiKey_0);
        case UnityKeyCode::Comma: return ImGuiKey_Comma;
        case UnityKeyCode::Minus: return ImGuiKey_Minus;
        case UnityKeyCode::Period: return ImGuiKey_Period;
        case UnityKeyCode::Slash: return ImGuiKey_Slash;
        case UnityKeyCode::Semicolon: return ImGuiKey_Semicolon;
        case UnityKeyCode::Plus: case UnityKeyCode::Equals: return ImGuiKey_Equal;
        case UnityKeyCode::LeftBracket: return ImGuiKey_LeftBracket;
        case UnityKeyCode::Backslash: return ImGuiKey_Backslash;
        case UnityKeyCode::Quote: return ImGuiKey_Apostrophe;
        case UnityKeyCode::RightBracket: return ImGuiKey_RightBracket;
        case UnityKeyCode::BackQuote: return ImGuiKey_GraveAccent;
        case UnityKeyCode::A: case UnityKeyCode::B: case UnityKeyCode::C: case UnityKeyCode::D: case UnityKeyCode::E: case UnityKeyCode::F: case UnityKeyCode::G: case UnityKeyCode::H: case UnityKeyCode::I: case UnityKeyCode::J: case UnityKeyCode::K: case UnityKeyCode::L: case UnityKeyCode::M: case UnityKeyCode::N: case UnityKeyCode::O: case UnityKeyCode::P: case UnityKeyCode::Q: case UnityKeyCode::R: case UnityKeyCode::S: case UnityKeyCode::T: case UnityKeyCode::U: case UnityKeyCode::V: case UnityKeyCode::W: case UnityKeyCode::X: case UnityKeyCode::Y: case UnityKeyCode::Z:
            return (ImGuiKey)(((int)KeyCode - (int)UnityKeyCode::A) + ImGuiKey_A);
        case UnityKeyCode::Numlock: return ImGuiKey_NumLock;
        case UnityKeyCode::CapsLock: return ImGuiKey_CapsLock;
        case UnityKeyCode::ScrollLock: return ImGuiKey_ScrollLock;
        case UnityKeyCode::RightShift: return (ImGuiKey)(ImGuiMod_Shift | ImGuiKey_RightShift);
        case UnityKeyCode::LeftShift: return (ImGuiKey)(ImGuiMod_Shift | ImGuiKey_LeftShift);
        case UnityKeyCode::RightControl: return (ImGuiKey)(ImGuiMod_Ctrl | ImGuiKey_RightCtrl);
        case UnityKeyCode::LeftControl: return (ImGuiKey)(ImGuiMod_Ctrl | ImGuiKey_LeftCtrl);
        case UnityKeyCode::RightAlt: return (ImGuiKey)(ImGuiMod_Alt | ImGuiKey_RightAlt);
        case UnityKeyCode::AltGr: case UnityKeyCode::LeftAlt: return (ImGuiKey)(ImGuiMod_Alt | ImGuiKey_LeftAlt);
        case UnityKeyCode::LeftMeta: /*case UnityKeyCode::LeftCommand: case UnityKeyCode::LeftApple:*/ case UnityKeyCode::LeftWindows:
            return (ImGuiKey)(ImGuiMod_Super | ImGuiKey_LeftSuper);
        case UnityKeyCode::RightMeta: /*case UnityKeyCode::RightCommand: case UnityKeyCode::RightApple:*/ case UnityKeyCode::RightWindows:
            return (ImGuiKey)(ImGuiMod_Super | ImGuiKey_RightSuper);
        case UnityKeyCode::Print: return ImGuiKey_PrintScreen;
        case UnityKeyCode::Menu: return ImGuiKey_Menu;
        case UnityKeyCode::Mouse0: return ImGuiKey_MouseLeft;
        case UnityKeyCode::Mouse1: return ImGuiKey_MouseRight;
        case UnityKeyCode::Mouse2: return ImGuiKey_MouseMiddle;
        //case UnityKeyCode::Mouse3: return ImGuiKey_AppBack;
        //case UnityKeyCode::Mouse4: return ImGuiKey_AppForward;
        case UnityKeyCode::Mouse5: return ImGuiKey_MouseX1;
        case UnityKeyCode::Mouse6: return ImGuiKey_MouseX2;
        case UnityKeyCode::JoystickButton0: case UnityKeyCode::JoystickButton1: case UnityKeyCode::JoystickButton2: case UnityKeyCode::JoystickButton3: case UnityKeyCode::JoystickButton4: case UnityKeyCode::JoystickButton5: case UnityKeyCode::JoystickButton6: case UnityKeyCode::JoystickButton7: case UnityKeyCode::JoystickButton8: case UnityKeyCode::JoystickButton9: case UnityKeyCode::JoystickButton10: case UnityKeyCode::JoystickButton11: case UnityKeyCode::JoystickButton12: case UnityKeyCode::JoystickButton13: case UnityKeyCode::JoystickButton14: case UnityKeyCode::JoystickButton15: case UnityKeyCode::JoystickButton16: case UnityKeyCode::JoystickButton17: case UnityKeyCode::JoystickButton18: case UnityKeyCode::JoystickButton19:
            return (ImGuiKey)(((int)KeyCode - (int)UnityKeyCode::JoystickButton0) + ImGuiKey_GamepadStart);
        default: return ImGuiKey_None;
    }
}
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_ImplUnity_AddKeyEvent(UnityKeyCode key, bool down)
{
    ImGuiIO& io = ImGui::GetIO();
    io.AddKeyEvent(ImGui_ImplUnity_KeyCodeToImGuiKey(key), down);
}
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_ImplUnity_AddMouseButtonEvent(int button, bool down)
{
    ImGuiIO& io = ImGui::GetIO();
    io.AddMouseButtonEvent(button, down);
}
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_ImplUnity_AddMousePosEvent(ImVec2 position)
{
    ImGuiIO& io = ImGui::GetIO();
    io.AddMousePosEvent(position.x, position.y);
}
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_ImplUnity_AddMouseWheelEvent(ImVec2 position)
{
    ImGuiIO& io = ImGui::GetIO();
    io.AddMouseWheelEvent(position.x, position.y);
}
//extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_ImplUnity_AddMouseHoverViewportEvent(ImVec2 position)
//{
//    ImGuiIO& io = ImGui::GetIO();
//    io.AddMouseViewportEvent(position.x, position.y);
//}
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ImGui_ImplUnity_AddFocusEvent(bool isFocused)
{
    ImGuiIO& io = ImGui::GetIO();
    io.AddFocusEvent(isFocused);
}

