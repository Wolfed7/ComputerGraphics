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

public class Window : GameWindow
{
   private readonly float[] _vertices =
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
      
      -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,   0.0f, 0.33f,   // Left face
      -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,   0.33f, 0.33f,
      -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,   0.33f, 0.0f,
      -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,   0.33f, 0.0f,
      -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,   0.0f, 0.0f,
      -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,   0.0f, 0.33f,
      
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

   private readonly Vector3[] _cubePositions =
   {
      new Vector3(0.0f, 0.0f, 0.0f),
      //new Vector3(0.0f, 0.0f, -2.0f),
      //new Vector3(-1.5f, -2.2f, -2.5f),
      //new Vector3(-3.8f, -2.0f, -12.3f),
      //new Vector3(2.4f, -0.4f, -3.5f),
      //new Vector3(-1.7f, 3.0f, -7.5f),
      //new Vector3(1.3f, -2.0f, -2.5f),
      //new Vector3(1.5f, 2.0f, -2.5f),
      //new Vector3(1.5f, 0.2f, -1.5f),
      //new Vector3(-1.3f, 1.0f, -1.5f)
   };


   private readonly Vector3[] _pointLightPositions =
   {
      new Vector3(0.7f, 0.2f, 2.0f),
      //new Vector3(2.3f, -3.3f, -4.0f),
      //new Vector3(-4.0f, 2.0f, -12.0f),
      //new Vector3(0.0f, 0.0f, -3.0f)
   };

   private List<PointLight> _pointLights;
   private SpotLight _flashlight;

   private int _vertexBufferObject;

   private int _vaoModel;

   private int _vaoLamp;

   private Shader _lampShader;

   private Shader _lightingShader;

   private Texture _diffuseMap;

   private Texture _specularMap;

   private Camera _camera;

   private bool _firstMove = true;
   private bool _iNeedMouse = false;

   private Vector2 _lastPos;


   public Window(int width = 1920, int height = 1080, string title = "CG_PR3")
    : base(
          GameWindowSettings.Default,
          new NativeWindowSettings()
          {
             Title = title,
             Size = (width, height),
             WindowBorder = WindowBorder.Resizable,
             WindowState = WindowState.Normal,
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
      _camera.Size = Size;
   }

   protected override void OnLoad()
   {
      base.OnLoad();

      //var a = new Vector3(-0.5f, -0.5f, -0.5f);
      //var b = new Vector3(0.5f, -0.5f, -0.5f);
      //var c = new Vector3(0.5f, 0.5f, -0.5f);

      //var ab = b - a;
      //var ac = c - a;

      //var res = Vector3.Cross(ac, ab);

      IsVisible = true;

      GL.ClearColor(new Color4(0.1f, 0.1f, 0.1f, 1.0f));
      GL.Enable(EnableCap.DepthTest);

      GL.Enable(EnableCap.Blend);
      GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha, (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);


      _vertexBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

      _lightingShader = new Shader("data/shaders/shader.vert", "data/shaders/lighting.frag");
      _lampShader = new Shader("data/shaders/shader.vert", "data/shaders/shader.frag");




      _pointLights = new()
      {
         PointLight.Default with { Position = new  Vector3(0.3f, 1.3f, 2.0f) }
      };

      //_pointLights.Add(_pointLights[0] with
      //{
      //   Position = new Vector3(0.3f, 1.3f, -2.0f),
      //   Ambient = new Vector3(0.0f),
      //   Diffuse = new Vector3(0.8f, 0.0f, 0.0f),
      //   Specular = new Vector3(1.0f, 0.0f, 0.0f)
      //});

      //_pointLights.Add(_pointLights[0] with
      //{
      //   Position = new Vector3(0.3f, -1.3f, -1.0f),
      //   Ambient = new Vector3(0.0f),
      //   Diffuse = new Vector3(0.0f, 0.0f, 0.8f),
      //   Specular = new Vector3(0.0f, 0.0f, 1.0f)
      //});


      _flashlight = SpotLight.Default;
      _flashlight.UpdateAllUniforms(_lightingShader);





      {
         _vaoModel = GL.GenVertexArray();
         GL.BindVertexArray(_vaoModel);

         var positionLocation = _lightingShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

         var normalLocation = _lightingShader.GetAttribLocation("aNormal");
         GL.EnableVertexAttribArray(normalLocation);
         GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

         var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
         GL.EnableVertexAttribArray(texCoordLocation);
         GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
      }

      {
         _vaoLamp = GL.GenVertexArray();
         GL.BindVertexArray(_vaoLamp);

         var positionLocation = _lampShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
      }

      _diffuseMap = Texture.LoadFromFile("data/Resources/ModernCrate/modern-crate-diffuse.jpg");
      _specularMap = Texture.LoadFromFile("data/Resources/ModernCrate/robbert-mouthaan-cratematerial-01-metallic-copy.jpg");

      _camera = new Camera(Vector3.UnitZ * 3, Size);
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

      _diffuseMap.Use(TextureUnit.Texture0);
      _specularMap.Use(TextureUnit.Texture1);
      _lightingShader.Use();

      // Matrix4.Identity is used as the matrix, since we just want to draw it at 0, 0, 0

      //float left = -10.0f;
      //float right = 10.0f;
      //float bottom = -10.0f;
      //float top = 10.0f;
      //float near = 1.0f;
      //float far = 10.0f;

      //Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);

      Material material = Material.Silver;


      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("viewPos", _camera.Position);

      bool IsTexUsed = true;
      _lightingShader.SetBool("isTextureUsed", IsTexUsed);
      if (IsTexUsed is true)
      {
         _lightingShader.SetInt("material.diffuseMap", 0);
         _lightingShader.SetInt("material.specularMap", 1);
         _lightingShader.SetFloat("material.shininess", 32.0f);
      }
      else
      {
         _lightingShader.SetVector3("material.ambient", material.Ambient);
         _lightingShader.SetVector3("material.diffuse", material.Diffuse);
         _lightingShader.SetVector3("material.specular", material.Specular);
         _lightingShader.SetFloat("material.shininess", material.Shininess);
      }


      //var objectColorNoAlpha = new Vector3(Color4.Gold.R, Color4.Gold.G, Color4.Gold.B);
      //_lightingShader.SetVector3("objectColor", objectColorNoAlpha);
      // Directional light
      //GL.Uniform3(GL.GetUniformLocation(_lightingShader.Handle, "dirLight.direction"), new Vector3(-0.2f, -1.0f, -0.3f));
      _lightingShader.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
      _lightingShader.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
      _lightingShader.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
      _lightingShader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));

