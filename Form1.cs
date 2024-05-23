using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsServiceController
{
    public partial class Form1 : Form
    {
        private List<string> listBox1_ServiceNames = new List<string>();
        private List<string> listBox2_ServiceNames = new List<string>();

        private Timer timer = new Timer();

        private bool isStartMonitoring = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1_ServiceNames = ServiceController.GetServices().Select(a => a.ServiceName).ToList();

            listBox1.DataSource = listBox1_ServiceNames;

            timer.Interval = 60000;
            timer.Tick += new EventHandler(CheckService);
        }

        private void start_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectedNames = listBox1.SelectedItems.Cast<string>();
            IEnumerable<ServiceController> selectedServices = ServiceController.GetServices().Where(a => selectedNames.Contains(a.ServiceName));

            foreach (ServiceController serviceController in selectedServices)
            {
                serviceController.Start();
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectedNames = listBox1.SelectedItems.Cast<string>();
            IEnumerable<ServiceController> selectedServices = ServiceController.GetServices().Where(a => selectedNames.Contains(a.ServiceName));

            foreach (ServiceController serviceController in selectedServices)
            {
                serviceController.Stop();
            }
        }

        private void addToMonitoring_Click(object sender, EventArgs e)
        {
            string[] selectedName = listBox1.SelectedItems.Cast<string>().ToArray();

            listBox2_ServiceNames.Add(selectedName[0]);
            listBox2.DataSource = null;
            listBox2.DataSource = listBox2_ServiceNames;
                
            listBox1_ServiceNames.RemoveAt(listBox1_ServiceNames.IndexOf(selectedName[0]));
            listBox1.DataSource = null;
            listBox1.DataSource = listBox1_ServiceNames;
        }

        private void removeFromMonitoring_Click(object sender, EventArgs e)
        {
            string[] selectedName = listBox2.SelectedItems.Cast<string>().ToArray();

            listBox1_ServiceNames.Add(selectedName[0]);
            listBox1.DataSource = null;
            listBox1.DataSource = listBox1_ServiceNames;

            listBox2_ServiceNames.RemoveAt(listBox2_ServiceNames.IndexOf(selectedName[0]));
            listBox2.DataSource = null;
            listBox2.DataSource = listBox2_ServiceNames;
        }

        private void startOrStopMonitoring_Click(object sender, EventArgs e)
        {
            if (isStartMonitoring)
            {
                isStartMonitoring = false;

                timer.Stop();

                Monitoring.Text = "Start monitoring";
            } else
            {
                isStartMonitoring = true;

                timer.Start();

                Monitoring.Text = "Stop monitoring";
            }

        }

        private void CheckService(object sender, EventArgs e)
        {
            ServiceController[] services = listBox2_ServiceNames.Select(name => new ServiceController(name)).ToArray();

            foreach (ServiceController service in services)
            {
                if (service.CanStop)
                    service.Stop();
                else
                    Console.WriteLine("ee");
            }
        }
    }
}
