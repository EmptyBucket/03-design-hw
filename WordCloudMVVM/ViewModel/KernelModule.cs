using System;
using System.IO;
using Ninject.Modules;
using WordCloudMVVM.Model.Cloud.BuildCloud;
using WordCloudMVVM.Model.Cloud.BuildCloud.Intersection;
using WordCloudMVVM.Model.Cloud.PaintCloud;
using WordCloudMVVM.Model.Read;
using WordCloudMVVM.Model.WordInspector;
using WordCloudMVVM.Model.WordParse;
using WordCloudMVVM.Model.WordParse.Clean;
using WordCloudMVVM.Model.WordParse.Token;

namespace WordCloudMVVM.ViewModel
{
    public class KernelModule : NinjectModule
    {
        public override void Load()
        {
            var pathDicitonaryBadWord = Path.Combine(Environment.CurrentDirectory, "InspectorDictionary", "InspectorDictionary.txt");
            var pathAffHunspell = Path.Combine(Environment.CurrentDirectory, "HunspellDictionary", "ru_RU.aff");
            var pathDicionaryHunspell = Path.Combine(Environment.CurrentDirectory, "HunspellDictionary", "ru_RU.dic");

            Bind<ITextReader>().To<TxtReader>();
            Bind<NHunspell.Hunspell>()
                .ToSelf()
                .WithConstructorArgument("affFile", pathAffHunspell)
                .WithConstructorArgument("dictFile", pathDicionaryHunspell);
            Bind<IWordInspector>()
                .To<BadWordInspector>()
                .WithConstructorArgument("path", pathDicitonaryBadWord);
            Bind<ICleaner>().To<TextCleaner>();
            Bind<ITokenizer>().To<StemTokenizer>();
            Bind<IWordWeightParser>().To<WordCountParser>();
            Bind<ICloudBuilder>().To<LineCloudBuilder>();
            Bind<ICloudPainter>().To<CloudPainter>();
            Bind<IIntersectionChecker>().To<IntersectionChecker>();
        }
    }
}
