using MBA.Marketplace.Data.Services.Interfaces;

namespace MBA.Marketplace.API.Utils
{
    public class AppEnvironment : IAppEnvironment
    {
        private readonly IWebHostEnvironment _env;

        public AppEnvironment(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string WebRootPath => _env.WebRootPath;
    }
}
