using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BlazorMovies.Client.Auth
{
    public class TokenRenewer : IDisposable
    {
        Timer timer;
        private readonly ILoginService _loginService;

        public TokenRenewer(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public void Initiate()
        {
       
            timer = new Timer();
            timer.Interval = 1000 * 60 *4; //4minutes
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _loginService.TryRenewToken();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
