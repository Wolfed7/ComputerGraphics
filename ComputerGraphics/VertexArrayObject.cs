using System;
using OpenTK.Graphics.OpenGL4;

namespace CG_PR1;

public sealed class VertexArrayObject : IDisposable
{
   private bool _disposed;

   public int Handle { get; private set; }
   public VertexBufferObject VertexBufferObject { get; private set; }

   public VertexArrayObject(VertexBufferObject vertexBufferObject)
   {
      _disposed = false;

      if (vertexBufferObject is null)
         throw new ArgumentNullException(nameof(vertexBufferObject));

      VertexBufferObject = vertexBufferObject;
      int vertexSizeInBytes = VertexPositionColor.VertexInfo.SizeInBytes;
      VertexAttribute[] attributes = VertexPositionColor.VertexInfo.VertexAttributes;

      Handle = GL.GenVertexArray();
      Bind();
      vertexBufferObject.Bind();

      for (int i = 0; i < attributes.Length; i++)
      {
         VertexAttribute attribute = attributes[i];
         GL.VertexAttribPointer(attribute.Index, attribute.ComponentCount, VertexAttribPointerType.Float, false, vertexSizeInBytes, attribute.Offset);
         GL.EnableVertexAttribArray(attribute.Index);
      }

      GL.BindVertexArray(0);
   }
   ~VertexArrayObject()
   {
      GC.SuppressFinalize(this);
      //VertexBufferObject.Dispose();
      //Dispose();
   }

   public void Bind()
   {
      GL.BindVertexArray(Handle);
   }

   public void Unbind()
   {
      GL.BindVertexArray(0);
   }

   public void Dispose()
   {
      if (_disposed is true)
         return;

      VertexBufferObject.Dispose();
      GL.BindVertexArray(0);
      GL.DeleteVertexArray(Handle);

      _disposed = true;
      //GC.SuppressFinalize(this);
   }
}
