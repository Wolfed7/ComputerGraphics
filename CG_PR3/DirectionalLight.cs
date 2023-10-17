using OpenTK.Mathematics;

namespace CG_PR3
{
   public record struct DirectionalLight
   {
      public bool IsTurnedOn {  get; private set; }

      public Vector3 Direction { get; set; }
      public Vector3 Ambient { get; set; }
      public Vector3 Diffuse { get; set; }
      public Vector3 Specular { get; set; }

      public static DirectionalLight Default
         => new (new(-0.2f, -1.0f, -0.3f),
                 new(0.05f, 0.05f, 0.05f),
                 new(0.4f, 0.4f, 0.4f),
                 new(0.5f, 0.5f, 0.5f));

      public DirectionalLight(Vector3 direction,
                              Vector3 ambient,
                              Vector3 diffuse,
                              Vector3 specular)
      {
         IsTurnedOn = false;

         Direction = direction;
         Ambient = ambient;
         Diffuse = diffuse;
         Specular = specular;
      }

      public void SetAllUniforms(Shader lightingShader)
      {
         lightingShader.SetVector3("dirLight.direction", Direction);

         if (IsTurnedOn is false)
         {
            lightingShader.SetVector3("dirLight.ambient", new Vector3(0.0f));
            lightingShader.SetVector3("dirLight.diffuse", new Vector3(0.0f));
            lightingShader.SetVector3("dirLight.specular", new Vector3(0.0f));
         }
         else
         {
            lightingShader.SetVector3("dirLight.ambient", Ambient);
            lightingShader.SetVector3("dirLight.diffuse", Diffuse);
            lightingShader.SetVector3("dirLight.specular", Specular);
         }
      }

      public void Toggle(Shader lightingShader, bool isOn)
      {
         IsTurnedOn = isOn;
         if (IsTurnedOn is false)
         {
            lightingShader.SetVector3("dirLight.ambient",   new Vector3(0.0f));
            lightingShader.SetVector3("dirLight.diffuse",   new Vector3(0.0f));
            lightingShader.SetVector3("dirLight.specular",  new Vector3(0.0f));
         }
         else
         {
            lightingShader.SetVector3("dirLight.ambient", Ambient);
            lightingShader.SetVector3("dirLight.diffuse", Diffuse);
            lightingShader.SetVector3("dirLight.specular", Specular);
         }
      }
   }
}
