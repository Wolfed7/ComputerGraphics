using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_PR1;

public class Layers
{
   private Dictionary<uint, Group> _container;

   public List<uint> LayerIndeces => _container.Keys.ToList();

   public uint? lastCreated
   => LayerIndeces.Count == 0 ? null : LayerIndeces.Max();


   public Layers() 
   {
      _container = new();
      _container.Add(0, new Group());
   }

   public Group? this[uint i]
   {
      get
      {
         if (!_container.ContainsKey(i))
            return null;
         else
            return _container[i];
      }
   }

   public void Add(uint layerIndex, Triangle triangle)
   {
      _container[layerIndex].Add(triangle);
   }

   public void Remove(uint layerIndex, uint inGroupIndex) 
   {
      _container[layerIndex].Remove(inGroupIndex);
   }

   public void CreateLayer()
   {
      uint newLayerIndex = _container.Keys.Max() + 1;
      _container.Add(newLayerIndex,  new());
   }

   public void RemoveLayer(uint layerIndex)
   {
      _container.Remove(layerIndex);
   }
}
