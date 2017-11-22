using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Controls.MenuLayout;
using Android.Content;
using PlatformBindings.Common;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using PlatformBindings.Models.DialogHandling;
using PlatformBindings.Models;
using Android.Widget;
using Android.App;

namespace PlatformBindings.Services
{
    public class AndroidUIBindings : UIBindings
    {
        public AndroidUIBindings() : base(Platform.Android)
        {
            DefaultUIBinding = new AndroidUIBindingInfo();
            TitleManager = new AndroidTitleManager();
        }

        public override InteractionManager InteractionManager { get; }
        public override IUIBindingInfo DefaultUIBinding { get; }
        public override NavigationManager NavigationManager { get; set; }
        public override ITitleManager TitleManager { get; set; }

        public override void OpenLink(Uri Uri)
        {
            var activity = AndroidHelpers.GetCurrentActivity(null);
            Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(Uri.ToString()));
            activity.StartActivity(browserIntent);
        }

        private SpannableStringBuilder CreateFormattedString(string RawText)
        {
            var sections = RawText.Split(new string[] { "^B^" }, StringSplitOptions.None);
            var builder = new SpannableStringBuilder();
            for (int i = 0; i < sections.Length; i++)
            {
                var section = sections[i];
                var start = builder.Length();
                var end = section.Length + start;

                builder.Append(section);
                if (i % 2 != 0) builder.SetSpan(new StyleSpan(TypefaceStyle.Bold), start, end, 0);
            }
            return builder;
        }

        public override async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            var activity = AndroidHelpers.GetCurrentActivity(UIBinding);

            var builder = AlertDialogBuilderBase.Pick(activity);

            builder.SetTitle(CreateFormattedString(Title));
            builder.SetMessage(CreateFormattedString(Message));
            builder.SetPrimaryButton(PrimaryButtonText);

            if (!string.IsNullOrWhiteSpace(SecondaryButtonText))
            {
                builder.SetSecondaryButton(SecondaryButtonText);
            }

            return await builder.ShowAsync();
        }

        public override async Task<string> RequestTextFromUserAsync(string Title, string Message, string OKButtonText, string CancelButtonText, IUIBindingInfo UIBinding)
        {
            var activity = AndroidHelpers.GetCurrentActivity(UIBinding);

            var builder = AlertDialogBuilderBase.Pick(activity);
            var entry = new EditText(activity);

            builder.SetTitle(CreateFormattedString(Title));
            builder.SetMessage(CreateFormattedString(Message));
            builder.SetPrimaryButton(OKButtonText);
            builder.SetView(entry);

            if (!string.IsNullOrWhiteSpace(CancelButtonText))
            {
                builder.SetSecondaryButton(CancelButtonText);
            }

            var result = await builder.ShowAsync();
            if (result == DialogResult.Primary) return entry.Text;
            return null;
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            PlatformBindingHelpers.OnUIThread(() =>
            {
                var context = Binding as AndroidContextMenuBinding;
                context.Activity.GetHandler().OpenContextMenuForDisplay(Menu, context);
            });
        }

        public override void ShowMenu(Menu Menu, object UIElement)
        {
            if (UIElement is Android.Views.View view)
            {
                PlatformBindingHelpers.OnUIThread(() =>
                {
                    var activity = view.Context as Activity;
                    activity.GetHandler().OpenContextMenuForDisplay(Menu, new AndroidContextMenuBinding(activity, view));
                });
            }
        }

        public override void RegisterMenu(Menu Menu, object UIElement)
        {
            if (UIElement is Android.Views.View view)
            {
                PlatformBindingHelpers.OnUIThread(() =>
                {
                    var activity = view.Context as Activity;
                    activity.GetHandler().AttachContextMenu(Menu, new AndroidContextMenuBinding(activity, view));
                });
            }
        }
    }
}