using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMoveMouse {
    public partial class Form1 : Form {
        public delegate void DelegateButtonStatusChange(bool status);
        public delegate void DelegateUpdateText(string text);

        DateTime nextTime = DateTime.MaxValue;
        int appendValue = 1;

        int defaultInterval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["defaultInterval"]);

        public Form1() {
            InitializeComponent();

            numericUpDown1.Value = defaultInterval;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            
            buttonStatusChange(true);
        }

        private void button1_Click(object sender, EventArgs e) {
            buttonStatusChange(false);
            timer1.Enabled = true;
            nextTime = DateTime.Now.AddMinutes(double.Parse(numericUpDown1.Value.ToString()));
        }
        private void buttonStatusChange(bool status) {

            //label1.Text = string.Format("{0}", count);
            if (this.InvokeRequired) {
                this.Invoke(new DelegateButtonStatusChange(this.buttonStatusChange), status);
                return;
            }
            button1.Enabled = status;
            button2.Enabled = !status;
            numericUpDown1.Enabled = status;
        }

        private void addStatus(string text) {

            //label1.Text = string.Format("{0}", count);
            if (this.InvokeRequired) {
                this.Invoke(new DelegateUpdateText(this.addStatus), text);
                return;
            }
            label1.Text = text;

        }

        private void button2_Click(object sender, EventArgs e) {
            buttonStatusChange(true);
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            DateTime nowTime = DateTime.Now;
            if(nextTime < nowTime) {
                int x = Cursor.Position.X;
                int y = Cursor.Position.Y;
                appendValue = -appendValue;
                Cursor.Position = new Point(x + appendValue, y);

                nextTime = nextTime.AddMinutes(double.Parse(numericUpDown1.Value.ToString()));
            }
            int nextSeconds = ((int)(nextTime - nowTime).TotalSeconds);
            int nextMinutes = ((int)nextSeconds / 60);
            nextSeconds = nextSeconds - (nextMinutes * 60);

            addStatus("あと" + nextMinutes + "分" + nextSeconds + "秒");
        }
    }
}
