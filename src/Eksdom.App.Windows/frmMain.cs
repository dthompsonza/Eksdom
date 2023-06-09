using Eksdom.Client;
using Eksdom.Shared;
using Eksdom.Shared.Models;

namespace Eksdom.App.Windows;

public partial class frmMain : Form
{
    private ServiceInfo? _serviceInfo;
    private AreaInformation? _areaInformation;
    private Allowance? _allowance;
    private Status? _status;

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
        _serviceInfo = SharedData.GetServiceInfo();
        _areaInformation = SharedData.GetAreaInformation();
        _allowance = SharedData.GetAllowance() ?? _allowance;
        _status = SharedData.GetStatus() ?? _status;
        UpdateLabels();
        timerMain.Start();
    }

    private void UpdateLabels()
    {
        if (_serviceInfo is null)
        {
            lblServiceInfo.Text = "Service unavailable";
        }
        else
        {
            lblServiceInfo.Text = $"{(_serviceInfo.IsRunning ? "Service running" : _serviceInfo.NotRunningReason)}";
        }
        if (_allowance is not null)
        {
            statusStripMain.Items[0].Text = $"Calls left: {_allowance.Balance}";
        }
        if (_areaInformation is not null)
        {
            this.Text = $"Eksdom - {_areaInformation.Info.Name} ({_areaInformation.Info.Region})";
        }
        if (_areaInformation is not null && _status is not null && _serviceInfo?.Override is not null)
        {
            var @event = Planner.NextOrCurrentLoadshedding(_areaInformation, _serviceInfo.Override.Value);
            if (@event is not null)
            {
                lblNextLoadshedding.Text = $"Stage {@event.StageLevel} on {@event.Start:dd MMM} starts at {@event.Start:h:mm tt} for {@event.Length.TotalHours} hours";
            }
            else
            {
                lblNextLoadshedding.Text = $"No loadshedding";
            }
        }
        else
        {
            lblNextLoadshedding.Text = $"?";
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