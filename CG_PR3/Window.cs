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
using System.Diagnostics;

namespace CG_PR3;

public class Window : GameWindow
{
   private float[] _lampVertices =
   {
      // Positions          Normals              Texture coords
      -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.33f, 0.67f,     // Front face
       0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.67f, 0.67f,
       0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.67f, 1.0f,
       0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.67f, 1.0f,
      -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.33f, 1.0f,
      -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.33f, 0.67f,

      -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.33f, 0.0f,   // Back face
       0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.67f, 0.0f,
       0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.67f, 0.33f,
       0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.67f, 0.33f,
      -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.33f, 0.33f,
      -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.33f, 0.0f,

      -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.33f,   // Left face
      -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.33f, 0.33f,
      -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.33f, 0.0f,
      -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.33f, 0.0f,
      -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
      -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.33f,

       0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.67f, 0.33f,    // Right face
       0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.33f,
       0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
       0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
       0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.67f, 0.0f,
       0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.67f, 0.33f,

      -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.33f, 0.67f,   // Bottom face
       0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.67f, 0.67f,
       0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.67f, 0.33f,
       0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.67f, 0.33f,
      -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.33f, 0.33f,
      -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.33f, 0.67f,

      -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.67f, 0.33f,      // Top face
       0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.33f, 0.33f,
       0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.33f, 0.67f,
       0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.33f, 0.67f,
      -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.67f, 0.67f,
      -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.67f, 0.33f
   };

   public Cube[] Cubes { get; private set; }

   private readonly Vector3[] _cubePositions =
   {
      new (0.0f, 0.0f, 0.0f),
      new (0.0f, -6.0f, -4.0f),
      new (-4.5f, -2.2f, -10f),
      new (-5.8f, 5.0f, -6.3f),
      new (2.4f, -12.4f, -3.5f),
      new (-4.7f, 2.0f, -11.5f),
      new (13.3f, -2.0f, -15.5f),
      new (1.5f, 12.0f, -2.5f),
      new (1.5f, 0.2f, -1.5f),
      new (-1.3f, 1.0f, -1.5f)
   };


   private readonly Vector3[] _pointLightPositions =
   {
      new (0.3f, 1.3f, 2.0f),
      new (0.7f, 0.2f, -2.0f),
      new (2.3f, -3.3f, -4.0f),
      new (2.0f, 2.0f, 1.0f),

      new (0.0f, 0.0f, -3.0f)
   };

   public DirectionalLight DirectionalLight;
   public List<PointLight> PointLights;
   public SpotLight Flashlight;

   private int _vertexBufferObject;
   private int _vaoLamp;

   public Shader LampShader { get; private set; }

   public Shader LightingShader { get; private set; }

   public Shader NormalShader { get; private set; }

   public Texture DiffuseMap;
   public Texture SpecularMap;

   public Camera Camera;

   private bool _firstMove = true;
   private bool _iNeedMouse = false;

   private Vector2 _lastPos;

   private int _frameCount = 0;
   private ImGuiController _controller;
   private GUI _gui;
   private readonly Stopwatch _stopwatch = new Stopwatch();


   public bool IsTextureModeUsed {  get; set; }
   public bool IsNormalsRendered { get; set; }
   public bool IsNormalsSmoothed {  get; set; }
   public bool IsOnlyFrameRendered {  get; set; }
   public bool IsFlashlightDefault { get; set; }

