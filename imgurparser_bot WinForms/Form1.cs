using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imgurparser_bot_WinForms
{
    public partial class Form1 : Form
    {
        BackgroundWorker bw;
        public Form1()
        {
            InitializeComponent();

            this.bw = new BackgroundWorker(); //Иниц переменной в конструкторе
            this.bw.DoWork += this.bw_DoWork; // метод bw_DoWork будет работать асинх
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var text = txtKey.Text; // получаем содержимое текстового поля txtKey в переменную text
            if (text != "" && this.bw.IsBusy != true) //если не запущен
            {
                this.bw.RunWorkerAsync(text);//запускаем, передаем эту переменную в виде аргумента методу bw_DoWork
            }
        }
        private void button1_DoWork(object sender, EventArgs e)
        {
            var worker = sender as BackgroundWorker; // получаем ссылку на класс вызываюший событие
        }
        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as String; // получаем ключ аргументов
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key); // инициализируем API
                await Bot.SetWebhookAsync(""); //Bot.SetWebhook(""); // Обязательно! убираем старую привязку к вебхуку для бота
                int offset = 0; // отступ по сообщениям
                while (true)
                {
                    var newUpdates = await Bot.GetUpdatesAsync(offset); // получаем массив обновлений
                    var updates = newUpdates.Distinct().ToArray(); // убираем повторения из списка новых запросов
                    foreach (var update in updates) // Перебираем все обновления
                    {
                        var message = update.Message;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        {
                            if (message.Text == "/getimage" || message.Text == "/getimage@rndpicture_bot")
                            {
                                ImgurPars imgurPars = new ImgurPars();
                                // в ответ на команду /getimage выводим картинку
                                await Bot.SendPhotoAsync(message.Chat.Id, imgurPars.GeneratePictur());
                            }
                        }
                        offset = update.Id + 1;
                    }
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }
        }
    }
}
