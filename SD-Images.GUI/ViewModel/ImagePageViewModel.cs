using NavigationMVVM;
using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using SD_Images.GUI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace SD_Images.GUI.ViewModel
{
    public class ImagePageViewModel : ObservableDisposableObject
    {
        public ImagePageViewModel(WebUIImage source, INavigationService backNavigation)
        {
            Source = source;

            Name = Path.GetFileNameWithoutExtension(Source.Path);
            BackCommand = new NavigateCommand(backNavigation);

            Positive = ParsePrompt(Source.PositivePrompt);
            Negative = ParsePrompt(Source.NegativePrompt);

            Properties = Source.Properties.Where(kv => kv.Key != "positive" && kv.Key != "negative").ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public ICommand BackCommand { get; }

        public WebUIImage Source { get; }

        public string[] Positive { get; }
        public string[] Negative { get; }
        public IReadOnlyDictionary<string, string> Properties { get; }

        public string Name { get; }        

        private static string[] ParsePrompt(string prompt)
        {
            return prompt?.Replace("(", "").Replace(")", "").Split(new char[] {',' }, 
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
        }
    }
}
