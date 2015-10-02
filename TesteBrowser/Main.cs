using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Skybound.Gecko;


namespace TesteBrowser
{
    public partial class Main : Form
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        string color = "";
        Thread A,S,J,K,L,m;
        
        int mX, mY, x, y, i, xA, xS, xJ, xK, xL, allY, contKey = 0;
        
        SaveFileDialog saveScreenshot = new SaveFileDialog();
        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        delegate void SetTextCallback(string text);
        

        public Main()
        {
            InitializeComponent();
            Skybound.Gecko.Xpcom.Initialize("C:\\Users\\gubat_000\\Documents\\Visual Studio 2013\\Projects\\TesteBrowser\\TesteBrowser\\bin\\Debug\\xulrunner");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            // Screenshot
            this.Hide();
            // Deixa o bitmap do tamanho da tela
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            // Cria o objeto a partir do bitmap
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            // Salva a screen
            bmpScreenshot.Save("C:\\Users\\gubat_000\\Documents\\Visual Studio 2013\\Projects\\TesteBrowser\\TesteBrowser\\bin\\Debug\\teste.png", ImageFormat.Png);

            this.Show();
            label1.Text = color;
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            x = 466;
            xA = 608;
            xS = 698;
            xJ = 788;
            xK = 878;
            xL = 968;
            y = 649;
            A = new Thread(updateA);
            S = new Thread(updateS);
            J = new Thread(updateJ);
            K = new Thread(updateK);
            L = new Thread(updateL);

            m = new Thread(mousePos);
            geckoWebBrowser1.Navigate("http:\\guitarflash.com");

        }

        private void updateA()
        {
           
                while (true)
                {
                    _pauseEvent.WaitOne(Timeout.Infinite);

                    if (_shutdownEvent.WaitOne(0))
                        break;
                    checkKey("A", xA,"ff009800");
                   
                }
          
        }

        private void updateS()
        {

            while (true)
            {
                _pauseEvent.WaitOne(Timeout.Infinite);

                if (_shutdownEvent.WaitOne(0))
                    break;

               checkKey("S", xS, "ffff0000");
                
            }

        }

        private void updateJ()
        {

            while (true)
            {
                _pauseEvent.WaitOne(Timeout.Infinite);

                if (_shutdownEvent.WaitOne(0))
                    break;

                checkKey("J", xJ, "fff4f402");
                
            }

        }

        private void updateK()
        {

            while (true)
            {
                _pauseEvent.WaitOne(Timeout.Infinite);

                if (_shutdownEvent.WaitOne(0))
                    break;

                checkKey("K", xK, "0098ff");
                
            }

        }


        private void updateL()
        {

            while (true)
            {
                _pauseEvent.WaitOne(Timeout.Infinite);

                if (_shutdownEvent.WaitOne(0))
                    break;

                checkKey("L", xL, "ffff6500");
            }

        }

        private void checkKey(string key, int xCord, string cor)
        {
            
            using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
            {
     Bitmap screenCopy = new Bitmap(1, 1);
            using (Graphics gdest = Graphics.FromImage(screenCopy))
            {
                IntPtr hSrcDC = gsrc.GetHdc();
                IntPtr hDC = gdest.GetHdc();
                int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, xCord, y, (int)CopyPixelOperation.SourceCopy);

                gdest.ReleaseHdc();
                gsrc.ReleaseHdc();

            }

            Color c = Color.FromArgb(screenCopy.GetPixel(0, 0).ToArgb());
            label1.ForeColor = c;
            color = c.Name;
            
            if (c.Name == cor)
            {
                
                SendKeys.SendWait("{"+key+"}");
            }
           }
        }
        private void mousePos()
        {
            while (true) 
           // while (MousePosition.IsEmpty != true && m.ThreadState == System.Threading.ThreadState.Running)
            {
                _pauseEvent.WaitOne(Timeout.Infinite);

                if (_shutdownEvent.WaitOne(0))
                    break;

                mX = Cursor.Position.X;
                mY = Cursor.Position.Y;
                setText("X = " + mX.ToString() + " Y = " + mY.ToString());
                
            }
            

        }

        

        private void setText(string text)
        {
            try
            {
                if (this.label2.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(setText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.label2.Text = text;
                }
            }

            catch
            {
                MessageBox.Show("Parado com Sucesso!");
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (i == 0)
            {
                m.Start();
                A.Start();
                S.Start();
                J.Start();
                K.Start();
                L.Start();
            }
            else 
            {
                Resume();
            }
            
            i++;

        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pause();
            
        }

        public void Pause()
        {
            _pauseEvent.Reset();
        }

        public void Resume()
        {
            _pauseEvent.Set();
        }

        private void configXYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            pictureBox1.Image = takeScreen();
        }

        private Bitmap takeScreen()
        {
            // Screenshot
            this.Hide();
            // Deixa o bitmap do tamanho da tela
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            // Cria o objeto a partir do bitmap
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            // Salva a screen
            bmpScreenshot.Save("C:\\Users\\gubat_000\\Documents\\Visual Studio 2013\\Projects\\TesteBrowser\\TesteBrowser\\bin\\Debug\\teste.png", ImageFormat.Png);

            this.Show();
            label1.Text = color;

            return bmpScreenshot;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(xA.ToString());
            if (contKey == 0)
            {
                allY = Cursor.Position.Y;
                xA = Cursor.Position.X;
                i++;
            }
            if (contKey == 1)
            {
                xS = Cursor.Position.X;
                i++;
            }

            if (contKey == 2)
            {
                xJ = Cursor.Position.X;
                i++;
            }

            if (contKey == 3)
            {
                xK = Cursor.Position.X;
                i++;
            }

            if (contKey == 4)
            {
                xL = Cursor.Position.X;
                i++;
            }
            if(contKey == 5)
            {
                MessageBox.Show("Teste");
            }
        }
        
    }

}
       
