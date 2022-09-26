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
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using BlazorMovies.Client;
using BlazorMovies.Client.Shared;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Client.Services;
using BlazorMovies.Client.Services.IService;
using BlazorMovies.Client.Helpers;
using MathNet.Numerics.Statistics;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;
        IJSObjectReference module;


        [JSInvokable]
        public async void IncrementCount()
        {
            var array = new double[] { 1, 2, 3, 4, 5, 6 };
            var max = array.Maximum();
            var min = array.Minimum();  


            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/CounterJs.js");
            await module.InvokeVoidAsync("displayAlert", $"Max is {max}, min {min}");
            currentCount++;
        }

        protected override void OnInitialized()
        {
            List<Movie> movies = _repo.GetMovies();
        }

        private async Task IncrementCountJs()
        {
            await _jsRuntime.InvokeVoidAsync("dotNetInstanceInovation", DotNetObjectReference.Create(this));
        }
    }
}