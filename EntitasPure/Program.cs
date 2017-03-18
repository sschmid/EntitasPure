using System;

namespace EntitasPure {

    class MainClass {

        public static void Main(string[] args) {
            var contexts = Contexts.sharedInstance;

            var entity = contexts.game.CreateEntity();
            entity.AddPosition(12, 34);
			Console.WriteLine(entity);


            Console.WriteLine("Done. Press any key...");
            Console.Read();
        }
    }
}
