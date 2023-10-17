namespace CG_PR3;

using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.WebRequestMethods;

public class GUI
{
   #region textures
   private string[] _modeItems
      => new string[]
      {
         "Material",
         "Texture",
      };

   private int _selectedModeItem;


   private string[] _textureNames
   => new string[]
   {
         "Modern crate",
         "Mercury",
         "Lavaaaaaa.jpg"
   };

   private int _selectedTextureNumber;
   #endregion textures

   #region Materials

   private string[] _materialNames
      => new string[]
      {
         "Gold",
         "Silver",
         "BlackPlastic",
         "Chrome",
         "Pearl",
      };

   private Material[] _materials
      => new Material[]
      {
         Material.Gold,
         Material.Silver,
         Material.BlackPlastic,
         Material.Chrome,
         Material.Pearl,
      };

   private int _selectedMaterialNumber;

   #endregion Materials

   #region Normal

   private string[] _normalMode
      => new string[]
      {
         "Default",
         "Smooth"
      };
   private int _currentNormalMode;

   #endregion

   private bool _isDirectionalLightOn;
   private bool _isPointLightsOn;

   private bool _isFlashlightOn;
   private string[] _flash
   => new string[]
   {
         "Default",
         "Laser",
   };
   int isFlashlightDefault;

   public GUI()
   {
      #region style
      ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(1.0f, 1.0f, 1.0f, 1.0f));
      ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0.1f, 0.1f, 0.1f, 0.8f));
      ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.0f, 0.0f, 0.0f, 1.0f));
      ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(0.0f, 0.0f, 0.0f, 0.6f));
      ImGui.PushStyleColor(ImGuiCol.CheckMark, new System.Numerics.Vector4(1.0f, 1.0f, 1.0f, 0.6f));
      #endregion style

      isFlashlightDefault = 1;
      _selectedModeItem = 0;
      _selectedMaterialNumber = 0;
      _currentNormalMode = 0;
      _selectedTextureNumber = 0;

      _isDirectionalLightOn = false;
      _isPointLightsOn = false;
      _isFlashlightOn = false;

   }

   public void DrawGui(Window window)
   {
      //ImGui.SetNextWindowSize(new System.Numerics.Vector2(120, 60));
      if (ImGui.Begin("Settings"))
      {
         ImGui.Text("Texturing mode");
         {
            _selectedModeItem = window.IsTextureModeUsed ? 1 : 0;
            bool selectionChanged = ImGui.Combo("0", ref _selectedModeItem, _modeItems, _modeItems.Length);
            if (selectionChanged)
            {
               window.IsTextureModeUsed = _selectedModeItem == 0 ? false : true;
            }
         }

         ImGui.Text("Normal computation mode");
         {
            _currentNormalMode = window.IsNormalsSmoothed ? 1 : 0;
            bool selectionChanged = ImGui.Combo("1", ref _currentNormalMode, _normalMode, _normalMode.Length);
            if (selectionChanged)
            {
               window.IsNormalsSmoothed = _currentNormalMode == 0 ? false : true;
               if (_currentNormalMode == 0)
               {
                  for (int i = 0; i < window.Cubes.Length; i++)
                  {
                     window.Cubes[i].SetDefaultNormals();
                  }
               }
               if (_currentNormalMode == 1)
               {
                  for (int i = 0; i < window.Cubes.Length; i++)
                  {
                     window.Cubes[i].SetSmoothedNormals();
                  }
               }
            }
         }

         _isDirectionalLightOn = window.DirectionalLight.IsTurnedOn;
         if (ImGui.Checkbox("Directional light on", ref _isDirectionalLightOn))
         {
            window.DirectionalLight.Toggle(window.LightingShader, _isDirectionalLightOn);
         }

         if (window.PointLights.Count > 0)
         {
            _isPointLightsOn = window.PointLights[0].IsTurnedOn;
         }
         if (ImGui.Checkbox("Point lights on", ref _isPointLightsOn))
         {
            for (int i = 0; i < window.PointLights.Count; i++)
            {
               window.PointLights[i].Toggle(window.LightingShader, i, _isPointLightsOn);
            }
         }


         ImGui.Text("Flashlight type");
         {
            isFlashlightDefault = window.IsFlashlightDefault ? 0 : 1;
            bool selectionChanged = ImGui.Combo("fl", ref isFlashlightDefault, _flash, _flash.Length);
            if (selectionChanged)
            {
               window.IsFlashlightDefault = isFlashlightDefault == 0 ? true : false;
               if (isFlashlightDefault == 0)
               {
                  window.Flashlight = SpotLight.Default;
                  window.Flashlight.UpdateAllUniforms(window.LightingShader);
               }
               if (isFlashlightDefault == 1)
               {
                  window.Flashlight = SpotLight.Laser;
                  window.Flashlight.UpdateAllUniforms(window.LightingShader);
               }
            }
         }
         _isFlashlightOn = window.Flashlight.IsTurnedOn;
         if (ImGui.Checkbox("Flashlight on", ref _isFlashlightOn))
         {
            window.Flashlight.Toggle(window.LightingShader, _isFlashlightOn);
         }

         bool isNormalsRendered = window.IsNormalsRendered;
         if (ImGui.Checkbox("Show normals", ref isNormalsRendered))
         {
            window.IsNormalsRendered = isNormalsRendered;
         }

         bool isOnlyFramesRendered = window.IsOnlyFrameRendered;
         if (ImGui.Checkbox("Frames only", ref isOnlyFramesRendered))
         {
            window.IsOnlyFrameRendered = isOnlyFramesRendered;
         }

         bool isPerspective = window.Camera.IsPerspective;
         if (ImGui.Checkbox("Perspective mode", ref isPerspective))
         {
            window.Camera.IsPerspective = isPerspective;
         }









         ImGui.End();
      }

      if (_selectedModeItem == 0)
      {
         SelectMaterial(window);
      }
      if (_selectedModeItem == 1)
      {
         SelectTexture(window);
      }

   }

   private void SelectMaterial(Window window)
   {
      //ImGui.SetNextWindowSize(new System.Numerics.Vector2(200, 300));
      if (ImGui.Begin("Materials"))
      {

         bool selectionChanged = ImGui.Combo("", ref _selectedMaterialNumber, _materialNames, _materialNames.Length);

         if (selectionChanged)
         {
            window.CubeMaterial = _materials[_selectedMaterialNumber];
         }
         ImGui.End();
      }
   }

   private void SelectTexture(Window window)
   {
      //ImGui.SetNextWindowSize(new System.Numerics.Vector2(200, 300));
      if (ImGui.Begin("Texture"))
      {

         bool selectionChanged = ImGui.Combo("2", ref _selectedTextureNumber, _textureNames, _textureNames.Length);

         if (selectionChanged)
         {
            if (_selectedTextureNumber == 0)
            {
               window.DiffuseMap = Texture.LoadFromFile("data/Resources/ModernCrate/modern-crate-diffuse.jpg");
               window.SpecularMap = Texture.LoadFromFile("data/Resources/ModernCrate/robbert-mouthaan-cratematerial-01-metallic-copy.jpg");
            }
            if (_selectedTextureNumber == 1)
            {
               window.DiffuseMap = Texture.LoadFromFile("data/Resources/2k_mercury.jpg");
               window.SpecularMap = Texture.LoadFromFile("data/Resources/2k_mercury.jpg");
            }
            if (_selectedTextureNumber == 2)
            {
               window.DiffuseMap = Texture.LoadFromFile("data/Resources/Lavaaaaaa.jpg");
               window.SpecularMap = Texture.LoadFromFile("data/Resources/Lavaaaaaa.jpg");
            }
         }
         ImGui.End();
      }
   }
}