      _lightingShader.SetInt("curLightsCount", _pointLights.Count);
      for (int i = 0; i < _pointLights.Count; i++)
      {
         _pointLights[i].SetAllUniforms(_lightingShader, i);
      }


      for (int i = 0; i < _cubePositions.Length; i++)
      {
         Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
         float angle = 20.0f * i;
         //model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
         _lightingShader.SetMatrix4("model", model);

         GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }


      _flashlight.Position = _camera.Position;
      _flashlight.UpdatePositionUniform(_lightingShader);

      _flashlight.Direction = _camera.Front;
      _flashlight.UpdateDirectionUniform(_lightingShader);



      GL.BindVertexArray(_vaoLamp);
      _lampShader.Use();
      _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
      // We use a loop to draw all the lights at the proper position
      for (int i = 0; i < _pointLights.Count; i++)
      {
         Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
         lampMatrix = lampMatrix * Matrix4.CreateTranslation(_pointLights[i].Position);

         _lampShader.SetMatrix4("model", lampMatrix);
         _lampShader.SetVector3("diffuse", _pointLights[i].Diffuse);

         GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }



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
            _camera.IsPerspective = !_camera.IsPerspective;
            break;
         }

         // Переключение отображения примитивов: полный / рамка.

         case Keys.M:
         {
            GL.GetInteger(GetPName.PolygonMode, out var polygonMode);

            switch (polygonMode)
            {
               case (int)PolygonMode.Fill:
                  polygonMode = (int)PolygonMode.Line;
                  break;

               case (int)PolygonMode.Line:
                  polygonMode = (int)PolygonMode.Fill;
                  break;
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, (PolygonMode)polygonMode);
            break;
         }

         // Включить / выключить фонарик.

         case Keys.H:
         {
            bool isTurnedOn = !_flashlight.IsTurnedOn;
            _flashlight.Toggle(_lightingShader, isTurnedOn);
            break;
         }
      }

      base.OnKeyDown(e);
   }
}