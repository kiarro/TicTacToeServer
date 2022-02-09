using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace ServerApp
{
    static class DeveloperFunctions
    {
        public static void DebugMessageWriteLine(string str)
        {
            Debug.Write(str);
            Console.WriteLine(str);
        }

        public static void DebugMessageWrite(string str)
        {
            Debug.Write(str);
            Console.Write(str);
        }

        public static string directoryPath = Directory.GetCurrentDirectory() + "\\site_for_server\\";
    }


    class CProgram
    {
        static void Main(string[] args)
        {
            
            DeveloperFunctions.DebugMessageWriteLine("App started");
            // set necessary number of simultanious threads
            // let it 4 for processor
            int sMaxThreadsCount = Environment.ProcessorCount * 4;
            // set max number of work threads
            ThreadPool.SetMaxThreads(sMaxThreadsCount, sMaxThreadsCount);
            // set min number of work threads
            ThreadPool.SetMinThreads(2, 2);

            new CServer(80);
        }
    }

    class CServer
    {
        static List<Game_TTT> games = new List<Game_TTT>();

        TcpListener mTcpListener; // object that receive TCP-clients

        public CServer(int Port)
        {
            DeveloperFunctions.DebugMessageWriteLine("Server started for " + Port + " port");
            mTcpListener = new TcpListener(IPAddress.Any, Port); // create listener for this port
            mTcpListener.Start(); // start listener

            while (true)
            {
                DeveloperFunctions.DebugMessageWriteLine("Waiting for new connection...");
                ////receive new client
                //TcpClient client = mTcpListener.AcceptTcpClient();
                //// create new thread for client
                //Thread clientThread = new Thread(new ParameterizedThreadStart(ClientThread));
                
                // receive new client and process it in new thread by ThreadPool
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), mTcpListener.AcceptTcpClient());
            }
        }

        ~CServer()
        {
            DeveloperFunctions.DebugMessageWriteLine("Server stopped");
            // if listener was created
            if (mTcpListener != null)
            {
                mTcpListener.Stop();
            }
        }

        static void ClientThread(Object StateInfo) // procedure of thread for creating new Client
        {
            new CClient((TcpClient)StateInfo);
        }

        class Game_TTT
        {
            int[,] pole = new int[3,3];
            bool turn; int win;
            public string player;

            public Game_TTT(string str)
            {
                win = 0;
                player = str;
                for (int i = 0; i<3; i++)
                {
                    for (int j=0; j<3; j++)
                    {
                        pole[i, j] = 0;
                    }
                }
                turn = false; // first player turn

            }

            override public string ToString()
            {
                string str = "";
                for (int i=0; i<3; i++)
                {
                    for (int j=0; j<3; j++)
                    {
                        str += pole[i, j];
                    }
                }
                switch (win)
                {
                    case (1):
                        {
                            str += "-w";
                            break;
                        }
                    case (2):
                        {
                            str += "-l";
                            break;
                        }
                    case (3):
                        {
                            str += "-d";
                            break;
                        }
                }
                return str;
            }

            private bool CheckWin(int x, int y, int turn)
            {
                if (pole[0, y] == turn && pole[1, y] == turn && pole[2, y] == turn) return true;
                if (pole[x, 0] == turn && pole[x, 1] == turn && pole[x, 2] == turn) return true;
                
                if ((x == 0 && y == 0) || (x == 1 && y == 1) || (x == 2 && y == 2))
                {
                    if (pole[0, 0] == turn && pole[1, 1] == turn && pole[2, 2] == turn) return true;
                }
                if ((x == 2 && y == 0) || (x == 1 && y == 1) || (x == 0 && y == 2))
                {
                    if (pole[2, 0] == turn && pole[1, 1] == turn && pole[0, 2] == turn) return true;
                }
                return false;
            }

            public void SetToken(int x, int y)
            {
                if (turn) return;

                turn = !turn;
                pole[x, y] = 1; // X token
                if (CheckWin(x, y, 1)) win = 1;

                // AI turn (second player - int token = 2)
                int max = -1; int maxx = -1; int maxy = -1;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (pole[i, j] > 0) continue;
                        int sum = 0; int ssum = 0;
                        for (int ix = 0; ix < 3; ix++)
                        {
                            if (pole[ix, j] == 1) ssum++;
                            if (pole[ix, j] == 2) ssum--;
                        }
                        if (Math.Abs(ssum) == 2)
                        {
                            maxx = i; maxy = j; max = 100;
                        }
                        sum += Math.Abs(ssum); ssum = 0;
                        for (int jy = 0; jy < 3; jy++)
                        {
                            if (pole[i, jy] == 1) ssum++;
                            if (pole[i, jy] == 2) ssum--;
                        }
                        if (Math.Abs(ssum) == 2)
                        {
                            maxx = i; maxy = j; max = 100;
                        }
                        sum += Math.Abs(ssum); ssum = 0;
                        if ((i==0 && j==0) || (i == 1 && j == 1) || (i == 2 && j == 2))
                        {
                            for (int ij = 0; ij < 3; ij++)
                            {
                                if (pole[ij, ij] == 1) ssum++;
                                if (pole[ij, ij] == 2) ssum--;
                            }
                            if (Math.Abs(ssum) == 2)
                            {
                                maxx = i; maxy = j; max = 100;
                            }
                            sum += Math.Abs(ssum); ssum = 0;
                        }
                        if ((i == 2 && j == 0) || (i == 1 && j == 1) || (i == 0 && j == 2))
                        {
                            for (int ij = 0; ij < 3; ij++)
                            {
                                if (pole[ij, 2-ij] == 1) ssum++;
                                if (pole[ij, 2-ij] == 2) ssum--;
                            }
                            if (Math.Abs(ssum) == 2)
                            {
                                maxx = i; maxy = j; max = 100;
                            }
                            sum += Math.Abs(ssum); ssum = 0;
                        }
                        if (sum > max)
                        { 
                            max = sum; maxx = i; maxy = j;
                        }
                    }
                }
                if (maxx == -1)
                { // game over with draw
                    win = 3; // draw
                }
                else
                {
                    pole[maxx, maxy] = 2;
                    if (CheckWin(x, y, 2)) win = 2;
                }
                turn = !turn; // first player turn

            }
        }

        class CClient
        {
            
            public CClient(TcpClient client)
            {
                DeveloperFunctions.DebugMessageWriteLine("New connection received. Client created");

                string request = "";
                byte[] buffer = new byte[1024]; // buffer for store data from client
                int count; // count of bytes that was received

                // receive data from client
                while ((count = client.GetStream().Read(buffer, 0, buffer.Length))>0)
                {
                    // transform data to string and add it to request var
                    request += Encoding.ASCII.GetString(buffer, 0, count);
                    // request must be ends by \r\n\r\n 
                    // or we termine receiving if lenght of request more than 4 Kbytes
                    // We mustn't receive data from POST-request and other requests must be smaller than 4 Kbytes
                    if (request.IndexOf("\r\n\r\n")>=0 || request.Length > 4096)
                    {
                        break;
                    }
                }

                DeveloperFunctions.DebugMessageWriteLine("Request__(my): \n\r"+request);

                // parse received data using regular words
                Match headNameMatch = Regex.Match(request, @"Name:([\w\W]*)");
                string name = headNameMatch.Groups[1].Value;
                DeveloperFunctions.DebugMessageWriteLine(name);
                Match reqMatch = Regex.Match(request, @"^\w+\s+([^\s\?]+)\??([^\s\?]*)\s+HTTP/.*|");
                //Match reqMatch = Regex.Match("aae da? ee?dUEA  AE", @"e\??d*");
                // if request wasn't a success
                if (reqMatch == Match.Empty)
                {
                    // send 400 error (wrong request) for client
                    SendError(client, 400);
                    return;
                }

                // get string request
                string requestUri = reqMatch.Groups[1].Value;
                if (requestUri == "")
                {
                    SendError(client, 400);
                    return;
                }
                string requestAfterQuestion = reqMatch.Groups[2].Value;

                // transform it to original view, converting escaped symbols
                requestUri = Uri.UnescapeDataString(requestUri);
                DeveloperFunctions.DebugMessageWriteLine("Request Uri__(my): " + requestUri);

                // if there is ".." in string send error 400
                // this is important to defence from this type of request: "http://example.com/../../file.txt"
                if (requestUri.IndexOf("..")>=0)
                {
                    SendError(client, 400);
                    return;
                }

                // if strind ends by "/" than add index.html;
                if (requestUri.EndsWith("/"))
                {
                    requestUri += "index.html";
                }



                switch (requestUri)
                {
                    case ("/server_command"):
                        {
                            DeveloperFunctions.DebugMessageWriteLine("Parse server command: "+requestUri);
                            ParseServerCommand();
                            break;
                        };
                    default:
                        {
                            DeveloperFunctions.DebugMessageWriteLine("Send requested file: "+requestUri);
                            SendStringFile();
                            break;
                        }
                     
                }                
                client.Close();
                DeveloperFunctions.DebugMessageWriteLine("Client processed and removed");

                void SendStringFile()
                {
                    // get file extension
                    string extension = requestUri.Substring(requestUri.LastIndexOf('.'));

                    // type of content
                    string contentType = "";

                    // try to find content type by extension
                    switch (extension)
                    {
                        case ".htm":
                        case ".html":
                            contentType = "text/html";
                            requestUri = "HTML/" + requestUri;
                            break;
                        case ".css":
                            contentType = "text/stylesheet";
                            requestUri = "" + requestUri;
                            break;
                        case ".js":
                            contentType = "text/javascript";
                            requestUri = "" + requestUri;
                            break;
                        case ".jpg":
                            contentType = "image/jpeg";
                            requestUri = "Sources/" + requestUri;
                            break;
                        case ".jpeg":
                        case ".png":
                        case ".gif":
                            contentType = "image/" + extension.Substring(1);
                            requestUri = "Sources/" + requestUri;
                            break;
                        default:
                            if (extension.Length > 1)
                            {
                                contentType = "application/" + extension.Substring(1);
                            }
                            else
                            {
                                contentType = "application/unknown";
                            }
                            requestUri = "Sources/" + requestUri;
                            break;
                    }

                    // work with files
                    string filePath = DeveloperFunctions.directoryPath + requestUri;

                    // if there is not asking file in www directory, send 404 error
                    if (!File.Exists(filePath))
                    {
                        SendError(client, 404);
                        DeveloperFunctions.DebugMessageWriteLine("File " + filePath + " not found");
                        return;
                    }

                    
                    // open file to avoid error
                    FileStream FS;
                    try
                    {
                        FS = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    }
                    catch (Exception)
                    {
                        // if error than send error 500 to client
                        SendError(client, 500);
                        return;
                    }

                    // send headers
                    string headers = "HTTP/1.1 200 OK\nContent-Type: " + contentType + "\nContent-Length: " + FS.Length + "\n\n";
                    byte[] headersBuffer = Encoding.ASCII.GetBytes(headers);
                    client.GetStream().Write(headersBuffer, 0, headersBuffer.Length);
                    DeveloperFunctions.DebugMessageWriteLine("Answer__(my): " + headers);

                    while (FS.Position < FS.Length)
                    {
                        // read data from file
                        count = FS.Read(buffer, 0, buffer.Length);
                        // send it to client
                        client.GetStream().Write(buffer, 0, count);
                        //for (int i = 0; i<count; i++)
                        //{
                        //    Debug.Write((char)buffer[i]);
                        //}
                    }

                    // close file and connection
                    FS.Close();
                }
                void ParseServerCommand()
                {
                    DeveloperFunctions.DebugMessageWriteLine(reqMatch.Groups[2].Value);
                    // for tic-tac-toe
                    string[] requestCode = requestAfterQuestion.Split('-');
                    Game_TTT game = null;
                    switch (requestCode[0])
                    {
                        case ("start"):
                            {
                                game = new Game_TTT(name);
                                games.Add(game);
                                DeveloperFunctions.DebugMessageWriteLine("new game started");
                                break;
                            }
                        //TODO: process click on cells and win option
                        case ("set"):
                            {
                                game = games.Find(x => x.player.Equals(name));
                                game.SetToken(Convert.ToInt32(requestCode[1]), Convert.ToInt32(requestCode[2]));
                                DeveloperFunctions.DebugMessageWriteLine("token set at ["+requestCode[1]+", "+requestCode[2]+"]");
                                break;
                            }
                    }
                    string headers = "HTTP/1.1 200 OK\nContent-Type: " + "source/TTT_pole" + "\nContent-Length: " + " " + "\n\n";
                    string message = game.ToString();
                    byte[] headersBuffer = Encoding.ASCII.GetBytes(headers + message);
                    client.GetStream().Write(headersBuffer, 0, headersBuffer.Length);
                }
            }

            

            private void SendError(TcpClient client, int code)
            {
                // create string "404 File Not Found"
                string codeStr = code.ToString() + " " + ((HttpStatusCode)code).ToString();
                // code of site
                string html = "<html><body><h1>" + codeStr + "</h1></body></html>";
                // headers: server request, type and length of content
                string str = "HTTP/1.1 " + codeStr +
                    "\nContent-type: type/html\nContent-Length:" + html.Length.ToString()
                    + "\n\n" + html;
                // transform to byte array
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                // send answer to client
                client.GetStream().Write(buffer, 0, buffer.Length);
                // close client
                client.Close();
            }
        }
    }
}
