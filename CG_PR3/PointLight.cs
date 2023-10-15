using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR3
{
   public record struct PointLight
   {
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
                           new Vector3(0.8f),
                           new Vector3(1.0f),
                           0.09f,
                           0.032f);


      public PointLight(Vector3 position,
                        Vector3 ambient,
                        Vector3 diffuse,
                        Vector3 specular,
                        float linear,
                        float quadratic)
      {
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
         lightingShader.SetVector3($"pointLights[{lightNumber}].ambient", Ambient);
         lightingShader.SetVector3($"pointLights[{lightNumber}].diffuse", Diffuse);
         lightingShader.SetVector3($"pointLights[{lightNumber}].specular", Specular);

         lightingShader.SetFloat($"pointLights[{lightNumber}].constant", Constant);
         lightingShader.SetFloat($"pointLights[{lightNumber}].linear", Linear);
         lightingShader.SetFloat($"pointLights[{lightNumber}].quadratic", Quadratic);
      }
   }
}
