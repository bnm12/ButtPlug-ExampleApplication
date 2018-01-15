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
        private List<ButtplugClientDevice> Devices = new List<ButtplugClientDevice>();

        private ButtplugWSClient _client;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitClient()
        {
            _client = new ButtplugWSClient("Test client");

            _client.DeviceAdded += OnDeviceChanged;
            _client.DeviceRemoved += OnDeviceChanged;
            _client.ErrorReceived += OnError;

            Connect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitClient();
        }

        private async void Connect()
        {
            try
            {
                if (_client != null)
                {
                    //await _client.Connect(new Uri("ws://localhost:12345/buttplug"), true);
                    await _client.Connect(new Uri("ws://localhost:12345/buttplug"), true);
                    await _client.RequestDeviceList();

                    Devices = _client.getDevices().ToList();
                }
            }
            catch (Exception ex)
            {
                OnError(this, new ErrorEventArgs(new Error(ex.Message, Error.ErrorClass.ERROR_UNKNOWN, 0)));
            }
        }

        private void OnDeviceChanged(object sender, DeviceEventArgs e)
        {
            switch (e.Action)
            {
                case DeviceAction.ADDED:
                    Devices.Add(e.Device);
                    break;

                case DeviceAction.REMOVED:
                    Devices.RemoveAll(dev => dev.Index == e.Device.Index);
                    break;
            }
            ToyName.Text = string.Join(Environment.NewLine, Devices.Select(d => d.Name));
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Message.ErrorMessage, "Error");
            if (_client?.IsConnected ?? false)
            {
                Task.Run(async () => { await _client.Disconnect(); });
            }
            _client = null;
        }

        private void SpeedControl_Scroll(object sender, EventArgs e)
        {
            foreach (var dev in Devices)
            {
                if (dev.AllowedMessages.TryGetValue("VibrateCmd", out var attrs))
                {
                    try
                    {
                        uint vibratorCount = attrs.FeatureCount ?? 0;

                        List<VibrateCmd.VibrateSubcommand> vibratorSettings = new List<VibrateCmd.VibrateSubcommand>();

                        for (uint i = 0; i < vibratorCount; i++)
                        {
                            vibratorSettings.Add(new VibrateCmd.VibrateSubcommand(i, (double)SpeedControl.Value / 100));
                        }
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
            if(_client == null)
            {
                InitClient();
            }
            bool success = await _client.StartScanning();
        }
    }
}
