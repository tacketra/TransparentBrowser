using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;

namespace TransparentForm
{

    public partial class Form1 : Form
    {
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        Form LockForm { get; set; } = new Form();
        bool pageLocked = false;
        bool IsFullScreen = false;

        public Form1() :base()
        {
            InitializeComponent();
            InitializeForm();
            this.LockFormLoad();
            this.LockForm.Owner = this;

            this.Shown += new EventHandler(Form1_Shown);
            this.SizeChanged += new EventHandler(Form1_SizeChange);
            this.LocationChanged += new EventHandler(Form1_LocationChanged);
            this.VisibleChanged += new EventHandler(Form1_VisibleChange);
            

            //this.LockForm.Load += new EventHandler(LockForm_Load);
            this.LockForm.MouseDown += new MouseEventHandler(LockForm_MouseClick);
            this.LockForm.Click += new EventHandler(LockForm_Click);
            webBrowser1.CanGoBackChanged += new EventHandler(webBrowser1_CanGoBackChanged);
            webBrowser1.CanGoForwardChanged += new EventHandler(webBrowser1_CanGoForwardChanged);
            webBrowser1.DocumentTitleChanged += new EventHandler(webBrowser1_DocumentTitleChanged);
            webBrowser1.StatusTextChanged += new EventHandler(webBrowser1_StatusTextChanged);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            Navigate("http://www.netflix.com/browse");
        }

         private void LockFormLoad()
         {
            this.LockForm.MinimumSize = new Size(1, 1);
            this.LockForm.BackgroundImage = TransparentForm.Properties.Resources.lock_img;
            this.LockForm.FormBorderStyle = FormBorderStyle.None;
            this.LockForm.Width = this.LockForm.BackgroundImage.Width;
            this.LockForm.Height = this.LockForm.BackgroundImage.Height;

            this.LockForm.BackColor = Color.AliceBlue;
            this.LockForm.TransparencyKey = Color.AliceBlue;//Color.FromArgb(0, 255, 0); //Contrast Color

            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.LockForm, "Click this button to Unlock Transparent Browser");

            this.LockForm.TopMost = true;
            this.LockForm.Location = new Point(this.Left + (this.Width / 2), this.Top + 5);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.LockForm.Location = new Point(this.Left + (this.Width / 2), this.Top);
        }

        private void Form1_VisibleChange(object sender, EventArgs e)
        {
            this.LockForm.Show();
        }

        private void LockForm_Click(object sender, EventArgs e)
        {
        }

        private void Form1_SizeChange(object sender, System.EventArgs e)
        {
            this.LockForm.Location = new Point(this.Left + (this.Width/2), this.Top + 5);
        }

        private void Form1_LocationChanged(object sender, System.EventArgs e)
        {
            this.LockForm.Location = new Point(this.Left + (this.Width / 2), this.Top + 5);
            // this.LockForm.Location = new Point(Screen.FromControl(this).Bounds.Width / 2, 5);
        }

        private void Form1_HandleCreated(object sender, System.EventArgs e, string app)
        {
        }

