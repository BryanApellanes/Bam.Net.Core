using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bam.Net.Logging;
using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    public class AppPageRendererManager : AppPageRenderer
    {
        public AppPageRendererManager(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
            Init();
        }

        public AppPageRendererManager(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
            Init();
        }

        private void Init()
        {
            _renderers = new HashSet<AppPageRenderer>();
            DefaultPageRenderer = new StaticContentPageRenderer(AppContentResponder, TemplateManager, ApplicationTemplateManager);
            AddPageRenderer(DefaultPageRenderer);
        }
        
        public StaticContentPageRenderer DefaultPageRenderer { get; private set; }
        
        private HashSet<AppPageRenderer> _renderers;
        public AppPageRenderer[] Renderers
        {
            get => _renderers.ToArray();
            set => _renderers = new HashSet<AppPageRenderer>(value);
        }

        public AppPageRendererManager AddPageRenderer(AppPageRenderer renderer)
        {
            _renderers.Add(renderer);
            return this;
        }
       
        public override byte[] RenderPage(IRequest request, IResponse response)
        {
            AppPageRenderer renderer = ResolveRenderer(request);
            if (renderer != null)
            {
                return renderer.RenderPage(request, response);
            }

            return RenderNotFound(request, response);
        }

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler RendererResolved;
        
        [Verbosity(VerbosityLevel.Warning)]
        public event EventHandler RendererNotResolved;

        private AppPageRenderer ResolveRenderer(IRequest request)
        {
            AppPageRenderer renderer = _renderers
                .ToImmutableSortedSet(new Comparer<AppPageRenderer>((x, y) => x.Order.CompareTo(y.Order)))
                .FirstOrDefault(apr => apr.FileExists(request));

            if (renderer != null)
            {
                FireEvent(RendererResolved, new RendererResolvedEventArgs {AppConf = AppConf, Request = request, AppPageRenderer = renderer});
            }
            else
            {
                renderer = DefaultPageRenderer;
                FireEvent(RendererNotResolved, new RendererResolvedEventArgs {AppConf = AppConf, Request = request, AppPageRenderer = renderer});
            }
            return renderer;
        }
    }
}