//GPS_Emulator.cs
//by Min Choi 1132188
//for ENEL712 Embedded Systems Design
//GPS Emulator Mini Project

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Device.Location;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace GPS_Emulator
{
    public partial class GPS_Emulator : Form
    {
        int baudRate;
        int dataBits;
        StopBits stopBits;
        Parity parity;
        string par;
        string dataB2;
        string dataIn;
        int keepCount;
        //setting up global variables

        public GPS_Emulator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comSel.Items.AddRange(ports);   //getting available serial ports and setting them to be selectable

            baudSel.SelectedIndex = 3;
            dataSel.SelectedIndex = 3;
            stopSel.SelectedIndex = 1;
            parSel.SelectedIndex = 0;
            portStatus.Text = "OFF";    //setting up default values for serial connection

            pauseB.Visible = true;
            pauseB.Enabled = true;
            resumeB.Visible = false;
            resumeB.Enabled = false;
            offB.Enabled = false;   //disabling some components that are not initially needed

            tab1B.BackColor = Color.FromArgb(42, 76, 120);  //change colour for initially selected tab

            speedChart.ChartAreas[0].AxisY.Maximum = 150;
            speedChart.ChartAreas[0].AxisY.Minimum = 0;
            speedChart.Series["Speed"].Points.Clear();
            speedChart.ChartAreas[0].AxisY.Title = "Speed(km/h)";
            speedChart.ChartAreas[0].AxisY.TitleForeColor = Color.White;
            speedChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            speedChart.ChartAreas[0].AxisX.Title = "Time(s)";
            speedChart.ChartAreas[0].AxisX.TitleForeColor = Color.White;
            speedChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;  //setting up speed chart
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); //exit application button
        }

        private void Title_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        //
        private void button1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            tab2B.BackColor = Color.FromArgb(26, 32, 40);
            tab3B.BackColor = Color.FromArgb(26, 32, 40);
            if (tabControl1.SelectedTab == tabPage1)
            {
                tab1B.BackColor = Color.FromArgb(42, 76, 120);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            tab1B.BackColor = Color.FromArgb(26, 32, 40);
            tab3B.BackColor = Color.FromArgb(26, 32, 40);
            if (tabControl1.SelectedTab == tabPage2)
            {
                tab2B.BackColor = Color.FromArgb(42, 76, 120);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            tab1B.BackColor = Color.FromArgb(26, 32, 40);
            tab2B.BackColor = Color.FromArgb(26, 32, 40);
            if (tabControl1.SelectedTab == tabPage3)
            {
                tab3B.BackColor = Color.FromArgb(42, 76, 120);
            }
        }
        //I made custom tab buttons because default is too ugly. these sets up the buttons and what to do when pressed

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = Convert.ToInt32(baudSel.Text);
        }   //baudrate select and set

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            par = parSel.SelectedItem.ToString();
            if (par == "None")
            {
                parity = Parity.None;
            }
            if (par == "Odd")
            {
                parity = Parity.Odd;
            }
            if (par == "Even")
            {
                parity = Parity.Even;
            }
            if (par == "Mark")
            {
                parity = Parity.Mark;
            }
            if (par == "Space")
            {
                parity = Parity.Space;
            }
        }   //parity select and set

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                comPort.PortName = comSel.Text; //set portname to what is selected from COM Port box
                comPort.BaudRate = baudRate;    //set baudrate
                comPort.DataBits = dataBits;    //set data bit
                comPort.StopBits = stopBits;    //set stopbit
                comPort.Parity = parity;        //set parity
                comPort.Open();                 //connect to serial

                if (comPort.IsOpen)             //when serial is conected
                {
                    offB.Enabled = true;        //enable close button
                    onB.Enabled = false;        //disable open button
                    portStatus.Text = "ON";     //change port status to ON
                    progressBar1.Value = 100;   //fill up progress bar
                }
            }
            catch (Exception err)               //if serial cannot be connected
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //display error message
                portStatus.Text = "OFF";    //set port status to OFF
                offB.Enabled = false;   //disable close button
                onB.Enabled = true;     //enable open button
            }
        }   //serial connect when OPEN button is pressed

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void offB_Click(object sender, EventArgs e)
        {
            comPort.Close();    //disconnect serial
            portStatus.Text = "OFF";    //change port status to OFF
            offB.Enabled = false;   //disable close button
            onB.Enabled = true;     //enable open button
            progressBar1.Value = 0; //progress bar empty
        }   //when close button is pressed disconnect serial

        private void dataSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataBits = Convert.ToInt32(dataSel.Text);
        }   //set databit to selected

        private void stopSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bitStop = stopSel.SelectedItem.ToString();
            if (bitStop == "None")
            {
                stopBits = StopBits.None;
            }
            if (bitStop == "One")
            {
                stopBits = StopBits.One;
            }
            if (bitStop == "One and Half")
            {
                stopBits = StopBits.OnePointFive;
            }
            if (bitStop == "Two")
            {
                stopBits = StopBits.Two;
            }
        }   //set stop bit to selected

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            dataBox.SelectionStart = dataBox.TextLength;
            dataBox.ScrollToCaret();
        }   //raw GPS data box auto scroll to bottom

        List<string> Lat_List = new List<string>();
        List<string> Lon_List = new List<string>();
        List<string> Tme_List = new List<string>();
        List<string> allData = new List<string>();
        List<string> keepData = new List<string>(); //some lists I need for capturing in coming data

        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = comPort.ReadExisting();    //read serial port data
            this.Invoke(new EventHandler(updateData));  //goto update data function
        }   //when new data is received

        private void updateData(object sender, EventArgs e)
        {
            if (checkBox1.Checked)  //if always update is checked
            {
                dataBox.Text = dataIn;  //display single line of received data to raw data box
            }
            else if(checkBox2.Checked)  //if add to old data is checked
            {
                dataBox.Text = string.Join("",allData); //add to previous data and display on raw data box
            }

            allData.Add(dataIn);    //saving all incoming data
            keepData.Add(dataIn);   //same as above but I want to keep seprate list 
            keepCount = keepData.Count; //count how many new lines are added
            dataB2 = dataIn.Remove(0, 1);   //remove the first character of incoming data, which is the $ sign
            dataBox2.Text = dataB2; //display the above string to box below raw data box
            int length = dataB2.Length; // count the length of string in updating databox
            label7.Text = length.ToString();    //display the string length

            string[] gpsArray = dataIn.Split(','); //remove the , of incoming data and save each portion in array
            string time = gpsArray[1];  //time is first part of the array
            string latitude = gpsArray[2] + gpsArray[3];    // latitude is the 2nd and 3rd part of the array
            string longitude = gpsArray[4] + gpsArray[5];   // longitude is the 4th and 5th part of the array
            Lat_List.Add(latitude);     //
            Lon_List.Add(longitude);    //add timem latitude and longitude to seperate list
            Tme_List.Add(time);         //

            textBox6.Text = Tme_List.Last().Insert(2, ":").Insert(5, ":");  //display lastest value in tme_List to time2 box

            int latCount = Lat_List.Count;  //count how many lines are added to Lat_List
            if (latCount>=2)    //if latcount is 2 or more
            {
                textBox3.Text = Tme_List[latCount - 2].Insert(2, ":").Insert(5, ":"); //time 1 box show the previous time
            }
            GPS_Cal(keepData,keepCount);    //run GPS Cal function
        }

        private void GPS_Cal(List<string> keepData, int keepCount)  //this is for calculating all necessary values
        {
            string firstLine = keepData.First();        //
            string[] firstArray = firstLine.Split(','); //getting the first line of data received and saving to firstArray

            double latOriginD = Convert.ToDouble(firstArray[2].Substring(0, 2));    //setting the latitude degree of origin
            double latOriginM = Convert.ToDouble(firstArray[2].Substring(2, 11));   //latitude minute of origin
            double lonOriginD = Convert.ToDouble(firstArray[4].Substring(0, 3));    //longitude degree of origin
            double lonOriginM = Convert.ToDouble(firstArray[4].Substring(3, 11));   //longitude minute of origin
            double latODir = 0, lonODir = 0;

            if (firstArray[3] == "S")
            {
                latODir = -1.0;
            }
            else if (firstArray[3] == "N")
            {
                latODir = 1.0;
            }   //changing the North South of origin latitude to + or -

            if (firstArray[5] == "W")
            {
                lonODir = -1.0;
            }
            else if (firstArray[5] == "E")
            {
                lonODir = 1.0;
            }   //changing the East West of origin longitude to + or -

            double time3 = 0;
            string prevLine;
            string[] prevArray;
            if (keepCount >= 2)
            {
                prevLine = keepData[keepCount - 2];
                prevArray = prevLine.Split(',');
            }
            else
            {
                prevLine = null;
                prevArray = null;
            }   //getting previous line to data received

            string lastLine = keepData.Last();  //latest line of data received
            string[] lastArray = lastLine.Split(',');
            string latDir = lastArray[3];

            double time1 = Convert.ToDouble(firstArray[1]); //starting time
            double time2 = Convert.ToDouble(lastArray[1]);  //current time

            TimeSpan totalTime = TimeSpan.FromSeconds(time2 - time1); //calculate totaltime
            textBox11.Text = totalTime.ToString(@"hh\:mm\:ss"); //display total time

            double lat1D, lat1M, lon1D, lon1M;
            double lat1Dir = 0, lon1Dir = 0, lat2Dir = 0, lon2Dir = 0;
            string tbox2Dir, tbox3Dir;

            if (prevArray!=null)
            {
                lat1D = Convert.ToDouble(prevArray[2].Substring(0,2));
                lat1M = Convert.ToDouble(prevArray[2].Substring(2,11));
                lon1D = Convert.ToDouble(prevArray[4].Substring(0, 3));
                lon1M = Convert.ToDouble(prevArray[4].Substring(3, 11));
                tbox2Dir = prevArray[3];
                tbox3Dir = prevArray[5];
                if (prevArray[3] == "S")
                {
                    lat1Dir = -1.0;
                }
                else if (prevArray[3] == "N")
                {
                    lat1Dir = 1.0;
                }
                if (prevArray[5] == "W")
                {
                    lon1Dir = -1.0;
                }
                else if (prevArray[5] == "E")
                {
                    lon1Dir = 1.0;
                }
                time3 = Convert.ToDouble(prevArray[1]);
            }
            else
            {
                lat1D = 0;
                lat1M = 0;
                lon1D = 0;
                lon1M = 0;
                tbox2Dir = null;
                tbox3Dir = null;
            }   //converting previous line to latitude and longitude values

            double lat2D = Convert.ToDouble(lastArray[2].Substring(0, 2));
            double lat2M = Convert.ToDouble(lastArray[2].Substring(2, 11));
            double lon2D = Convert.ToDouble(lastArray[4].Substring(0, 3));
            double lon2M = Convert.ToDouble(lastArray[4].Substring(3, 11));
            if (lastArray[3] == "S")
            {
                lat2Dir = -1.0;
            }
            else if (lastArray[3] == "N")
            {
                lat2Dir = 1.0;
            }

            if (lastArray[5] == "W")
            {
                lon2Dir = -1.0;
            }
            else if (lastArray[5] == "E")
            {
                lon2Dir = 1.0;
            }   // converting latest line to latitude longitude values

            double lat1 = lat1Dir * (lat1D + (lat1M / 60)); //degree minute to degrees
            double lat2 = lat2Dir * (lat2D + (lat2M / 60)); //
            double lon1 = lon1Dir * (lon1D + (lon1M / 60)); //
            double lon2 = lon2Dir * (lon2D + (lon2M / 60)); //

            textBox1.Text = tbox2Dir + Convert.ToString((lat1D + (lat1M / 60)));    //display previous latitude
            textBox2.Text = tbox3Dir + Convert.ToString((lon1D + (lon1M / 60)));    //display previous longitude
            textBox4.Text = lastArray[3] + Convert.ToString((lat2D + (lat2M / 60)));    //display currrent latitude
            textBox5.Text = lastArray[5] + Convert.ToString((lon2D + (lon2M / 60)));    //display current longitude

            double latOrigin = latODir * (latOriginD + (latOriginM / 60));  //
            double lonOrigin = lonODir * (lonOriginD + (lonOriginM / 60));  //converting latitude and longitude of origine to degrees

            GeoCoordinate point1 = new GeoCoordinate(lat1, lon1);   //converting previous latitude and longitude to geocoordinate
            GeoCoordinate point2 = new GeoCoordinate(lat2, lon2);   //converting current latitude and longitude to geocoordinate

            double distance = Math.Round(point1.GetDistanceTo(point2),2); 
            // calculate distance from preivous to current location and round to 2dp
            textBox7.Text = distance.ToString();    //display distance

            GeoCoordinate pointOrigin = new GeoCoordinate(latOrigin, lonOrigin); // original latitude and longitude to geocoordinate
            double totalDistance = (pointOrigin.GetDistanceTo(point2))/1000;    // calculate origin to current location
            textBox10.Text = Math.Round(totalDistance, 3).ToString();   //diplay total distance

            double speed = 0;
            if (lat1D != 0)
            {
                speed = (distance / (time2 - time3)) * 3.6;
            }
            else
            {
                speed = 0;
            }   //calculate speed
            textBox8.Text = Math.Round(speed,2).ToString(); //display speed in 2dp

            double angle = DegreeBearing(point1.Latitude, point1.Longitude, point2.Latitude, point2.Longitude);
            //calculate the bearing angle(used code from blackboard)

            string[] compass = { "N",  "NE", "E", "SE", "S", "SW", "W", "NW", "N" };  //
            var test = compass[(int)Math.Round(((double)angle % 3600) / 45)];         //converting bearing angle to compass heading
            textBox9.Text = test.ToString();                                          //

            updateChart(speed,totalTime);   //run updateChart function
        }

        //!//////////////////////////////!//
        static double DegreeBearing(double lat3, double lon3, double lat4, double lon4)
        {
            var dLon = ToRad(lon4 - lon3);
            var dPhi = Math.Log(
                Math.Tan(ToRad(lat4) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat3) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }
        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
        public static double ToBearing(double radians)
        {
            return (ToDegrees(radians) + 360) % 360;
        }
        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
        private void updateChart(double speed, TimeSpan totalTime)
        {
            int time4 = (int)totalTime.TotalSeconds;
            if (speed != 0)
            {
                speedChart.Series["Speed"].Points.AddXY(time4, speed);
            }
            else
            {
                speedChart.Series["Speed"].Points.AddXY(time4, 0);
            }
            if (time4 == 0)
            {
                speedChart.Series["Speed"].Points.Clear();
            }
        }
        //!////// this is not my code, it is from the CSharpGeoCoordinate file from blackboard //////!//
        
        //
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }   //this is to move the application window with mouse click

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.AutoCheck = false;
                checkBox1.AutoCheck = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            pauseB.Visible = true;
            pauseB.Enabled = true;
            resumeB.Visible = false;
            resumeB.Enabled = false;
            textBox12.Clear();  //clear longitude box
            textBox13.Clear();  //clear latitude box
        }   //when resume button is press disable resume button and enable pause button

        private void button1_Click_2(object sender, EventArgs e)
        {
            pauseB.Visible = false;
            pauseB.Enabled = false;
            resumeB.Visible = true;
            resumeB.Enabled = true;

            double latDPause, latMPause, longDPause, longMPause;
            string latDirPause, longDirPause;
            string[] pauseArray;
            if (keepCount>=1)
            {
                pauseArray = keepData.Last().Split(',');
                latDPause = Convert.ToDouble(pauseArray[2].Substring(0, 2));
                latMPause = Convert.ToDouble(pauseArray[2].Substring(2, 11));
                longDPause = Convert.ToDouble(pauseArray[4].Substring(0, 3));
                longMPause = Convert.ToDouble(pauseArray[4].Substring(3, 11));
                latDirPause = pauseArray[3];
                longDirPause = pauseArray[5];
            }
            else
            {
                pauseArray = null;
                latDPause = 0;
                latMPause = 0;
                longDPause = 0;
                longMPause = 0;
                latDirPause = null;
                longDirPause = null;
            }
            double latitudePause = (latDPause + (latMPause / 60));
            double longitudePause = (longDPause + (longMPause / 60));

            textBox13.Text = latDirPause + latitudePause.ToString();
            textBox12.Text = longDirPause + longitudePause.ToString();
        }   //when pause button is press capture current position and display them
            //also disables pause button and enables resume button
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox1.AutoCheck = false;
                checkBox2.AutoCheck = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataB_Click(object sender, EventArgs e)
        {
            dataBox.Text = "";
            dataBox2.Text = "";
            allData.Clear();
        }   //clear received data button clears received data list and the received data boxes

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            textBox7.TextAlign = HorizontalAlignment.Center;    //aligns the text to centre as it looked better
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox8.TextAlign = HorizontalAlignment.Center;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            textBox9.TextAlign = HorizontalAlignment.Center;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            textBox10.TextAlign = HorizontalAlignment.Center;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            textBox11.TextAlign = HorizontalAlignment.Center;
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            if (pauseB.Enabled == false)    //when paused
            {
                System.Diagnostics.Process.Start("https://www.google.co.nz/maps/place/" + textBox13.Text + "," + textBox12.Text);
            }
            else
            {
                MessageBox.Show("Please press the pause button","ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }   //need to press pause button to capture current location before being able to map the location
        }   // when map location button is pressed go to google maps of paused location
    }

}
