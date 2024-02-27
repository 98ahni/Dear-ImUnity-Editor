***THIS PROJECT IS CURRENTLY IN A PROOF-OF-CONCEPT STAGE AND MIGHT NOT BE STABLE OR MIGHT BEHAVE IN AN UNEXPECTED WAY***

---

# Dear-ImUnity-Editor
Unlike previous attempts at adding Dear ImGui to Unity, this plug-in can replace Unity Editor windows or draw both to the same window. It can also work with most Dear ImGui libraries. 
---

## What can this plug-in do?
#### Code Generation
This plug-in includes a code generator which can parse a C++ header file using Dear ImGui formatting together with a filter word (usually IMGUI_API) and settings like putting the label on the left. Then create a C++ file exporting the functions and variables as well as a C# file containing the imports, structs and enums. It also splits up functions with default parameters into overloads.

Currently the generator ignores typedefs and delegates, these can be defied in a seperate file. It also has some problems that require manual fixes, particularly with nested structs and non-standard formatting where it sometimes gets confused and might miss the line or end a scope early.

#### Replacing Editor Windows
By inheriting from *ImGui.EditorWindow* the methods *ImGui_BeginWindow()* and *ImGui_EndWindow()* will handle rendering, capture events and switching contexts as necessary. Between these in the *OnGui()* method is where code for Dear ImGui can be written. Dear ImGui and Unity widgets can be interleaved in the same window by passing the result of *ImGui.LayoutUtility.GetUnityRect()* as the rect argument to the *EditorGUI* function used. By default the first window will fill the Unity window and windows can be moved and docked using Unity logic.  

While these windows can be docked to the main Unity window, they can not yet replace the built-in windows such as the *Console* or *Hierarchy* and they currently don't show the other tabs docked in the window.  

#### Drawing from a MonoBehavior
Using the *OnGui()* method in a *MonoBehavior* works similarly to the *EditorWindow*s but there is currently no functions for it meaning the setup needs to be done in *OnGui()* to work. Drawing to the inspector is currently not tested.  

#### Platforms
The current backend implemented is more of a wrapper/translation for the existing platforms. The only tested configuration is *Windows/DX11*. Support for *Windows/DX12* and *macOS/Metal* is only partially implemented because of lack of testing equipment. Though it might be better to use a fully custom Unity backend, skipping OS/API specific calls.  

#### Unity Integration
Unity includes a set of header files with the editor. These include structs and functions that Unity will try to call to tell a plug-in what the engine is doing and vice versa. In order to make integration as seamless as possible these functions are used to select backends and allows errors to be printed to the *Unity Console*. 

It would also be possible to use Unity's own memory allocation if a custom backend is built.

---

## Why?
#### Why not use *unsafe* mode?
While that would make it easier to translate calls between C++ and C# it also defeats the purpose of using C# in the first place. This plug-in is meant to feel like a part of Unity and introducing pointers and direct memory access doesn't have that feel. Besides, Unity complains if unsafe mode is on.

#### Why not settle for Unity's built-in imgui?
The built-in imgui can feel clunky and outdated in comparison and has nowhere near the amount of external libraries that have been made for Dear ImGui. That is the reason for including the *Code Generator*, so anyone can rebuild the library with any features they want. There is also way more ways to customize the look of Dear ImGui.

#### Why does this exist?
Why not?
