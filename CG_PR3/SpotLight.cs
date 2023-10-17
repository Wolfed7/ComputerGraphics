using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR3
{
   public record struct SpotLight
   {
      public bool IsTurnedOn {  get; private set; } 

      public Vector3 Position { get; set; }
      public Vector3 Direction { get; set; }
      public Vector3 Ambient { get; set; }
      public Vector3 Diffuse { get; set; }
      public Vector3 Specular { get; set; }

      public readonly float Constant = 1.0f;
      public float Linear { get; set; }
      public float Quadratic { get; set; }

      /// Углы в радианах:
      /// <param name="CutOff"> 
      /// <param name="OuterCutOff"> 
      public float CutOff { get; set; }
      public float OuterCutOff { get; set; }

      public static SpotLight Default
         => new (new Vector3(0.0f),
                 new Vector3(0.0f),
                 new Vector3(0.8f),
                 new Vector3(0.8f),
                 0.09f,
                 0.032f,
                 MathF.Cos(MathHelper.DegreesToRadians(12.5f)),
                 MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

      public static SpotLight Laser
   => new(new Vector3(0.0f),
           new Vector3(0.0f),
           new Vector3(0.8f, 0.0f, 0.0f),
           new Vector3(0.1f, 0.0f, 0.0f),
           0.09f,
           0.032f,
           MathF.Cos(MathHelper.DegreesToRadians(0.2f)),
           MathF.Cos(MathHelper.DegreesToRadians(0.4f)));

      public SpotLight(Vector3 position,
                       Vector3 ambient,
                       Vector3 diffuse,
                       Vector3 specular,
                       float linear,
                       float quadratic,
                       float cutOff,
                       float outerCutOff)
      {
         IsTurnedOn = false;

         Position = position;
         Ambient = ambient;
         Diffuse = diffuse;
         Specular = specular;
         Linear = linear;
         Quadratic = quadratic;
         CutOff = cutOff;
         OuterCutOff = outerCutOff;
      }

      public void UpdatePositionUniform(Shader lightingShader)
      {
         lightingShader.SetVector3("spotLight.position", Position);
      }

      public void UpdateDirectionUniform(Shader lightingShader)
      {
         lightingShader.SetVector3("spotLight.direction", Direction);
      }

      public void UpdateAllUniforms(Shader lightingShader)
      {
         UpdatePositionUniform(lightingShader);
         UpdateDirectionUniform(lightingShader);
         if (IsTurnedOn is false)
         {
            lightingShader.SetVector3("spotLight.ambient", new Vector3(0.0f));
            lightingShader.SetVector3("spotLight.diffuse", new Vector3(0.0f));
            lightingShader.SetVector3("spotLight.specular", new Vector3(0.0f));
         }
         else
         {
            lightingShader.SetVector3("spotLight.ambient", Ambient);
            lightingShader.SetVector3("spotLight.diffuse", Diffuse);
            lightingShader.SetVector3("spotLight.specular", Specular);
         }

         lightingShader.SetFloat("spotLight.constant", Constant);
         lightingShader.SetFloat("spotLight.linear", Linear);
         lightingShader.SetFloat("spotLight.quadratic", Quadratic);

         lightingShader.SetFloat("spotLight.cutOff", CutOff);
         lightingShader.SetFloat("spotLight.outerCutOff", OuterCutOff);
      }

      public void Toggle(Shader lightingShader, bool isTurnedOn)
      {
         IsTurnedOn = isTurnedOn;

         if (IsTurnedOn is false)
         {
            lightingShader.SetVector3("spotLight.ambient", new Vector3(0.0f));
            lightingShader.SetVector3("spotLight.diffuse", new Vector3(0.0f));
            lightingShader.SetVector3("spotLight.specular", new Vector3(0.0f));
         }
         else
         {
            lightingShader.SetVector3("spotLight.ambient", Ambient);
            lightingShader.SetVector3("spotLight.diffuse", Diffuse);
            lightingShader.SetVector3("spotLight.specular", Specular);
         }
      }
   }
}
