using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Disasmo.Utils;

namespace Disasmo.ViewModels
{
    public sealed class IntrinsicsViewModel : ObservableObject
    {
        private string _input;
        private List<IntrinsicsInfo> _suggestions;
        private List<IntrinsicsInfo> _intrinsics;
        private bool _isBusy;
        private bool _isDownloading;
        private string _loadingStatus;

        public async void DownloadSources()
        {
            if (_isDownloading || _intrinsics?.Any() == true)
                return;

            IsBusy = true;
            _isDownloading = true;
            _intrinsics = await IntrinsicsSourcesService.ParseIntrinsics(file => { LoadingStatus = "Loading data from Github...\nParsing " + file; });
            IsBusy = false;
            _isDownloading = false;
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Input
        {
            get => _input;
            set
            {
                SetProperty(ref _input, value);
                if (_intrinsics == null || string.IsNullOrWhiteSpace(value) || value.Length < 3)
                    Suggestions = null;
                else
                    Suggestions = _intrinsics.Where(i => i.Contains(value)).Take(15).ToList();
            }
        }

        public string LoadingStatus
        {
            get => _loadingStatus;
            set => SetProperty(ref _loadingStatus, value);
        }

        public List<IntrinsicsInfo> Suggestions
        {
            get => _suggestions;
            set => SetProperty(ref _suggestions, value);
        }
    }
}
