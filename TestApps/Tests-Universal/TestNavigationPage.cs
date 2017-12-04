// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.ComponentModel;

namespace Tests
{
    public enum TestNavigationPage
    {
        Unknown,
        Home,

        [Description("Test Context Menu")]
        ContextMenu,

        [Description("Test Credential Manager")]
        Credential,

        [Description("Test Files/Folders")]
        FileFolder,

        [Description("Test Loop Timer")]
        LoopTimer,

        [Description("Test OAuth")]
        OAuth,

        [Description("Test Pickers")]
        Pickers,

        [Description("Test Settings")]
        Settings
    }
}