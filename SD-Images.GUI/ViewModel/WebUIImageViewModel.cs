using NavigationMVVM;
using SD_Images.GUI.Model;
using System;
using System.IO;
using System.Windows.Media;

namespace SD_Images.GUI.ViewModel
{
    public class WebUIImageViewModel : ObservableDisposableObject
    {
        public WebUIImageViewModel(WebUIImage source)
        {
            Source = source;

            Name = Path.GetFileNameWithoutExtension(Source.Path);

            if (Source.ImageRewardScore.HasValue)
            {
                var value = Math.Clamp(Source.ImageRewardScore.Value, 0.0, 1.0);
                ScoreColor = new SolidColorBrush(Color.FromArgb(255, (byte)Math.Round(255.0 - value * 255.0), (byte)Math.Round(value * 255.0), 0));
            }            
        }
        public WebUIImage Source { get; }

        public string Name { get; }
        public Brush ScoreColor { get; }
    }
}
