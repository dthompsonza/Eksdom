using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Eksdom.EskomSePush.Client;
using Eksdom.Integration.EskomSePush;
using Eksdom.Shared;
using Integration.EskomSePush;

namespace Eksdom.App.Windows;

public partial class frmConfig : Form
{
    private string? _licenceKey;

    public frmConfig()
    {
        InitializeComponent();
        _licenceKey = Environment.GetEnvironmentVariable(Constants.EnvironmentVarApiKey, EnvironmentVariableTarget.Machine);
        txtLicenceKey.Text = _licenceKey;
    }

    private void frmConfig_Load(object sender, EventArgs e)
    {
        linkSubscribe.Links.Add(0, linkSubscribe.Text.Length, "https://eskomsepush.gumroad.com/l/api");
    }

    private void btnTestApiKey_Click(object sender, EventArgs e)
    {
        const string caption = "API Licence Key";
        if (!ValidateLicenceKey(txtLicenceKey.Text))
        {
            MessageBox.Show("Invalid licence key", caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var key = txtLicenceKey.Text.Trim().ToUpperInvariant();
        if (_licenceKey is not null && _licenceKey.Equals(key.ToString(), StringComparison.InvariantCultureIgnoreCase))
        {
            this.Close();
            return;
        }

        var espClient = EspClient.CreateNonStatic(new EspClientOptions(key));

        try
        {
            var allowance = espClient.GetAllowance();
            if (allowance is null)
            {
                MessageBox.Show("The licence key appears to work however no valid response from the server was received", 
                    caption, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            Environment.SetEnvironmentVariable(Constants.EnvironmentVarApiKey, key.ToString(), EnvironmentVariableTarget.Machine);
            this.Close();
            return;
        }
        catch (EksdomException)
        {
            MessageBox.Show("That licence key was rejected by the Eskom Se Push server",
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }

    private bool ValidateLicenceKey(string input)
    {
        string pattern = @"^[a-fA-F0-9]{8}-[a-fA-F0-9]{8}-[a-fA-F0-9]{8}-[a-fA-F0-9]{8}$";

        return Regex.IsMatch(input, pattern);
    }
}
