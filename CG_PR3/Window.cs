using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Collections.Immutable;
using ImGuiNET;

namespace CG_PR3;

// In this chapter we will focus on how to use lighting to make our games and other applications more lifelike

// In this first part the focus will mainly be on setting up a scene for testing the different coloring options.
// We draw two cubes one at 0,0,0 for testing our light shader, the second one is drawn where we have the light.
// Furthermore in the shaders we have set up some basic physically based coloring.

public class Window : GameWindow
{
   private readonly float[] _vertices =
   {
             // Position          Normal
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

   private Vector3 _lightPos = new Vector3(1.2f, 1.5f, 2.0f);

   private int _vertexBufferObject;

   private int _vaoModel;

   private int _vaoLamp;

   private Shader _lampShader;

   private Shader _lightingShader;

   private Camera _camera;

   private bool _firstMove = true;

   private Vector2 _lastPos;


   public Window(int width = 1920, int height = 1080, string title = "CG_PR3")
    : base(
          GameWindowSettings.Default,
          new NativeWindowSettings()
          {
             Title = title,
             Size = (width, height),
             WindowBorder = WindowBorder.Resizable,
             WindowState = WindowState.Fullscreen,
             StartVisible = false,
             StartFocused = true,
             API = ContextAPI.OpenGL,
             Profile = ContextProfile.Core,
             APIVersion = new Version(4, 6),
             Flags = ContextFlags.ForwardCompatible
          }
          )
   {

   }

   protected override void OnResize(ResizeEventArgs e)
   {
      base.OnResize(e);

      GL.Viewport(0, 0, Size.X, Size.Y);
      _camera.AspectRatio = Size.X / (float)Size.Y;
   }

   protected override void OnLoad()
   {
      base.OnLoad();

      GL.Enable(EnableCap.Blend);
      GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha, (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);

      IsVisible = true;
      GL.ClearColor(new Color4(0.1f, 0.1f, 0.1f, 1.0f));
      GL.Enable(EnableCap.DepthTest);

      _vertexBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

      _lightingShader = new Shader("data/shaders/shader.vert", "data/shaders/lighting.frag");
      _lampShader = new Shader("data/shaders/shader.vert", "data/shaders/shader.frag");

      {
         _vaoModel = GL.GenVertexArray();
         GL.BindVertexArray(_vaoModel);

         var positionLocation = _lightingShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         // Remember to change the stride as we now have 6 floats per vertex
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

         // We now need to define the layout of the normal so the shader can use it
         var normalLocation = _lightingShader.GetAttribLocation("aNormal");
         GL.EnableVertexAttribArray(normalLocation);
         GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
      }

      {
         _vaoLamp = GL.GenVertexArray();
         GL.BindVertexArray(_vaoLamp);

         var positionLocation = _lampShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
      }

      _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
      CursorState = CursorState.Grabbed;
   }

   protected override void OnUnload()
   {
      base.OnUnload();
   }

   protected override void OnUpdateFrame(FrameEventArgs e)
   {
      base.OnUpdateFrame(e);

      if (!IsFocused)
      {
         return;
      }

      var input = KeyboardState;

      const float cameraSpeed = 2.5f;
      const float sensitivity = 0.2f;

      if (input.IsKeyDown(Keys.W))
      {
         _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
      }
      if (input.IsKeyDown(Keys.S))
      {
         _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
      }
      if (input.IsKeyDown(Keys.A))
      {
         _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
      }
      if (input.IsKeyDown(Keys.D))
      {
         _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
      }
      if (input.IsKeyDown(Keys.Space))
      {
         _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
      }
      if (input.IsKeyDown(Keys.LeftShift))
      {
         _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
      }

      var mouse = MouseState;

      if (_firstMove)
      {
         _lastPos = new Vector2(mouse.X, mouse.Y);
         _firstMove = false;
      }
      else
      {
         var deltaX = mouse.X - _lastPos.X;
         var deltaY = mouse.Y - _lastPos.Y;
         _lastPos = new Vector2(mouse.X, mouse.Y);

         _camera.Yaw += deltaX * sensitivity;
         _camera.Pitch -= deltaY * sensitivity;
      }
   }

   protected override void OnRenderFrame(FrameEventArgs e)
   {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      // Draw the model/cube with the lighting shader
      GL.BindVertexArray(_vaoModel);

      _lightingShader.Use();

      // Matrix4.Identity is used as the matrix, since we just want to draw it at 0, 0, 0
      _lightingShader.SetMatrix4("model", Matrix4.Identity);
      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      //var objectColorNoAlpha = new Vector3(Color4.Gold.R, Color4.Gold.G, Color4.Gold.B);
      //_lightingShader.SetVector3("objectColor", objectColorNoAlpha);
      _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
      _lightingShader.SetVector3("lightPos", _lightPos);
      _lightingShader.SetVector3("viewPos", _camera.Position);

      _lightingShader.SetVector3("material.ambient", Pearl.Ambient);
      _lightingShader.SetVector3("material.diffuse", Pearl.Diffuse);
      _lightingShader.SetVector3("material.specular", Pearl.Specular);
      _lightingShader.SetFloat("material.shininess", Pearl.Shininess);

      _lightingShader.SetVector3("light.ambient", new Vector3(0.2f, 0.2f, 0.2f));
      _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f)); // darken the light a bit to fit the scene
      _lightingShader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));

      GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


      // Draw the lamp, this is mostly the same as for the model cube
      GL.BindVertexArray(_vaoLamp);

      _lampShader.Use();

      Matrix4 lampMatrix = Matrix4.CreateScale(0.2f); // We scale the lamp cube down a bit to make it less dominant
      lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

      _lampShader.SetMatrix4("model", lampMatrix);
      _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      //_camera.Position = new Vector3(-0.8f, -0.8f, 1.5f);
      SwapBuffers();
   }

   protected override void OnMouseDown(MouseButtonEventArgs e)
   {

      base.OnMouseDown(e);
   }

   protected override void OnMouseWheel(MouseWheelEventArgs e)
   {
      base.OnMouseWheel(e);

      _camera.Fov -= e.OffsetY;
   }

   protected override void OnKeyDown(KeyboardKeyEventArgs e)
   {
      if (!IsFocused)
      {
         return;
      }

      switch (e.Key)
      {
         case Keys.F:
         {
            if (IsFullscreen)
            {
               WindowState = WindowState.Normal;
               CursorState = CursorState.Normal;
            }
            else
            {
               WindowState = WindowState.Fullscreen;
               CursorState = CursorState.Grabbed;
            }
            break;
         }

         case Keys.Escape:
         {
            this.Close();
            break;
         }
      }

      // Временно двигаю свет.
      float moveSpeed = 0.1f;
      switch (e.Key)
      {
         //case Keys.Left:
         //{
         //   var vec = _lightPos - _camera.Position;
         //   var perp = new Vector3(-(vec.Z + vec.Y) / vec.X, 1.0f, 1.0f);
         //   _lightPos -= Vector3.Normalize(perp) * moveSpeed * new Vector3(1.0f, 0.0f, 1.0f);
         //   break;
         //}

         //case Keys.Right:
         //{
         //   var vec = _lightPos - _camera.Position;
         //   var perp = new Vector3(-(vec.Z + vec.Y) / vec.X, 1.0f, 1.0f);
         //   _lightPos += Vector3.Normalize(perp) * moveSpeed * new Vector3(1.0f, 0.0f, 1.0f);
         //   break;
         //}

         case Keys.Up:
         {
            _lightPos += Vector3.Normalize(_lightPos - _camera.Position) * moveSpeed;
            break;
         }

         case Keys.Down:
         {
            _lightPos -= Vector3.Normalize(_lightPos - _camera.Position) * moveSpeed;
            break;
         }
      }

      base.OnKeyDown(e);
   }
}