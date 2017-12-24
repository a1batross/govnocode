using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using OOP_ExtraTaskShared;

namespace OOP_ExtraTaskServer
{
    class Client
    {
        public Client(string id, string passwd)
        {
            Id = id;
            Passwd = passwd;
        }

        public string Id { get; }
        public string Passwd { get; }
        public long Account { get; set; }

        [NonSerialized]
        public List<AccountChange> history = null;
    }

    class ConnectedClient
    {
        public ConnectedClient(int id) { Id = id; }
        public int Id { get; }
    }

    class Server
    {
        public SortedDictionary<int, ConnectedClient> connectedClients = new SortedDictionary<int, ConnectedClient>();
        public List<Client> clients = new List<Client>();
        public List<AccountChange> globalHistory;
        public Random randDevice = new Random();

        public void Initialize()
        {

            try
            {
                String sHistory = File.ReadAllText("history.json");
                globalHistory = sHistory.DeserializeObject<List<AccountChange>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading history.json: " + ex.Message);

                globalHistory = new List<AccountChange>();
            }



            try
            {
                String sClients = File.ReadAllText("clients.json");
                clients = sClients.DeserializeObject<List<Client>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading clients.json: " + ex.Message);

                clients = new List<Client>();

                Register_internal("default");
                Register_internal("securepassword");
            }



            Console.WriteLine(String.Format("Loaded {0} transactions, loaded {1} clients", globalHistory.Count, clients.Count));
        }

        public void Run()
        {
            TcpListener listenserver = null;
            try
            {
                listenserver = new TcpListener(new IPAddress(0), 29015);

                listenserver.Start();

                byte[] data = new byte[4096];

                while (true)
                {
                    Console.WriteLine("Waiting...");

                    Save();

                    TcpClient client = listenserver.AcceptTcpClient();
                    Console.WriteLine("Accepted");

                    NetworkStream stream = client.GetStream();

                    int len;

                    len = stream.Read(data, 0, data.Length);
                    {
                        Console.WriteLine("got info");
                        long readOffset = 0;
                        long headerSize = 1; // one byte
                        byte[] skipped = data.Skip((int)(readOffset + headerSize)).ToArray();
                        long readSize = 0;
                        byte[] newData = null;

                        // may occur buffer overrun!
                        switch ((ClientProtocol)data[readOffset])
                        {
                            case ClientProtocol.clc_login:
                                Console.WriteLine("Login");
                                newData = Login(skipped, out readSize);
                                break;
                            case ClientProtocol.clc_register:
                                Console.WriteLine("Reg");
                                newData = Register(skipped, out readSize);
                                break;
                            case ClientProtocol.clc_getaccounthistory:
                                Console.WriteLine("GetAccHist");
                                newData = GetAccountHistory(skipped, out readSize);
                                break;
                            case ClientProtocol.clc_getaccountvalue:
                                Console.WriteLine("GetAccVal");
                                newData = GetAccountValue(skipped, out readSize);
                                break;
                            case ClientProtocol.clc_send:
                                Console.WriteLine("Send");
                                newData = Send(skipped, out readSize);
                                break;
                            case ClientProtocol.clc_disconnect:
                                Console.WriteLine("Disconnect");
                                Disconnect(skipped, out readSize);
                                break;
                        }

                        if (newData != null)
                            stream.Write(newData, 0, newData.Length);
                        readOffset += readSize;


                    }

                    stream.Close();
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ашыпка " + ex.Message);
            }
        }

        public void Save()
        {
            File.WriteAllText("history.json", globalHistory.SerializeToString());
            File.WriteAllText("clients.json", clients.SerializeToString());
        }

        public void Close()
        {
            Save();
            Console.WriteLine("Byte-byte");
        }

        private byte[] CreateMessage(ServerProtocol msg, byte[] data)
        {
            byte[] newData = new byte[1 + data.Length];

            newData[0] = (byte)msg;
            data.CopyTo(newData, 1);
            return newData;
        }

        private byte[] Login(byte[] data, out long size)
        {
            LoginCredentials creds = data.DeserializeObject<LoginCredentials>(out size);
            LoginResponse response = new LoginResponse() { error = LoginError.LOGIN_SUCCESS, sessionId = 0 };

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Passwd == creds.passwd && clients[i].Id == creds.login)
                {
                    foreach (var check in connectedClients)
                    {
                        if (check.Value.Id == i)
                        {
                            response.error = LoginError.LOGIN_ALREADYLOGIN;
                            break;
                        }
                    }

                    if (response.error != LoginError.LOGIN_ALREADYLOGIN)
                    {
                        do
                        {
                            response.sessionId = randDevice.Next();
                        }
                        while (response.sessionId == 0);
                        connectedClients.Add(response.sessionId, new ConnectedClient(i));
                    }
                    break;
                }
            }

