﻿using System;
using OpenTK.Mathematics;

namespace CG_PR1;

public readonly struct VertexAttribute
{
   public readonly string Name;
   public readonly int Index;
   public readonly int ComponentCount;
   public readonly int Offset;

   public VertexAttribute(string name, int index, int componentCount, int offset)
   {
      Name = name;
      Index = index;
      ComponentCount = componentCount;
      Offset = offset;
   }
}

public sealed class VertexInfo
{
   public readonly Type Type;
   public readonly int SizeInBytes;
   public readonly VertexAttribute[] VertexAttributes;

   public VertexInfo(Type type, params VertexAttribute[] attributes)
   {
      Type = type;
      SizeInBytes = 0;
      VertexAttributes = attributes;

      for (int i = 0; i < VertexAttributes.Length; i++)
      {
         VertexAttribute attribute = VertexAttributes[i];
         SizeInBytes += attribute.ComponentCount * sizeof(float);
      }
   }
}

public readonly struct VertexPositionColor
{
   public readonly Vector2 Position;
   public readonly Color4 Color;

   public static readonly VertexInfo VertexInfo = new VertexInfo
   (
      typeof(VertexPositionColor),
      new VertexAttribute("Position", 0, 2, 0),
      new VertexAttribute("Color", 1, 4, 2 * sizeof(float))
   );

   public VertexPositionColor(Vector2 position, Color4 color)
   {
      Position = position;
      Color = color;
   }
}
