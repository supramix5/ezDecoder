using System.Windows.Forms;

namespace EzDecoder.PopUpProgress;
public class ProgressForm : Form
{
    private Label label;
    private ProgressBar progressBar;

    public ProgressForm()
    {
        Width = 400;
        Height = 120;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Decoding Files...";
        ControlBox = false;
        BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

        label = new Label()
        {
            Left = 20,
            Top = 10,
            Width = 350,
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            ForeColor = System.Drawing.Color.White,
            BackColor = System.Drawing.Color.Transparent
        };
        Controls.Add(label);

        progressBar = new ProgressBar()
        {
            Left = 20,
            Top = 40,
            Width = 350,
            Height = 25,
            Minimum = 0,
            ForeColor = System.Drawing.Color.White,
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30)
        };
        Controls.Add(progressBar);
    }

    public void UpdateStatus(string fileName, int value, int max)
    {
        label.Text = fileName;
        progressBar.Maximum = max;
        progressBar.Value = value;
        Refresh();
    }
}
