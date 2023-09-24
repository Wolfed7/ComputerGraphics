using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Collections.Immutable;
using CG_PR1;
using ImGuiNET;

namespace CG_PR1;

public class Window : GameWindow
{
   private Scene _scene;
   private ImGuiController _controller;
   private GUI _gui;

   public Window(int width = 1600, int height = 900, string title = "CG_PR1")
       : base(
             GameWindowSettings.Default,
             new NativeWindowSettings()
             {
                Title = title,
                Size = (width, height),
                WindowBorder = WindowBorder.Resizable,
                StartVisible = false,
                StartFocused = true,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(4, 6)
             })
   {
      CenterWindow();

      _scene = new Scene();
      _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
      _gui = new GUI();
   }

   protected override void OnResize(ResizeEventArgs e)
   {
      _controller.WindowResized(ClientSize.X, ClientSize.Y);
      GL.Viewport(0, 0, e.Width, e.Height);

      base.OnResize(e);
   }

   protected override void OnLoad()
   {
      IsVisible = true;
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha, (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);
      GL.PointSize(10.0f);
      GL.Enable(EnableCap.LineSmooth);
      GL.ClearColor(Color4.White);


      base.OnLoad();
   }

   protected override void OnUnload()
   {
      _scene.Dispose();
      base.OnUnload();
   }

   protected override void OnUpdateFrame(FrameEventArgs args)
   {
      base.OnUpdateFrame(args);
   }

   protected override void OnRenderFrame(FrameEventArgs args)
   {
      _controller.Update(this, (float)args.Time);
      GL.Clear(ClearBufferMask.ColorBufferBit);

      _scene.RenderFrame();

      _gui.DrawGui(_scene);
      _controller.Render();

      SwapBuffers();
      base.OnRenderFrame(args);
   }

   protected override void OnMouseDown(MouseButtonEventArgs e)
   {
      if (ImGui.GetIO().WantCaptureMouse)
      {
         return;
      }

      _scene.CommandMode.OnMouseDown(MouseState, ClientSize, _scene, e);

      base.OnMouseDown(e);
   }

   protected override void OnKeyDown(KeyboardKeyEventArgs e)
   {
      switch (e.Key)
      {
         case Keys.F:
         {
            WindowState = IsFullscreen ? WindowState.Normal : WindowState.Fullscreen;
            break;
         }

         case Keys.Escape:
         {
            this.Close();
            break;
         }

         case Keys.E:
         {
            _scene.CurrentMode = Scene.CommandModes.EditMode;
            _scene.CommandMode = new CommandModeEdit();
            _scene.ResetTemporaryPoints();
            break;
         }

         case Keys.D:
         {
            _scene.CurrentMode = Scene.CommandModes.DrawMode;
            _scene.CommandMode = new CommandModeDraw();
            _scene.ResetTemporaryPoints();
            break;
         }

         case Keys.V:
         {
            _scene.CurrentMode = Scene.CommandModes.ViewMode;
            _scene.CommandMode = new CommandModeView();
            _scene.ResetTemporaryPoints();
            break;
         }

         default:
            break;
      }

      _scene.CommandMode.OnKeyDown(_scene, e);

      base.OnKeyDown(e);
   }
}




