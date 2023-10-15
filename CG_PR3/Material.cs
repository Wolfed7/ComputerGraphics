using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR3
{
   public class Material
   {
      public readonly Vector3 Ambient;
      public readonly Vector3 Diffuse;
      public readonly Vector3 Specular;
      public readonly float Shininess;

      public static Material Silver
         => new (new Vector3(0.19225f),
                 new Vector3(0.50754f),
                 new Vector3(0.508273f),
                 0.4f * 128.0f);

      public static Material Chrome
         => new (new Vector3(0.25f),
                 new Vector3(0.4f),
                 new Vector3(0.774597f),
                 0.6f * 128.0f);
      public static Material Gold
         => new (new Vector3(0.24725f, 0.1995f, 0.0745f),
                 new Vector3(0.75164f, 0.60648f, 0.22648f),
                 new Vector3(0.628281f, 0.555802f, 0.366065f),
                 0.4f * 128.0f);

      public static Material BlackPlastic
         => new(new Vector3(0.0f),
                new Vector3(0.01f),
                new Vector3(0.5f),
                0.25f * 128.0f);

      public static Material Pearl
         => new(new Vector3(0.25f, 0.20725f, 0.20725f),
                new Vector3(1.0f, 0.829f, 0.829f),
                new Vector3(0.296648f, 0.296648f, 0.296648f),
                0.088f * 128.0f);

      public Material(Vector3 ambient,
                      Vector3 diffuse,
                      Vector3 specular,
                      float shininess)
      {
         Ambient = ambient;
         Diffuse = diffuse;
         Specular = specular;
         Shininess = shininess;
      }
   }
}
