using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;

namespace imgurparser_bot_WinForms
{
    class ImgurPars
    {
        Random rnd = new Random();

        string url;
        string filename;
        string highLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //string lowerLetters = "abcdefghijklmnopqrstuvwxyz0123456789";
        string randomNewDirectory = "Photos" + DateTime.Now.ToString("dd.MM.yyyyInhh-mm-ss");
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        int position = 0;
        int length;

        WebClient client = new WebClient();
        StringBuilder sb;

        int[] invalidSizeImages = new int[] { 0, 503, 5082, 4939, 4940, 4941, 12003, 5556 };

        public string GeneratePictur()
        {
            Directory.CreateDirectory(@currentDirectory + @randomNewDirectory);
            bool validation = true;
            try
            {
                while (validation)
                {
                    //url = "http://img.prntscr.com/img?url=http://i.imgur.com/";
                    url = "https://i.imgur.com/";

                    length = rnd.Next(5, 6);
                    sb = new StringBuilder(length - 1);

                    if (length == 5) // 5
                    {
                        for (int i = 0; i < length; i++)
                        {
                            //получаем случайное число от 0 до последнего символа в строке length
                            position = rnd.Next(0, highLetters.Length - 1);
                            //добавляем выбранный символ в объект StringBuilder
                            sb.Append(highLetters[position]);
                        }
                        url += sb;
                    }
                    else // 3
                    {
                        for (int i = 0; i < length; i++)
                        {
                            //получаем случайное число от 0 до последнего символа в строке length
                            position = rnd.Next(0, highLetters.Length - 1);
                            //добавляем выбранный символ в объект StringBuilder
                            sb.Append(highLetters[position]);
                        }
                        url += sb;
                    }
                    url += ".jpg";
                    //Console.WriteLine("Url:" + url);

                    //Console.WriteLine("CurrentDirectory:" + currentDirectory);

                    filename = url.Split('/').Last();
                    //Console.WriteLine("FileName:" + filename);

                    client.DownloadFile(new Uri(url), @currentDirectory + @randomNewDirectory + "\\" + filename);
                    //client.DownloadFile(new Uri(url), "\\" + filename);
                    //Console.WriteLine(@currentDirectory + @randomNewDirectory + "\\" + filename);

                    FileInfo f = new FileInfo(@currentDirectory + @randomNewDirectory + "\\" + filename);
                    long file_size = f.Length;
                    //Console.WriteLine("Size:" + file_size);
                    for (int i = 0; i < invalidSizeImages.Length; i++)
                    {
                        if (invalidSizeImages[i] == file_size)
                        {
                            Console.WriteLine("[-] Invalid " + url);
                            File.Delete(@currentDirectory + @randomNewDirectory + "\\" + filename);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("[+] Valid " + url);
                            if (i == (invalidSizeImages.Length) - 1)
                            {
                                validation = false;
                            }
                        }
                    }
                    //Console.WriteLine("After Check Size:" + file_size);
                }
                File.Delete(@currentDirectory + @randomNewDirectory + "\\" + filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " ROFLAN " + url);
            }
            return url;
        }
    }
}
