using CommunityToolkit.Mvvm.Input;
using NavigationMVVM;
using NavigationMVVM.Services;
using SD_Images.GUI.Model;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace SD_Images.GUI.ViewModel
{
    public class HomePageViewModel : ObservableDisposableObject
    {
        private WebUIImageViewModel[] _images;

        public HomePageViewModel(IParameterNavigationService<WebUIImage> openImage)
        {
            _images = UISettingsManager.Current.ImagesDirectories
                .SelectMany(id => Directory.GetFiles(id, "*.png", SearchOption.AllDirectories))
                .Select(img => new WebUIImageViewModel(new WebUIImage(img)))
                .OrderByDescending(img => img.Source.Date)
                .ToArray();

            OpenImageCommand = new RelayCommand<WebUIImageViewModel>(img => openImage.Navigate(img.Source));
        }

        public WebUIImageViewModel[] Images => _images;

        public ICommand OpenImageCommand { get; }
    }
}
