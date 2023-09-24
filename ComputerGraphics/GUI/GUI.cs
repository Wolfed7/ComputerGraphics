namespace CG_PR1;

using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class GUI
{
   private bool isVerticeSelected;
   private int? selectedVertice;


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
      isVerticeSelected = false;

      selectedVertice = null;

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
         //Vector3 color = new Vector3(scene.BasicColor.R, scene.BasicColor.G, scene.BasicColor.B);
         //System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);

         System.Numerics.Vector3 color = new System.Numerics.Vector3(scene.BasicColor.R, scene.BasicColor.G, scene.BasicColor.B);
         System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         ImGui.ColorEdit3("color", ref newColor);

         scene.BasicColor = new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f);
         //if (ImGui.Button("Set color"))
         {
            foreach (var objectIndex in group.ObjectIndeces)
            {
               var triangle = group[objectIndex];
               triangle!.SetColor(scene.BasicColor);

               //Vector2[] vertexPositions = new Vector2[triangle.Vertices.Length];
               //for (int i = 0; i < vertexPositions.Length; i++)
               //{
               //   vertexPositions[i] = triangle.Vertices[i].Position;
               //}

               //for (int i = 0; i < triangle.Vertices.Length; i++)
               //{
               //   var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);


               //   {
               //      Vector2 newVertex = new Vector2(vert.X - 0.005f, vert.Y);
               //      triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
               //   } 
               //}
            }


            if (ImGui.BeginTable("Movement", 3))
            {
               Vector2 shift = new Vector2(0.0f, 0.0f);
               float sensitive = 0.005f;
               bool isDown = false;
               ImGui.TableNextColumn();
               ImGui.TableNextColumn();
               if (ImGui.ArrowButton("Up", ImGuiDir.Up))
               {
                  shift.Y += sensitive;
                  isDown = true;
               }

               ImGui.TableNextRow();
               ImGui.TableSetColumnIndex(0);







               //ImGui.PushButtonRepeat();

               if (ImGui.ArrowButton("Left", ImGuiDir.Left))
               {
                  shift.X -= sensitive;
                  isDown = true;
               }
               ImGui.TableNextColumn();
               if (ImGui.ArrowButton("Down", ImGuiDir.Down))
               {
                  shift.Y -= sensitive;
                  isDown = true;
               }
               ImGui.TableNextColumn();
               if (ImGui.ArrowButton("Right", ImGuiDir.Right))
               {
                  shift.X += sensitive;
                  isDown = true;
               }

               if (isDown is true)
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



         }



         //var R = scene.BasicColor.R;
         //var G = scene.BasicColor.G;
         //var B = scene.BasicColor.B;

         //Vector3 color = new Vector3(R, G, B);
         //System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         //ImGui.ColorEdit3("Group color", ref newColor);

         //foreach (var objectIndex in group)
         //{

         //}
         //triangle.SetColor(new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f));

         //for (int i = 0; i < triangle.Vertices.Length; i++)
         //{
         //   var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
         //   ImGui.DragFloat2((i / 3).ToString(), ref vert, 0.005f);
         //   Vector2 newVertex = new Vector2(vert.X, vert.Y);
         //   triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
         //}




         bool isVisible = group.IsVisible;
         ImGui.Checkbox("Visible", ref isVisible);
         group.IsVisible = isVisible;


         //ImGui.Text("Position");
         //Vector2[] vertexPositions = new Vector2[group.Vertices.Length];
         //for (int i = 0; i < vertexPositions.Length; i++)
         //{
         //   vertexPositions[i] = group.Vertices[i].Position;
         //}

         //for (int i = 0; i < vertexPositions.Length; i++)
         //{
         //   var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
         //   ImGui.DragFloat2((i / 3).ToString(), ref vert, 0.005f);
         //   Vector2 newVertex = new Vector2(vert.X, vert.Y);
         //   group.MoveVertice((Triangle.VertexNumber)i, newVertex);
         //}

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
               if (ImGui.Selectable(i.ToString(), isVerticeSelected))
               {
                  isVerticeSelected = !isVerticeSelected;
                  selectedVertice = i;
               }
            }
            ImGui.EndListBox();
         }


         ImGui.Text("Position");
         Vector2[] vertexPositions = new Vector2[triangle.Vertices.Length];
         for (int i = 0; i < vertexPositions.Length; i++)
         {
            vertexPositions[i] = triangle.Vertices[i].Position;
         }

         //for (int i = 0; i < triangle.Vertices.Length; i++)
         //{
         //   if (selectedVertice != null && selectedVertice == i)
         //   {
         //      ImGui.SetItemDefaultFocus();

         //      var R = triangle.Vertices[i].Color.R;
         //      var G = triangle.Vertices[i].Color.G;
         //      var B = triangle.Vertices[i].Color.B;

         //      Vector3 color = new Vector3(R, G, B);
         //      System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         //      ImGui.ColorEdit3("color", ref newColor);
               
         //      triangle.SetVertexColor((Triangle.VertexNumber)i, new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f));




         //      var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
         //      ImGui.DragFloat2((i / 3).ToString(), ref vert, 0.005f);
         //      Vector2 newVertex = new Vector2(vert.X, vert.Y);
         //      triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);

         //   }
         //}













         ImGui.LabelText("Fragment", "_");

         var R = triangle.Color.R;
         var G = triangle.Color.G;
         var B = triangle.Color.B;

         Vector3 color = new Vector3(R, G, B);
         System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         ImGui.ColorEdit3("color", ref newColor);
         triangle.SetColor(new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f));

         for (int i = 0; i < triangle.Vertices.Length; i++)
         {
            var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
            ImGui.DragFloat2((i / 3).ToString(), ref vert, 0.005f);
            Vector2 newVertex = new Vector2(vert.X, vert.Y);
            triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
         }








         //if (ImGui.BeginListBox("Vertices"))
         //{
         //   for (int i = 0; i < triangle.Vertices.Length; i++)
         //   {
         //      if (ImGui.Selectable(i.ToString(), isVerticeSelected[i]))
         //      {
         //         isVerticeSelected[i] = !isVerticeSelected[i];
         //      }

         //      if (isVerticeSelected[i])
         //      {
         //         ImGui.SetItemDefaultFocus();

         //         var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
         //         ImGui.DragFloat2((i / 3).ToString(), ref vert, 0.005f);
         //         Vector2 newVertex = new Vector2(vert.X, vert.Y);
         //         triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
         //      }
         //   }
         //   ImGui.EndListBox();
         //}


         bool isBoundaryVisible = triangle.IsBoundaryVisible;
         ImGui.Checkbox("Visible boundary", ref isBoundaryVisible);
         triangle.IsBoundaryVisible = isBoundaryVisible;

         //bool isVerticesVisible = triangle.IsVerticesVisible;
         //ImGui.Checkbox("Visible vertices", ref isVerticesVisible);
         //triangle.IsVerticesVisible = isVerticesVisible;

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