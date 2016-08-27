using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UHACK
{
    public partial class frmMain : Form
    {
        private FilterInfoCollection captureDevice;
        private VideoCaptureDevice finalFrame;

        int counter = 0;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (finalFrame.IsRunning == true)
            {
                finalFrame.Stop();
            }
        }

        void finalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            captureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            finalFrame = new VideoCaptureDevice();

            connectToWebCam();
        }

        private void connectToWebCam()
        {
            try
            {
                finalFrame = new VideoCaptureDevice(captureDevice[0].MonikerString);
                finalFrame.NewFrame += new NewFrameEventHandler(finalFrame_NewFrame);
                finalFrame.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            counter++;

            if (counter % 2 != 0)
            {
                pbFirst.Image = pictureBox1.Image;
            }
            else
            {
                pbSecond.Image = pictureBox1.Image;
            }            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string fileName1 = txtAccountNumber.Text + " - 1st.jpg";
            string fileName2 = txtAccountNumber.Text + " - 2nd.jpg";

            //Utilities.saveImageToFile(pbFirst, fileName1);
            //Utilities.saveImageToFile(pbSecond, fileName2);

            //Utilities.createCSV(txtAccountNumber.Text, txtFirstName.Text + " " + txtLastName.Text, fileName1, fileName2);


            bool r1 = Utilities.saveImageToFile(pbFirst, fileName1);
            bool r2 = Utilities.saveImageToFile(pbSecond, fileName2);

            if (r1 && r2)
            {
                bool r3 = Utilities.createCSV(txtAccountNumber.Text, txtFirstName.Text + " " + txtLastName.Text, fileName1, fileName2);
                if (r3)
                {

                    Utilities.uploadFileToServer(@"D:\temp\" + fileName1);

                    //UDPClient.sendUDP("HI");

                    MessageBox.Show("Records Sent!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearFields();
                }
                else
                {
                    MessageBox.Show("Failed to save record", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Failed to save record", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void clearFields()
        {
            txtAccountNumber.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();

            pbFirst.Image = null;
            pbSecond.Image = null;
        }
    }
}
