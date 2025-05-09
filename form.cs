using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EzDecoder.App
{
    public partial class MainForm : Form
    {
        private ComboBox comboBoxMethods;
        private Button buttonDecode;
        private ArrayList methodList;
        private Image logoImage;

        public MainForm()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Set initial form properties
            Text = "-- ezDecoder --";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            ClientSize = new Size(400, 200);
            Icon = SystemIcons.Application;

            // Set background image
            string backgroundImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "background.jpg");
            if (File.Exists(backgroundImagePath))
            {
                BackgroundImage = Image.FromFile(backgroundImagePath);
                BackgroundImageLayout = ImageLayout.Center;
            }
            else
            {
                BackColor = Color.FromArgb(45, 45, 48);
            }
            ForeColor = Color.White;

            // Initialize method list
            methodList = new ArrayList { "Decode a CSV file", "Decode a TOML file", "Decode a BANK file", "Decode a directory" };

            // Initialize logo
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(basePath, "assets", "logo.png");

            if (File.Exists(imagePath))
            {
                logoImage = Image.FromFile(imagePath);
                Invalidate(); // Trigger repaint
            }

            // Initialize ComboBox
            comboBoxMethods = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = methodList,
                Width = 200,
                Location = new Point((ClientSize.Width - 200) / 2, 100),
                BackColor = Color.FromArgb(63, 63, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            FixComboBox();

            // Initialize Button
            buttonDecode = new Button
            {
                Text = "Decode!!",
                Width = 100,
                Location = new Point((ClientSize.Width - 100) / 2, 150),
                BackColor = Color.FromArgb(63, 63, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            buttonDecode.FlatAppearance.BorderColor = Color.FromArgb(104, 104, 104);
            buttonDecode.Click += buttonDecode_Click;

            RainbowEffect();

            // Small Secret - Check for music file
            string musicFilePath = Utilities.Methods.GetMusicFile();

            if (File.Exists(musicFilePath))
            {
                var waveOut = new NAudio.Wave.WaveOutEvent();
                var audioFileReader = new NAudio.Wave.AudioFileReader(musicFilePath);
                waveOut.Init(audioFileReader);
                waveOut.Play();

                // Mute Button
                Button buttonMute = new Button
                {
                    Text = "Mute",
                    Width = 80,
                    Location = new Point(10, 150),
                    BackColor = Color.FromArgb(63, 63, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                bool isMuted = false;
                buttonMute.Click += (s, e) =>
                {
                    if (isMuted)
                    {
                        waveOut.Volume = 1.0f;
                        buttonMute.Text = "Mute";
                    }
                    else
                    {
                        waveOut.Volume = 0.0f;
                        buttonMute.Text = "Unmute";
                    }
                    isMuted = !isMuted;
                };

                buttonMute.FlatAppearance.BorderColor = Color.FromArgb(104, 104, 104);

                Controls.Add(buttonMute);
                FormClosed += (s, e) =>
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    audioFileReader.Dispose();
                };
            }

            // Add controls to form
            Controls.Add(comboBoxMethods);
            Controls.Add(buttonDecode);
        }

        private void FixComboBox()
        {
            comboBoxMethods.DrawMode = DrawMode.OwnerDrawVariable;
            comboBoxMethods.DrawItem += (s, e) =>
            {
                e.Graphics.DrawString(methodList[e.Index].ToString(), e.Font, Brushes.White, e.Bounds);

            };
        }

        private void RainbowEffect()
        {
            // Add rainbow color effect to the button text
            Timer rainbowTimer = new Timer { Interval = 125 };
            int colorIndex = 0;
            Color[] rainbowColors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };

            rainbowTimer.Tick += (s, e) =>
            {
                buttonDecode.ForeColor = rainbowColors[colorIndex];
                colorIndex = (colorIndex + 1) % rainbowColors.Length;
            };

            rainbowTimer.Start();
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            string selectedMethod = comboBoxMethods.SelectedItem?.ToString();
            switch (selectedMethod)
            {
                case "Decode a CSV file":
                    Utilities.Methods.DecodeSingleCSV(false);
                    break;
                case "Decode a TOML file":
                    Utilities.Methods.DecodeSingleTOML(false);
                    break;
                case "Decode a BANK file":
                    Utilities.Methods.DecodeSingleBANK(false);
                    break;
                case "Decode a directory":
                    Utilities.Methods.DecodeDirectory();
                    break;
                default:
                    MessageBox.Show("Please select a valid method.");
                    break;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (BackgroundImage != null)
            {
                e.Graphics.DrawImage(BackgroundImage, ClientRectangle, new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (logoImage != null)
            {
                int x = 0;
                int y = 10;
                e.Graphics.DrawImage(logoImage, x, y, ClientSize.Width, 100);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
            Environment.Exit(0);
        }

    }
}
