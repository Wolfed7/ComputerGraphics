namespace CG_PR1;

using ImGuiNET;
using OpenTK.Mathematics;

public class GUI
{
   private ImGuiNET.ImGuiWindowFlags _windowFlags =
       ImGuiWindowFlags.NoDecoration |
       ImGuiWindowFlags.AlwaysAutoResize |
       ImGuiWindowFlags.NoNav |
       ImGuiWindowFlags.NoSavedSettings |
       ImGuiWindowFlags.NoFocusOnAppearing |
       ImGuiWindowFlags.NoMove;

   public GUI()
   {

   }

   public void DrawGui(Layers layers, ref uint currentLayer, ref uint? currentObject)
   {
      if (ImGui.BeginMainMenuBar())
      {
         if (ImGui.BeginMenu("Help"))
         {
            if (ImGui.MenuItem("Keybindings", "F1"))
            {
               if (ImGui.TreeNode("Mouse buttons"))
               {
                  ImGui.Text("Left mouse button - create primitive in current object");
                  ImGui.Text("Right mouse button - create new object");

                  ImGui.TreePop();
               }

               if (ImGui.TreeNode("Keyboard buttons"))
               {
                  ImGui.Text("E - change to edit mode");
                  ImGui.Text("R - change to view mode (set by default)");
               }
            }

            ImGui.EndMenu();
         }

         ImGui.EndMainMenuBar();
      }

      ImGui.SetNextWindowBgAlpha(0.5f);

      if (ImGui.Begin("Layer selector"))
      {
         if (ImGui.BeginListBox("Groups"))
         {
            for (int i = 0; i < layers.LayerIndeces.Count; i++)
            {
               bool isSelected = (currentLayer == layers.LayerIndeces[i]);

               if (ImGui.Selectable(layers.LayerIndeces[i].ToString(), isSelected))
               {
                  currentLayer = layers.LayerIndeces[i];
                  currentObject = layers[currentLayer].lastCreatedObject;
               }

               if (isSelected)
               {
                  ImGui.SetItemDefaultFocus();
               }
            }
            ImGui.EndListBox();
         }

         if (ImGui.BeginListBox("Triangles"))
         {
            ImGui.SetWindowSize(new System.Numerics.Vector2(60, 60), ImGuiCond.Always);
            for (int i = 0; i < layers[currentLayer].IngroupIndeces.Count; i++)
            {
               bool isSelected = (currentObject == layers[currentLayer].IngroupIndeces[i]);

               if (ImGui.Selectable(layers[currentLayer].IngroupIndeces[i].ToString(), isSelected))
               {
                  currentObject = layers[currentLayer].IngroupIndeces[i];
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

      if (currentObject is not null)
      {
         ObjectProperties(currentObject, layers[currentLayer][currentObject.Value]);
      }
   }

   private void ObjectProperties(uint? id, Triangle triangle)
   {
      ImGui.SetNextWindowBgAlpha(0.5f);

      if (ImGui.Begin("Object properties"))
      {
         ImGui.Text("Color");
         Vector3 color = new Vector3(triangle.Color.R, triangle.Color.G, triangle.Color.B);
         System.Numerics.Vector3 newColor = new System.Numerics.Vector3(color.X, color.Y, color.Z);
         ImGui.ColorEdit3("color", ref newColor);
         triangle.SetColor(new Color4(newColor.X, newColor.Y, newColor.Z, 1.0f));


         bool isVis = triangle.IsFrameVisible;
         ImGui.Checkbox("Visible boundary", ref isVis);
         triangle.IsFrameVisible = isVis;


         ImGui.Text("Position");
         Vector2[] vertexPositions = new Vector2[triangle.Vertices.Length];
         for (int i = 0; i < vertexPositions.Length; i++)
         {
            vertexPositions[i] = triangle.Vertices[i].Position;
         }

         for (int i = 0; i < vertexPositions.Length; i++)
         {
            var vert = new System.Numerics.Vector2(vertexPositions[i].X, vertexPositions[i].Y);
            ImGui.DragFloat2((i / 3).ToString(), ref vert, 0.005f);
            Vector2 newVertex = new Vector2(vert.X, vert.Y);
            triangle.MoveVertice((Triangle.VertexNumber)i, newVertex);
         }

         //bool deleteObject = ImGui.Button("Delete object");
         //if (deleteObject)
         //{
         //   _window._layers.Remove(cur)
         //}
      }
   }
}