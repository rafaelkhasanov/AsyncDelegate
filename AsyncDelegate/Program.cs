using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AsyncDelegate
{
    public delegate int BinaryOp(int x, int y);
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ASync Delegate Invocation");
            //Вывести идентификатор выполняющегося потока
            Console.WriteLine($"Main() invoked on thread {Thread.CurrentThread.ManagedThreadId}");

            //Вызвать Add() во вторичном потоке
            BinaryOp b = new BinaryOp(Add);
            IAsyncResult ar = b.BeginInvoke(10, 10, null, null);

            //Это сообщение продолжит выводиться до тех пор, пока не будет завершен метод Add()
            while (!ar.AsyncWaitHandle.WaitOne(1000, true))
            {
                Console.WriteLine($"The {Thread.CurrentThread.ManagedThreadId} wait when another thread end");
            }
            
            //Теперь известно, что метод Add() завершен
            int answer = b.EndInvoke(ar);
            Console.WriteLine("10 + 10 is {0}.", answer);
            Console.ReadLine();
        }

        static int Add(int x, int y)
        {
            //Вывести идентификатор выполняющегося потока
            Console.WriteLine($"Add() invoked on thread {Thread.CurrentThread.ManagedThreadId}");
            //Сделать паузу для моделирования длительной операции.
            Thread.Sleep(3000);
            Console.WriteLine("Подсчет завершен");
            return x + y;
        }
    }
}
