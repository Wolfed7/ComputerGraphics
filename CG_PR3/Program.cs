using OpenTK.Windowing.Common;
using System;

namespace CG_PR3
{
   class Program
   {
      static void Main(string[] args)
      {
         using (Window window = new Window())
         {
            //window.UpdateFrequency = 60;
            window.VSync = VSyncMode.On;
            window.Run();
         }
      }
   }

}