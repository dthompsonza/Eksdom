using Eksdom.Shared;
using EnsureThat;

namespace Eksdom.Client;

/// <summary>
/// Client for <see href="https://eskomsepush.gumroad.com/l/api">EskomSePush API</see> 2.0. 
/// <para>
///     Documentation on <see href="https://documenter.getpostman.com/view/1296288/UzQuNk3E">Postman Documenter</see>.
/// </para>
/// </summary>
public sealed partial class EspClient : IDisposable
{
    #region Static initializers

    /// <summary>
    /// Returns a singleton instance of the Eskom Se Push client.
    /// </summary>
    /// <param name="options">Options to configure client</param>
    /// <returns></returns>
    public static EspClient Create(EspClientOptions options)
    {
        if (_instance is null)
        {
            _instance = new EspClient(options);
        }
        else
        {
            if (!_instance.LicenceKeyMatches(options.LicenceKey))
            {
                _instance.SetLicenceKey(options.LicenceKey);
            }
        }

        return _instance;
    }

    /// <summary>
    /// Creates and returns a new instance instead of the static Singleton instance.
    /// </summary>
    /// <param name="options">Options to configure client</param>
    /// <returns></returns>
    public static EspClient CreateNonStatic(EspClientOptions options)
    {
        return new EspClient(options); 
    }

    #endregion

    #region Licence key methods

    public bool LicenceKeyMatches(string licenceKey)
    {
        if (!HasLicenceKey())
        {
            return false;
        }

        return _licenceKey!.Equals(licenceKey, StringComparison.InvariantCultureIgnoreCase);
    }

    public bool HasLicenceKey() => _licenceKey != null;

    public void SetLicenceKey(string licenceKey)
    {
        _instance?.CheckAndSetLicenceKeyWithHeaders(licenceKey);
    }

    public void ClearLicenceKey()
    {
        _instance?.ClearLicenceKeyAndRemoveHeaders();
    }

    public static bool ValidateLicenceKey(string licenceKey, out string? validatedKey)
    {
        return licenceKey.ValidateLicenceKey(out validatedKey);
    }

    private void CheckAndSetLicenceKeyWithHeaders(string licenceKey)
    {
        Ensure.That(licenceKey).IsNotNullOrEmpty();
        _licenceKey = licenceKey;
        _httpClient.DefaultRequestHeaders.Remove(Constants.Headers.Token);
        _httpClient.DefaultRequestHeaders.Add(Constants.Headers.Token, licenceKey);
    }

    private void ClearLicenceKeyAndRemoveHeaders()
    {
        _licenceKey = null;
        _httpClient.DefaultRequestHeaders.Remove(Constants.Headers.Token);
    }

    #endregion
}
