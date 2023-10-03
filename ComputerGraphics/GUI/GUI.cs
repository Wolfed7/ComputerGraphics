namespace CG_PR1;

using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class GUI
{
   private bool isVertexSelected;
   private Triangle.VertexNumber? currentVertice;
   private const int vertexDragger = 0;
   private const int fragmentDragger = 1;

   private string[] _modeItems;
   private int _selectedModeItem;


   private ImGuiNET.ImGuiWindowFlags _windowFlags =
       ImGuiWindowFlags.NoDecoration |
       ImGuiWindowFlags.AlwaysAutoResize |
       ImGuiWindowFlags.NoNav |
       ImGuiWindowFlags.NoSavedSettings |
       ImGuiWindowFlags.NoFocusOnAppearing |
       ImGuiWindowFlags.NoMove;

   public GUI()
   {
      isVertexSelected = false;

      currentVertice = null;

      _modeItems = new string[3]
      {
         Scene.CommandModes.DrawMode.ToString(),
         Scene.CommandModes.EditMode.ToString(),
         Scene.CommandModes.ViewMode.ToString()
      };

      _selectedModeItem = 0;

      ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(1.0f, 1.0f, 1.0f, 1.0f));
      ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0.1f, 0.1f, 0.1f, 0.8f));
      ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.0f, 0.0f, 0.0f, 1.0f));
      ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(0.0f, 0.0f, 0.0f, 0.6f));
      ImGui.PushStyleColor(ImGuiCol.CheckMark, new System.Numerics.Vector4(1.0f, 1.0f, 1.0f, 0.6f));
   }

   public void DrawGui(Scene scene)
   {

      if (ImGui.Begin("Mode"))
      {
         bool selectionChanged = ImGui.Combo("", ref _selectedModeItem, _modeItems, _modeItems.Length);

         if (selectionChanged)
         {
            switch (_selectedModeItem)
            {
               case (int)Scene.CommandModes.EditMode:
               {
                  scene.CurrentMode = Scene.CommandModes.EditMode;
                  scene.CommandMode = new CommandModeEdit();
                  scene.ResetTemporaryPoints();
                  break;
               }

               case (int)Scene.CommandModes.DrawMode:
               {
                  scene.CurrentMode = Scene.CommandModes.DrawMode;
                  scene.CommandMode = new CommandModeDraw();
                  scene.ResetTemporaryPoints();
                  break;
               }


               case (int)Scene.CommandModes.ViewMode:
               {
                  scene.CurrentMode = Scene.CommandModes.ViewMode;
                  scene.CommandMode = new CommandModeView();
                  scene.ResetTemporaryPoints();
                  break;
               }
               default:
                  break;
            }
         }

         if (scene.CurrentMode == Scene.CommandModes.DrawMode)
         {
            System.Numerics.Vector3 color = new System.Numerics.Vector3(scene.BasicColor.R, scene.BasicColor.G, scene.BasicColor.B);
            System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
            if (ImGui.ColorEdit3("color", ref newColor))
               scene.BasicColor = new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f);
         }

         ImGui.End();
      }

      if (scene.CurrentMode == Scene.CommandModes.ViewMode)
      {
         scene.CurrentObject = null;
         return;
      }

      if (ImGui.Begin("Object selector"))
      {
         //ImGui.Text("Groups");
         if (ImGui.BeginListBox("Groups", new System.Numerics.Vector2(100, 200)))
         {
            if (ImGui.Button("New group"))
            {
               scene.CreateGroup();
            }

            for (int i = 0; i < scene.GroupIndeces.Count; i++)
            {
               bool isSelected = (scene.CurrentGroup == scene.GroupIndeces[i]);

               if (ImGui.Selectable(scene.GroupIndeces[i].ToString(), isSelected))
               {
                  scene.CurrentGroup = scene.GroupIndeces[i];
                  scene.CurrentObject = scene[scene.CurrentGroup]!.LastCreatedObject;
               }

               if (isSelected)
               {
                  ImGui.SetItemDefaultFocus();
               }
            }
            ImGui.EndListBox();
         }

         //ImGui.Text("Triangles");
         if (ImGui.BeginListBox("Triangles", new System.Numerics.Vector2(100, 200)))
         {
            for (int i = 0; i < scene[scene.CurrentGroup]!.ObjectIndeces.Count; i++)
            {
               bool isSelected = (scene.CurrentObject == scene[scene.CurrentGroup]!.ObjectIndeces[i]);

               if (ImGui.Selectable(scene[scene.CurrentGroup]!.ObjectIndeces[i].ToString(), isSelected))
               {
                  scene.CurrentObject = scene[scene.CurrentGroup]!.ObjectIndeces[i];
                  scene.ResetTemporaryPoints();
               }

               if (isSelected)
               {
                  ImGui.SetItemDefaultFocus();
               }
            }

            ImGui.EndListBox();
         }

         ImGui.End();
      }

      if (scene.CurrentMode == Scene.CommandModes.EditMode)
      {
         GroupProperties(scene);

         if (scene.CurrentObject is not null)
         {
            ObjectProperties(scene);
         }
      }
   }

   private void GroupProperties(Scene scene)
   {
      var group = scene[scene.CurrentGroup];
      if (group is null)
      {
         return;
      }

      if (ImGui.Begin("Group properties"))
      {
         ImGui.Text("Color");

         System.Numerics.Vector3 color = new System.Numerics.Vector3(scene.BasicColor.R, scene.BasicColor.G, scene.BasicColor.B);
         System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         ImGui.ColorEdit3("color", ref newColor);

         scene.BasicColor = new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f);
         if (ImGui.Button("Set color"))
         {
            foreach (var objectIndex in group.ObjectIndeces)
            {
               var triangle = group[objectIndex];
               triangle!.SetColor(scene.BasicColor);
            }

         }



         if (ImGui.BeginTable("Movement", 3))
         {
            Vector2 shift = new Vector2(0.0f, 0.0f);
            const float sensitive = 0.005f;
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            if (ImGui.ArrowButton("Up", ImGuiDir.Up))
            {
               shift.Y += sensitive;
               MoveGroup();
            }

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);

            if (ImGui.ArrowButton("Left", ImGuiDir.Left))
            {
               shift.X -= sensitive;
               MoveGroup();
            }
            ImGui.TableNextColumn();
            if (ImGui.ArrowButton("Down", ImGuiDir.Down))
            {
               shift.Y -= sensitive;
               MoveGroup();
            }
            ImGui.TableNextColumn();
            if (ImGui.ArrowButton("Right", ImGuiDir.Right))
            {
               shift.X += sensitive;
               MoveGroup();
            }

            void MoveGroup()
            {
               foreach (var objectIndex in group.ObjectIndeces)
               {
                  var triangle = group[objectIndex];

                  Vector2[] vertexPositions = new Vector2[triangle.Vertices.Length];
                  for (int i = 0; i < vertexPositions.Length; i++)
                  {
                     vertexPositions[i] = triangle.Vertices[i].Position;
                  }

                  for (int i = 0; i < triangle.Vertices.Length; i++)
                  {
                     var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
                     Vector2 newVertex = new Vector2(vert.X + shift.X, vert.Y + shift.Y);
                     triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
                  }
               }
            }

            ImGui.EndTable();
         }

         bool isVisible = group.IsVisible;
         ImGui.Checkbox("Visible", ref isVisible);
         group.IsVisible = isVisible;

         if (ImGui.Button("Delete group"))
         {
            scene[scene.CurrentGroup]!.Delete();
            scene.DeleteGroup(scene.CurrentGroup);
            if (scene.GroupIndeces.Count == 0)
            {
               scene.CreateGroup();
            }
            scene.CurrentGroup = scene.LastCreatedGroup!.Value;
            scene.CurrentObject = scene[scene.CurrentGroup]!.LastCreatedObject;
            scene.ResetTemporaryPoints();
         }
      }
   }

   private void ObjectProperties(Scene scene)
   {
      var triangle = scene[scene.CurrentGroup]![scene.CurrentObject!.Value];
      if (triangle is null)
      {
         return;
      }


      if (ImGui.Begin("Object properties"))
      {
         ImGui.LabelText("Vertex","_");
         if (ImGui.BeginListBox("", new System.Numerics.Vector2(30, 60)))
         {
            for (int i = 0; i < triangle.Vertices.Length; i++)
            {
               isVertexSelected = (currentVertice == (Triangle.VertexNumber)i);
               if (ImGui.Selectable(i.ToString(), isVertexSelected))
               {
                  if (isVertexSelected)
                     currentVertice = null;
                  else
                     currentVertice = (Triangle.VertexNumber)i;
               }

               if (isVertexSelected)
               {
                  ImGui.SetItemDefaultFocus();
               }
            }

            ImGui.EndListBox();
            if (currentVertice is not null)
            {
               var vR = triangle.Vertices[(int)currentVertice.Value].Color.R;
               var vG = triangle.Vertices[(int)currentVertice.Value].Color.G;
               var vB = triangle.Vertices[(int)currentVertice.Value].Color.B;

               Vector3 vertexColor = new Vector3(vR, vG, vB);
               System.Numerics.Vector3 newVertexColor = new System.Numerics.Vector3(vertexColor.X, vertexColor.Y, vertexColor.Z);
               if(ImGui.ColorEdit3("Vertex color", ref newVertexColor))
               {
                  triangle.SetVertexColor(currentVertice.Value, new Color4(newVertexColor.X, newVertexColor.Y, newVertexColor.Z, 1.0f));
               }


               Vector2 vertexPosition = triangle.Vertices[(int)currentVertice.Value].Position;
               var vert = new System.Numerics.Vector2(vertexPosition.X, vertexPosition.Y);
               if (ImGui.DragFloat2(vertexDragger.ToString(), ref vert, 0.005f))
               {
                  Vector2 newVertex = new Vector2(vert.X, vert.Y);
                  triangle.MoveVertice(currentVertice.Value, newVertex);
               }
            }
         }


         ImGui.Text("Position");
         Vector2[] vertexPositions = new Vector2[triangle.Vertices.Length];
         for (int i = 0; i < vertexPositions.Length; i++)
         {
            vertexPositions[i] = triangle.Vertices[i].Position;
         }

         ImGui.LabelText("Fragment", "_");

         var R = triangle.Color.R;
         var G = triangle.Color.G;
         var B = triangle.Color.B;

         Vector3 color = new Vector3(R, G, B);
         System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         if(ImGui.ColorEdit3("color", ref newColor))
            triangle.SetColor(new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f));

         for (int i = 0; i < triangle.Vertices.Length; i++)
         {
            var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
            if(ImGui.DragFloat2(fragmentDragger.ToString(), ref vert, 0.005f))
            {
               Vector2 newVertex = new Vector2(vert.X, vert.Y);
               triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
            }
         }

         bool isBoundaryVisible = triangle.IsBoundaryVisible;
         ImGui.Checkbox("Visible boundary", ref isBoundaryVisible);
         triangle.IsBoundaryVisible = isBoundaryVisible;

         bool deleteObject = ImGui.Button("Delete object");
         if (deleteObject)
         {
            scene[scene.CurrentGroup]!.DeleteObject(scene.CurrentObject.Value);
            scene.CurrentObject = scene[scene.CurrentGroup]!.LastCreatedObject;
            scene.ResetTemporaryPoints();
         }
      }
   }
}