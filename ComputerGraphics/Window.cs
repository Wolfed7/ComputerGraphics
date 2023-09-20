using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Collections.Immutable;

namespace CG_PR1;

public class Window : GameWindow
{
   public Layers _layers;
   private VertexPositionColor[] _boundaryPoints;
   private int freePoints = 0;
   private VertexBufferObject vbo;
   private VertexArrayObject vao;
   private bool _drawingMode;
   private bool _editMode;

   Color4 basicColor;
   private uint currentLayer;
   private uint? currentObject;

   ShaderProgram shader;
   ShaderProgram triangleShader;

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
      _layers = new Layers();
      CenterWindow();
      currentLayer = _layers.LayerIndeces.Max();
      currentObject = null;
      basicColor = Color4.Gray;

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
      _drawingMode = true;
      _editMode = false;
      IsVisible = true;

      GL.Enable(EnableCap.Blend);
      GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha, (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);
      GL.PointSize(10.0f);
      GL.Enable(EnableCap.LineSmooth);
      GL.ClearColor(new Color4(1.0f, 1.0f, 1.0f, 1.0f));

      _boundaryPoints = new VertexPositionColor[] { };
      vbo = new VertexBufferObject(_boundaryPoints, BufferUsageHint.StaticDraw);
      vao = new VertexArrayObject(vbo);
      shader = new ShaderProgram("data/shaders/pointShader.vert", "data/shaders/pointShader.frag");
      triangleShader = new ShaderProgram("data/shaders/shader.vert", "data/shaders/shader.frag");

