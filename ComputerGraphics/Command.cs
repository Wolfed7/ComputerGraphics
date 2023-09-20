//using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.GraphicsLibraryFramework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CG_LR1;

//public class Command
//{
//   private ShaderProgram shader;
//   private ShaderProgram triangleShader;
//   private VertexArrayObject vao;
//   private Vertex[] _boundaryPoints;
//   private int freePoints;

//   private Layers _layers;

//   private uint currentLayer;
//   private uint currentObject;

//   private CommandModeState _mode;

//   public Command()
//   {
//      _layers = new Layers();
//      currentLayer = _layers.LayerIndeces.Max();
//      _boundaryPoints = new Vertex[] { };
//      freePoints = 0;
//      vao = new VertexArrayObject(new VertexBufferObject(_boundaryPoints, BufferUsageHint.StaticDraw));
//      shader = new ShaderProgram("data/shaders/pointShader.vert", "data/shaders/pointShader.frag");
//      triangleShader = new ShaderProgram("data/shaders/shader.vert", "data/shaders/shader.frag");
//   }

//   public void ShowUserClics()
//   {

//   }

//   public void ResetFreePointsWithCondition()
//   {
//      if (freePoints % 3 == 0)
//      {
//         ResetFreePoints();
//      }
//   }

//   public void UpdateFreePoints()
//   {
//      Array.Resize(ref _boundaryPoints, _boundaryPoints.Length + 1);
//      float normalizedX = 2.0f * MouseState.X / ClientSize.X - 1.0f;
//      float normalizedY = 1.0f - 2.0f * MouseState.Y / ClientSize.Y;
//      _boundaryPoints[^1] = new Vertex(new Vector2(normalizedX, normalizedY), Color4.White);
//      freePoints++;

//      vao.VertexBufferObject.Update(_boundaryPoints);
//   }

//   public void DeleteLastObject()
//   {
//      if (_layers[currentLayer] is not null && _layers[currentLayer]!.lastCreated is not null)
//         _layers.Remove(currentLayer, _layers[currentLayer]!.lastCreated!.Value);
//   }

//   public void ResetFreePoints()
//   {
//      _boundaryPoints = new Vertex[] { };
//      freePoints = 0;
//      vao.VertexBufferObject.Update(_boundaryPoints);
//   }

//   public void CreateNewLayer()
//   {
//      _layers.CreateLayer();
//      currentLayer = _layers.LayerIndeces.Max();
//   }

//   public void ChangeMode(CommandModeState newMode)
//   {
//      _mode = newMode;
//   }
//}
