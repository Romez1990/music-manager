using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Core.FileSystemElement;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace WpfApp {
    public class MainViewModel : ViewModelBase {
        public MainViewModel() {
            DirectoryChecked = new RelayCommand(OnDirectoryChecked);

            FsNodeElementFactory fsNodeElementFactory = null;
            var directory = fsNodeElementFactory
                .CreateDirectoryElement(@"D:\Dev projects\C#\music-manager-old\test\TestCompilation\Asking Alexandria")
                .IfLeft(() => throw new Exception());
            directory.RootChanged += OnDirectoryChanged;
            SetDirectory(directory);
        }

        public ICommand DirectoryChecked { get; }

        private void OnDirectoryChecked() {
            Debug.Print("123");
        }

        private void OnDirectoryChanged(object sender, RootDirectoryElementChangedEventArgs e) =>
            SetDirectory(e.Directory);

        private IReadOnlyList<IFsNodeElement> _directory;

        public IReadOnlyList<IFsNodeElement> Directory {
            get => _directory;
            private set => Set(() => Directory, ref _directory, value, true);
        }

        private void SetDirectory(IDirectoryElement directory) =>
            Directory = new IFsNodeElement[] {directory};
    }
}
