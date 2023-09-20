//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.GraphicsLibraryFramework;
//using OpenTK.Input;
//using OpenTK.Graphics.OpenGL4;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CG_LR1;

//public abstract class CommandModeState
//{
//   public abstract void CatchKeys(Command command, KeyboardKeyEventArgs e);
//   public abstract void CatchMouse(Command command, MouseButtonEventArgs e);
//}

//public class CommandModeEdit : CommandModeState
//{
//   public override void CatchKeys(Command command, KeyboardKeyEventArgs e)
//   {
//      if (e.Key == Keys.E)
//      {
//         command.ChangeMode(new CommandModeDraw());
//         command.ResetFreePoints();
//         return;
//      }
//   }

//   public override void CatchMouse(Command command, MouseButtonEventArgs e)
//   {
      
//   }
//}

//public class CommandModeDraw : CommandModeState
//{
//   public override void CatchKeys(Command command, KeyboardKeyEventArgs e)
//   {
//      if (e.Key == Keys.E)
//      {
//         command.ChangeMode(new CommandModeEdit());
//         command.ResetFreePoints();
//         return;
//      }

//      if (e.Key == Keys.Space)
//      {
//         command.CreateNewLayer();
//         return;
//      }

//      if (e.Modifiers == KeyModifiers.Control && e.Key == Keys.Z)
//      {
//         command.DeleteLastObject();
//         return;
//      }
//   }

//   public override void CatchMouse(Command command, MouseButtonEventArgs e)
//   {
//      command.ResetFreePointsWithCondition();

//      if (e.Button == MouseButton.Left)
//      {
//         MouseState mouseState = ;
//         Vector2 position = mouseState.Position;
//         int mouseX = mouseState.X;
//         int mouseY = mouseState.Y;
//      }

//      if (freePoints % 3 == 0)
//      {
//         _layers.Add(0, new Triangle(_boundaryPoints, Color4.Violet));
//      }

//      if (e.Button == MouseButton.Right)
//      {
//         _boundaryPoints = new Vertex[] { };
//         freePoints = 0;
//         vao.VertexBufferObject.Update(_boundaryPoints);
//      }
//   }
//}
