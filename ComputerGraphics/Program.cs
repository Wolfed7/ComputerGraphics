using System;

namespace CG_PR1;

class Program
{
   static void Main(string[] args)
   {
      using (Window game = new Window())
      {
         game.Run();
      }
   }
}
