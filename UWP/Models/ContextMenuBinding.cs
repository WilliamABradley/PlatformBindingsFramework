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

using Windows.Foundation;
using Windows.UI.Xaml;

namespace PlatformBindings.Models
{
    public class ContextMenuBinding : IMenuBinding
    {
        public ContextMenuBinding(FrameworkElement Element, Point? Position)
        {
            this.Element = Element;
            this.Position = Position;
        }

        public FrameworkElement Element { get; }
        public Point? Position { get; }

        public object DataContext => Element.DataContext;
    }
}