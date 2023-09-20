using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR1;

public class Shader
{
   private readonly int _vertexShader;
   private readonly int _fragmentShader;
   private bool _disposedValue = false;
   public int ProgramHandle { get; private set; }

   public Shader(string vertexPath, string fragmentPath)
   {
      string vertexShaderSource = File.ReadAllText(vertexPath);
      string fragmentShaderSource = File.ReadAllText(fragmentPath);

      _vertexShader = CreateShader(ShaderType.VertexShader, vertexShaderSource);
      _fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderSource);

      ProgramHandle = GL.CreateProgram();
      GL.AttachShader(ProgramHandle, _vertexShader);
      GL.AttachShader(ProgramHandle, _fragmentShader);
      GL.LinkProgram(ProgramHandle);
      GL.GetProgram(ProgramHandle, GetProgramParameterName.LinkStatus, out int success);
      if (success == 0)
      {
         string infoLog = GL.GetProgramInfoLog(ProgramHandle);
         Console.WriteLine(infoLog);
      }
      DeleteShader(ProgramHandle, _vertexShader);
      DeleteShader(ProgramHandle, _fragmentShader);
   }

   public void Use()
   {
      if (_disposedValue is true)
      {
         GL.UseProgram(ProgramHandle);
         _disposedValue = false;
      }
   }

   protected virtual void Dispose(bool disposing)
   {
      if (_disposedValue is false)
      {
         GL.DeleteProgram(ProgramHandle);

         _disposedValue = true;
      }
   }

   public int GetAttribLocation(string attribName)
   {
      return GL.GetAttribLocation(ProgramHandle, attribName);
   }

   ~Shader()
   {
      if (_disposedValue == false)
      {
         Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
      }
   }


   public void Dispose()
   {
      Dispose(true);
      GC.SuppressFinalize(this);
   }

   private int CreateShader(ShaderType shaderType, string shaderSource)
   {
      int shader = GL.CreateShader(shaderType);
      GL.ShaderSource(shader, shaderSource);
      GL.CompileShader(shader);

      GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
      if (success == 0)
      {
         string infoLog = GL.GetShaderInfoLog(shader);
         Console.WriteLine(infoLog);
      }

      return shader;
   }

   private void DeleteShader(int programHandle, int shaderHandle)
   {
      GL.DetachShader(programHandle, shaderHandle);
      GL.DeleteShader(shaderHandle);
   }
}
