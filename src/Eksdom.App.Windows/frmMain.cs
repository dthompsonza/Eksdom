using Eksdom.Shared;

namespace Eksdom.App.Windows;

public partial class frmMain : Form
{
    private AreaInformation? _areaInformation;
    private Allowance? _allowance;

    public frmMain()
    {
        InitializeComponent();
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        notifyIcon.Visible = true;
        timerMain.Start();
    }
    private void button1_Click(object sender, EventArgs e)
    {
        if (_areaInformation is null)
        {
            return;
        }
        notifyIcon.BalloonTipTitle = $"New schedule for {_areaInformation.Info.Name}";
        notifyIcon.BalloonTipText = "Your next loadshedding event will be today at 14h30";
        notifyIcon.ShowBalloonTip(3000);
    }

    private void timerMain_Tick(object sender, EventArgs e)
    {
        timerMain.Stop();
        var area = SharedData.GetAreaInformation();
        _areaInformation ??= area;
        _allowance = SharedData.GetAllowance() ?? _allowance;
        UpdateLabels();
        timerMain.Start();

        if (area is not null)
        {
            if (_areaInformation is not null && area != _areaInformation)
            {
                ShowBalloon("Schedule Updated", $"Next loadshedding for '{area.Info.Name}' is at 14h00");
            }
        }
    }

    private void UpdateLabels()
    {
        if (_allowance is not null)
        {
            statusStripMain.Items[0].Text = _allowance.ToString();
        }
        if (_areaInformation is not null)
        {
            this.Text = $"Eksdom - {_areaInformation.Info.Name} ({_areaInformation.Info.Region})";
        }
    }

    private void ShowBalloon(string title, string text, int timer = 3000)
    {
        notifyIcon.BalloonTipTitle = title;
        notifyIcon.BalloonTipText = text;
        notifyIcon.ShowBalloonTip(timer);
    }

    private void aPIKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var frmConfig = new frmConfig();
        frmConfig.ShowDialog(this);
    }
}