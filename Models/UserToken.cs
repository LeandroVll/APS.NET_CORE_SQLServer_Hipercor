using System;

namespace HipercoreASPNETCORE.Controllers
{
    internal class UserToken
    {
        public UserToken()
        {
        }

        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}