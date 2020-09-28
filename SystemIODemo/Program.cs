/*
 C# 文件的输入与输出
一个 文件 是一个存储在磁盘中带有指定名称和目录路径的数据集合。当打开文件进行读写时，它变成一个 流。

从根本上说，流是通过通信路径传递的字节序列。有两个主要的流：输入流 和 输出流。输入流用于从文件读取数据（写操作），输出流用于向文件写入数据（读操作）。

C# I/O 类
System.IO 命名空间有各种不同的类，用于执行各种文件操作，如创建和删除文件、读取或写入文件，关闭文件等。

下表列出了一些 System.IO 命名空间中常用的非抽象类：

I/O 类	描述
BinaryReader	从二进制流读取原始数据。
BinaryWriter	以二进制格式写入原始数据。
BufferedStream	字节流的临时存储。
Directory	有助于操作目录结构。
DirectoryInfo	用于对目录执行操作。
DriveInfo	提供驱动器的信息。
File	有助于处理文件。
FileInfo	用于对文件执行操作。
FileStream	用于文件中任何位置的读写。
MemoryStream	用于随机访问存储在内存中的数据流。
Path	对路径信息执行操作。
StreamReader	用于从字节流中读取字符。
StreamWriter	用于向一个流中写入字符。
StringReader	用于读取字符串缓冲区。
StringWriter	用于写入字符串缓冲区。

FileStream 类
System.IO 命名空间中的 FileStream 类有助于文件的读写与关闭。该类派生自抽象类 Stream。

您需要创建一个 FileStream 对象来创建一个新的文件，或打开一个已有的文件。创建 FileStream 对象的语法如下：

FileStream <object_name> = new FileStream( <file_name>,
<FileMode Enumerator>, <FileAccess Enumerator>, <FileShare Enumerator>);
例如，创建一个 FileStream 对象 F 来读取名为 sample.txt 的文件：

FileStream F = new FileStream("sample.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
参数	描述
FileMode	
FileMode 枚举定义了各种打开文件的方法。FileMode 枚举的成员有：

Append：打开一个已有的文件，并将光标放置在文件的末尾。如果文件不存在，则创建文件。
Create：创建一个新的文件。如果文件已存在，则删除旧文件，然后创建新文件。
CreateNew：指定操作系统应创建一个新的文件。如果文件已存在，则抛出异常。
Open：打开一个已有的文件。如果文件不存在，则抛出异常。
OpenOrCreate：指定操作系统应打开一个已有的文件。如果文件不存在，则用指定的名称创建一个新的文件打开。
Truncate：打开一个已有的文件，文件一旦打开，就将被截断为零字节大小。然后我们可以向文件写入全新的数据，但是保留文件的初始创建日期。如果文件不存在，则抛出异常。
FileAccess	
FileAccess 枚举的成员有：Read、ReadWrite 和 Write。

FileShare	
FileShare 枚举的成员有：

Inheritable：允许文件句柄可由子进程继承。Win32 不直接支持此功能。
None：谢绝共享当前文件。文件关闭前，打开该文件的任何请求（由此进程或另一进程发出的请求）都将失败。
Read：允许随后打开文件读取。如果未指定此标志，则文件关闭前，任何打开该文件以进行读取的请求（由此进程或另一进程发出的请求）都将失败。但是，即使指定了此标志，仍可能需要附加权限才能够访问该文件。
ReadWrite：允许随后打开文件读取或写入。如果未指定此标志，则文件关闭前，任何打开该文件以进行读取或写入的请求（由此进程或另一进程发出）都将失败。但是，即使指定了此标志，仍可能需要附加权限才能够访问该文件。
Write：允许随后打开文件写入。如果未指定此标志，则文件关闭前，任何打开该文件以进行写入的请求（由此进程或另一进过程发出的请求）都将失败。但是，即使指定了此标志，仍可能需要附加权限才能够访问该文件。
Delete：允许随后删除文件。
 */using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
namespace MyNetDemo
{
    /// <summary>
    /// C#反射遍历对象类
    /// </summary>
    public class ForeachClass
    {
        /// <summary>
        /// C#反射遍历对象属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        public static Dictionary<string, string> ForeachClassProperties<T>(T model)
        {
            //List<string> list = new List<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                var name = item.Name;
                var value = item.GetValue(model).ToString();
                //list.Add(value);
                dictionary.Add(name, value);
            }
            //return list;
            return dictionary;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
           
            string path = @"C:\MyDirectory";
            string filePath = path + @"\MyFile.txt";
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            foreach (var item in driveInfo)
            {
                
                //使用自定义遍历方法
                var list = ForeachClass.ForeachClassProperties<DriveInfo>(item);
                foreach (var str in list)
                {
                    Console.WriteLine("key : "+str.Key+ "\t\tvalue : " + str.Value);
                }
            }
            //获取目录下所有的文件夹
            DirectoryInfo directory = new DirectoryInfo(@"c:\");
            FileSystemInfo[] directoryInfos = directory.GetFileSystemInfos();
            foreach (var item in directoryInfos)
            {
                var result=ForeachClass.ForeachClassProperties<FileSystemInfo>(item);
                foreach (var res in result)
                {
                    Console.WriteLine("key : " + res.Key + "\t\tvalue : " + res.Value);
                }
            }
         

            //删除该路径下所有的文件
            if (Directory.Exists(path))
            {
                foreach (string content in Directory.GetFileSystemEntries(path))
                {
                    if (Directory.Exists(content))
                    {
                        Directory.Delete(content, true);
                    }
                    else if (File.Exists(content))
                    {
                        File.Delete(content);
                    }
                }
            }
            //创建 MyDirectory 文件夹
            Directory.CreateDirectory(path);
            //创建 File.txt 文件
            //File.Create(filePath);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    //开始写入
                    sw.WriteLine("Hello World!\r\nOK?");

                }
            }

            //读取 File.txt 文件
            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
            {
                //逐行读取
                while (sr.Peek() > 0)
                {
                    Console.WriteLine(sr.ReadLineAsync().Result);
                }
                //一次全部读取
                //Console.WriteLine(sr.ReadToEndAsync().Result);
            }

            //追加文件内容
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("Append Context");
            }
        }
    }
}
