using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.AspNet.SignalR;
using System.ComponentModel;
using NmeaParser;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using AISManager;
using Newtonsoft.Json;

namespace AISServer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // Dongle Key DLL definition
        #region
        // Error Code
        static public uint LC_SUCCESS = 0;  // Successful
        static public uint LC_OPEN_DEVICE_FAILED = 1;  // Open device failed
        static public uint LC_FIND_DEVICE_FAILED = 2;  // No matching device was found
        static public uint LC_INVALID_PARAMETER = 3;  // Parameter Error
        static public uint LC_INVALID_BLOCK_NUMBER = 4;  // Block Error
        static public uint LC_HARDWARE_COMMUNICATE_ERROR = 5;  // Communication error with hardware
        static public uint LC_INVALID_PASSWORD = 6;  // Invalid Password
        static public uint LC_ACCESS_DENIED = 7;  // No privileges
        static public uint LC_ALREADY_OPENED = 8;  // Device is open
        static public uint LC_ALLOCATE_MEMORY_FAILED = 9;  // Allocate memory failed
        static public uint LC_INVALID_UPDATE_PACKAGE = 10; // Invalid update package
        static public uint LC_SYN_ERROR = 11; // thread Synchronization error
        static public uint LC_OTHER_ERROR = 12;// Other unknown exceptions


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        // Hardware information structure
        public struct LC_hardware_info
        {
            public int developerNumber;             // Developer ID
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] serialNumber;             // Unique Device Serial Number
            public int setDate;                     // Manufacturing date
            public int reservation;                // Reserve
        }

        // Software information structure
        public struct LC_software_info
        {
            public int version;        // Software edition
            public int reservation;    // Reserve
        }
        // Dongle Key DLL definition

        // DLL
        // LC API function interface
        [DllImport(@"lc1.dll")]
        private static extern int LC_open(int vendor, int index, ref int handle);
        [DllImport(@"lc1.dll")]
        private static extern int LC_close(int handle);
        [DllImport(@"lc1.dll")]
        private static extern int LC_passwd(int handle, int type, byte[] passwd);
        [DllImport(@"lc1.dll")]
        private static extern int LC_read(int handle, int block, byte[] buffer);
        [DllImport(@"lc1.dll")]
        private static extern int LC_write(int handle, int block, byte[] buffer);
        [DllImport(@"lc1.dll")]
        private static extern int LC_encrypt(int handle, byte[] plaintext, byte[] ciphertext);
        [DllImport(@"lc1.dll")]
        private static extern int LC_decrypt(int handle, byte[] ciphertext, byte[] plaintext);
        [DllImport(@"lc1.dll")]
        private static extern int LC_set_passwd(int handle, int type, byte[] newpasswd, int retries);
        [DllImport(@"lc1.dll")]
        private static extern int LC_change_passwd(int handle, int type, byte[] oldpasswd, byte[] newpasswd);
        [DllImport(@"lc1.dll")]
        private static extern int LC_get_hardware_info(int handle, ref LC_hardware_info info);
        [DllImport(@"lc1.dll")]
        private static extern int LC_get_software_info(ref LC_software_info info);
        [DllImport(@"lc1.dll")]
        private static extern int LC_update(int handle, byte[] buffer);
        //////////////////
        #endregion

        private IDisposable m_SignalR;
        private BindingList<ClientItem> m_Clients = new BindingList<ClientItem>();

        int[] baudRateList = { 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
        string[] comlist = System.IO.Ports.SerialPort.GetPortNames();

        public NmeaDevice AISDevice;

        int m_Handle = -1;

        IniFile ini;
        public string appStartPath;

        public DispatcherTimer sendJsonTimer;

        public DispatcherTimer timerPORTMIS;
        public DispatcherTimer timerDongle;
        public string m_DBID, m_DBName, m_DBPassword;

        public bool m_bDBConnected = false;

        private Parser m_AisParser = new Parser();
        AISCollection m_AISList = new AISCollection();

        public MainWindow()
        {
            InitializeComponent();

            //Register to static hub events
            ConnectHub.ClientConnected += ConnectHub_ClientConnected;
            ConnectHub.ClientDisconnected += ConnectHub_ClientDisconnected;
            ConnectHub.ClientNameChanged += ConnectHub_ClientNameChanged;
            ConnectHub.MessageReceived += ConnectHub_MessageReceived;

            CheckDongle();

        }

        public void CheckDongle()
        {
            int res;

            // Open device
            res = LC_open(0x3f3f3f3f, 0, ref m_Handle);
            if (res != LC_SUCCESS)
            {
                MessageBox.Show("동글키가 없습니다. 프로그램을 종료합니다.");
                System.Environment.Exit(0);
            }

            // Verify Generic Password.
            //byte[] passwd = { (byte)'r', (byte)'i', (byte)'a', (byte)'n', (byte)'5', (byte)'0', (byte)'1', (byte)'!' };
            byte[] passwd = { (byte)'r', (byte)'i', (byte)'a', (byte)'n', (byte)'3', (byte)'1', (byte)'2', (byte)'3' };
            res = LC_passwd(m_Handle, 1, passwd);
            if (res != LC_SUCCESS)
            {
                MessageBox.Show("동글키가 맞지 않습니다. 프로그램을 종료합니다.");
                LC_close(m_Handle);
                System.Environment.Exit(0);
            }
        }

        public void ReadSettings()
        {
            txtServerIP.Text = ini.ReadValue("Network", "IP", "127.0.0.1");
            txtServerPort.Text = ini.ReadValue("Network", "Port", "5000");

            string portNum = ini.ReadValue("Serial", "Port", "COM6");
            int index = cmbPort.Items.IndexOf(portNum);
            cmbPort.SelectedIndex = index;

            int baudRate = ini.ReadValue("Serial", "BaudRate", 38400);
            index = cmbBaudRate.Items.IndexOf(baudRate);
            cmbBaudRate.SelectedIndex = index;

            m_DBName = txtDBName.Text = ini.ReadValue("DataBase", "Name", "DESKTOP-D14TH4T");
            m_DBID = txtDBID.Text = ini.ReadValue("DataBase", "ID", "AIS_SERVER");
            m_DBPassword = txtDBPW.Text = ini.ReadValue("DataBase", "PW", "q1w2e3r4t5!!");
        }


        private void InitTimer()
        {
            timerPORTMIS = new DispatcherTimer();
            timerPORTMIS.Tick += new EventHandler(timerUpdatePortMIS_Tick);
            timerPORTMIS.Interval = new TimeSpan(1, 0, 0);//1시간주기마다 timer 실행
                                                          // 디버깅용
                                                          // timerPORTMIS.Start();

            sendJsonTimer = new DispatcherTimer();
            sendJsonTimer.Tick += new EventHandler(SendAISDataJSON);
            sendJsonTimer.Interval = new TimeSpan(0, 0, 3);
            sendJsonTimer.Start();

            timerDongle = new DispatcherTimer();
            timerDongle.Tick += new EventHandler(timerCheckDongle_Tick);
            timerDongle.Interval = new TimeSpan(0, 10, 0);//1시간주기마다 timer 실행
                                                          // 디버깅용
                                                          // timerDongle.Start();

        }

        private void timerUpdatePortMIS_Tick(object sender, EventArgs e)
        {
            new Thread(() => UpdatePORTMISInfo()).Start();
        }

        private void timerCheckDongle_Tick(object sender, EventArgs e)
        {
            new Thread(() => CheckDongleTimer()).Start();
        }

        private void CheckDongleTimer()
        {
            int res;

            // Open device
            res = LC_open(0x3f3f3f3f, 0, ref m_Handle);
            if (res != LC_SUCCESS)
            {
                MessageBox.Show("동글키가 없습니다. 프로그램을 종료합니다.");
                System.Environment.Exit(0);
            }

            // Verify Generic Password.
            //byte[] passwd = { (byte)'r', (byte)'i', (byte)'a', (byte)'n', (byte)'5', (byte)'0', (byte)'1', (byte)'!' };
            byte[] passwd = { (byte)'r', (byte)'i', (byte)'a', (byte)'n', (byte)'3', (byte)'1', (byte)'2', (byte)'3' };
            res = LC_passwd(m_Handle, 1, passwd);
            if (res != LC_SUCCESS)
            {
                MessageBox.Show("동글키가 맞지 않습니다. 프로그램을 종료합니다.");
                LC_close(m_Handle);
                System.Environment.Exit(0);
            }
        }

        public void InitSystemSetting()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                cmbPort.ItemsSource = comlist;
                cmbBaudRate.ItemsSource = baudRateList;

                appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                ini = new IniFile(appStartPath + "\\AISServer.ini");

                ReadSettings();
                // 디버깅용
                //InitCheckPortMIS();
                // 디버깅용
                //if (m_bDBConnected)
                InitTimer();

                txtLoding.Visibility = Visibility.Hidden;
                tabMainControl.IsEnabled = true;
            }));

        }
        public void UpdatePORTMISInfo()
        {
            try
            {
                DelectPortMISTable();

                InsertPortMISTable();

            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 설정 변경해 주십시오.");
                // MessageBox.Show(ex.ToString());
            }

        }

        private void InitCheckPortMIS()
        {
            try
            {
                DelectPortMISTable();

                InsertPortMISTable();

                m_bDBConnected = true;
            }
            catch (Exception ex)
            {
                m_bDBConnected = false;
                MessageBox.Show("데이터베이스 설정 변경해 주십시오.");
                // MessageBox.Show(ex.ToString());
            }
        }

        private void InsertPortMISTable()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SqlConnection sqlCon = new SqlConnection();

            sqlCon.ConnectionString =
            "Data Source=58.224.119.142,5398;" +
                   "Database=NEOE;" +
                   "User id=NEOE;" +
                   "Password=NEOE;";
            sqlCon.Open();

            SqlDataAdapter sda = new SqlDataAdapter("SELECT DISTINCT NO_ASSIGN, DT_SHIP, TM_SHIP, CD_PILOT, ASSIGN.CD_CALLSIGN, NM_CALLSIGN, FG_INOUTPORT FROM CZ_OR_ASSIGN AS ASSIGN, CZ_BS_CALLSIGN AS CALLSIGN WHERE ASSIGN.CD_CALLSIGN = CALLSIGN.CD_CALLSIGN AND DT_SHIP='" + DateTime.Now.ToString("yyyyMMdd") + "';", sqlCon);

            DataTable portMISTable = new DataTable();

            sda.Fill(portMISTable);

            sw.Stop();

            Dispatcher.BeginInvoke((Action)delegate ()
            {
                tbxTheZoneMessage.Text = portMISTable.Rows.Count.ToString() + " : " + DateTime.Now.ToString() + " : " + sw.Elapsed.Milliseconds.ToString() + " ms";
            });


            Stopwatch insertWatch = new Stopwatch();
            insertWatch.Start();

            SqlConnection InsertConnection = new SqlConnection();
            InsertConnection.ConnectionString =
                  "Data Source=" + m_DBName + ";" +
                            "Database=BadaroWeb_db;" +
                            "User id=" + m_DBID + ";" +
                            "Password=" + m_DBPassword;

            InsertConnection.Open();

            using (var bulk = new SqlBulkCopy(InsertConnection))
            {
                bulk.DestinationTableName = "PORTMIS";
                bulk.WriteToServer(portMISTable);
            }

            InsertConnection.Close();

            SqlConnection selectCon = new SqlConnection();
            selectCon.ConnectionString =
                   "Data Source=" + m_DBName + ";" +
                            "Database=BadaroWeb_db;" +
                            "User id=" + m_DBID + ";" +
                            "Password=" + m_DBPassword;

            DataTable temp = new DataTable();

            string selectString = string.Format("SELECT * FROM PORTMIS;");
            selectCon.Open();
            SqlDataAdapter selectSda = new SqlDataAdapter(selectString, selectCon);
            selectSda.Fill(temp);
            selectCon.Close();
            insertWatch.Stop();

            Dispatcher.BeginInvoke((Action)delegate ()
            {
                tbxPortMISMessage.Text = temp.Rows.Count.ToString() + " : " + DateTime.Now.ToString() + " : " + sw.Elapsed.Milliseconds.ToString() + " ms";
            });

        }
        private void DelectPortMISTable()
        {
            SqlConnection deleteConnection = new SqlConnection();
            deleteConnection.ConnectionString =
                  "Data Source=" + m_DBName + ";" +
                            "Database=BadaroWeb_db;" +
                            "User id=" + m_DBID + ";" +
                            "Password=" + m_DBPassword;

            SqlCommand deleteCmd = new SqlCommand("DELETE from PORTMIS;", deleteConnection);
            deleteCmd.Connection.Open();
            deleteCmd.ExecuteNonQuery();
            deleteCmd.Connection.Close();
        }

        private void ConnectHub_ClientConnected(string clientId)
        {
            //Add client to our clients list
            Dispatcher.BeginInvoke(new Action(() =>
            {
                m_Clients.Add(new ClientItem() { Id = clientId, Name = clientId, IsConnected = "Connected" });
                ListViewClient.ItemsSource = m_Clients;
                ListViewClient.UpdateLayout();
            }));

            writeToLog($"Client connected:{clientId}");
        }

        private void ConnectHub_ClientDisconnected(string clientId)
        {
            //Remove client from the list
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var client = m_Clients.FirstOrDefault(x => x.Id == clientId);
                if (client != null)
                    m_Clients.Remove(client);

                ListViewClient.ItemsSource = m_Clients;
                ListViewClient.UpdateLayout();
            }));

            writeToLog($"Client disconnected:{clientId}");
        }

        private void ConnectHub_ClientNameChanged(string clientId, string newName)
        {
            //Update the client's name if it exists
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var client = m_Clients.FirstOrDefault(x => x.Id == clientId);
                if (client != null)
                    client.Name = newName;

                ListViewClient.ItemsSource = m_Clients;
                ListViewClient.UpdateLayout();
            }));

            writeToLog($"Client name changed. Id:{clientId}, Name:{newName}");
        }

        private void ConnectHub_MessageReceived(string senderClientId, string message)
        {
            //One of the clients sent a message, log it
            Dispatcher.BeginInvoke(new Action(() =>
            {
                string clientName = m_Clients.FirstOrDefault(x => x.Id == senderClientId)?.Name;

                writeToLog($"{clientName}:{message}");
            }));
        }

        private void InitNMEADevices()
        {
            AISDevice = new NmeaParser.SerialPortDevice(new System.IO.Ports.SerialPort(cmbPort.SelectedValue.ToString(), Convert.ToInt32(cmbBaudRate.SelectedValue)));
            StartAISDevice(AISDevice);
        }

        private void StartAISDevice(NmeaParser.NmeaDevice device)
        {
            device.AISMessageReceived += AIS_MessageReceived;
            var _ = device.OpenAsync();
        }

        public void EndAISDevice(NmeaParser.NmeaDevice device)
        {
            device.AISMessageReceived -= AIS_MessageReceived;
            var _ = device.CloseAsync();
            device.Dispose();

            AISDevice.Dispose();
            AISDevice = null;
        }

        private void SendAISDataJSON(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < m_AISList.m_AISs.Count; i++)
            {
                string makeJson;
                makeJson = string.Format("{{\"mmsi\": \"{0}\",\"name\": \"{1}\",\"callsign\": \"{2}\" ,\"refA\": \"{3}\" ,\"refB\": \"{4}\" ,\"refC\": \"{5}\" ,\"refD\": \"{6}\" ,\"length\": \"{7}\" ,\"breadth\": \"{8}\" ,\"longitude\": \"{9}\" ,\"latitude\": \"{10}\" ,\"time\": \"{11}\"" +
                    ",\"sog\": \"{12}\" ,\"cog\": \"{13}\" ,\"rot\": \"{14}\" ,\"heading\": \"{15}\" ,\"messageType\": \"{16}\"}}"
                    , m_AISList.m_AISs[i].m_MMSI, m_AISList.m_AISs[i].m_Name, m_AISList.m_AISs[i].m_CallSign, m_AISList.m_AISs[i].m_PosRefA, m_AISList.m_AISs[i].m_PosRefB, m_AISList.m_AISs[i].m_PosRefC, 
                    m_AISList.m_AISs[i].m_PosRefD, (m_AISList.m_AISs[i].m_PosRefA + m_AISList.m_AISs[i].m_PosRefB), (m_AISList.m_AISs[i].m_PosRefC + m_AISList.m_AISs[i].m_PosRefD), m_AISList.m_AISs[i].m_Longitude,
                    m_AISList.m_AISs[i].m_Latitude, m_AISList.m_AISs[i].m_TimeReceived, m_AISList.m_AISs[i].m_SOG, m_AISList.m_AISs[i].m_COG, m_AISList.m_AISs[i].m_ROT, m_AISList.m_AISs[i].m_Heading, m_AISList.m_AISs[i].m_MessageType);
                // ,\"name\": \"{1}\"
                // makeJson = string.Format("{0}", m_AISList.m_AISs[i].m_MMSI);
                SendMessageToClient(makeJson);

                writeToLog(makeJson);
            }
            */
        }

        private void SendAllDataJson()
        {
            string makeJson = "{data:[";
            for (int i = 0; i < m_AISList.m_AISs.Count; i++)
            {
                string aisData = string.Format("\"mmsi\":\"{0}\",\"name\":\"{1}\",\"callsign\":\"{2}\",\"length\":\"{3}\",\"longitude\":\"{4}\",\"latitude\":\"{5}\",\"time\":\"{6}\",\"sog\":\"{7}\",\"cog\":\"{8}\",\"heading\":\"{9}\",\"messageType\":\"{10}"
                                        , m_AISList.m_AISs[i].m_MMSI, m_AISList.m_AISs[i].m_Name, m_AISList.m_AISs[i].m_CallSign, (m_AISList.m_AISs[i].m_PosRefA + m_AISList.m_AISs[i].m_PosRefB), m_AISList.m_AISs[i].m_Longitude, m_AISList.m_AISs[i].m_Latitude, m_AISList.m_AISs[i].m_TimeReceived
                                        , m_AISList.m_AISs[i].m_SOG, m_AISList.m_AISs[i].m_COG, m_AISList.m_AISs[i].m_Heading, m_AISList.m_AISs[i].m_MessageType);
                makeJson += "{" + aisData + "},";
            }

            makeJson += "],totalcnt:" + m_AISList.m_AISs.Count.ToString() + "}\"";

            writeToLog(makeJson);
            SendMessageToClient(makeJson);
        }

        private void AIS_MessageReceived(object sender, string p)
        {

            Dispatcher.BeginInvoke((Action)delegate ()
            {
                SendAllDataJson();
                try
                {
                    Hashtable rs = m_AisParser.Parse(p);
                    m_AISList.ProcessParsingData(rs);

                    if (m_Clients.Count > 0)
                    {
                        SendAllDataJson();
                    }

                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        tbxReceivedMessage.Text = p;
                    });
                }
                catch (Exception ex)
                {
                    writeToLog(string.Format("{0} : AIS parse error Message : {1}\r\nSentence : {2}\r\n", DateTime.Now.ToString(), p, ex.ToString()));
                }
            });
        }

        private void SendMessageToClient(string sentence)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ConnectHub>();

            hubContext.Clients.All.addMessage("SERVER", sentence);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnStartServer_Click(object sender, RoutedEventArgs e)
        {
            //tbxLog.Clear();

            try
            {
                //Start SignalR server with the give URL address
                //Final server address will be "URL/signalr"
                //Startup.Configuration is called automatically
                string uri = "http://" + txtServerIP.Text + ":" + txtServerPort.Text;
                m_SignalR = WebApp.Start<Startup>(uri);

                btnStartServer.IsEnabled = false;
                txtServerIP.IsEnabled = false;
                txtServerPort.IsEnabled = false;
                btnStopServer.IsEnabled = true;

                writeToLog($"Server started at {uri}");
                ini.WriteValue("Network", "IP", txtServerIP.Text);
                ini.WriteValue("Network", "Port", txtServerPort.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnStopServer_Click(object sender, RoutedEventArgs e)
        {
            m_Clients.Clear();

            ConnectHub.ClearState();

            if (m_SignalR != null)
            {
                m_SignalR.Dispose();
                m_SignalR = null;

                btnStopServer.IsEnabled = false;
                btnStartServer.IsEnabled = true;
                txtServerIP.IsEnabled = true;
                txtServerPort.IsEnabled = true;

                writeToLog("Server stopped.");
            }
        }

        private void BtnConnectSerialPort_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPort.SelectedItem == null) MessageBox.Show("Port를 설정해주세요.");
            if (cmbBaudRate.SelectedItem == null) MessageBox.Show("BaudRate를 설정해주세요.");

            InitNMEADevices();
            writeToLog("Serial Connected at " + cmbPort.SelectedValue.ToString());

            ini.WriteValue("Serial", "Port", cmbPort.SelectedValue.ToString());
            ini.WriteValue("Serial", "BaudRate", cmbBaudRate.SelectedValue.ToString());

            btnConnectSerialPort.IsEnabled = false;
            btnDisconnectSerialPort.IsEnabled = true;
        }

        private void BtnDisconnectSerialPort_Click(object sender, RoutedEventArgs e)
        {
            if (AISDevice != null)
            {
                EndAISDevice(AISDevice);
                tbxReceivedMessage.Text = "";
                writeToLog("Serial Disconnected");

                btnConnectSerialPort.IsEnabled = true;
                btnDisconnectSerialPort.IsEnabled = false;
            }
        }

        private void writeToLog(string log)
        {
            string dirPath = appStartPath + @"\Logs";
            string filePath = appStartPath + @"\Logs\Log" + DateTime.Today.ToString("yyyyMMdd") + ".txt";

            DirectoryInfo di = new DirectoryInfo(dirPath);
            FileInfo fi = new FileInfo(filePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(dirPath);

                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " : " + log);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " : " + log);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            Dispatcher.BeginInvoke(new Action(() => tbxLog.AppendText(DateTime.Now.ToString() + " : " + log + Environment.NewLine)));
        }

        private void BtnSaved_Click(object sender, RoutedEventArgs e)
        {
            ini.WriteValue("DataBase", "Name", txtDBName.Text);
            ini.WriteValue("DataBase", "ID", txtDBID.Text);
            ini.WriteValue("DataBase", "PW", txtDBPW.Text);

            MessageBox.Show("환경설정이 변경되었습니다. 프로그램을 다시 실행해 주십시오");

            System.Environment.Exit(0);
        }

        private void MainWin_ContentRendered(object sender, EventArgs e)
        {
            tabMainControl.IsEnabled = false;
            new Thread(() => InitSystemSetting()).Start();

        }

        private void AISServer_Closed(object sender, EventArgs e)
        {
            writeToLog("Close Server Program.");
        }

        private void AISServer_Loaded(object sender, RoutedEventArgs e)
        {
            writeToLog("Start Server Program.");
        }
    }
}
