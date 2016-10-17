using System;
using Entitas;

namespace EntitasPure {

    class MainClass {

        public static void Main(string[] args) {
            var pools = Pools.sharedInstance;
            pools.SetAllPools();

            var e = pools.core.CreateEntity()
                         .AddPosition(12, 34);


            Console.WriteLine(e);
            Console.WriteLine("Done. Press any key...");
            Console.Read();
        }
    }
}
