﻿using Android.App;
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

namespace PlatformBindings.Services
{
    public class AndroidUIBindings : UIBindingsBase
    {
        public AndroidUIBindings()
        {
            DefaultUIBinding = new AndroidUIBindingInfo();
        }

        public override InteractionManagerBase InteractionManager { get; }
        public override IUIBindingInfo DefaultUIBinding { get; }
        public override INavigationManager NavigationManager { get; set; }

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

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            var context = Binding as AndroidContextMenuBinding;
            context.Activity.GetHandler().OpenContextMenuForDisplay(Menu, context);
        }

        //Unsupported
        public override void SetWindowText(string Text)
        {
        }
    }
}