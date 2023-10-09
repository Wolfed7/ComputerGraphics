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
      readonly Vector3 Ambient;
      readonly Vector3 Diffuse;
      readonly Vector3 Specular;

      readonly float Shininess;
   }

   public class Silver : Material
   {
      public readonly static Vector3 Ambient 
         = new Vector3(0.19225f, 0.19225f, 0.19225f);

      public readonly static Vector3 Diffuse 
         = new Vector3(0.50754f, 0.50754f, 0.50754f);

      public readonly static Vector3 Specular 
         = new Vector3(0.508273f, 0.508273f, 0.508273f);

      public readonly static float Shininess 
         = 0.4f * 128.0f;
   }

   public class Chrome : Material
   {
      public readonly static Vector3 Ambient
         = new Vector3(0.25f, 0.25f, 0.25f);

      public readonly static Vector3 Diffuse
         = new Vector3(0.4f, 0.4f, 0.4f);

      public readonly static Vector3 Specular
         = new Vector3(0.774597f, 0.774597f, 0.774597f);

      public readonly static float Shininess
         = 0.6f * 128.0f;
   }

   public class Gold : Material
   {
      public readonly static Vector3 Ambient
         = new Vector3(0.24725f, 0.1995f, 0.0745f);

      public readonly static Vector3 Diffuse
         = new Vector3(0.75164f, 0.60648f, 0.22648f);

      public readonly static Vector3 Specular
         = new Vector3(0.628281f, 0.555802f, 0.366065f);

      public readonly static float Shininess
         = 0.4f * 128.0f;
   }

   public class BlackPlastic : Material
   {
      public readonly static Vector3 Ambient
         = new Vector3(0.0f, 0.0f, 0.0f);

      public readonly static Vector3 Diffuse
         = new Vector3(0.01f, 0.01f, 0.01f);

      public readonly static Vector3 Specular
         = new Vector3(0.50f, 0.50f, 0.50f);

      public readonly static float Shininess
         = 0.25f * 128.0f;
   }

   public class Pearl : Material
   {
      public readonly static Vector3 Ambient
         = new Vector3(0.25f, 0.20725f, 0.20725f);

      public readonly static Vector3 Diffuse
         = new Vector3(1.0f, 0.829f, 0.829f);

      public readonly static Vector3 Specular
         = new Vector3(0.296648f, 0.296648f, 0.296648f);

      public readonly static float Shininess
         = 0.088f * 128.0f;
   }
}
