using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncMakeCake
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await MakeCakeAsync();
        }

        public static async Task MakeCakeAsync()
        {
            Task<bool> preheatTask = PreheatOvenAsync();//ここでもう実行開始
            AddCakeIngredients();//この間は並行で処理
            bool isPreheated = await preheatTask;//ここで結果を取り出す

            Console.WriteLine("");

            //そういうことかー!
            //いつもこういう使い方しかしていなかったから...
            //UIスレッドをブロックしない程度の使い方しか...
            //bool isPreheated2 = await PreheatOvenAsync();

            Task<bool> bakeCakeTask = BakeCakeAsync(isPreheated);
            AddFrostingIngredients();//ケーキ焼いている間に他のことやる
            Task<bool> coolFrostingTask = CoolFrostingAsync();
            bool isBaked = await bakeCakeTask;

            Console.WriteLine("");

            Task<bool> coolCakeTask = CoolCakeAsync(isBaked);

            bool cakeIsCooled = await coolCakeTask;
            bool frostingIsCooled = await coolFrostingTask;

            Console.WriteLine("Cake is served");

            ////Task.WhenAllの確認
            //List<Task> tasks = new List<Task>() {
            //    CoolCakeAsync(true),
            //    BakeCakeAsync(true),
            //    PreheatOvenAsync(),
            //    CoolFrostingAsync(),
            //};
            //await Task.WhenAll(tasks);
        }

        private static async Task<bool> CoolCakeAsync(bool isBaked)
        {
            Console.WriteLine("Cooling cake...");
            await Task.Delay(3000);
            Console.WriteLine("Cake is cooled");
            return true;
        }

        private static async Task<bool> CoolFrostingAsync()
        {
            Console.WriteLine("Cooling frosting...");
            await Task.Delay(5000);
            Console.WriteLine("Frosting is cooled");
            return true;
        }

        private static async Task<bool> PreheatOvenAsync()
        {
            Console.WriteLine("preheating oven...");
            await Task.Delay(3000);
            Console.WriteLine("oven is ready");
            return true;
        }

        private static void AddCakeIngredients()
        {
            Thread.Sleep(1000);
            Console.WriteLine("added cake mix");
            Thread.Sleep(1000);
            Console.WriteLine("added milk ");
            Thread.Sleep(1000);
            Console.WriteLine("added vegetable oil");

            Console.WriteLine("Cake ingredients mixed");
        }

        private static async Task<bool> BakeCakeAsync(bool isPreheated)
        {
            Console.WriteLine("baking cake...");
            await Task.Delay(5000);
            Console.WriteLine("cake is done baking");
            return true;
        }

        private static void AddFrostingIngredients()
        {
            Thread.Sleep(1000);
            Console.WriteLine("added cream cheese");
            Thread.Sleep(1000);
            Console.WriteLine("added milk");
            Thread.Sleep(1000);
            Console.WriteLine("added vegetable oil");
            Thread.Sleep(1000);
            Console.WriteLine("added eggs");

            Console.WriteLine("Frosting ingredients mixed");
        }
    }
}