        #region browser methods
        // Displays the Save dialog box.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowSaveAsDialog();
        }

        // Displays the Page Setup dialog box.
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPageSetupDialog();
        }

        // Displays the Print dialog box.
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        // Displays the Print Preview dialog box.
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        // Displays the Properties dialog box.
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPropertiesDialog();
        }

        // Selects all the text in the text box when the user clicks it. 
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.SelectAll();
        }

        // Navigates to the URL in the address box when 
        // the ENTER key is pressed while the ToolStripTextBox has focus.
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Navigate(toolStripTextBox1.Text);
            }
        }

        // Navigates to the URL in the address box when 
        // the Go button is clicked.
        private void goButton_Click(object sender, EventArgs e)
        {
            Navigate(toolStripTextBox1.Text);
        }

        // Navigates to the given URL if it is valid.
        private void Navigate(String address)
        {
            if (String.IsNullOrEmpty(address))
                return;
            if (address.Equals("about:blank"))
                return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                webBrowser1.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }

        // Updates the URL in TextBoxAddress upon navigation.
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            toolStripTextBox1.Text = webBrowser1.Url.ToString();
        }

        // Navigates webBrowser1 to the previous page in the history.
        private void backButton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        // Disables the Back button at the beginning of the navigation history.
        private void webBrowser1_CanGoBackChanged(object sender, EventArgs e)
        {
            backButton.Enabled = webBrowser1.CanGoBack;
        }

        // Navigates webBrowser1 to the next page in history.
        private void forwardButton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        // Disables the Forward button at the end of navigation history.
        private void webBrowser1_CanGoForwardChanged(object sender, EventArgs e)
        {
            forwardButton.Enabled = webBrowser1.CanGoForward;
        }

        // Halts the current navigation and any sounds or animations on 
        // the page.
        private void stopButton_Click(object sender, EventArgs e)
        {
            webBrowser1.Stop();
        }

        // Reloads the current page.
        private void refreshButton_Click(object sender, EventArgs e)
        {
            // Skip refresh if about:blank is loaded to avoid removing
            // content specified by the DocumentText property.
            if (!webBrowser1.Url.Equals("about:blank"))
            {
                webBrowser1.Refresh();
            }
        }

        // Navigates webBrowser1 to the home page of the current user.
        private void homeButton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        // Navigates webBrowser1 to the search page of the current user.
        private void searchButton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoSearch();
        }

        // Prints the current document using the current print settings.
        private void printButton_Click(object sender, EventArgs e)
        {
            webBrowser1.Print();
        }

        // Updates the status bar with the current browser status text.
        private void webBrowser1_StatusTextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = webBrowser1.StatusText;
        }

        // Updates the title bar with the current document title.
        private void webBrowser1_DocumentTitleChanged(object sender, EventArgs e)
        {
            this.Text = webBrowser1.DocumentTitle;
        }

        // Exits the application.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private WebBrowser webBrowser1;

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem,
            saveAsToolStripMenuItem, printToolStripMenuItem,
            printPreviewToolStripMenuItem, exitToolStripMenuItem,
            pageSetupToolStripMenuItem, propertiesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1, toolStripSeparator2;

        private ToolStrip toolStrip1, toolStrip2, toolStrip3;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripButton goButton, backButton,
            forwardButton, stopButton, refreshButton,
            homeButton, searchButton, printButton;

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;

        private TrackBar trackBar1;
        private Label scrollLabel = new Label();
        public CheckBox lockPage = new CheckBox();
        public Label lockPageLabel = new Label();

        private Control[] nonBrowserArray;

        private void InitializeForm()
        {
            webBrowser1 = new WebBrowser();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            printToolStripMenuItem = new ToolStripMenuItem();
            printPreviewToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            pageSetupToolStripMenuItem = new ToolStripMenuItem();
            propertiesToolStripMenuItem = new ToolStripMenuItem();

            toolStrip1 = new ToolStrip();
            goButton = new ToolStripButton();
            backButton = new ToolStripButton();
            forwardButton = new ToolStripButton();
            stopButton = new ToolStripButton();
            refreshButton = new ToolStripButton();
            homeButton = new ToolStripButton();
            searchButton = new ToolStripButton();
            printButton = new ToolStripButton();

            toolStrip2 = new ToolStrip();
            toolStripTextBox1 = new ToolStripTextBox();

            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();

            toolStrip3 = new ToolStrip();
            trackBar1 = new TrackBar();
            lockPageLabel = new Label();


            printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;

            saveAsToolStripMenuItem.Click +=
                new System.EventHandler(saveAsToolStripMenuItem_Click);
            pageSetupToolStripMenuItem.Click +=
                new System.EventHandler(pageSetupToolStripMenuItem_Click);
            printToolStripMenuItem.Click +=
                new System.EventHandler(printToolStripMenuItem_Click);
            printPreviewToolStripMenuItem.Click +=
                new System.EventHandler(printPreviewToolStripMenuItem_Click);
            propertiesToolStripMenuItem.Click +=
                new System.EventHandler(propertiesToolStripMenuItem_Click);
            exitToolStripMenuItem.Click +=
                new System.EventHandler(exitToolStripMenuItem_Click);

            toolStrip1.Items.AddRange(new ToolStripItem[] {
                goButton, backButton, forwardButton, stopButton,
                refreshButton, homeButton, searchButton, printButton});

            goButton.Text = "Go";
            backButton.Text = "Back";
            forwardButton.Text = "Forward";
            stopButton.Text = "Stop";
            refreshButton.Text = "Refresh";
            homeButton.Text = "Home";
            searchButton.Text = "Search";
            printButton.Text = "Print";

            backButton.Enabled = false;
            forwardButton.Enabled = false;

            goButton.Click += new System.EventHandler(goButton_Click);
            backButton.Click += new System.EventHandler(backButton_Click);
            forwardButton.Click += new System.EventHandler(forwardButton_Click);
            stopButton.Click += new System.EventHandler(stopButton_Click);
            refreshButton.Click += new System.EventHandler(refreshButton_Click);
            homeButton.Click += new System.EventHandler(homeButton_Click);
            searchButton.Click += new System.EventHandler(searchButton_Click);
            printButton.Click += new System.EventHandler(printButton_Click);

            toolStrip2.Items.Add(toolStripTextBox1);

            toolStripTextBox1.Size = new System.Drawing.Size(500, 25);
            toolStripTextBox1.KeyDown += new KeyEventHandler(toolStripTextBox1_KeyDown);
            toolStripTextBox1.Click += new System.EventHandler(toolStripTextBox1_Click);

            statusStrip1.Items.Add(toolStripStatusLabel1);

            webBrowser1.Dock = DockStyle.Fill;
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webBrowser1_Navigated);

            scrollLabel.Text = "Transparency: " + trackBar1.Value;
            scrollLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Set up the TrackBar.
            //this.trackBar1.Location = new System.Drawing.Point(scrollLabel.Right);
            this.scrollLabel.Location = new System.Drawing.Point(toolStripTextBox1.Bounds.Right + 2);
            this.trackBar1.Location = new System.Drawing.Point(scrollLabel.Right + 2);
            this.trackBar1.Size = new System.Drawing.Size(224, 10);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);

            // this.lockPageLabel.Location = new Point(trackBar1.Right + 2);
            // this.lockPageLabel.Text = "LOCK";
 
            // this.lockPageLabel.TextAlign = ContentAlignment.MiddleRight;

            // this.lockPage.Location = new Point(lockPageLabel.Right + 2);
            // this.lockPage.CheckedChanged += new EventHandler(lockPage_Check);
            // this.ControlRemoved += new ControlEventHandler(Control_Removed);
            //this.lockPage.LocationChanged += new EventHandler(lockPage_LocationChanged);
            //this.lockPage.TextAlign = ContentAlignment.MiddleLeft;
 
            trackBar1.Maximum = 100;
            trackBar1.TickFrequency = 5;
            trackBar1.LargeChange = 10;
            trackBar1.SmallChange = 5;

            this.Controls.AddRange(new Control[] {
                scrollLabel, trackBar1, /*lockPage, lockPageLabel,*/ webBrowser1,
                toolStrip1, toolStrip2, statusStrip1});

            this.nonBrowserArray = new Control[5] { scrollLabel, trackBar1, toolStrip1, toolStrip2, statusStrip1  };
        }

        private void Control_Removed(object sender, EventArgs e)
        {
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var stuff = trackBar1;
            scrollLabel.Text = "Transparency: " + trackBar1.Value;
            this.Opacity = ((trackBar1.Value * -1) + 100) / 100d;
        }

        //TODO delete this after confirming not needed
        private void lockPage_Set(object sender, EventArgs e)
        {
            if (pageLocked)
            {
                pageLocked = true;
                /* have to set to formWindowState.normal because some weird bug where if windows form
                is already full screen (but not covering task bar) maximizing won't cover task bar?*/
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //changeback
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Bounds = Screen.FromControl(this).Bounds;
                this.TopMost = true;
                this.LockForm.TopMost = true;

                for (int i = 0; i < nonBrowserArray.Length; i++)
                {
                    this.Controls.Remove(this.nonBrowserArray[i]);
                }

                this.LockForm.Show();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.TopMost = false;

                this.Controls.AddRange(this.nonBrowserArray);
            }
        }


        private void lockPage_Check(object sender, EventArgs e)
        {
            if (lockPage.Checked)
            {
                /* have to set to formWindowState.normal because some weird bug where if windows form
                is already full screen (but not covering task bar) maximizing won't cover task bar?*/
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //changeback
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Bounds = Screen.FromControl(this).Bounds;
                this.TopMost = true;
                this.LockForm.TopMost = true;

                for (int i = 0; i < nonBrowserArray.Length; i++)
                {
                    this.Controls.Remove(this.nonBrowserArray[i]);
                }

                this.LockForm.Show();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.TopMost = false;

                this.Controls.AddRange(this.nonBrowserArray);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (!pageLocked) return base.CreateParams;

                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT

                return createParams;
            }
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_NCHITTEST = 0x0084;
        //    const int HTTRANSPARENT = (-1);

        //    if (m.Msg == 0x0112) // WM_SYSCOMMAND
        //    {
        //        // Check your window state here
        //        if (m.WParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
        //        {
        //            this.IsFullScreen = true;
        //            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        //            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        //            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        //            this.Bounds = Screen.FromControl(this).Bounds;

        //            this.LockForm.Location = new Point(Screen.FromControl(this).Bounds.Width/2, 0);
        //        }
        //    }
        //    //else if (m.Msg == WM_NCHITTEST)
        //    //{
        //    //    m.Result = (IntPtr)HTTRANSPARENT;
        //    //}

        //    base.WndProc(ref m);
        //}

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void LockForm_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.pageLocked = !this.pageLocked;

            if (pageLocked)
            {
                /* have to set to formWindowState.normal because some weird bug where if windows form
                is already full screen (but not covering task bar) maximizing won't cover task bar?*/
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //changeback
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Bounds = Screen.FromControl(this).Bounds;
                this.LockForm.BackgroundImage = TransparentForm.Properties.Resources.unlock_img;//Image
                this.TopMost = true;
                this.LockForm.TopMost = true;

                for (int i = 0; i < nonBrowserArray.Length; i++)
                {
                    this.Controls.Remove(this.nonBrowserArray[i]);
                }
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.TopMost = false;

                this.Controls.AddRange(this.nonBrowserArray);

                this.LockForm.BackgroundImage = TransparentForm.Properties.Resources.lock_img;//Image
            }

            return;
        }

        //private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        ReleaseCapture();
        //        if (lockPage.Checked)
        //        {
        //            MessageBox.Show("lock: " + lockPage.Location.ToString() + ", click: " + e.Location.ToString()); 
        //            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        //        }
        //        else
        //        {
        //            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        //        }
        //    }
        //}

    }
}
