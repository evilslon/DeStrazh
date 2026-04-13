using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStrazh
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                {
                    // Specify a file to read from and to create.
                    if (args.Length == 0)
                    {
                        Console.WriteLine("DeStrazh v0.2\r\n");
                        Console.WriteLine("Не указано откуда брать исходный и куда помещать раскодированный файл!\r\n");
                        Console.WriteLine(@"Пример использования программы:");
                        Console.WriteLine(@"destrazh.exe C:\strazh\inputfile.mp4 D:\arhiv\outputfile.mp4");
                        Console.WriteLine(@"Где C:\strazh\inputfile.mp4 - путь и имя закодированного файла с ПВР, а D:\arhiv\outputfile.mp4 - имя и путь сохраняемого раскодированного файла");
                        //Console.ReadLine();
                        Environment.Exit(0);
                    }
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Не указаны имя и путь сохраняемого раскодированного файла!");
                        //Console.ReadLine();
                        Environment.Exit(0);
                    }
                    //Console.WriteLine("\r\nДекодирую файл " + args[0] + " в файл " + args[1]);
                    string pathSource = args[0];
                    string pathNew = args[1];
                    Console.WriteLine("\r\nДекодирую файл " + pathSource + " в файл " + pathNew);

                    try
                    {
                        using (FileStream fsSource = new FileStream(pathSource,
                            FileMode.Open, FileAccess.Read))
                        {

                            // Read the source file into a byte array.
                            byte[] bytes = new byte[fsSource.Length];
                            int numBytesToRead = (int)fsSource.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead.
                                int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                                // Break when the end of the file is reached.
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                            numBytesToRead = bytes.Length;

                            // Проверим, точно ли это закодированный файл
                            if (bytes[0] != 0x31 && bytes[1] != 0x32 && bytes[2] != 0x33 && bytes[3] != 0x28)
                            {
                                Console.WriteLine("Файл уже декодирован или это не файл с ПВР!");
                                //Console.ReadLine();
                                Environment.Exit(0);
                            }

                            // Write the byte array to the other FileStream.                        
                            using (FileStream fsNew = new FileStream(pathNew,
                            FileMode.Create, FileAccess.Write))
                            {
                                for (int i = 0; i < 0x3ff; i++)
                                {
                                    int xoredIntByte1 = bytes[i] ^ 0x31;
                                    bytes[i] = (byte)xoredIntByte1;
                                    fsNew.WriteByte(bytes[i]);
                                    if (i == 0x3ff) break;
                                    i++;

                                    int xoredIntByte2 = bytes[i] ^ 0x32;
                                    bytes[i] = (byte)xoredIntByte2;
                                    fsNew.WriteByte(bytes[i]);
                                    if (i == 0x3ff) break;
                                    i++;

                                    int xoredIntByte3 = bytes[i] ^ 0x33;
                                    bytes[i] = (byte)xoredIntByte3;
                                    fsNew.WriteByte(bytes[i]);
                                    if (i == 0x3ff) break;
                                    i++;

                                    int xoredIntByte4 = bytes[i] ^ 0x34;
                                    bytes[i] = (byte)xoredIntByte4;
                                    fsNew.WriteByte(bytes[i]);
                                    if (i == 0x3ff) break;
                                    i++;

                                    int xoredIntByte5 = bytes[i] ^ 0x35;
                                    bytes[i] = (byte)xoredIntByte5;
                                    fsNew.WriteByte(bytes[i]);
                                    if (i == 0x3ff) break;
                                    i++;

                                    int xoredIntByte6 = bytes[i] ^ 0x36;
                                    bytes[i] = (byte)xoredIntByte6;
                                    fsNew.WriteByte(bytes[i]);
                                }

                                for (int i = 0x400; i < bytes.Length; i++)
                                {
                                    fsNew.WriteByte(bytes[i]);
                                }
                            }
                            //Скопируем время и дату создания файла в новый файл
                            File.SetCreationTime(pathNew, File.GetCreationTime(pathSource));
                            //А ещё и дату изменения до кучи, хз зачем...
                            File.SetLastWriteTime(pathNew, File.GetLastWriteTime(pathSource));
                        }
                    }
                    catch (FileNotFoundException ioEx)
                    {
                        Console.WriteLine(ioEx.Message);
                        Console.WriteLine("Папка назначения не существует. Создайте её и попробуйте заново!");
                    }
                }

            }
        }
    }
}
