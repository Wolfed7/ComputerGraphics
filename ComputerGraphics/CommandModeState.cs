using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;

namespace CG_PR1;

public abstract class CommandModeState
{
   public abstract void OnKeyDown(Scene scene, KeyboardKeyEventArgs e);
   public abstract void OnMouseDown(MouseState mouseState, Vector2i ClientSize, Scene scene, MouseButtonEventArgs e);
}

public class CommandModeEdit : CommandModeState
{
   public override void OnKeyDown(Scene scene, KeyboardKeyEventArgs e)
   {
      const float colorChangingSensivity = 0.05f;

      var currentGroupIndex = scene.CurrentGroup;
      var currentObjectIndex = scene.CurrentObject;
      var currentGroup = scene[currentGroupIndex];
      if (currentGroup is null)
      {
         return;
      }

      switch (e.Key)
      {
         // Changing CurrentObject in CurrentLayer. 

         case Keys.M:
         {
            if (currentGroup.ObjectIndeces.Where(obj => obj > currentObjectIndex).Count() > 0)
            {
               scene.CurrentObject = currentGroup.ObjectIndeces.Where(obj => obj > currentObjectIndex).Min();
            }
            break;
         }

         case Keys.N:
         {
            if (currentGroup.ObjectIndeces.Where(obj => obj < currentObjectIndex).Count() > 0)
            {
               scene.CurrentObject = currentGroup.ObjectIndeces.Where(obj => obj < currentObjectIndex).Max();
            }
            break;
         }

         // Changing CurrentLayer

         case Keys.L:
         {
            if (scene.GroupIndeces.Where(obj => obj > currentGroupIndex).Count() > 0)
            {
               scene.CurrentGroup = scene.GroupIndeces.Where(obj => obj > currentGroupIndex).Min();
               scene.CurrentObject = scene[scene.CurrentGroup]!.LastCreatedObject;
            }
            break;
         }

         case Keys.K:
         {
            if (scene.GroupIndeces.Where(obj => obj < currentGroupIndex).Count() > 0)
            {
               scene.CurrentGroup = scene.GroupIndeces.Where(obj => obj < currentGroupIndex).Max();
               scene.CurrentObject = scene[scene.CurrentGroup]!.LastCreatedObject;
            }
            break;
         }
         default:
            break;
      }



      if (currentObjectIndex is null)
      {
         return;
      }

      var currentObject = currentGroup[currentObjectIndex.Value];
      if (currentObject is null)
      {
         return;
      }

      switch (e.Key)
      {
         // Changing color of CurrentObject

         case Keys.R:
         {
            Color4 newColor = new()
            {
               R = e.Modifiers == KeyModifiers.Alt ?
                  Math.Max(currentObject.Color.R - colorChangingSensivity, 0.0f)
                  : Math.Min(currentObject.Color.R + colorChangingSensivity, 1.0f),
               G = currentObject.Color.G,
               B = currentObject.Color.B,
               A = currentObject.Color.A
            };

            currentObject.SetColor(newColor);
            break;
         }

         case Keys.G:
         {
            Color4 newColor = new()
            {
               R = currentObject.Color.R,
               G = e.Modifiers == KeyModifiers.Alt ?
                  Math.Max(currentObject.Color.G - colorChangingSensivity, 0.0f)
                  : Math.Min(currentObject.Color.G + colorChangingSensivity, 1.0f),
               B = currentObject.Color.B,
               A = currentObject.Color.A
            };

            currentObject.SetColor(newColor);
            break;
         }

         case Keys.B:
         {
            Color4 newColor = new()
            {
               R = currentObject.Color.R,
               G = currentObject.Color.G,
               B = e.Modifiers == KeyModifiers.Alt ?
                  Math.Max(currentObject.Color.B - colorChangingSensivity, 0.0f)
                  : Math.Min(currentObject.Color.B + colorChangingSensivity, 1.0f),
               A = currentObject.Color.A
            };

            currentObject.SetColor(newColor);
            break;
         }

         default:
            break;
      }
   }

   public override void OnMouseDown(MouseState mouseState, Vector2i ClientSize, Scene scene, MouseButtonEventArgs e)
   {
      return;
   }
}

public class CommandModeDraw : CommandModeState
{
   public override void OnKeyDown(Scene scene, KeyboardKeyEventArgs e)
   {

      switch (e.Key)
      {
         case Keys.Space:
         {
            scene.ResetTemporaryPoints();
            scene.CreateGroup();
            scene.CurrentGroup = scene.GroupIndeces.Max();
            scene.CurrentObject = null;
            break;
         }

         case Keys.Z:
         {
            if (e.Modifiers == KeyModifiers.Control)
            {
               var group = scene[scene.CurrentGroup];
               if (group is not null && group.LastCreatedObject is not null)
               {
                  scene.ResetTemporaryPoints();
                  group.DeleteObject(group.LastCreatedObject.Value);
                  scene.CurrentObject = group.LastCreatedObject;
               }
            }
            break;
         }

         default:
         {
            break;
         }
      }
   }

   public override void OnMouseDown(MouseState mouseState, Vector2i clientSize, Scene scene, MouseButtonEventArgs e)
   {
      switch (e.Button)
      {
         case MouseButton.Left:
         {
            float normalizedX = 2.0f * mouseState.X / clientSize.X - 1.0f;
            float normalizedY = 1.0f - 2.0f * mouseState.Y / clientSize.Y;
            scene.AddPoint(new VertexPositionColor(new Vector2(normalizedX, normalizedY), Color4.Black));
            break;
         }
         case MouseButton.Right:
         {
            scene.ResetTemporaryPoints();
            break;
         }

         default:
            break;
      }
   }
}

public class CommandModeView : CommandModeState
{
   public override void OnKeyDown(Scene scene, KeyboardKeyEventArgs e)
   {
      return;
   }

   public override void OnMouseDown(MouseState mouseState, Vector2i ClientSize, Scene scene, MouseButtonEventArgs e)
   {
      return;
   }
}
