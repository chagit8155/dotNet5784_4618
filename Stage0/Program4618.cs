namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome4618();
            Console.ReadKey();
        }
        static partial void WelcomeYYYY();

        private static void Welcome4618()
        {
            Console.WriteLine("Enter your name: ");
            string? name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to first console application", name);

        }
    }
}