   public Material CubeMaterial {  get; set; }


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
      _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
      _gui = new GUI();
   }

   protected override void OnResize(ResizeEventArgs e)
   {
      base.OnResize(e);

      GL.Viewport(0, 0, Size.X, Size.Y);
      Camera.Size = Size;
   }

   protected override void OnLoad()
   {
      base.OnLoad();

      //GL.Enable(EnableCap.CullFace);
      //GL.CullFace(CullFaceMode.Front);
      //GL.FrontFace(FrontFaceDirection.Ccw);

      IsVisible = true;

      GL.ClearColor(new Color4(0.8f, 0.8f, 0.8f, 1.0f));
      GL.Enable(EnableCap.DepthTest);

      GL.Enable(EnableCap.Blend);
      GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha, (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);

      LightingShader = new Shader("data/shaders/shader.vert", "data/shaders/lighting.frag");
      LampShader = new Shader("data/shaders/shader.vert", "data/shaders/shader.frag");
      NormalShader = new Shader("data/shaders/normalShader.vert", "data/shaders/normalShader.frag");

      IsTextureModeUsed = false;
      IsOnlyFrameRendered = false;
      IsNormalsSmoothed = false;
      IsFlashlightDefault = true;

      DirectionalLight = DirectionalLight.Default;
      DirectionalLight.SetAllUniforms(LightingShader);

      PointLights = new()
      {
         PointLight.Default,
         PointLight.Cyan,
         PointLight.Sun,
         PointLight.Violet,
      };

      for (int i = 0; i < PointLights.Count; i++)
      {
         PointLights[i].Position = _pointLightPositions[i];
      }

      Flashlight = SpotLight.Default;
      Flashlight.UpdateAllUniforms(LightingShader);

      Cubes = new Cube[]
      {
         new(LightingShader, NormalShader, _cubePositions[0]),

         new(LightingShader, NormalShader, _cubePositions[1]),
         new(LightingShader, NormalShader, _cubePositions[2]),
         new(LightingShader, NormalShader, _cubePositions[3]),
         new(LightingShader, NormalShader, _cubePositions[4]),
         new(LightingShader, NormalShader, _cubePositions[5]),
         new(LightingShader, NormalShader, _cubePositions[6]),
      };


      // Отрисовка ламп на месте позиций точечного освещения.
      {
         _vertexBufferObject = GL.GenBuffer();
         GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
         GL.BufferData(BufferTarget.ArrayBuffer, _lampVertices.Length * sizeof(float), _lampVertices, BufferUsageHint.StaticDraw);

         _vaoLamp = GL.GenVertexArray();
         GL.BindVertexArray(_vaoLamp);

         var positionLocation = LampShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

         GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
         GL.BindVertexArray(0);
      }

      CubeMaterial = Material.Gold;
      DiffuseMap = Texture.LoadFromFile("data/Resources/ModernCrate/modern-crate-diffuse.jpg");
      SpecularMap = Texture.LoadFromFile("data/Resources/ModernCrate/robbert-mouthaan-cratematerial-01-metallic-copy.jpg");

      Camera = new Camera(Vector3.UnitZ * 3, Size);
      CursorState = CursorState.Normal;
      _iNeedMouse = true;

   }

   protected override void OnUnload()
   {
      base.OnUnload();
   }

   protected override void OnUpdateFrame(FrameEventArgs e)
   {
      base.OnUpdateFrame(e);

      _frameCount++;
      _stopwatch.Start();

      if (_stopwatch.ElapsedMilliseconds >= 1000)
      {
         double fps = _frameCount / (_stopwatch.ElapsedMilliseconds / 1000.0);
         _frameCount = 0;
         _stopwatch.Restart();
         Title = "CG_PR3 FPS: " + Math.Round(fps);
      }


      if (!IsFocused)
      {
         return;
      }

      if (_iNeedMouse is true)
      {
         return;
      }

      var input = KeyboardState;

      const float cameraSpeed = 2.5f;
      const float sensitivity = 0.2f;

      if (input.IsKeyDown(Keys.W))
      {
         Camera.Position += Camera.Front * cameraSpeed * (float)e.Time; // Forward
      }
      if (input.IsKeyDown(Keys.S))
      {
         Camera.Position -= Camera.Front * cameraSpeed * (float)e.Time; // Backwards
      }
      if (input.IsKeyDown(Keys.A))
      {
         Camera.Position -= Camera.Right * cameraSpeed * (float)e.Time; // Left
      }
      if (input.IsKeyDown(Keys.D))
      {
         Camera.Position += Camera.Right * cameraSpeed * (float)e.Time; // Right
      }
      if (input.IsKeyDown(Keys.Space))
      {
         Camera.Position += Camera.Up * cameraSpeed * (float)e.Time; // Up
      }
      if (input.IsKeyDown(Keys.LeftShift))
      {
         Camera.Position -= Camera.Up * cameraSpeed * (float)e.Time; // Down
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

         Camera.Yaw += deltaX * sensitivity;
         Camera.Pitch -= deltaY * sensitivity;
      }
   }

   protected override void OnRenderFrame(FrameEventArgs e)
   {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      DiffuseMap.Use(TextureUnit.Texture0);
      SpecularMap.Use(TextureUnit.Texture1);
      LightingShader.Use();

      LightingShader.SetMatrix4("view", Camera.GetViewMatrix());
      LightingShader.SetMatrix4("projection", Camera.GetProjectionMatrix());

      LightingShader.SetVector3("viewPos", Camera.Position);


      LightingShader.SetBool("isTextureUsed", IsTextureModeUsed);
      if (IsTextureModeUsed is true)
      {
         LightingShader.SetInt("material.diffuseMap", 0);
         LightingShader.SetInt("material.specularMap", 1);
         LightingShader.SetFloat("material.shininess", 32.0f);
      }
      else
      {
         LightingShader.SetVector3("material.ambient", CubeMaterial.Ambient);
         LightingShader.SetVector3("material.diffuse", CubeMaterial.Diffuse);
         LightingShader.SetVector3("material.specular", CubeMaterial.Specular);
         LightingShader.SetFloat("material.shininess", CubeMaterial.Shininess);
      }

      LightingShader.SetInt("curLightsCount", PointLights.Count);
      for (int i = 0; i < PointLights.Count; i++)
      {
         PointLights[i].SetAllUniforms(LightingShader, i);
      }

      var scales = new List<float>()
      {
         0.5f,
         0.2f,
         0.3f,
         0.35f,
         0.1f,
         0.2f,
         0.4f,
      };

      var angles = new List<int>()
      {
         20,
         40,
         53,
         123,
         150,
         4,
         62,
      };

      for (int i = 0; i < Cubes.Length; i++)
      {
         Cubes[i].Scale(scales[i]);
         Cubes[i].Rotate(new Vector3(1.0f, 0.2f, 0.3f), angles[i]);
         Cubes[i].Render(IsOnlyFrameRendered);
      }

      // Смена позиции и направления фонарика на камере.
      {
         Flashlight.Position = Camera.Position;
         Flashlight.UpdatePositionUniform(LightingShader);
         Flashlight.Direction = Camera.Front;
         Flashlight.UpdateDirectionUniform(LightingShader);
      }

      // Отрисовка тела лампы.
      {
         GL.BindVertexArray(_vaoLamp);
         LampShader.Use();
         LampShader.SetMatrix4("view", Camera.GetViewMatrix());
         LampShader.SetMatrix4("projection", Camera.GetProjectionMatrix());

         for (int i = 0; i < PointLights.Count; i++)
         {
            Matrix4 lampMatrix = Matrix4.CreateScale(0.1f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(PointLights[i].Position);

            LampShader.SetMatrix4("model", lampMatrix);
            if (PointLights[i].IsTurnedOn is true)
            {
               LampShader.SetVector3("diffuse", PointLights[i].Diffuse);
            }
            else
            {
               LampShader.SetVector3("diffuse", 0.5f * PointLights[i].Diffuse);
            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
         }
         GL.BindVertexArray(0);
      }


      // Отрисовка нормалей.
      if (IsNormalsRendered)
      {
         NormalShader.Use();
         for (int i = 0; i < Cubes.Length; i++)
         {
            //Cubes[i].ScaleNormalsAsCube();
            Cubes[i].RotateNormalsAsCube();

            NormalShader.SetMatrix4("nview", Camera.GetViewMatrix());
            NormalShader.SetMatrix4("nprojection", Camera.GetProjectionMatrix());
            Cubes[i].DrawNormals();
         }
      }

      //Camera.Position = new Vector3(-2.0f, 1.0f, 3.0f);
      //Camera.Position = new(-0.7f, 0.5f, 2.0f);

      // Графический интерфейс.
      _controller.Update(this, (float)e.Time);
      _gui.DrawGui(this);
      _controller.Render();


      SwapBuffers();
   }

   protected override void OnMouseDown(MouseButtonEventArgs e)
   {
      if (e.Button == MouseButton.Right)
      {
         _iNeedMouse = !_iNeedMouse;
         _firstMove = true;

         CursorState = _iNeedMouse is true ? CursorState.Normal : CursorState.Grabbed;
      }
      base.OnMouseDown(e);
   }

   protected override void OnMouseWheel(MouseWheelEventArgs e)
   {
      base.OnMouseWheel(e);

      Camera.Fov -= e.OffsetY;
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
            WindowState = IsFullscreen == true ? WindowState.Normal : WindowState.Fullscreen;
            break;
         }

         case Keys.Escape:
         {
            this.Close();
            break;
         }

         // Переключение перспективы.

         case Keys.P:
         {
            Camera.IsPerspective = !Camera.IsPerspective;
            break;
         }

         // Переключение отображения примитивов: полный / рамка.

         case Keys.M:
         {
            IsOnlyFrameRendered = !IsOnlyFrameRendered;
            break;
         }

         // Включить / выключить фонарик.

         case Keys.H:
         {
            bool isTurnedOn = !Flashlight.IsTurnedOn;
            Flashlight.Toggle(LightingShader, isTurnedOn);
            break;
         }

         case Keys.T:
         {
            IsTextureModeUsed = !IsTextureModeUsed;
            break;
         }

         case Keys.N:
         {
            IsNormalsSmoothed = !IsNormalsSmoothed;

            for (int i = 0; i < Cubes.Length; i++)
            {
               if (Cubes[i].IsNormalDefault is false)
               {
                  Cubes[i].SetDefaultNormals();
               }
               else
               {
                  Cubes[i].SetSmoothedNormals();
               }
            }

            break;
         }

         case Keys.V:
         {
            IsNormalsRendered = !IsNormalsRendered;

            break;
         }

         case Keys.Z:
         {
            bool isTurnedOn = !DirectionalLight.IsTurnedOn;
            DirectionalLight.Toggle(LightingShader, isTurnedOn);

            break;
         }

         case Keys.X:
         {
            for (int i = 0; i < PointLights.Count; i++)
            {
               bool isTurnedOn = !PointLights[i].IsTurnedOn;
               PointLights[i].Toggle(LightingShader, i, isTurnedOn);
            }

            break;
         }

         case Keys.L:
         {
            Flashlight = SpotLight.Laser;
            Flashlight.UpdateAllUniforms(LightingShader);
            break;
         }

         case Keys.K:
         {
            Flashlight = SpotLight.Default;
            Flashlight.UpdateAllUniforms(LightingShader);
            break;
         }

      }

      base.OnKeyDown(e);
   }
}