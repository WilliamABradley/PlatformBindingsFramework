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

using PlatformBindings.Enums;

namespace PlatformBindings.Models
{
    public class Shortcut
    {
        public Shortcut(KeyboardKey Key, bool CTRL = false, bool SHFT = false, bool ALT = false)
        {
            this.Key = Key;
            this.CTRL = CTRL;
            this.SHFT = SHFT;
            this.ALT = ALT;
        }

        public bool CTRL, SHFT, ALT;
        public KeyboardKey Key;

        public override string ToString()
        {
            return " (" + (CTRL ? "Ctrl + " : "") + (ALT ? "Alt + " : "") + (SHFT ? "Shft + " : "") + Key.ToString() + ")";
        }
    }
}