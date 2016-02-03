using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using WordCloudMVVM.Model.Cloud.PaintCloud;
using WordCloudMVVM.Model.Read;
using WordCloudMVVM.Model.Word;
using WordCloudMVVM.Model.WordInspector;
using WordCloudMVVM.Model.WordParse;

namespace WordCloudMVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public void OverviewTextFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                DefaultExt = ".txt"
            };

            if (openFileDialog.ShowDialog() == true)
                PathTextFile = openFileDialog.FileName;
        }

        public async void OpenTextFileAsync(object sender)
        {
            var button = (Button)sender;
            if (PathTextFile == null)
            {
                MessageBox.Show("Specify the path to the text", "Error path file", MessageBoxButton.OK);
                return;
            }
            IndeterminateOpen = true;
            button.IsEnabled = false;
            await Task.Run(() =>
            {
                var text = _textReader.ReadAll(_pathTextFile);
                var wordsWeight = _wordParser.Parse(text).ToArray();
                _goodWord = wordsWeight
                    .Where(word => !_wordInspector.IsBad(word.Say))
                    .ToList();
                _badWord = wordsWeight
                    .Where(word => _wordInspector.IsBad(word.Say))
                    .ToList();
                GoodWordCollection = new ObservableCollection<WordModelView>(
                    WordWeightToWordStyleConverter.Convert(_goodWord, MaxFontSize)
                    .Select(word => new WordModelView(word.Say, word.FontSize, word.Color, true)));
                BadWordCollection = new ObservableCollection<WordModelView>(
                    WordWeightToWordStyleConverter.Convert(_badWord, MaxFontSize)
                    .Select(word => new WordModelView(word.Say, word.FontSize, word.Color, false)));
            });
            button.IsEnabled = true;
            IndeterminateOpen = false;
        }

        public async void CreateImageAsync(object sender)
        {
            var button = (Button)sender;

            IndeterminateCreate = true;
            button.IsEnabled = false;
            await Task.Run(() =>
            {
                var wordFontSizeList = GoodWordCollection.Concat(BadWordCollection)
                    .Where(wordIsActive => wordIsActive.Active)
                    .Select(word => new WordStyle(word.Say, word.FontSize, word.Color))
                    .ToArray();
                var drawImage = _cloudPaninter.DrawCloudWord(wordFontSizeList, SizeWidth, SizeHeight, MaxFontSize);
                drawImage.Freeze();
                BitmapImage = drawImage;
            });
            button.IsEnabled = true;
            IndeterminateCreate = false;
        }

        public void SaveImage()
        {
            if (BitmapImage == null)
            {
                MessageBox.Show("The image is not created", "Error image", MessageBoxButton.OK);
                return;
            }
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".png",
                Filter = "Image Files (*.jpg, *.jpeg, *.jpe, *.tif, *.png) | *.jpg; *.jpeg; *.jpe; *.tif; *.png"
            };

            if (saveFileDialog.ShowDialog() != true) return;
            var drawingImage = new Image { Source = BitmapImage };
            var widthDraw = BitmapImage.Drawing.Bounds.Width;
            var heightDraw = BitmapImage.Drawing.Bounds.Height;
            drawingImage.Arrange(new Rect(0, 0, widthDraw, heightDraw));

            const int dpiX = 1000;
            const int dpiY = 1000;

            var width = (int)Math.Floor(widthDraw * dpiX / 96);
            var height = (int)Math.Floor(heightDraw * dpiY / 96);

            var bitmap = new RenderTargetBitmap(width, height, dpiX, dpiY, PixelFormats.Pbgra32);
            bitmap.Render(drawingImage);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                encoder.Save(stream);
        }

        public void UpdateMaxFont()
        {
            GoodWordCollection = new ObservableCollection<WordModelView>(
                WordWeightToWordStyleConverter.Convert(_goodWord, MaxFontSize)
                .Select(word => new WordModelView(word.Say, word.FontSize, word.Color, true)));
            BadWordCollection = new ObservableCollection<WordModelView>(
                WordWeightToWordStyleConverter.Convert(_badWord, MaxFontSize)
                .Select(word => new WordModelView(word.Say, word.FontSize, word.Color, false)));
        }

        private DrawingImage _bitmapImage;
        public DrawingImage BitmapImage
        {
            get
            {
                return _bitmapImage;
            }
            private set
            {
                Set(nameof(BitmapImage), ref _bitmapImage, value);
            }
        }
        private ObservableCollection<WordModelView> _goodWordCollection = new ObservableCollection<WordModelView>();
        public ObservableCollection<WordModelView> GoodWordCollection
        {
            get
            {
                return _goodWordCollection;
            }
            private set
            {
                Set(nameof(GoodWordCollection), ref _goodWordCollection, value);
            }
        }
        private ObservableCollection<WordModelView> _badWordCollection = new ObservableCollection<WordModelView>();
        public ObservableCollection<WordModelView> BadWordCollection
        {
            get
            {
                return _badWordCollection;
            }
            set
            {
                Set(nameof(BadWordCollection), ref _badWordCollection, value);
            }
        }

        public RelayCommand OverviewTextFileCommand { get; private set; }
        public RelayCommand<object> OpenTextFileCommand { get; private set; }
        public RelayCommand<object> CreateImageCommand { get; private set; }
        public RelayCommand SaveImageCommand { get; private set; }
        public RelayCommand UpdateMaxFontCommand { get; private set; }

        private string _pathTextFile;
        public string PathTextFile
        {
            get
            {
                return _pathTextFile;
            }
            set
            {
                Set(nameof(PathTextFile), ref _pathTextFile, value);
            }
        }
        public int MaxFontSize { get; set; } = 20;
        public int SizeWidth { get; set; } = 100;
        public int SizeHeight { get; set; } = 100;

        private readonly IWordWeightParser _wordParser;
        private readonly IWordInspector _wordInspector;
        private readonly ICloudPainter _cloudPaninter;
        private IReadOnlyCollection<WordWeight> _goodWord = new List<WordWeight>();
        private IReadOnlyCollection<WordWeight> _badWord = new List<WordWeight>();
        private readonly ITextReader _textReader;

        private bool _indeterminateOpen;
        public bool IndeterminateOpen
        {
            get
            {
                return _indeterminateOpen;
            }
            set
            {
                Set(nameof(IndeterminateOpen), ref _indeterminateOpen, value);
            }
        }
        private bool _indeterminateCreate;
        public bool IndeterminateCreate
        {
            get
            {
                return _indeterminateCreate;
            }
            set
            {
                Set(nameof(IndeterminateCreate), ref _indeterminateCreate, value);
            }
        }

        public MainViewModel(ITextReader textReader, ICloudPainter cloudPainter, IWordWeightParser wordParser, IWordInspector wordInspector)
        {
            _cloudPaninter = cloudPainter;
            _wordParser = wordParser;
            _wordInspector = wordInspector;
            _textReader = textReader;
            OverviewTextFileCommand = new RelayCommand(OverviewTextFile);
            OpenTextFileCommand = new RelayCommand<object>(OpenTextFileAsync);
            CreateImageCommand = new RelayCommand<object>(CreateImageAsync);
            SaveImageCommand = new RelayCommand(SaveImage);
            UpdateMaxFontCommand = new RelayCommand(UpdateMaxFont);
        }
    }
}