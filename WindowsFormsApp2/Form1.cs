using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        InputSimulator sim = new InputSimulator();
        bool stop = true;
        string spamtext = null;
        int delayms = 5;
        bool usecliplib = false;
        bool discordEdtLastmsg = false;
        int animvar1;

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void interpretText()
        {
            spamtext = textBox1.Text;
            spamtext = spamtext.Replace("{TIME}", DateTime.Now.ToString("HH:mm:ss"));
            spamtext = spamtext.Replace("{RANDOM}", RandomString(10));
            switch (animvar1)
            {
                case 0:
                    spamtext = spamtext.Replace("{LOADING}", "[..........]");
                    animvar1 = 10;
                    break;
                case 10:
                    spamtext = spamtext.Replace("{LOADING}", "[=.........]");
                    animvar1 = 20;
                    break;
                case 20:
                    spamtext = spamtext.Replace("{LOADING}", "[==........]");
                    animvar1 = 30;
                    break;
                case 30:
                    spamtext = spamtext.Replace("{LOADING}", "[===.......]");
                    animvar1 = 40;
                    break;
                case 40:
                    spamtext = spamtext.Replace("{LOADING}", "[====......]");
                    animvar1 = 50;
                    break;
                case 50:
                    spamtext = spamtext.Replace("{LOADING}", "[=====.....]");
                    animvar1 = 60;
                    break;
                case 60:
                    spamtext = spamtext.Replace("{LOADING}", "[======....]");
                    animvar1 = 70;
                    break;
                case 70:
                    spamtext = spamtext.Replace("{LOADING}", "[=======...]");
                    animvar1 = 80;
                    break;
                case 80:
                    spamtext = spamtext.Replace("{LOADING}", "[========..]");
                    animvar1 = 90;
                    break;
                case 90:
                    spamtext = spamtext.Replace("{LOADING}", "[=========.]");
                    animvar1 = 100;
                    break;
                case 100:
                    spamtext = spamtext.Replace("{LOADING}", "[==========]");
                    animvar1 = 0;
                    break;
                default:
                    spamtext = spamtext.Replace("{LOADING}", "[..........]");
                    animvar1 = 0;
                    break;

            }
        }

        public void Spam()
        {
            Console.WriteLine("[INFO] Spam starting in 5.");
            Thread.Sleep(5000);
            if (!usecliplib)
            {
                while (!stop)
                {
                    interpretText();
                    if (discordEdtLastmsg)
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.UP);
                        Thread.Sleep(1);
                        sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
                        Thread.Sleep(1);
                    }
                    sim.Keyboard.TextEntry(spamtext);
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    Thread.Sleep(delayms);
                }
            }
            else
            {
                while (!stop)
                {
                    interpretText();
                    Thread thread = new Thread(() => Clipboard.SetText(spamtext));
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start();
                    thread.Join();
                    if (discordEdtLastmsg)
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.UP);
                        Thread.Sleep(1);
                        sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
                        Thread.Sleep(1);
                    }
                    sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    Thread.Sleep(delayms);
                }
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            button2.Enabled = true;
            stop = false;
            Thread spamThread = new Thread(new ThreadStart(Spam));
            spamThread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = delayms;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.checkBox1, "Better and faster for long texts but not for short texts, but minimum delay is 100ms if enabled.");
            toolTip1.SetToolTip(this.checkBox2, "If enabled, edits last message instead of sending new message, but minimum delay is 500ms if enabled to bypass discord restrictions.");
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (delayms < 5 && !usecliplib)
                numericUpDown1.Value = 5;
            else if (delayms < 100 && usecliplib)
                numericUpDown1.Value = 100;

            delayms = (int)numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            stop = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            usecliplib = checkBox1.Checked;
            if (delayms < 100 && usecliplib)
                numericUpDown1.Value = 100;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            discordEdtLastmsg = checkBox1.Checked;
            if (delayms < 500 && usecliplib)
                numericUpDown1.Value = 500;

        }
    }
}
