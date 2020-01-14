using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Presentation
{
    public class PageModel<AP> : PageModel // AP = Action Provider
    {
        public PageModel(IEnumerable<object> state, AP actionProvider, IRequest request, AppContentResponder appContentResponder) : this(actionProvider, request, appContentResponder)
        {
            ViewModel.State = state;
        }
        
        public PageModel(AP actionProvider, IRequest request, AppContentResponder appContentResponder) : this(request,
            appContentResponder)
        {
            ViewModel = new ViewModel<AP> {ActionProvider = actionProvider};
        }
        
        public PageModel(IRequest request, AppContentResponder appContentResponder) : base(request, appContentResponder)
        {
            ViewModel = new ViewModel<AP>();
        }
        
        public new ViewModel<AP> ViewModel { get; set; }
    }
    
    public class PageModel
    {
        public PageModel(IRequest request, AppContentResponder appContentResponder)
        {
            Request = request;
            AppContentResponder = appContentResponder;
            ApplicationModel = new ApplicationModel(appContentResponder.AppConf);
            string absolutePath = request.Url.AbsolutePath;
            string extension = Path.GetExtension(absolutePath);
            string path = absolutePath.Truncate(extension.Length);
            LayoutModel = AppContentResponder.GetLayoutModelForPath(path);
            ViewModel = new ViewModel();
        }
        
        public ApplicationModel ApplicationModel { get; set; }
        
        public string Name { get; set; }
        public LayoutModel LayoutModel { get; set; }
        public ViewModel ViewModel { get; set; }
        
        internal IRequest Request { get; set; }
        internal AppContentResponder AppContentResponder { get; set; }
    }
}
