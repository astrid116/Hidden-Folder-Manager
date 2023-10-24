using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;  
using static System.Net.Mime.MediaTypeNames;

namespace HiddenFolder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime currentDateTime = DateTime.Now;
            string input;
            string path;
            string dirname;
            string configDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (!File.Exists(configDir + "\\config"))
            {
                File.Create(configDir + "\\config");
                DirectoryInfo di = new DirectoryInfo(configDir + "\\config");
                di.Attributes = FileAttributes.Hidden;
                Console.WriteLine("Succesfully initialized config file!");
            }

            Console.WriteLine("Welcome to Hidden Folder Manager v1.0!");
            while (true)
            {
                Console.Write(">");
                input = Console.ReadLine();
                if (input == "c" || input == "create")
                {
                    Console.WriteLine("Specify the path. Leave blank if you want to use the current directory.");
                    Console.Write(">");
                    path = Console.ReadLine();
                    Console.WriteLine("Name:");
                    Console.Write(">");
                    dirname = Console.ReadLine();
                    try
                    {
                        if (Directory.Exists(@path + "\\" + dirname) || Directory.Exists(Directory.GetCurrentDirectory() + "\\" + dirname))
                        {
                            Console.WriteLine("The file already exists.");
                        }
                        else
                        {
                            if (dirname == "")
                            {
                                Console.WriteLine("Invalid directory name.");
                            }
                            else
                            {
                                if (path == "")
                                {
                                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + dirname);
                                    DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + dirname);
                                    di.Attributes = FileAttributes.Hidden;
                                    using StreamWriter sw = File.AppendText(configDir + "\\config");
                                    sw.WriteLine(Directory.GetCurrentDirectory() + " " + dirname);
                                    sw.WriteLine(currentDateTime);
                                }
                                else
                                {
                                    Directory.CreateDirectory(@path + "\\" + dirname);
                                    DirectoryInfo di = new DirectoryInfo(@path + "\\" + dirname);
                                    di.Attributes = FileAttributes.Hidden;
                                    using StreamWriter sw = File.AppendText(configDir + "\\config");
                                    sw.WriteLine(@path + " " + dirname);
                                    sw.WriteLine(currentDateTime);
                                }
                            }
                        }
                        path = "";
                    }
                    catch
                    {
                        Console.WriteLine("The specified path doesn't exists.");
                    }
                }

                else if (input == "rm" || input == "remove")
                {
                    Console.WriteLine("Specify the path. Leave blank if you want to use the current directory.");
                    Console.Write(">");
                    path = Console.ReadLine();
                    Console.WriteLine("Name:");
                    Console.Write(">");
                    dirname = Console.ReadLine();
                    try
                    {
                        if (dirname == "")
                        {
                            Console.WriteLine("Invalid directory name.");
                        }
                        else
                        {
                            if (path == "")
                            {
                                string tempFile = Path.GetTempFileName();

                                using (var sr = new StreamReader(configDir + "\\config"))
                                using (var sw = new StreamWriter(tempFile))
                                {
                                    string line;

                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        if (line != Directory.GetCurrentDirectory() + " " + dirname)
                                            sw.WriteLine(line);
                                        else
                                            line = sr.ReadLine();
                                    }
                                }

                                File.Delete(configDir + "\\config");
                                File.Move(tempFile, configDir + "\\config");
                                DirectoryInfo diconf = new DirectoryInfo(configDir + "\\config");
                                diconf.Attributes = FileAttributes.Hidden;

                                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + dirname);
                                di.Attributes = FileAttributes.Normal;
                            }
                            else
                            {
                                string tempFile = Path.GetTempFileName();

                                using (var sr = new StreamReader(configDir + "\\config"))
                                using (var sw = new StreamWriter(tempFile))
                                {
                                    string line;

                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        if (line != path + " " + dirname)
                                            sw.WriteLine(line);
                                        else
                                            line = sr.ReadLine();
                                    }
                                }

                                File.Delete(configDir + "\\config");
                                File.Move(tempFile, configDir + "\\config");
                                DirectoryInfo diconf = new DirectoryInfo(configDir + "\\config");
                                diconf.Attributes = FileAttributes.Hidden;

                                DirectoryInfo di = new DirectoryInfo(@path + "\\" + dirname);
                                di.Attributes = FileAttributes.Normal;
                            }
                        }
                        path = "";
                    }   
                    catch
                    {
                        Console.WriteLine("The specified path doesn't exists.");
                    }
                }

                else if(input == "a" || input == "add")
                {
                    Console.WriteLine("Specify the path. Leave blank if you want to use the current directory.");
                    Console.Write(">");
                    path = Console.ReadLine();
                    Console.WriteLine("Name:");
                    Console.Write(">");
                    dirname = Console.ReadLine();
                    try
                    {
                        if (dirname == "")
                        {
                            Console.WriteLine("Invalid directory name.");
                        }
                        else
                        {
                            if (path == "")
                            {
                                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + dirname);
                                di.Attributes = FileAttributes.Hidden;
                                using StreamWriter sw = File.AppendText(configDir + "\\config");
                                sw.WriteLine(Directory.GetCurrentDirectory() + " " + dirname);
                                sw.WriteLine(currentDateTime);
                            }
                            else
                            {
                                DirectoryInfo di = new DirectoryInfo(@path + "\\" + dirname);
                                di.Attributes = FileAttributes.Hidden;
                                using StreamWriter sw = File.AppendText(configDir + "\\config");
                                sw.WriteLine(@path + " " + dirname);
                                sw.WriteLine(currentDateTime);
                            }
                        }
                        path = "";
                    }
                    catch
                    {
                        Console.WriteLine("The specified path doesn't exists.");
                    }
                }

                else if(input == "l" || input == "list")
                {
                    string list = File.ReadAllText(configDir + "\\config");
                    Console.WriteLine(list);
                }

                else if(input == "pwd")
                {
                    Console.WriteLine(Directory.GetCurrentDirectory());
                }

                else if(input == "delconfig")
                {
                    File.Delete(configDir + "\\config");
                    Environment.Exit(0);
                }

                else if(input == "h" || input == "help")
                {
                    Console.WriteLine("c / create: Creates a hidden directory.");
                    Console.WriteLine("a / add: Makes an existing directory hidden, and logs it.");
                    Console.WriteLine("rm / remove: Makes a hidden directory visible again, and removes it from the log.");
                    Console.WriteLine("l / list: Lists the log.");
                    Console.WriteLine("pwd: Reveals your path.");
                }

                else
                {
                    Console.WriteLine("Wrong command.");
                }
            }
        }
    }
}