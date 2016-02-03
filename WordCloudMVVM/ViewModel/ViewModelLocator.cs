using Ninject;

namespace WordCloudMVVM.ViewModel
{
    public class ViewModelLocator
    {
        private static readonly IKernel Kernel = new StandardKernel(new KernelModule());

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => Kernel.Get<MainViewModel>();

        public static void Cleanup()
        {
        }
    }
}