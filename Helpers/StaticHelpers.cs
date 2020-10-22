using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GAMERS_TECH
{
    public static class StaticHelpers
    {
        public static string ServerBaseAddress = Environment.GetEnvironmentVariable("GamersServerUri");

        public static readonly HttpClient httpclient = new HttpClient();
    }
}
