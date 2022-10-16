using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorMovies.Client;
using BlazorMovies.Shared.Entities;
using BlazorMovies.SharedComponents.Helpers;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; }

        private int currentCount = 0;
        IJSObjectReference module;


        [JSInvokable]
        public async Task IncrementCount()
        {

            var authState = await AuthenticationState;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                currentCount++;
            }
            else
            {
                currentCount--;
            }


            //var array = new double[] { 1, 2, 3, 4, 5, 6 };
            //var max = array.Maximum();
            //var min = array.Minimum();  


            //module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/CounterJs.js");
            //await module.InvokeVoidAsync("displayAlert", $"Max is {max}, min {min}");
            //currentCount++;
        }

    
    }
}