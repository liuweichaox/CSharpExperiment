using System;

namespace MyNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {


            DateTime time2 = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            DateTime time3 = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
          
            Console.WriteLine(time2);
            Console.WriteLine(time3);
        }
    }
}
