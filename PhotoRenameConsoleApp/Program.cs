using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoRenameConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("请输入文件夹路径：");
            string folderPath = Console.ReadLine();
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            int imageChanged = 0;
            int imageUnChanged = 0;
            IEnumerable<FileInfo> images = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly)
                .Where(x => x.Extension.ToLower() == ".jpg" || 
                            x.Extension.ToLower() == ".jpeg" || 
                            x.Extension.ToLower()==".heic" || 
                            x.Extension.ToLower()==".cr2"
                );
            if (images != null)
            {
                foreach (FileInfo image in images)
                {
                    ShellObject shell = ShellObject.FromParsingName(image.FullName);
                    DateTime? DateTaken = shell.Properties.System.Photo.DateTaken.Value;
                    if (DateTaken != null)
                    {
                        string oldName = image.Name;
                        string newFileName = DateTaken.Value.ToString("yyyy-MM-dd") + " " + oldName;
                        image.MoveTo(Path.Combine(directoryInfo.FullName, newFileName));
                        Console.WriteLine(oldName+" -> "+newFileName);
                        imageChanged++;
                    }
                    else
                    {
                        imageUnChanged++;
                    }
                }
            }


            int videoChanged = 0;
            int videoUnChanged = 0;
            IEnumerable<FileInfo> videos = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly)
                .Where(x => x.Extension.ToLower() == ".mp4" || x.Extension.ToLower()== ".mov");

            if (videos != null)
            {
                foreach (FileInfo video in videos)
                {
                    ShellObject shell = ShellObject.FromParsingName(video.FullName);
                    DateTime? MediaCreated = shell.Properties.System.Media.DateEncoded.Value;
                    if (MediaCreated != null)
                    {
                        string oldName = video.Name;
                        string newFileName = MediaCreated.Value.ToString("yyyy-MM-dd") + " " + oldName;
                        video.MoveTo(Path.Combine(directoryInfo.FullName, newFileName));
                        Console.WriteLine(oldName + " -> " + newFileName);
                        videoChanged++;
                    }
                    else
                    {
                        videoUnChanged++;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("已重命名照片：" + imageChanged);
            Console.WriteLine("未重命名照片：" + imageUnChanged);
            Console.WriteLine("已重命名视频：" + videoChanged);
            Console.WriteLine("未重命名视频：" + videoUnChanged);
            Console.WriteLine();
            Console.WriteLine("...按任意键退出程序");
            Console.ReadKey();

        }
    }
}
