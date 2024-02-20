using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// TODO: Generate typedef to structs and maybe delegates
// TODO: Handle nested structs
// TODO: Handle unions
// TODO: Cascade add comments to below function
// TODO: Set array size
// TODO: Figure out how to do Lists
public class CodeGenerator : EditorWindow
{
    class EnumDef
    {
        public bool ShouldInclude;
        public string Namespace;
        public string Name;
        public List<string> Values;
    }
    protected enum TypeDefVariant
    {
        Void,
        Standard,
        String,
        Reference,
        Pointer,
        ObjectPointer,
        CArray,
        CList
    }
    class TypeDef
    {
        public string Name;
        public string ArraySize;
        public TypeDefVariant Variant;
    }
    class ParamDef
    {
        public TypeDef Type;
        public string Name;
        public string Default;
    }
    class FunctionDef
    {
        public bool LeftLabel;
        public bool ShouldInclude;
        public bool IsVar;
        public bool IsStatic;
        public string Namespace;
        public string Struct;
        public TypeDef Return;
        public string Name;
        public int ID;
        public List<ParamDef> Params;
        public string Comment;
        public string Original;
    }

    [MenuItem("Dear ImGui/Code Generator")]
    public static void ShowWindow()
    {
        GetWindow<CodeGenerator>();
    }

    string FilePath = "";
    string APIWord = "";
    Vector2 ScrollPos;
    private void OnGUI()
    {
        APIWord = EditorGUILayout.TextField("API Word ", APIWord);
        if (GUILayout.Button("Read ImGui header file"))
        {
            FilePath = EditorUtility.OpenFilePanel("Select ImGui header file to parse", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "h");
        }
        if (FilePath != "")
        {
            EditorGUILayout.LabelField(FilePath);
            if (GUILayout.Button("Parse!"))
            {
                GenerateFunctionList();
            }
        }
        EditorGUILayout.LabelField("Left aligned label/");
        EditorGUILayout.LabelField("V |Import| namespace\t | struct\t\t | Type\t\t | Name\t | Params");
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
        bool lightBG = true;
        for (int i = 0; i < _enumLines.Count; i++)
        {
            EnumDef def = _enumLines[i];
            lightBG = !lightBG;
            Rect listPos = GUILayoutUtility.GetRect(position.width - 4, 20);
            GUI.backgroundColor = lightBG ? Color.black : Color.white;
            GUI.Box(listPos, "");
            GUI.backgroundColor = Color.white;
            listPos.x += 35;
            listPos.y += 1;
            _enumLines[i].ShouldInclude = EditorGUI.ToggleLeft(listPos, FillTextWidth(def.Namespace, "::") + def.Name, def.ShouldInclude);
        }
        for (int i = 0; i < _functionLines.Count; i++)
        {
            FunctionDef def = _functionLines[i];
            lightBG = !lightBG;
            Rect listPos = GUILayoutUtility.GetRect(position.width - 4, 20);
            GUI.backgroundColor = lightBG ? Color.black : Color.white;
            GUI.Box(listPos, "");
            GUI.backgroundColor = Color.white;
            listPos.x += 5;
            listPos.y += 1;
            if (def.Params.Any(p => p.Name == "label"))
            {
                _functionLines[i].LeftLabel = EditorGUI.ToggleLeft(listPos, "", def.LeftLabel);
            }
            listPos.x += 30;
            string paramDisp = "";
            foreach (ParamDef p in def.Params)
            {
                paramDisp += p.Type.Name + " " + p.Name + "; ";
            }
            _functionLines[i].ShouldInclude = EditorGUI.ToggleLeft(listPos, FillTextWidth(def.Namespace, "::") + FillTextWidth(def.Struct, "::") + FillTextWidth(def.Return.Name, " ") + def.Name + (def.IsVar ? "" : " (" + paramDisp.TrimEnd().TrimEnd(';') + ")") + ";", def.ShouldInclude);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Write C++ Export File"))
        {
            GenerateCppExport(EditorUtility.SaveFilePanel("Where should the C++ files be saved?", Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath) + "Exp", "cpp"));
        }
        if (GUILayout.Button("Write C# Import File"))
        {
            GenerateCsImport(EditorUtility.SaveFilePanel("Where should the C# file be saved?", Application.dataPath, Path.GetFileNameWithoutExtension(FilePath) + "Impl", "cs"));
        }
        EditorGUILayout.EndHorizontal();
    }

    string FillTextWidth(string Text, string Addition)
    {
        if (string.IsNullOrEmpty(Text))
        {
            return "\t\t";
        }
        GUIStyle style = GUI.skin.box;
        if (style.CalcSize(new GUIContent(Text + Addition)).x < style.CalcSize(new GUIContent("\t")).x)
        {
            return Text + Addition + "\t\t";
        }
        return Text + Addition + "\t";
    }

