using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR1;

public class Scene
{
   public enum CommandModes
   {
      DrawMode,
      EditMode,
      ViewMode
   }
   public CommandModes CurrentMode;

   private VertexArrayObject vao;
   private ShaderProgram _pointShader;
   private ShaderProgram _triangleShader;

   private Dictionary<uint, Group> _objectGroups;
   private VertexPositionColor[] _temporaryPoints; 
   public byte TemporaryPointsCount { get; set; }

   public List<uint> GroupIndeces 
      => _objectGroups.Keys.Order().ToList();
   public uint? LastCreatedGroup
      => GroupIndeces.Count == 0 ? null : GroupIndeces.Max();
   public uint CurrentGroup { get; set; }
   public uint? CurrentObject { get; set; }
   public Color4 BasicColor { get; set; }

   public CommandModeState CommandMode;

   public Scene() 
   {
      _objectGroups = new();
      _objectGroups.Add(0, new Group());


      _temporaryPoints = new VertexPositionColor[] { };
      vao = new VertexArrayObject(new VertexBufferObject(_temporaryPoints, BufferUsageHint.StaticDraw));
      _pointShader = new ShaderProgram("data/shaders/pointShader.vert", "data/shaders/pointShader.frag");
      _triangleShader = new ShaderProgram("data/shaders/triangleShader.vert", "data/shaders/triangleShader.frag");

      CurrentGroup = GroupIndeces.Max();
      CurrentObject = null;
      BasicColor = Color4.Blue;
      CommandMode = new CommandModeDraw();
      TemporaryPointsCount = 0;
   }

   public Group? this[uint i]
   {
      get
      {
         if (!_objectGroups.ContainsKey(i))
            return null;
         else
            return _objectGroups[i];
      }
   }

   public void RenderFrame()
   {
      _triangleShader.Use();
      foreach (var groupIndex in GroupIndeces.Where(index => _objectGroups[index].IsVisible == true))
      {
         foreach (var objectIndex in _objectGroups[groupIndex].ObjectIndeces)
         {
            if (groupIndex == CurrentGroup && objectIndex == CurrentObject)
            {
               continue;
            }
            // Always exists.
            _objectGroups[groupIndex][objectIndex]!.Draw();
         }
      }
      if (CurrentObject != null)
         _objectGroups[CurrentGroup][CurrentObject.Value]!.Draw(true);



      _pointShader.Use();
      vao.Bind();
      GL.DrawArrays(PrimitiveType.Points, 0, _temporaryPoints.Length);
      vao.Unbind();
   }

   public void CreateGroup()
   {
      uint newGroupIndex = _objectGroups.Count == 0 ? 0 : _objectGroups.Keys.Max() + 1;
      _objectGroups.Add(newGroupIndex, new());
   }

   public void DeleteGroup(uint groupIndex)
   {
      _objectGroups[groupIndex].Delete();
      _objectGroups.Remove(groupIndex);
   }

   public void Dispose()
   {
      _pointShader.Dispose();
      _triangleShader.Dispose();
   }

   public void ResetTemporaryPoints()
   {
      _temporaryPoints = new VertexPositionColor[] { };
      vao.VertexBufferObject.Update(_temporaryPoints);
   }

   public void AddPoint(VertexPositionColor vertexPositionColor)
   {
      if (_temporaryPoints.Length % 3 == 0)
      {
         ResetTemporaryPoints();
      }

      Array.Resize(ref _temporaryPoints, _temporaryPoints.Length + 1);
      _temporaryPoints[^1] = vertexPositionColor;
      vao.VertexBufferObject.Update(_temporaryPoints);

      if (_temporaryPoints.Length % 3 == 0)
      {
         _objectGroups[CurrentGroup].AddObject(new Triangle(_temporaryPoints, BasicColor));
         CurrentObject = _objectGroups[CurrentGroup].LastCreatedObject;
      }
   }
}
