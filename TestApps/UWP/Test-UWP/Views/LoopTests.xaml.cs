﻿using Tests.Tests;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tests_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoopTests : Page
    {
        public LoopTimerTests Viewmodel { get; } = new LoopTimerTests();

        public LoopTests()
        {
            this.InitializeComponent();
        }
    }
}