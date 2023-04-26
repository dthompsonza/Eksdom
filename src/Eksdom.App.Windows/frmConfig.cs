﻿using System.Text.RegularExpressions;
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
        if (!ValidateLicenceKey(txtLicenceKey.Text, out var key))
        {
            MessageBox.Show("Invalid licence key. Ensure you have entered all 35 characters correctly.", 
                caption, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
            return;
        }

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
                MessageBox.Show("The licence key appears to work however no valid response from the server was received.", 
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
            MessageBox.Show("That licence key was rejected by the Eskom Se Push server.",
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }

    private bool ValidateLicenceKey(string inputKey, out string? key)
    {
        var pattern = @"^[a-fA-F0-9]{8}-[a-fA-F0-9]{8}-[a-fA-F0-9]{8}-[a-fA-F0-9]{8}$";
        var testKey = inputKey.Trim().ToUpperInvariant();
        if (Regex.IsMatch(testKey, pattern))
        {
            key = testKey;
            return true;
        };

        key = null;
        return false;
    }
}