            // nothing was found
            if (response.sessionId == 0)
            {
                response.error = LoginError.LOGIN_INVALIDPASSWD;
            }

            return CreateMessage(ServerProtocol.svc_login, response.SerializeToByteArray());
        }

        private string GenRandomString()
        {
            int size = randDevice.Next(6, 8);

            string str = "";

            for (int i = 0; i < size; i++)
            {
                str += (char)randDevice.Next('A', 'z');
            }

            return str;
        }


        private byte[] Register(byte[] data, out long size)
        {
            string str = data.DeserializeObject<string>(out size);

            return Register_internal(str);
        }

        private byte[] Register_internal(string str)
        {
            RegisterResponse response = new RegisterResponse()
            {
                error = RegisterError.REGISTER_SUCCESS
            };

            bool genNew = true;
            do
            {
                if (genNew)
                {
                    response.id = GenRandomString();
                    genNew = false;
                }

                foreach (Client check in clients)
                {
                    if (check.Id == response.id)
                    {
                        genNew = true;
                    }
                }
            }
            while (genNew);


            Client newCl = new Client(response.id, str);

            // generate random number of money! :)
            newCl.Account = randDevice.Next(0, 4096);

            clients.Add(newCl);

            return CreateMessage(ServerProtocol.svc_register, response.SerializeToByteArray());
        }

        private byte[] GetAccountHistory(byte[] data, out long size)
        {
            int sessionId = data.DeserializeObject<int>(out size);

            ConnectedClient cl = connectedClients[sessionId];

            if (cl == null)
            {
                Console.WriteLine("Unconnected!" + sessionId);
            }

            if (clients[cl.Id].history == null)
            {
                clients[cl.Id].history = new List<AccountChange>();
                foreach (AccountChange acc in globalHistory)
                {
                    if (acc.from == clients[cl.Id].Id)
                    {
                        clients[cl.Id].history.Add(acc);
                    }
                }
            }

            return CreateMessage(ServerProtocol.svc_accounthistory, clients[cl.Id].history.SerializeToByteArray());
        }

        private byte[] GetAccountValue(byte[] data, out long size)
        {
            int sessionId = data.DeserializeObject<int>(out size);

            ConnectedClient cl = connectedClients[sessionId];

            if (cl == null)
            {
                Console.WriteLine("Unconnected!" + sessionId);
            }

            return CreateMessage(ServerProtocol.svc_accountvalue, clients[cl.Id].Account.SerializeToByteArray());
        }

        private byte[] Send(byte[] data, out long size)
        {
            SendResponseError error;
            SendMoney money = data.DeserializeObject<SendMoney>(out size);

            ConnectedClient cl = connectedClients[money.sessionId];
            if (cl == null)
                Console.WriteLine("Unconnected!" + money.sessionId);

            bool withdraw = money.to.Length == 0;
            int to = 0;

            if (!withdraw)
            {
                int i;
                for (i = 0; i < clients.Count; i++)
                {
                    if (clients[i].Id == money.to)
                    {
                        to = i;
                        break;
                    }
                }

                if (i == clients.Count)
                {
                    error = SendResponseError.SEND_INVAL_ID;
                    return CreateMessage(ServerProtocol.svc_sendresponse, error.SerializeToByteArray());
                }
            }


            if (clients[cl.Id].Account > money.value)
            {
                error = SendResponseError.SEND_SUCCESS;
                clients[cl.Id].Account -= money.value;
                if (!withdraw)
                    clients[to].Account += money.value;



                AccountChange acc = new AccountChange()
                {
                    from = clients[cl.Id].Id,
                    isSent = false,
                    loginId = money.to,
                    account = -money.value
                };
                globalHistory.Add(acc);
                if (clients[cl.Id].history != null)
                    clients[cl.Id].history.Add(acc);

                if (!withdraw)
                {
                    AccountChange acc2 = new AccountChange()
                    {
                        from = clients[to].Id,
                        isSent = true,
                        loginId = clients[cl.Id].Id,
                        account = money.value
                    };

                    globalHistory.Add(acc2);
                    if (clients[to].history != null)
                        clients[to].history.Add(acc2);
                }


            }
            else error = SendResponseError.SEND_NOTENOUGH;

            return CreateMessage(ServerProtocol.svc_sendresponse, error.SerializeToByteArray());
        }

        private void Disconnect(byte[] data, out long size)
        {
            int sessionId = data.DeserializeObject<int>(out size);

            connectedClients.Remove(sessionId);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Server sv = new Server();
            sv.Initialize();

            sv.Run();

            sv.Close();
        }
    }
}
