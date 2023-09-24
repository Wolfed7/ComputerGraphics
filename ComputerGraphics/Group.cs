using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR1;

public class Group
{
   private Dictionary<uint, Triangle> _objects;

   public List<uint> ObjectIndeces
      => _objects.Keys.Order().ToList();

   public uint Count
   => (uint)_objects.Count;

   public uint? LastCreatedObject
      => ObjectIndeces.Count == 0 ? null : ObjectIndeces.Max();

   public bool IsVisible { get; set; }

   //public bool IsTransparent { get; set; }


   public Group()
   {
      _objects = new();
      IsVisible = true;
   }

   public Triangle? this[uint i]
   {
      get
      {
         if (!_objects.ContainsKey(i))
            return null;
         else
            return _objects[i];
      }
   }

   public void Delete()
   {
      foreach (var obj in _objects)
         obj.Value.Dispose();
   }

   public void AddObject(Triangle triangle)
   {
      uint newInGroupIndex = Count == 0 ? 0 : _objects.Keys.Max() + 1;
      _objects[newInGroupIndex] = triangle;
   }

   public void DeleteObject(uint inGroupIndex)
   {
      _objects[inGroupIndex].Dispose();
      _objects.Remove(inGroupIndex);
   }
}
