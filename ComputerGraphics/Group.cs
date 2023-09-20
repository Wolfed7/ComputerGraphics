using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR1;

public class Group
{
   private Dictionary<uint, Triangle> _container;

   public List<uint> IngroupIndeces 
      => _container.Keys.ToList();

   public uint? lastCreatedObject
      => IngroupIndeces.Count == 0 ? null : IngroupIndeces.Max();

   public Group() 
   {
      _container = new();
   }

   public Triangle? this[uint i]
   {
      get
      {
         if (!_container.ContainsKey(i))
            return null;
         else
            return _container[i];
      }
   }

public void Add(Triangle triangle)
   {
      uint newInGroupIndex = _container.Count == 0 ? 0 : _container.Keys.Max() + 1;
      _container[newInGroupIndex] = triangle;
   }

   public void Remove(uint inGroupIndex)
   {
      _container[inGroupIndex].Dispose();
      _container.Remove(inGroupIndex);
   }
}