    List<FunctionDef> _functionLines = new List<FunctionDef>();
    List<EnumDef> _enumLines = new List<EnumDef>();
    string _currentNamespace = "";
    string _currentStruct = "";
    int _currentScope = 0;
    int _enumScopeInd = -1;
    int _structScopeInd = -1;

    void GenerateFunctionList()
    {
        void SetTypeVariant(ref TypeDef Type, ref string Name)
        {
            if (Name != null && Name.Contains("["))
            {
                Type.ArraySize = Name[Name.Split('[')[0].Length..];
                Name = Name.Split("[")[0];
                Type.Variant = TypeDefVariant.CArray;
                return;
            }
            if (Type.Name == "void")
            {
                Type.Variant = TypeDefVariant.Void;
                return;
            }
            if (Type.Name.Contains("char*"))
            {
                Type.Variant = TypeDefVariant.String;
                return;
            }
            if (Type.Name.Contains("*") || Type.Name.Contains("&"))
            {
                string tName = Type.Name;
                if ((new string[] { "bool", "char", "short", "int", "long", "size_t", "float", "double", "ImVec", "Func", "Callback" }).Where(t => tName.Contains(t)).Count() != 0)
                {
                    Type.Variant = Type.Name.Contains("*") ? TypeDefVariant.Pointer : TypeDefVariant.Reference;
                }
                else if (Type.Name.Contains("void"))
                {
                    Type.Variant = TypeDefVariant.Standard;
                }
                else if (Type.Name.Contains("*"))
                {
                    Type.Variant = TypeDefVariant.ObjectPointer;
                }
                return;
            }
            if (Type.Name.Contains("ector<"))
            {
                Type.Variant = TypeDefVariant.CList;
                return;
            }
            Type.Variant = TypeDefVariant.Standard;
        }
        _functionLines.Clear();
        _enumLines.Clear();
        _currentNamespace = "";
        _currentStruct = "";
        _currentScope = 0;
        _enumScopeInd = -1;
        _structScopeInd = -1;
        string[] fileLines = File.ReadAllLines(FilePath);
        int lineID = -1;
        foreach (string line in fileLines)
        {
            lineID++;
            if (line.Contains("typedef")) continue;
            string lineKU = line.Split("//")[0];
            if (lineKU.Contains("#")) continue;
            lineKU = lineKU.TrimStart();
            if (lineKU.Contains("{"))
            {
                _currentScope += lineKU.Where(c => c == '{').Count();
            }
            if (lineKU.Contains("}"))
            {
                _currentScope -= lineKU.Where(c => c == '}').Count();
                if (_currentScope == 0)
                {
                    _currentNamespace = "";
                }
            }
            if (lineKU.Contains("namespace"))
            {
                _currentNamespace = line.Split("namespace ")[1];
            }
            if (lineKU.Contains("struct") && !lineKU.Contains(";"))
            {
                if (_structScopeInd != -1) continue;
                _currentStruct = line.Split("struct ")[1];
                _structScopeInd = _currentScope;
                if (!lineKU.Contains("{")) _structScopeInd++;
                continue;
            }
            if (_structScopeInd != -1 && _structScopeInd > _currentScope)
            {
                _structScopeInd = -1;
                _currentStruct = "";
            }
            if (lineKU.Contains("enum") && (!lineKU.Contains(";") || lineKU.Contains("{")))
            {
                EnumDef enumDef = new EnumDef();
                enumDef.ShouldInclude = true;
                enumDef.Namespace = _currentNamespace;
                enumDef.Name = line.Split("enum ")[1];
                enumDef.Values = new List<string>();
                _enumLines.Add(enumDef);
                _enumScopeInd = _currentScope;
                //if (!lineKU.Contains("{")) _structScopeInd++;
                if (lineKU.Contains("{") && lineKU.Contains("}"))
                {
                    _enumScopeInd = -1;
                    enumDef.Values.AddRange(lineKU.Split("{")[1].Split("}")[0].Split(","));
                    for (int i = 0; i < enumDef.Values.Count - 1; i++)
                    {
                        enumDef.Values[i] += ", ";
                    }
                }
                continue;
            }
            if (_enumScopeInd != -1)
            {
                if (_enumScopeInd > _currentScope)
                {
                    _enumScopeInd = -1;
                }
                else
                {
                    if (lineKU.Contains("{")) continue;
                    if (lineKU.Contains("}"))
                    {
                        _enumScopeInd = -1;
                        continue;
                    }
                    _enumLines.Last().Values.Add(line);
                }
            }
            if (lineKU.Contains(APIWord) && lineKU.Contains("(") && lineKU.Contains(")"))
            {
                FunctionDef function = new FunctionDef();
                function.ShouldInclude = // Should probably handle them correctly instead.
                    !(
                    line.Contains("[Internal]") ||      // For obvious reasons.
                    lineKU.Contains("nused") ||         // For obvious reasons.
                    lineKU.Contains("...") ||
                    lineKU.Contains("va_list") ||
                    lineKU.Contains("ImGuiContext") ||  // The context is used weirdly, let's not mess with it.
                    lineKU.Contains(")(") ||            // For line 526 in imgui.h
                    lineKU.Contains("Font") ||          // All font structs are very terse and not nessesary in 99% of cases.
                    _currentStruct.Contains("Font") ||  // All font structs are very terse and not nessesary in 99% of cases.
                    _currentStruct.Contains("ImVec")    // ImVec2, ImVec4 and ImVector get converted to Vector2, Vector4 and List respectively.
                    );
                function.Namespace = _currentNamespace;
                function.Struct = _currentStruct;
                function.Name = lineKU.Split(";")[0].Split("(")[0].TrimEnd().Split(new char[] { ' ', '>', '*', '&' }).Last();
                function.Return = new TypeDef();
                function.Return.Name = lineKU.Split(APIWord + " ")[1].TrimStart().Split("(")[0].TrimEnd()[..^function.Name.Length].Trim();
                SetTypeVariant(ref function.Return, ref function.Name);
                function.IsStatic = function.Struct == "";
                if (function.Return.Name.Contains("static"))
                {
                    function.Return.Name = function.Return.Name.Split("static")[1];
                    function.IsStatic = true;
                }
                function.ID = lineID;
                List<string> tempParams = new List<string>();
                //tempParams.AddRange(lineKU.Replace(lineKU.Split("(")[0] + "(", "").Replace(")" + lineKU.Split(")").Last(), "").Split(","));
                tempParams.AddRange(lineKU[(lineKU.Split("(")[0].Length + 1)..^(1 + lineKU.Split(")").Last().Length)].Split(","));
                for (int i = tempParams.Count - 1; i > 0; i--)
                {
                    if (tempParams[i].Where(c => c == ')').Count() != tempParams[i].Where(c => c == '(').Count())
                    {
                        tempParams[i - 1] = tempParams[i - 1] + ", " + tempParams[i];
                        tempParams.RemoveAt(i);
                    }
                }
                function.Params = new List<ParamDef>();
                foreach (string p in tempParams)
                {
                    if (p == "") continue;
                    ParamDef pDef = new ParamDef();
                    pDef.Name = p.Split('=')[0].Split(new char[] { ' ', '>', '*', '&' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    pDef.Type = new TypeDef();
                    pDef.Type.Name = p.Split('=')[0].Trim()[..^pDef.Name.Length].Trim();
                    pDef.Name = pDef.Name.Trim();
                    pDef.Default = p.Contains('=') ? p.Split('=').Last().Trim() : "";
                    SetTypeVariant(ref pDef.Type, ref pDef.Name);
                    function.Params.Add(pDef);
                }
                function.Comment = line.Replace(line.Split("//")[0] + "//", "");
                function.Original = lineKU[..];
                function.LeftLabel = function.Params.Where(p => p.Name == "label").Count() != 0;
                function.IsVar = false;
                _functionLines.Add(function);
            }
            if (_structScopeInd != -1 && lineKU.Contains(";") && !lineKU.Split("=")[0].Contains("(") && !lineKU.Split("=")[0].Contains("}"))
            {
                FunctionDef function = new FunctionDef();
                function.LeftLabel = false;
                function.ShouldInclude = // Should probably handle them correctly instead.
                    !(
                    line.Contains("[Internal]") ||      // For obvious reasons.
                    lineKU.Contains("nused") ||         // For obvious reasons.
                    lineKU.Contains("...") ||
                    lineKU.Contains("va_list") ||
                    lineKU.Contains("ImGuiContext") ||  // The context is used weirdly, let's not mess with it.
                    lineKU.Contains(")(") ||            // For line 526 in imgui.h
                    lineKU.Contains("Font") ||          // All font structs are very terse and not nessesary in 99% of cases.
                    _currentStruct.Contains("Font") ||  // All font structs are very terse and not nessesary in 99% of cases.
                    _currentStruct.Contains("ImVec")    // ImVec2, ImVec4 and ImVector get converted to Vector2, Vector4 and List respectively.
                    );
                function.Namespace = _currentNamespace;
                function.Struct = _currentStruct;
                function.Name = lineKU.Split(";")[0].Split(new char[] { '=', ':', '[' })[0].TrimEnd().Split(new char[] { ' ', '>', '*', '&' }).Last();
                if (lineKU.Contains('['))
                {
                    function.Name += lineKU[lineKU.Split("[")[0].Length..^lineKU.Split("]").Last().Length];
                }
                function.Return = new TypeDef();
                function.Return.Name = lineKU.TrimStart().Split(new char[] { '=', ':' })[0].TrimEnd()[..^(function.Name.Length + 1)].Trim();
                function.IsStatic = function.Struct == "";
                if (function.Return.Name.Contains("static"))
                {
                    function.Return.Name = function.Return.Name.Split("static")[1].Trim();
                    function.IsStatic = true;
                }
                SetTypeVariant(ref function.Return, ref function.Name);
                function.ID = lineID;
                function.Params = new List<ParamDef>();
                function.Comment = line.Replace(line.Split("//")[0] + "//", "");
                function.Original = lineKU[..];
                function.IsVar = true;
                _functionLines.Add(function);
            }
        }
    }

    void GenerateCppExport(string SavePath)
    {
        string sourceText = "#include \"" + Path.GetRelativePath(Path.GetDirectoryName(SavePath), FilePath) + "\"\n" +
            "#include \"imgui/imgui_internal.h\"\n" +
            "#define LabelLeft(label) {float indent = ImGui::GetCursorPosX(); std::string shownLabel(label, ImGui::FindRenderedTextEnd(label));" +
            "if (!shownLabel.empty()) { ImGui::Text(shownLabel.c_str()); ImGui::SameLine(); ImGui::SetCursorPosX(indent + (ImGui::GetWindowWidth() < 384 ? 96 : ImGui::GetWindowWidth() * .25f)); }}\n" +
            "#define HideLabel(label) (std::string(\"##\") + label).c_str()\n" +
            "#define FillWidth ImGui::PushItemWidth((ImGui::GetWindowContentRegionMax().x - ImGui::GetCursorPosX()) - 10)\n\n" +
            "extern \"C\"\n{\n";
        string WriteCPPFunction(FunctionDef cleanDef)
        {
            string returnName = cleanDef.Return.Name;
            if (cleanDef.Return.Name == "")
            {
                if (cleanDef.Name.Contains("~"))
                {
                    returnName = "void";
                }
                else
                {
                    returnName = cleanDef.Struct.Trim() + "*";
                }
            }
            if (cleanDef.Return.Name.Contains("ImVec") && cleanDef.Return.Variant == TypeDefVariant.Standard)
            {
                returnName += "&";
            }
            string functionReturn = "__declspec(dllexport) " + returnName + (cleanDef.Return.Variant == TypeDefVariant.CArray ? "*" : "") + " __stdcall " +
                FillTextWidth(cleanDef.Namespace.Trim(), "_").TrimEnd() + FillTextWidth(cleanDef.Struct.Trim(), "_").TrimEnd();
            if (cleanDef.IsVar)
            {
                if (cleanDef.Params.Count == 0)
                {
                    functionReturn += "Get_";
                }
                else
                {
                    functionReturn += "Set_";
                }
            }
            functionReturn += cleanDef.Name.Trim().Replace("~", "Delete") + cleanDef.ID.ToString() + "(";
            if (cleanDef.Return.Variant == TypeDefVariant.CList)
            {
                functionReturn += "int* returnListSize, ";
            }
            if (cleanDef.Struct != "" && !cleanDef.IsStatic && (cleanDef.Return.Name != "" || cleanDef.Name.Contains("~")))
            {
                functionReturn += FillTextWidth(cleanDef.Namespace.Trim(), "::").TrimEnd() + cleanDef.Struct.Trim() + "* objectPtr, ";
            }
            foreach (ParamDef param in cleanDef.Params)
            {
                functionReturn += param.Type.Name + (param.Type.Name.Contains("ImVec") && param.Type.Variant == TypeDefVariant.Standard ? "& " : " ") + param.Name + (param.Type.Variant == TypeDefVariant.CArray ? param.Type.ArraySize : "") + ", ";
            }
            functionReturn = functionReturn.TrimEnd().TrimEnd(',') + ")\n{\n\t";
            if (cleanDef.Return.Variant == TypeDefVariant.CList)
            {
                functionReturn += "*returnListSize = ";
                if (cleanDef.Struct != "" && !cleanDef.IsStatic)
                {
                    functionReturn += "objectPtr->";
                }
                functionReturn += cleanDef.Name.Trim() + ".size();\n\t";
            }
            if (cleanDef.LeftLabel)
            {
                functionReturn += "LabelLeft(label);\n\tFillWidth;\n\t";
            }
            if (cleanDef.Return.Variant != TypeDefVariant.Void)
            {
                functionReturn += "return ";
            }
            if (cleanDef.Return.Name == "")
            {
                if (cleanDef.Name.Contains("~"))
                {
                    functionReturn += "IM_DELETE(objectPtr);\n}\n";
                    return functionReturn;
                }
                else
                {
                    functionReturn += "IM_NEW(" + returnName.Trim('*') + ")";
                }
            }
            else if (cleanDef.Struct != "" && !cleanDef.IsStatic)
            {
                functionReturn += "objectPtr->";
            }
            else
            {
                functionReturn += FillTextWidth(cleanDef.Namespace.Trim(), "::").TrimEnd();
            }
            if (cleanDef.Return.Name != "")
            {
                functionReturn += cleanDef.Name.Trim();
            }
            if (cleanDef.IsVar)
            {
                if (cleanDef.Params.Count != 0)
                {
                    if (cleanDef.Params[0].Type.Variant == TypeDefVariant.CArray) return "";
                    functionReturn += " = value";
                }
            }
            else
            {
                functionReturn += "(";
                foreach (ParamDef param in cleanDef.Params)
                {
                    if (cleanDef.LeftLabel && param.Name.Contains("label"))
                    {
                        functionReturn += "HideLabel(label), ";
                    }
                    else
                    {
                        functionReturn += param.Name + ", ";
                    }
                }
                functionReturn = functionReturn.TrimEnd().TrimEnd(',') + ")";
            }
            functionReturn += ";\n}\n";
            return functionReturn;
        }
        foreach (FunctionDef function in _functionLines)
        {
            if (!function.ShouldInclude) continue;
            if (function.IsVar)
            {
                sourceText += WriteCPPFunction(new FunctionDef      // Get
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Trim(),
                    Return = function.Return,
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = new List<ParamDef>(),
                    IsVar = function.IsVar,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                });
                sourceText += WriteCPPFunction(new FunctionDef      // Set
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Trim(),
                    Return = new TypeDef { Name = "void", Variant = TypeDefVariant.Void, ArraySize = "" },
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = new List<ParamDef> { new ParamDef { Type = function.Return, Name = " value", Default = "" } },
                    IsVar = function.IsVar,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                });
            }
            else
            {
                sourceText += WriteCPPFunction(new FunctionDef
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Trim(),
                    Return = function.Return,
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = function.Params,
                    IsVar = function.IsVar,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                });
            }
        }
        sourceText += "}";
        File.WriteAllText(SavePath, sourceText);
    }
    void GenerateCsImport(string SavePath)
    {
        string MakeCamelCase(string CppType)
        {
            if (CppType == "") return "";
            string[] words = CppType.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
            }
            return string.Join("", words);
        }
        TypeDef TranslateType(TypeDef CppType, bool IsReturn)
        {
            //if (((CppType.Contains("*") || (CppType.Contains("&") && !CppType.Contains("const"))) && IsReturn) || CppType.Contains("void*"))
            //{
            //    return "IntPtr";
            //}
            string output = CppType.Name[..];
            output = output.Replace("void*", "IntPtr");
            output = output.Replace("const char*", "string");
            output = output.Replace("char*", "ref string");
            output = output.Replace("ImVector", "List");
            output = output.Replace("ImVec", "Vector");
            output = output.Replace("ImChar", "char");
            output = output.Replace("ImWchar16", "char");
            output = output.Replace("ImWchar", "char");
            output = output.Replace("ImS8", "byte");
            output = output.Replace("ImS16", "short");
            output = output.Replace("size_t", "long");
            output = output.Replace("unsigned ", "u");
            output = output.Replace("mutable", "");
            output = output.Replace("ImU16", "ushort");
            output = output.Replace("ImU32", "uint");
            output = output.Replace("ImU64", "ulong");
            if (CppType.Variant == TypeDefVariant.Pointer)
            {
                output = (IsReturn ? "ref " : "out ") + output;
            }
            if (output.Contains("&") && CppType.Variant != TypeDefVariant.ObjectPointer)
            {
                if (output.Contains("const") && !IsReturn)
                {
                    output = "out " + output;
                }
                else
                {
                    output = "ref " + output;
                }
            }
            if (output.Contains("[") && output.Contains("]") && !IsReturn)
            {
                //string tName = output.TrimStart(' ').Split(new string[] { " ", "*", "&", ">" }, StringSplitOptions.None)[0];
                //output = "ref " + output.Replace("[", "").Replace("]", "");
                //output = output.Replace(tName, tName + "[]");
            }
            output = output.Replace("*", "").Replace("&", "").Replace("const", "");
            return new TypeDef { Name = output, Variant = CppType.Variant, ArraySize = CppType.ArraySize };
        }
        string MarshalType(TypeDef CsType, bool IsVarOrReturn)
        {
            if (CsType == null) return "";
            if (CsType.Name.Contains("List") || (CsType.Variant == TypeDefVariant.CArray && IsVarOrReturn))
            {
                // Fix for lists
                return "[MarshalAs(UnmanagedType.LPArray" +
                    (CsType.Name.Contains("string") ? ", ArraySubType = UnmanagedType.LPStr" :
                    (CsType.Name.Contains("bool") ? ", ArraySubType = UnmanagedType.I1" : "")) +
                    (CsType.Variant == TypeDefVariant.CArray ? ", SizeConst = " + CsType.ArraySize.Replace("[", "").Split("]")[0] :
                    "") + ")]";
            }
            if (CsType.Name.Contains("string"))
            {
                return "[MarshalAs(UnmanagedType.LPStr)]";
            }
            if (CsType.Name.Contains("bool"))
            {
                return "[MarshalAs(UnmanagedType.I1)]";
            }
            return "";
        }
        string WriteCSImportFunction(FunctionDef cleanDef)
        {
            TypeDef returnName = TranslateType(cleanDef.Return, true);
            if (returnName.Name == "")
            {
                if (cleanDef.Name.Contains("~"))
                {
                    returnName.Name = "void";
                }
                else
                {
                    returnName.Name = cleanDef.Struct.Trim();
                }
            }
            if (returnName.Variant == TypeDefVariant.ObjectPointer)
            {
                returnName.Name = "IntPtr";
            }
            string functionReturn = "\n\t[DllImport(\"Dear ImGui\")]\n\t" + MarshalType(returnName, true).Replace("[", "[return: ").Replace("]", "]\n\t") +
                 "private static extern " + returnName.Name + (returnName.Variant == TypeDefVariant.CArray ? "[]" : "") + " " +
                FillTextWidth(cleanDef.Namespace.Trim(), "_").TrimEnd() + FillTextWidth(cleanDef.Struct.Trim(), "_").TrimEnd();
            if (cleanDef.IsVar)
            {
                if (cleanDef.Params.Count == 0)
                {
                    functionReturn += "Get_";
                }
                else
                {
                    functionReturn += "Set_";
                }
            }
            functionReturn += cleanDef.Name.Trim().Replace("~", "Delete") + cleanDef.ID.ToString() + "(";
            if (cleanDef.Return.Variant == TypeDefVariant.CList)
            {
                functionReturn += "out int ReturnListSize, ";
            }
            if (!cleanDef.IsStatic && (cleanDef.Return.Name != "" || cleanDef.Name.Contains("~")))
            {
                functionReturn += "IntPtr objectPtr, ";
            }
            foreach (ParamDef param in cleanDef.Params)
            {
                TypeDef pType = TranslateType(param.Type, false);
                if (param.Type.Variant == TypeDefVariant.ObjectPointer)
                {
                    pType.Name = "IntPtr";
                }
                functionReturn += MarshalType(pType, cleanDef.IsVar);
                if (cleanDef.IsVar)
                {
                    functionReturn += pType.Name + (param.Type.Variant == TypeDefVariant.CArray ? "[]" : "");
                }
                else
                {
                    functionReturn += (param.Type.Variant == TypeDefVariant.CArray ? "out " : "") + pType.Name;
                }
                functionReturn += " " + MakeCamelCase(param.Name) + ", ";
            }
            functionReturn = functionReturn.TrimEnd().TrimEnd(',') + ");\n";
            return functionReturn;
        }
        string WriteCSFunction(FunctionDef cleanDef, int until)
        {
            string functionReturn = "\n\t/// <summary><code>" + cleanDef.Original + "</code>\n\t\t///" + cleanDef.Comment.Replace("\n", "\n\t\t/// ") + " </summary>";
            TypeDef returnName = TranslateType(cleanDef.Return, true);
            functionReturn += "\n\t" + (cleanDef.Name.Contains("~") ? "" : "public ") + (cleanDef.IsStatic ? "static " : "") + returnName.Name + (returnName.Variant == TypeDefVariant.CArray ? "[]" : "") + " " + cleanDef.Name.Trim() + "(";
            for (int i = 0; i < until; i++)
            {
                ParamDef param = cleanDef.Params[i];
                if (param.Name == "") continue;
                ParamDef translatedParam = new ParamDef { Type = TranslateType(param.Type, false), Default = param.Default, Name = MakeCamelCase(param.Name) };
                //if (param.Default != "")
                //{
                //    if (param.Default.Contains("(") && param.Default.Contains(")") && !param.Default.Contains("sizeof") && !param.Default.Contains("\""))
                //    {
                //        translatedParam.Default = param.Default.Split("(")[0].Replace("=", "= new ") + "()";
                //    }
                //    if(param.Type.Variant == TypeDefVariant.Pointer || param.Type.Variant == TypeDefVariant.ObjectPointer)
                //    {
                //        translatedParam.Default = translatedParam.Default.Replace("NULL", "null");
                //        translatedParam.Default = translatedParam.Default.Replace("nullptr", "null"); // If this don't work then generate different versions
                //    }
                //    else
                //    {
                //        translatedParam.Default = translatedParam.Default.Replace("NULL", "default");
                //        translatedParam.Default = translatedParam.Default.Replace("nullptr", "default");
                //    }
                //    translatedParam.Default = translatedParam.Default.Replace("FLT_MAX", "float.MaxValue");
                //    translatedParam.Default = translatedParam.Default.Replace("FLT_MIN", "float.MinValue");
                //    if (int.TryParse(param.Default[0].ToString(), out _))
                //    {
                //        translatedParam.Default = "(" + translatedParam.Type.Name + ")" + translatedParam.Default;
                //    }
                //}
                if (translatedParam.Type.Name.Contains("?"))
                {
                    translatedParam.Type.Name = translatedParam.Type.Name.Replace("out ", "");
                }
                if (cleanDef.IsVar)
                {
                    functionReturn += translatedParam.Type.Name + (translatedParam.Type.Variant == TypeDefVariant.CArray ? "[]" : "");
                }
                else
                {
                    functionReturn += (translatedParam.Type.Variant == TypeDefVariant.CArray ? "out " : "") + translatedParam.Type.Name;
                }
                functionReturn += " " + translatedParam.Name + ", ";
            }
            functionReturn = functionReturn.TrimEnd().TrimEnd(',') + ")\n\t{\n\t\t";
            string functionCall = "";
            if (cleanDef.Return.Variant != TypeDefVariant.Void && cleanDef.Return.Name != "")
            {
                functionCall += "return ";
            }
            if (!cleanDef.IsStatic && cleanDef.Return.Name == "" && !cleanDef.Name.Contains("~"))
            {
                functionCall += "_objectPtr = ";
            }
            if (cleanDef.Return.Variant == TypeDefVariant.Reference)
            {
                functionCall += "ref ";
            }
            if (cleanDef.Return.Variant == TypeDefVariant.ObjectPointer)
            {
                functionCall += "new " + returnName.Name + "(";
            }
            functionCall += FillTextWidth(cleanDef.Namespace.Trim(), "_").TrimEnd() + FillTextWidth(cleanDef.Struct.Trim(), "_").TrimEnd() +
                cleanDef.Name.Trim().Replace("~", "Delete") + cleanDef.ID.ToString();
            functionCall += "(";
            if (!cleanDef.IsStatic && (cleanDef.Return.Name != "" || cleanDef.Name.Contains("~")))
            {
                functionCall += "_objectPtr, ";
            }
            for (int i = 0; i < cleanDef.Params.Count; i++)
            {
                ParamDef param = cleanDef.Params[i];
                TypeDef translatedParam = TranslateType(param.Type, false);
                if (translatedParam.Name.Contains("out ") || (param.Type.Variant == TypeDefVariant.CArray && !cleanDef.IsVar))
                {
                    functionCall += "out ";
                }
                if (translatedParam.Name.Contains("ref "))
                {
                    functionCall += "ref ";
                }
                if (i >= until)
                {
                    if (param.Default != "")
                    {
                        string outParam = param.Default[..];
                        if (param.Default.Contains("(") && param.Default.Contains(")") && !param.Default.Contains("sizeof") && !param.Default.Contains("\""))
                        {
                            functionReturn += translatedParam.Name.Replace("out ", "").Replace("ref ", "") + " param" + i.ToString() + " = new " +
                                translatedParam.Name.Replace("out ", "").Replace("ref ", "") + " " + param.Default[param.Default.Split("(")[0].Length..]
                                .Replace("FLT_MAX", "float.MaxValue").Replace("FLT_MIN", "float.MinValue") + ";\n\t\t";
                            outParam = "param" + i.ToString();
                        }
                        if (param.Type.Variant == TypeDefVariant.Pointer)
                        {
                            outParam = outParam.Replace("NULL", "_");
                            outParam = outParam.Replace("nullptr", "_");
                        }
                        else if (param.Type.Variant == TypeDefVariant.ObjectPointer)
                        {
                            outParam = outParam.Replace("NULL", "null");
                            outParam = outParam.Replace("nullptr", "null");
                        }
                        else
                        {
                            outParam = outParam.Replace("NULL", "default");
                            outParam = outParam.Replace("nullptr", "default");
                        }
                        outParam = outParam.Replace("FLT_MAX", "float.MaxValue");
                        outParam = outParam.Replace("FLT_MIN", "float.MinValue");
                        if (int.TryParse(param.Default[0].ToString(), out _))
                        {
                            outParam = "(" + translatedParam.Name + ")" + outParam;
                        }
                        functionCall += outParam;
                    }
                }
                else
                {
                    functionCall += MakeCamelCase(param.Name);
                    if (param.Type.Variant == TypeDefVariant.ObjectPointer)
                    {
                        functionCall += ".AsPtr";
                    }
                }
                functionCall += ", ";
            }
            functionCall = functionCall.TrimEnd().TrimEnd(',') + ")";
            if (!cleanDef.IsStatic && cleanDef.Return.Name == "" && !cleanDef.Name.Contains("~"))
            {
                functionCall += "._objectPtr";
            }
            if (cleanDef.Return.Variant == TypeDefVariant.ObjectPointer)
            {
                functionCall += ")";
            }
            functionReturn += functionCall + ";\n\t}\n";
            if (until > 0 && cleanDef.Params.Count != 0 && cleanDef.Params[until - 1].Default != "")
            {
                functionReturn = WriteCSFunction(cleanDef, until - 1) + functionReturn;
            }
            return functionReturn;
        }

        _currentNamespace = "";
        _currentStruct = "";
        _currentScope = 0;
        _enumScopeInd = -1;
        _structScopeInd = -1;
        string sourceText = "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.Runtime.InteropServices;\nusing UnityEngine;\n" + "\nnamespace Dear\n{" +
            "\n\t#region Enums\n\t";
        foreach (EnumDef num in _enumLines)
        {
            if (!num.ShouldInclude) continue;
            sourceText += "public enum " + num.Name.Split("_")[0] + "\n\t{";
            foreach (string v in num.Values)
            {
                sourceText += "\n\t\t" + v.Replace(num.Name.Split(":")[0].Trim(), "").Split("=")[0];
                if (v.Contains("="))
                {
                    sourceText += "=" + v.Split("=")[1].Replace("_", ".");
                }
            }
            sourceText += "\n\t}\n\n\t";
        }
        sourceText += "\n#endregion // Enums\n\n#region Functions\n\t";
        foreach (FunctionDef function in _functionLines)
        {
            if (!function.ShouldInclude) continue;
            if (function.Namespace != _currentNamespace)
            {
                if (function.Namespace == "" || _currentNamespace != "")
                {
                    sourceText += "\t}\n";
                }
                if (function.Namespace != "")
                {
                    sourceText += "\tpublic static partial class " + function.Namespace + "\n\t{\n";
                }
                _currentNamespace = function.Namespace;
            }
            if (function.Struct != _currentStruct)
            {
                if (function.Struct == "" || _currentStruct != "")
                {
                    sourceText += "\t}\n";
                }
                if (function.Struct != "")
                {
                    sourceText += "\tpublic class " + function.Struct + "\n\t{\n\t\tprivate IntPtr _objectPtr;" +
                        "\n\t\tpublic IntPtr AsPtr { get => _objectPtr; }\n\t\tpublic " + function.Struct + "(IntPtr Ptr){ _objectPtr = Ptr; }\n";
                }
                _currentStruct = function.Struct;
            }
            if (function.IsVar)
            {
                sourceText += WriteCSImportFunction(new FunctionDef      // Get
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Split('[')[0].Trim(),
                    Return = function.Return,
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = new List<ParamDef>(),
                    IsVar = function.IsVar,
                    IsStatic = function.IsStatic,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                }).Replace("ref ", "");
                sourceText += WriteCSImportFunction(new FunctionDef      // Set
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Split('[')[0].Trim(),
                    Return = new TypeDef { Name = "void", Variant = TypeDefVariant.Void, ArraySize = "" },
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = new List<ParamDef> { new ParamDef { Type = function.Return, Name = " Value", Default = "" } },
                    IsVar = function.IsVar,
                    IsStatic = function.IsStatic,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                }).Replace("ref ", "");
                sourceText += "\n\t/// <summary><code>" + function.Original + "</code>\n\t\t///" + function.Comment.Replace("\n", "\n\t\t/// ") + " </summary>";
                string typeName = TranslateType(function.Return, true).Name.Replace("ref ", "") + (function.Return.Variant == TypeDefVariant.CArray ? "[]" : "");
                sourceText += "\n\tpublic " + (function.IsStatic ? "static " : "") + typeName + " " + function.Name.Split('[')[0].Trim() + "\n\t{\n\t\tget {" + " return " + (function.Return.Variant == TypeDefVariant.ObjectPointer ? "new " + typeName + "(" : "") +
                    FillTextWidth(function.Namespace.Trim(), "_").TrimEnd() + FillTextWidth(function.Struct.Trim(), "_").TrimEnd() + "Get_" + function.Name.Trim() + function.ID.ToString() + "(" + (function.Return.Variant == TypeDefVariant.CList ? "out _, " : "") + (function.IsStatic ? "" : "_objectPtr") + ")" + (function.Return.Variant == TypeDefVariant.ObjectPointer ? ")" : "") + ";}\n\t\tset {" +
                    FillTextWidth(function.Namespace.Trim(), "_").TrimEnd() + FillTextWidth(function.Struct.Trim(), "_").TrimEnd() + "Set_" + function.Name.Trim() + function.ID.ToString() + "(" + (function.IsStatic ? "" : "_objectPtr, ") + "value" + (function.Return.Variant == TypeDefVariant.ObjectPointer ? ".AsPtr" : "") + ");}" + "\n\t}";
            }
            else
            {
                sourceText += WriteCSImportFunction(new FunctionDef
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Trim(),
                    Return = function.Return,
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = function.Params,
                    IsVar = function.IsVar,
                    IsStatic = function.IsStatic,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                });
                sourceText += WriteCSFunction(new FunctionDef
                {
                    Namespace = function.Namespace.Trim(),
                    Name = function.Name.Trim(),
                    Return = function.Return,
                    Struct = function.Struct.Trim(),
                    ID = function.ID,
                    Params = function.Params,
                    IsVar = function.IsVar,
                    IsStatic = function.IsStatic,
                    LeftLabel = function.LeftLabel,
                    ShouldInclude = function.ShouldInclude,
                    Comment = function.Comment,
                    Original = function.Original
                }, function.Params.Count);
            }
        }
        sourceText += (_currentNamespace != "" ? "\n\t}" : "") + (_currentStruct != "" ? "\n\t}" : "") + "\n#endregion // Functions\n}";
        File.WriteAllText(SavePath, sourceText);
    }
}