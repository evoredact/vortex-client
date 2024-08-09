//using ImGuiNET;
//using System;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using UnityEngine;

//namespace VortexClient.DearImGui;

//internal class ImGuiComponent : MonoBehaviour {
//    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    static extern bool SetDllDirectory(string lpPathName);

//    [DllImport("kernel32", SetLastError = true)]
//    static extern IntPtr LoadLibrary(string lpFileName);

//    //internal ImGuiComponent(IntPtr ptr) : base(ptr) { }

//    void Awake() {
//        //string rootPath = AppDomain.CurrentDomain.BaseDirectory;
//        //string file = Directory.GetFiles(rootPath, "cimgui.dll", SearchOption.AllDirectories).FirstOrDefault();
//        //if (file != null || file != string.Empty) {
//        //    var res = SetDllDirectory(Path.GetDirectoryName(file));
//        //}
//    }

//    private static int _counter = 0;
//    private static int _dragInt = 0;
//    void OnGUI() {
//        ImGui.Begin("Example Trainer");
//        ImGui.Text("Hello, world!");
//        ImGui.Text($"Mouse position: {ImGui.GetMousePos().ToString()}");

//        if (ImGui.Button("Button"))
//            _counter++;
//        ImGui.SameLine(0, -1);
//        ImGui.Text($"counter = {_counter}");

//        ImGui.DragInt("Draggable Int", ref _dragInt);

//        float framerate = ImGui.GetIO().Framerate;
//        ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");
//        ImGui.End();
//    }
//}
