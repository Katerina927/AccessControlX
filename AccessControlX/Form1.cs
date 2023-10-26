using System.Net;

namespace AccessControlX
{
    public partial class Form1 : Form
    {
        // создание объекта FtpWebRequest для подключения к FTP серверу
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://адрес_сервера/имя_файла");
        FtpWebResponse response;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        { // для подключения к серверу
            string login = textBox1.Text;
            string password = textBox2.Text;
            request.Credentials = new NetworkCredential(login, password);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // загрузка файла на сервер
            string login = textBox1.Text;
            string password = textBox2.Text;
            // использование логина и парооля для подключения
            request.Credentials = new NetworkCredential(login, password);
            // метод передачи для загрузки
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            byte[] fileContents = File.ReadAllBytes("путь_к_файлу_локально");
            // открытие потока для записи на серевер
            Stream requestStream = request.GetRequestStream();
            // записывание содержимого файла в поток
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {   // скачать файл с сервера
            string login = textBox1.Text;
            string password = textBox2.Text;
            request.Credentials = new NetworkCredential(login, password);
            // метод (передачи) загрузки из сервера
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            Stream ftpStream = request.GetResponse().GetResponseStream();
            // создание нового файла на компьютере
            FileStream localFileStream = new FileStream("путь_к_файлу_локально", FileMode.Create);
            // записывание содержимого файла в поток записи
            byte[] buffer = new byte[1024];
            int bytesRead = ftpStream.Read(buffer, 0, 1024);

            while (bytesRead > 0)
            {
                localFileStream.Write(buffer, 0, bytesRead);
                bytesRead = ftpStream.Read(buffer, 0, 1024);
            }
            localFileStream.Close();
            ftpStream.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            request.Credentials = new NetworkCredential(login, password);
            // метод возвращения списка файлов на сервере
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            // создание класса FtpWebResponse и получие потока для чтения списка файлов
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            // чтение содержимого потока и добавление файлов в listBox1
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length > 0)
                {
                    string fileName = tokens[tokens.Length - 1];

                    if (fileName != "." && fileName != "..")
                    {
                        listBox1.Items.Add(fileName);
                    }
                }
            }
            reader.Close();
            responseStream.Close();
            response.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // отключение от сервера
            request = null;
            response = null;
        }
    }
}