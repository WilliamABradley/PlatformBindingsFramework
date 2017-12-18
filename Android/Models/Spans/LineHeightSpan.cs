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

using Android.Graphics;
using Android.Text;
using Java.Lang;

namespace PlatformBindings.Models.Spans
{
    public class LineHeightSpan : Object, Android.Text.Style.ILineHeightSpanWithDensity
    {
        public LineHeightSpan(int Height)
        {
            this.Height = Height;
        }

        public void ChooseHeight(ICharSequence text, int start, int end, int spanstartv, int v, Paint.FontMetricsInt fm)
        {
            // Should not get called, at least not by StaticLayout.
            ChooseHeight(text, start, end, spanstartv, v, fm, null);
        }

        public void ChooseHeight(ICharSequence text, int start, int end, int spanstartv, int v, Paint.FontMetricsInt fm, TextPaint paint)
        {
            var size = Height;

            if (paint != null)
            {
                size *= (int)paint.Density;
            }

            if (fm.Bottom - fm.Top < size)
            {
                fm.Top = fm.Bottom - size;
                fm.Ascent -= size;
            }
            else
            {
                if (Proportion == 0)
                {
                    /*
                     * Calculate what fraction of the nominal ascent
                     * the height of a capital letter actually is,
                     * so that we won't reduce the ascent to less than
                     * that unless we absolutely have to.
                     */

                    Paint p = new Paint
                    {
                        TextSize = 100
                    };
                    Rect r = new Rect();
                    p.GetTextBounds("ABCDEFG", 0, 7, r);

                    Proportion = (r.Top) / p.Ascent();
                }

                int need = (int)Math.Ceil(-fm.Top * Proportion);

                if (size - fm.Descent >= need)
                {
                    /*
                     * It is safe to shrink the ascent this much.
                     */

                    fm.Top = fm.Bottom - size;
                    fm.Ascent = fm.Descent - size;
                }
                else if (size >= need)
                {
                    /*
                     * We can't show all the descent, but we can at least
                     * show all the ascent.
                     */

                    fm.Top = fm.Ascent = -need;
                    fm.Bottom = fm.Descent = fm.Top + size;
                }
                else
                {
                    /*
                     * Show as much of the ascent as we can, and no descent.
                     */

                    fm.Top = fm.Ascent = -size;
                    fm.Bottom = fm.Descent = 0;
                }
            }
        }

        public int Height { get; }

        private static float Proportion = 0;
    }
}