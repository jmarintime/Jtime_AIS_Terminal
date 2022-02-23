using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.AspNet.SignalR;
using System.ComponentModel;
using NmeaParser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WinFormsServer
{
    public partial class FrmServer : Form
    {
        string dbTable = "JMarine_ais";
        MySqlConnection conn;
        private BindingList<string> _groups = new BindingList<string>();
        public NmeaDevice AISDevice;
        string folder = Application.StartupPath + @"\Log_data";
        FileStream fileStream;
        StreamWriter strWriter;
        SaveFileDialog saveFile = new SaveFileDialog();
        bool AIS_LogSave = false;
        string old_time_date = "";
        

        public FrmServer()
        {
            InitializeComponent();
            BadarodbOpen();

            saveFile.InitialDirectory = @"c:";
            saveFile.Title = "AIS_LOG 데이터 저장 위치 지정";
            saveFile.DefaultExt = "txt";
            saveFile.Filter = "Text file (*.txt) | *.txt | Xls Files(*.xls) | *.xls";

            btnOpenPort.Enabled = true;
            btnClosePort.Enabled = false;
            btnLogSave.Enabled = false;
            btnLogSaveStop.Enabled = false;

            //AIS_LOG 데이터 파일 생성 폴더    
            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }

        private void btnClosePort_Click(object sender, EventArgs e)
        {
            AIS_LogSave = false;
            txtErrorLog.AppendText(DateTime.Now.ToString("yyyy:MM:dd ") + saveFile.FileName + " 저장중지..." + "\r\n");
            if (strWriter != null)
            {
                strWriter.Close();
            }
            if (fileStream != null)
            {
                fileStream.Close();
            }

            if (AISDevice != null)
            {
                AISDevice.AISMessageReceived -= AIS_MessageReceived;
                var _ = AISDevice.CloseAsync();
                AISDevice.Dispose();
                txtErrorLog.AppendText(DateTime.Now.ToString("yyyy:MM:dd ") + cmbPortNum.Text + " Close\r\n");
            }

            btnOpenPort.Enabled = true;
            btnClosePort.Enabled = false;
            btnLogSave.Enabled = false;
            btnLogSaveStop.Enabled = false;         
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                AISDevice = new NmeaParser.SerialPortDevice(new System.IO.Ports.SerialPort(cmbPortNum.Text, Convert.ToInt32(cmbBaudRate.Text)));
                AISDevice.AISMessageReceived += AIS_MessageReceived;
                AISDevice.MessageReceived += NMEA_MessageReceived;
                
                var _ = AISDevice.OpenAsync();
                txtErrorLog.AppendText(DateTime.Now.ToString("yyyy:MM:dd ") + cmbPortNum.Text + " Open\r\n");
               
                btnOpenPort.Enabled = false;
                btnClosePort.Enabled = true;
                btnLogSave.Enabled = true;
                btnLogSaveStop.Enabled = false;
            }
            catch(Exception ex)
            {
                txtErrorLog.AppendText(ex.ToString());
            }        
        }

        private void NMEA_MessageReceived(object sender, NmeaParser.NmeaMessageReceivedEventArgs args)
        {
            this.BeginInvoke((Action)delegate ()
            {
                try
                {
                    if (txtAISLog.Lines.Length >= 300)
                    {
                        txtAISLog.Clear();
                    }
                    txtAISLog.AppendText(args.Message + Environment.NewLine);               
                    string Log_date = DateTime.Now.ToString("yyyyMMdd HHmmss.fff") + " > " + args.Message;
                    AIS_LogCreate(Log_date);
                }
                catch (Exception ex)
                {
                    txtErrorLog.AppendText(ex.ToString() + "\r\n");
                }
            });
        }

        private void AIS_MessageReceived(object sender, string p)
        {
            this.BeginInvoke((Action)delegate ()
            {
                try
                {
                    if (txtAISLog.Lines.Length >= 300)
                    {
                        txtAISLog.Clear();
                    }
                    txtAISLog.AppendText(p + Environment.NewLine);         
                    string Log_date = DateTime.Now.ToString("yyyyMMdd HHmmss.fff") + " > " +  p;
                    AIS_LogCreate(Log_date);
                }
                catch (Exception ex)
                {
                    txtErrorLog.AppendText(ex.ToString() + "\r\n");
                }
            });         
        }

        private void AIS_LogCreate(string Log_date)
        {      
            if(AIS_LogSave)
            {
                // 자정일때 save 파일 변환
                string time_date = DateTime.Now.ToString("yyyyMMdd");
                if (!old_time_date.Equals(time_date))
                {
                    old_time_date = time_date;

                    string newtxtname = saveFile.FileName.Replace(".txt", "");
                    saveFile.FileName = newtxtname + "SaveDay(" + DateTime.Now.ToString("MMdd") + ").txt";
                    if (strWriter != null)
                    {
                        strWriter.Close();
                    }
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }

                    fileStream = new FileStream(saveFile.FileName, FileMode.Append, FileAccess.Write);
                    strWriter = new StreamWriter(fileStream, System.Text.Encoding.Default);
                    txtErrorLog.AppendText(DateTime.Now.ToString("yyyy:MM:dd ") + saveFile.FileName + " 저장중..." + "\r\n");
                }
                strWriter.WriteLine(Log_date);
                strWriter.Flush();
            }    
        }

        private void btnLogSave_Click(object sender, EventArgs e)
        {
            if (strWriter != null)
            {
                strWriter.Close();
            }
            if (fileStream != null)
            {
                fileStream.Close();
            }

            old_time_date = DateTime.Now.ToString("yyyyMMdd");
            saveFile.FileName = "AIS" + DateTime.Now.ToString("yyyyMMdd") + "(" + cmbPortNum.Text + ")" + ".txt";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                fileStream = new FileStream(saveFile.FileName, FileMode.Append, FileAccess.Write);
                strWriter = new StreamWriter(fileStream, System.Text.Encoding.Default);
                AIS_LogSave = true;

                btnOpenPort.Enabled = false;
                btnClosePort.Enabled = true;
                btnLogSave.Enabled = false;
                btnLogSaveStop.Enabled = true;
                txtErrorLog.AppendText(DateTime.Now.ToString("yyyy:MM:dd ") + saveFile.FileName + " 저장중..." + "\r\n");
            }
            else
            {
                AIS_LogSave = false;
            }      
        }

        private void btnLogSaveStop_Click(object sender, EventArgs e)
        {
            AIS_LogSave = false;
            if (strWriter != null)
            {
                strWriter.Close();
            }
            if (fileStream != null)
            {
                fileStream.Close();
            }
            
            btnOpenPort.Enabled = false;
            btnClosePort.Enabled = true;
            btnLogSave.Enabled = true;
            btnLogSaveStop.Enabled = false;
            txtErrorLog.AppendText(DateTime.Now.ToString("yyyy:MM:dd ") + saveFile.FileName + " 저장중지..." + "\r\n");
        }

        private string dbInfo()
        {
            string dbServer = "127.0.0.1";
            string dbDatabase = "Badaro";
            string dbUid = "root";
            string dbPwd = "rian";
            string dbSslMode = "none";
            string Conn = "Server=" + dbServer + ";" + "Database=" + dbDatabase + ";" + "Uid=" + dbUid + ";" + "Pwd=" + dbPwd + ";" + "SslMode=" + dbSslMode;
            return Conn;
        }

        private void BadarodbOpen()
        {
            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        // Mysql DB Table 생성
                        try
                        {
                            string sql = "create table " + dbTable + "(MMSI int,TIME VARCHAR(20),AIS_LOW VARCHAR(30))";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();      
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("테이블 생성 실패\n이미 존재하는 테이블 입니다.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("서버에 연결실패\n서버 상태 확인 필요");
                    }
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }

        private void BadarodbClose()
        {
            try
            {
                //Badarodb AIS 연결종료
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
