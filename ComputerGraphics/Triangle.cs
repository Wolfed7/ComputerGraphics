using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

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
   public bool IsBoundaryVisible { get; set; }
   public bool IsVerticesVisible { get; set; }

   public Triangle(VertexPositionColor[] vertices, Color4 color)
   {
      IsBoundaryVisible = true;
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

      if (IsVerticesVisible is true ? IsVerticesVisible : oneTimeVisible)
      {
         GL.Uniform4(GL.GetUniformLocation(activeShaderHandle, "innerColor"), Color4.Black);
         GL.DrawArrays(PrimitiveType.Points, 0, 3);
      }

      if (IsBoundaryVisible is true)
      {
         GL.Uniform4(GL.GetUniformLocation(activeShaderHandle, "color"), Color4.Black);
         GL.DrawArrays(PrimitiveType.LineLoop, 0, 3);
      }

      GL.Uniform4(GL.GetUniformLocation(activeShaderHandle, "color"), Color4.White);


      _vao.Unbind();
   }

   public void Update(VertexPositionColor[] vertices)
   {
      Vertices = vertices;
      _vao.VertexBufferObject.Update(Vertices);
   }

   public void SetVertexColor(VertexNumber vertexNumber, Color4 color)
   {
      var index = (int)vertexNumber;
      Vertices[index] = new VertexPositionColor(Vertices[index].Position, color);
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
      var index = (int)vertexNumber;
      Vertices[index] = new VertexPositionColor(position, Vertices[index].Color);
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
         var newPosX = verticesPositions[i].X > 1.0f ? 1.0f : verticesPositions[i].X;
         var newPosY = verticesPositions[i].Y > 1.0f ? 1.0f : verticesPositions[i].Y;
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