      base.OnLoad();
   }

   protected override void OnUnload()
   {
      shader.Dispose();
      triangleShader.Dispose();
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

      GL.UseProgram(triangleShader.Handle);
      foreach (var layerIndex in _layers.LayerIndeces)
      {
         foreach (var inGroupIndex in _layers[layerIndex]!.IngroupIndeces)
         {
            if (layerIndex == currentLayer && inGroupIndex == currentObject)
               continue;
            //else
            //   GL.Uniform4(GL.GetUniformLocation(triangleShader.Handle, "color"), new Vector4(0.0f, 1.0f, 1.0f, 1.0f));
            _layers[layerIndex][inGroupIndex]!.Draw();
         }
      }
      if(currentObject != null)
         _layers[currentLayer][currentObject.Value]!.Draw(true);



      GL.UseProgram(shader.Handle);
      vao.Bind();
      GL.DrawArrays(PrimitiveType.Points, 0, freePoints);
      vao.Unbind();


      _gui.DrawGui(_layers, ref currentLayer, ref currentObject);
      _controller.Render();

      SwapBuffers();
      base.OnRenderFrame(args);
   }

   protected override void OnMouseDown(MouseButtonEventArgs e)
   {
      if (_drawingMode is false)
      {
         return;
      }

      if (freePoints % 3 == 0)
      {
         _boundaryPoints = new VertexPositionColor[] { };
         freePoints = 0;
      }

      if (e.Button == MouseButton.Left)
      {

         Array.Resize(ref _boundaryPoints, _boundaryPoints.Length + 1);
         float normalizedX = 2.0f * MouseState.X / ClientSize.X - 1.0f;
         float normalizedY = 1.0f - 2.0f * MouseState.Y / ClientSize.Y;
         _boundaryPoints[^1] = new VertexPositionColor(new Vector2(normalizedX, normalizedY), Color4.Black);
         freePoints++;

         vao.VertexBufferObject.Update(_boundaryPoints);
      }

      if (freePoints % 3 == 0)
      {
         _layers.Add(currentLayer, new Triangle(_boundaryPoints, basicColor));
         //var temp = _layers[currentLayer].IngroupIndeces;
         currentObject = _layers[currentLayer]!.lastCreatedObject;
      }

      if (e.Button == MouseButton.Right)
      {
         _boundaryPoints = new VertexPositionColor[] { };
         freePoints = 0;
         vao.VertexBufferObject.Update(_boundaryPoints);
      }

      base.OnMouseDown(e);
   }

   protected override void OnKeyDown(KeyboardKeyEventArgs e)
   {
      void ClearFreePoints()
      {
         _boundaryPoints = new VertexPositionColor[] { };
         freePoints = 0;
         vao.VertexBufferObject.Update(_boundaryPoints);
      }

      if (e.Key == Keys.F)
      {
         WindowState = IsFullscreen ? WindowState.Normal : WindowState.Fullscreen;
      }

      if (e.Key == Keys.Escape)
      {
         Close();
      }

      if (e.Key == Keys.E)
      {
         _drawingMode = !_drawingMode;
         _editMode = !_editMode;
         ClearFreePoints();
      }

      if (_drawingMode is true)
      {

         if (e.Key == Keys.Space) 
         {
            ClearFreePoints();
            _layers.CreateLayer();
            currentLayer = _layers.LayerIndeces.Max();
            currentObject = null;
         }

         if (e.Modifiers == KeyModifiers.Control && e.Key == Keys.Z)
         {
            if (_layers[currentLayer] is not null && _layers[currentLayer]!.lastCreatedObject is not null)
            {
               ClearFreePoints();
               _layers.Remove(currentLayer, _layers[currentLayer]!.lastCreatedObject!.Value);
               currentObject = _layers[currentLayer]!.lastCreatedObject;

               //if (currentLayer != 0 && currentObject is null)
               //{
               //   _layers.RemoveLayer(currentLayer);
               //}
            }
         }
      }

      if (_editMode is true)
      {
         // CHANGING CURRENT OBJECT IN CURRENT LAYER 

         if (e.Key == Keys.M)
         {
            if (_layers[currentLayer]!.IngroupIndeces.Where(obj => obj > currentObject).Count() > 0)
            {
               currentObject = _layers[currentLayer]!.IngroupIndeces.Where(obj => obj > currentObject).Min();
            }
         }

         if (e.Key == Keys.N)
         {
            if (_layers[currentLayer]!.IngroupIndeces.Where(obj => obj < currentObject).Count() > 0)
            {
               currentObject = _layers[currentLayer]!.IngroupIndeces.Where(obj => obj < currentObject).Max();
            }
         }

         // CHANGING CURRENT LAYER

         if (e.Key == Keys.L)
         {
            if (_layers.LayerIndeces.Where(obj => obj > currentLayer).Count() > 0)
            {
               currentLayer = _layers.LayerIndeces.Where(obj => obj > currentLayer).Min();
               currentObject = _layers[currentLayer]!.lastCreatedObject;
            }
         }

         if (e.Key == Keys.K)
         {
            if (_layers.LayerIndeces.Where(obj => obj < currentLayer).Count() > 0)
            {
               currentLayer = _layers.LayerIndeces.Where(obj => obj < currentLayer).Max();
               currentObject = _layers[currentLayer]!.lastCreatedObject;
            }
         }

         // CHANGING COLOR OF CURRENT OBJECT

         if (currentObject is not null)
         {
            if (e.Key == Keys.R)
            {
               var tempTriangle = _layers[currentLayer]![currentObject!.Value];
               if (e.Modifiers == KeyModifiers.Alt)
               {
                  tempTriangle.SetColor(new Color4(Math.Max(tempTriangle.Color.R - 0.1f, 0.0f), tempTriangle.Color.G, tempTriangle.Color.B, tempTriangle.Color.A));
               }
               else
               {
                  tempTriangle.SetColor(new Color4(Math.Min(tempTriangle.Color.R + 0.1f, 1.0f), tempTriangle.Color.G, tempTriangle.Color.B, tempTriangle.Color.A));
               }
            }

            if (e.Key == Keys.G)
            {
               var tempTriangle = _layers[currentLayer]![currentObject!.Value];
               if (e.Modifiers == KeyModifiers.Alt)
               {
                  tempTriangle.SetColor(new Color4(tempTriangle.Color.R, Math.Max(tempTriangle.Color.G - 0.1f, 0.0f), tempTriangle.Color.B, tempTriangle.Color.A));
               }
               else
               {
                  tempTriangle.SetColor(new Color4(tempTriangle.Color.R, Math.Min(tempTriangle.Color.G + 0.1f, 1.0f), tempTriangle.Color.B, tempTriangle.Color.A));
               }
            }

            if (e.Key == Keys.B)
            {
               var tempTriangle = _layers[currentLayer]![currentObject!.Value];
               if (e.Modifiers == KeyModifiers.Alt)
               {
                  tempTriangle.SetColor(new Color4(tempTriangle.Color.R, tempTriangle.Color.G, Math.Max(tempTriangle.Color.B - 0.1f, 0.0f), tempTriangle.Color.A));
               }
               else
               {
                  tempTriangle.SetColor(new Color4(tempTriangle.Color.R, tempTriangle.Color.G, Math.Min(tempTriangle.Color.B + 0.1f, 1.0f), tempTriangle.Color.A));
               }
            }
         }
      }



      base.OnKeyDown(e);
   }
}




