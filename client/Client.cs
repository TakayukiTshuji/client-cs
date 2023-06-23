using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        // サーバーのIPアドレスとポート番号
        string serverIP = "127.0.0.1";
        int serverPort = 8888;

        // サーバーに接続するためのTcpClientオブジェクトを作成
        TcpClient client = new TcpClient(serverIP, serverPort);

        Console.WriteLine("サーバーに接続しました。");

        try
        {
            // NetworkStreamを取得してデータの送受信を行う
            NetworkStream stream = client.GetStream();

            // サーバーからのメッセージを受信するスレッドを開始
            var receiveThread = new System.Threading.Thread(() =>//スレッドを作成しメッセージの受信を実行
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("サーバーからのメッセージ: " + message);
                }
            });
            receiveThread.Start();

            // クライアントからのメッセージを入力し、サーバーに送信する
            while (true)
            {
                string input = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(input);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        finally
        {
            // 接続を閉じる
            client.Close();
        }
    }
}
