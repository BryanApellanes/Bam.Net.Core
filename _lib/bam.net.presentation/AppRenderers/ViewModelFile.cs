using System;
using Bam.Net.Server;

namespace Bam.Net.Presentation.AppRenderers
{
    public class ViewModelFile
    {
        public string Path { get; set; }
        public string ActionProvider { get; set; }
        
        public T GetActionProvider<T>(AppConf appConf)
        {
            throw new NotImplementedException();
        }

        public object GetActionProvider(AppConf appConf)
        {
            throw new NotImplementedException();
        }
        
        public ViewModel<T> GetViewModel<T>(AppConf appConf)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(Path);
            return new ViewModel<T>() {Name = name, ActionProvider = GetActionProvider<T>(appConf)};
        }

        public ViewModel GetViewModel(AppConf appConf)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(Path);
            return new ViewModel() {Name = name, ActionProvider = GetActionProvider(appConf)};
        }

        public virtual ViewModel Load(AppConf appConf)
        {
            // load action provider and cache in memory
            // Path/ActionProvider, object 
            throw new NotImplementedException();
        }
    }
}