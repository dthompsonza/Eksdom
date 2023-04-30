using System.Diagnostics;
using Eksdom.Client;
using Eksdom.Shared;

namespace Eksdom.App.Windows;

public partial class frmConfig : Form
{
    private string? _licenceKey;
    
    public frmConfig()
    {
        InitializeComponent();
        _licenceKey = Config.LicenceKey;
        txtLicenceKey.Text = _licenceKey;
    }

    private void frmConfig_Load(object sender, EventArgs e)
    {
        //linkSubscribe.Links.Add(0, linkSubscribe.Text.Length, "https://eskomsepush.gumroad.com/l/api");
        linkSubscribe.LinkClicked += LinkedSubscribedLabelClicked;
    }

    private void btnTestApiKey_Click(object sender, EventArgs e)
    {
        const string caption = "API Licence Key";
        if (!EspClient.ValidateLicenceKey(txtLicenceKey.Text, out var key))
        {
            MessageBox.Show("Invalid licence key. Ensure you have entered all 35 characters correctly.",
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        if (_licenceKey is not null &&
            _licenceKey.Equals(key!, StringComparison.InvariantCultureIgnoreCase))
        {
            this.Close();
            return;
        }

        var espClient = EspClient.CreateNonStatic(new EspClientOptions(key!));

        try
        {
            var allowance = espClient.GetAllowance();
            if (allowance is null)
            {
                MessageBox.Show("The licence key appears to work however no valid response from the server was received.",
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Environment.SetEnvironmentVariable(GlobalConstants.EnvironmentVarApiKey, key.ToString(), EnvironmentVariableTarget.Machine);
            this.Close();
            return;
        }
        catch (EksdomException)
        {
            MessageBox.Show("That licence key was rejected by the Eskom Se Push server.",
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }

    private void LinkedSubscribedLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        linkSubscribe.LinkVisited = true;
        var url = "https://eskomsepush.gumroad.com/l/api";
        System.Diagnostics.Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
    }
}
