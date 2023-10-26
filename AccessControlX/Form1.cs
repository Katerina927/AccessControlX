using System.Net;

namespace AccessControlX
{
    public partial class Form1 : Form
    {
        // �������� ������� FtpWebRequest ��� ����������� � FTP �������
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://�����_�������/���_�����");
        FtpWebResponse response;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        { // ��� ����������� � �������
            string login = textBox1.Text;
            string password = textBox2.Text;
            request.Credentials = new NetworkCredential(login, password);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // �������� ����� �� ������
            string login = textBox1.Text;
            string password = textBox2.Text;
            // ������������� ������ � ������� ��� �����������
            request.Credentials = new NetworkCredential(login, password);
            // ����� �������� ��� ��������
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            byte[] fileContents = File.ReadAllBytes("����_�_�����_��������");
            // �������� ������ ��� ������ �� �������
            Stream requestStream = request.GetRequestStream();
            // ����������� ����������� ����� � �����
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {   // ������� ���� � �������
            string login = textBox1.Text;
            string password = textBox2.Text;
            request.Credentials = new NetworkCredential(login, password);
            // ����� (��������) �������� �� �������
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            Stream ftpStream = request.GetResponse().GetResponseStream();
            // �������� ������ ����� �� ����������
            FileStream localFileStream = new FileStream("����_�_�����_��������", FileMode.Create);
            // ����������� ����������� ����� � ����� ������
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
            // ����� ����������� ������ ������ �� �������
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            // �������� ������ FtpWebResponse � ������� ������ ��� ������ ������ ������
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            // ������ ����������� ������ � ���������� ������ � listBox1
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
            // ���������� �� �������
            request = null;
            response = null;
        }
    }
}