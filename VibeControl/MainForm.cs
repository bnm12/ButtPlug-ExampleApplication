using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Core.Messages;
using static Buttplug.Client.DeviceEventArgs;

namespace VibeControl
{
    public partial class MainForm : Form
    {
        // Create a list to store the devicelist
        private List<ButtplugClientDevice> _devices = new List<ButtplugClientDevice>();

        private ButtplugWSClient _client;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitClient()
        {
            // Set name to connect with
            _client = new ButtplugWSClient("Test client");

            // Bind eventhandlers
            _client.DeviceAdded += OnDeviceChanged;
            _client.DeviceRemoved += OnDeviceChanged;
            _client.ErrorReceived += OnError;

            Connect();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitClient();
        }

        private async void Connect()
        {
            try
            {
                if (_client != null)
                {
                    // URL to connect to, in this case don't use SSL
                    await _client.Connect(new Uri("ws://localhost:12345/buttplug"), true);
                    
                    // Get the list of devices the server currently knows of
                    _devices = _client.Devices.ToList();
                }
            }
            catch (Exception ex)
            {
                OnError(this, new ErrorEventArgs(new Error(ex.Message, Error.ErrorClass.ERROR_UNKNOWN, 0)));
            }
        }

        private void OnDeviceChanged(object sender, DeviceEventArgs e)
        {
            // Handle our events for added and removed devices
            switch (e.Action)
            {
                case DeviceAction.ADDED:
                    // Add new device to list
                    _devices.Add(e.Device);
                    break;

                case DeviceAction.REMOVED:
                    // Remove all devices (really should only ever be one) which matches the removed one
                    _devices.RemoveAll(dev => dev.Index == e.Device.Index);
                    break;
            }
            ToyName.Text = string.Join(Environment.NewLine, _devices.Select(d => d.Name));
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Message.ErrorMessage, "Error");
            if (_client?.IsConnected ?? false)
            {
                // Disconnect the client as we failed connecting
                Task.Run(async () => { await _client.Disconnect(); }); 
            }
            _client = null;
        }

        private void SpeedControl_Scroll(object sender, EventArgs e)
        {
            // Loop all devices
            foreach (var dev in _devices)
            {
                // See if the device allows the "Vibrate" command (it might be e-stim or something else for all we know)
                if (dev.AllowedMessages.TryGetValue("VibrateCmd", out var attrs))
                {
                    try
                    {
                        // If it has vibrators the attrs will tell us the number of vibrators (e.g. we-vibes has an internal and external one)
                        uint vibratorCount = attrs.FeatureCount ?? 0;

                        // Create list to store our vibrate settings in to send to the server later
                        List<VibrateCmd.VibrateSubcommand> vibratorSettings = new List<VibrateCmd.VibrateSubcommand>();

                        // For each vibrator the vibrator has
                        for (uint i = 0; i < vibratorCount; i++)
                        {
                            // Add a vibrate command to the list for the specific vibrator of the client with the value of our slider 0 -> 1
                            vibratorSettings.Add(new VibrateCmd.VibrateSubcommand(i, (double)SpeedControl.Value / 100));
                        }
                        
                        // Send our combined settings to the server to execute
                        _client.SendDeviceMessage(dev, new VibrateCmd(dev.Index, vibratorSettings));
                    }
                    catch
                    {
                    }
                }
            }
        }

        private async void ScanBtn_Click(object sender, EventArgs e)
        {
            // Make sure the client exists before we continue
            if(_client == null)
            {
                InitClient();
            }
            // Tell the server to look for an updated list of devices
            bool success = await _client.StartScanning();
        }
    }
}
