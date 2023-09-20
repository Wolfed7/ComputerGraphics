using System;
using OpenTK.Graphics.OpenGL4;

namespace CG_PR1;

public sealed class VertexBufferObject : IDisposable
{
   private bool _disposed;

   public int Handle { get; private set; }
   public BufferUsageHint Hint { get; }

   public VertexBufferObject(VertexPositionColor[] vertices, BufferUsageHint hint)
   {
      _disposed = false;
      Hint = hint;

      Handle = GL.GenBuffer();
      Bind();
      GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPositionColor.VertexInfo.SizeInBytes, vertices, Hint);
      Unbind();
   }

   ~VertexBufferObject()
   {
      GC.SuppressFinalize(this);
      //Dispose();
   }

   public void Bind()
   {
      GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
   }

   public void Unbind()
   {
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
   }

   public void Dispose()
   {
      if (_disposed)
         return;

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.DeleteBuffer(Handle);

      _disposed = true;
      GC.SuppressFinalize(this);
   }

   public void Update(VertexPositionColor[] vertices)
   {
      Bind();
      GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPositionColor.VertexInfo.SizeInBytes, vertices, Hint);
      Unbind();
   }
}
