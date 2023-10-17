using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_PR3
{
   public class PointLight
   {
      public bool IsTurnedOn;

      private float[] _vertices =
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
      private int _vertexBufferObject;
      private int _vaoLamp;

      public Vector3 Position { get; set; }
      public Vector3 Ambient { get; set; }
      public Vector3 Diffuse { get; set; }
      public Vector3 Specular { get; set; }

      public readonly float Constant = 1.0f;

      public  float Linear { get; set; }
      public  float Quadratic { get; set; }

      public static PointLight Default
         => new PointLight(new Vector3(0.0f),
                           new Vector3(0.0f),
                           new Vector3(0.9f),
                           new Vector3(1.0f),
                           0.09f,
                           0.032f);

      public static PointLight Violet
          => new PointLight(new Vector3(0.0f),
                     new Vector3(0.0f),
                     new Vector3(0.8f, 0.0f, 0.6f),
                     new Vector3(0.9f, 0.0f, 0.7f),
                     0.09f,
                     0.032f);

      public static PointLight Sun
         => new PointLight(new Vector3(0.0f),
                     new Vector3(0.0f),
                     new Vector3(0.7f, 0.5f, 0.2f),
                     new Vector3(0.9f, 0.7f, 0.4f),
                     0.09f,
                     0.032f);

      
      public static PointLight Cyan
         => new PointLight(new Vector3(0.0f),
               new Vector3(0.0f),
               new Vector3(0.3f, 0.8f, 0.8f),
               new Vector3(0.4f, 0.9f, 0.9f),
               0.09f,
               0.032f);

      public PointLight(Vector3 position,
                        Vector3 ambient,
                        Vector3 diffuse,
                        Vector3 specular,
                        float linear,
                        float quadratic)
      {
         IsTurnedOn = false;

         Position = position;
         Ambient = ambient;
         Diffuse = diffuse;
         Specular = specular;
         Linear = linear;
         Quadratic = quadratic;
      }

      public void SetAllUniforms(Shader lightingShader, int lightNumber)
      {
         lightingShader.SetVector3($"pointLights[{lightNumber}].position", Position);

         lightingShader.SetFloat($"pointLights[{lightNumber}].constant", Constant);
         lightingShader.SetFloat($"pointLights[{lightNumber}].linear", Linear);
         lightingShader.SetFloat($"pointLights[{lightNumber}].quadratic", Quadratic);

         if (IsTurnedOn is false)
         {
            lightingShader.SetVector3($"pointLights[{lightNumber}].ambient", new Vector3(0.0f));
            lightingShader.SetVector3($"pointLights[{lightNumber}].diffuse", new Vector3(0.0f));
            lightingShader.SetVector3($"pointLights[{lightNumber}].specular", new Vector3(0.0f));
         }
         else
         {
            lightingShader.SetVector3($"pointLights[{lightNumber}].ambient", Ambient);
            lightingShader.SetVector3($"pointLights[{lightNumber}].diffuse", Diffuse);
            lightingShader.SetVector3($"pointLights[{lightNumber}].specular", Specular);
         }


      }

      public void Toggle(Shader lightingShader, int lightNumber, bool isOn)
      {
         this.IsTurnedOn = isOn;

         if (IsTurnedOn is false) 
         {
            lightingShader.SetVector3($"pointLights[{lightNumber}].ambient",  new Vector3(0.0f));
            lightingShader.SetVector3($"pointLights[{lightNumber}].diffuse",  new Vector3(0.0f));
            lightingShader.SetVector3($"pointLights[{lightNumber}].specular", new Vector3(0.0f));
         }
         else 
         {
            lightingShader.SetVector3($"pointLights[{lightNumber}].ambient", Ambient);
            lightingShader.SetVector3($"pointLights[{lightNumber}].diffuse", Diffuse);
            lightingShader.SetVector3($"pointLights[{lightNumber}].specular", Specular);
         }

      }

      //public void CreateLamp(Shader lampShader)
      //{

      //   _vertexBufferObject = GL.GenBuffer();
      //   GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
      //   GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

      //   _vaoLamp = GL.GenVertexArray();
      //   GL.BindVertexArray(_vaoLamp);

      //   var positionLocation = lampShader.GetAttribLocation("aPos");
      //   GL.EnableVertexAttribArray(positionLocation);
      //   GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

      //   GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      //   GL.BindVertexArray(0);
      //}

      //public void RenderLamp(Shader lampShader)
      //{
      //   GL.BindVertexArray(_vaoLamp);
      //   GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      //}
   }
}
