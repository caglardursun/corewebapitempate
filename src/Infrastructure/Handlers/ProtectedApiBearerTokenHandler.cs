using API.Contracts;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace API.Infrastructure.Handlers
{
    public class ProtectedApiBearerTokenHandler : DelegatingHandler
    {
        private readonly IApiAuthenticationService _authServerConnect;

        public ProtectedApiBearerTokenHandler(IApiAuthenticationService authServerConnect)
        {
            _authServerConnect = authServerConnect;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // request the access token
            //var accessToken = await _authServerConnect.RequestClientCredentialsTokenAsync();

            //// set the bearer token to the outgoing request as Authentication Header
            //request.SetBearerToken(accessToken);

            //// Proceed calling the inner handler, that will actually send the requestto our protected api
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
