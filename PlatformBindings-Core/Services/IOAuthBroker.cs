using System;
using System.Threading.Tasks;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public interface IOAuthBroker
    {
        Task<OAuthResponse> Authenticate(Uri RequestUri, Uri CallbackUri);
    }
}