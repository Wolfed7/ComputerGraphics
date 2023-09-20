using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using static CG_PR1.Triangle;

namespace CG_PR1;

public class Triangle : IDisposable
{
   public enum VertexNumber
   {
      First = 0,
      Second = 1,
      Third = 2
   };

   public VertexArrayObject _vao;
   public VertexPositionColor[] Vertices { get; private set; }
   public Color4 Color { get; private set; }
   public bool IsFrameVisible { get; set; }
   public bool IsVerticesVisible { get; set; }

   public Triangle(VertexPositionColor[] vertices, Color4 color)
   {
      IsFrameVisible = true;
      IsVerticesVisible = false;
      Vertices = vertices; 
      Color = color;
      for (int i = 0; i < vertices.Length; i++)
      {
         Vertices[i] = new VertexPositionColor(vertices[i].Position, color);
      }

      _vao = new VertexArrayObject(new VertexBufferObject(vertices, BufferUsageHint.StaticDraw));
   }

   ~Triangle()
   {
      GC.SuppressFinalize(this);
   }

   public void Draw(bool oneTimeVisible = false)
   {
      int activeShaderHandle;
      GL.GetInteger(GetPName.CurrentProgram, out activeShaderHandle);
      _vao.Bind();

      GL.Uniform4(GL.GetUniformLocation(activeShaderHandle, "innerColor"), Color4.White);
      GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

      if (IsFrameVisible is true)
      {
         GL.Uniform4(GL.GetUniformLocation(activeShaderHandle, "color"), Color4.Black);
         GL.DrawArrays(PrimitiveType.LineLoop, 0, 3);
      }

      GL.Uniform4(GL.GetUniformLocation(activeShaderHandle, "color"), Color4.White);
      if (IsVerticesVisible is true ? IsVerticesVisible : oneTimeVisible)
      {
         GL.DrawArrays(PrimitiveType.Points, 0, 3);
      }

      _vao.Unbind();
   }

   public void Update(VertexPositionColor[] vertices)
   {
      Vertices = vertices;
      _vao.VertexBufferObject.Update(Vertices);
   }

   public void SetVertexColor(VertexNumber vertexNumber, Color4 color)
   {
      Vertices[(int)vertexNumber] = new VertexPositionColor(Vertices[(int)vertexNumber].Position, color);
      _vao.VertexBufferObject.Update(Vertices);
   }

   public void SetColor(Color4 color)
   {
      Color = color;
      for (int i = 0; i < Vertices.Length; i++)
         Vertices[i] = new VertexPositionColor(Vertices[i].Position, color);
      _vao.VertexBufferObject.Update(Vertices);
   }

   public void MoveVertice(VertexNumber vertexNumber, Vector2 position)
   {
      Vertices[(int)vertexNumber] = new VertexPositionColor(position, Vertices[(int)vertexNumber].Color);
      _vao.VertexBufferObject.Update(Vertices);
   }

   public void Move(Vector2[] verticesPositions)
   {
      if (verticesPositions.Length != Vertices.Length)
      {
         throw new ArgumentException("Wrong number of new positions.");
      }

      for (int i = 0; i < Vertices.Length; i++)
      {
         var newPosX = verticesPositions[i].X > 1.0f ? verticesPositions[i].X : 1.0f;
         var newPosY = verticesPositions[i].Y > 1.0f ? verticesPositions[i].X : 1.0f;
         Vector2 newPos = new Vector2(newPosX, newPosY);
         Vertices[i] = new VertexPositionColor(newPos, Vertices[i].Color);
      }
      _vao.VertexBufferObject.Update(Vertices);
   }

   public void Dispose() 
   {
      _vao.Dispose();
   }
}