using System.Text.Json;
using Eksdom.Shared;
using Eksdom.Shared.Serialization;

namespace Eksdom.App.Windows;

public partial class frmMain : Form
{
    //private readonly IntercomSender _intercomClient;
    private AreaInformation _areaInformation;

    public frmMain()
    {
        InitializeComponent();
        //_intercomClient = new IntercomSender(Global.IntercomNames.Service);
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        notifyIcon.Visible = true;
    }

    private void btnPing_Click(object sender, EventArgs e)
    {
        var allowance = SharedData.GetAllowance();
        lblPing.Text = allowance is null ? "Unavailable" : allowance.ToString();
    }

    private void btnArea_Click(object sender, EventArgs e)
    {
        var area = SharedData.GetAreaInformation();

        txtArea.Text = area is null ? "None" : EksdomJsonSerializer.ToJson(area, indented: true);
        if (area is not null)
        {
            _areaInformation = area;
            lblPing.Text = area.Schedule.Days.First().Stages.FirstOrDefault()!.Events.FirstOrDefault()!.Minutes.ToString();
        }
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
}