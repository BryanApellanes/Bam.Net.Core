/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bam.Net.Web;
using Bam.Net.Presentation.Html;
using Bam.Net;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;
using Bam.Net.Presentation;

namespace Bam.Net.Server.Renderers
{
    /// <summary>
    /// Factory for creating renderers based on file extension.
    /// </summary>
    public partial class WebRendererFactory: WebRenderer
    {
        Dictionary<string, Func<IWebRenderer>> _renderers;
        public WebRendererFactory(ILogger logger)
            : base("text/plain", "")
        {
            this._renderers = new Dictionary<string, Func<IWebRenderer>>();
			this.Logger = logger ?? Log.Default;
            this.AddBaseRenderers();
        }

        protected ILogger Logger { get; set; }

        public void Respond(ExecutionRequest request, ContentResponder contentResponder)
        {
            IWebRenderer renderer = CreateRenderer(request, contentResponder);

            renderer.Respond(request);
        }

        protected internal IWebRenderer CreateRenderer(ExecutionRequest request, ContentResponder contentResponder)
        {
            IRequest webRequest = request.Request;
            IResponse webResponse = request.Response;

            OnCreatingRenderer(webRequest);
            
            string path = request.Request.Url.AbsolutePath;
            string ext = Path.GetExtension(path);

            IWebRenderer renderer = CreateRenderer(webRequest, ext);

            if (request.HasCallback || ScriptRenderer.Extensions.Contains(ext.ToLowerInvariant()))
            {
                renderer = new ScriptRenderer(request, contentResponder);
            }

            OnCreatedRenderer(webRequest, renderer);
            return renderer;
        }

        protected internal IWebRenderer CreateRenderer(IRequest webRequest, string ext = null)
        {
            IWebRenderer renderer = null;
            if (_renderers.ContainsKey(ext))
            {
                renderer = _renderers[ext]();
            }

            if (string.IsNullOrEmpty(ext))
            {
                // check for a format querystring param
                string format = webRequest.QueryString["format"];
                if (!string.IsNullOrEmpty(format))
                {
                    if (_renderers.ContainsKey(format))
                    {
                        renderer = _renderers[format]();
                    }
                }
                else
                {
                    renderer = _renderers[".json"]();
                }

                renderer.ContentType = GetContentType(renderer, webRequest);
            }

            return renderer;
        }

        /// <summary>
        /// The event that fires before resolving the renderer for the current request
        /// </summary>
        public event Action<WebRendererFactory, IRequest> CreatingRenderer;
        protected void OnCreatingRenderer(IRequest request)
        {
            CreatingRenderer?.Invoke(this, request);
        }
        public event Action<WebRendererFactory, IRequest, IWebRenderer> CreatedRenderer;
        protected void OnCreatedRenderer(IRequest request, IWebRenderer renderer)
        {
            if (CreatedRenderer != null)
            {
                CreatedRenderer(this, request, renderer);
            }
        }

        protected internal string GetContentType(IWebRenderer renderer, IRequest webRequest)
        {
            // TODO: revisit this to handle Accept and/or Content-Type headers 
            // see http://www.xml.com/pub/a/2005/06/08/restful.html
            // determine what the requestor wants the content type to be
            // set to in the response
            string contentType = renderer.ContentType;
            string queryContentType = webRequest.QueryString["contenttype"];

            if (!string.IsNullOrEmpty(queryContentType))
            {
                contentType = queryContentType;
            }

            return contentType;
        }

        /// <summary>
        /// Adds the renderer.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        public void AddRenderer(Func<IWebRenderer> renderer)
        {
            if (_renderers == null)
            {
                _renderers = new Dictionary<string, Func<IWebRenderer>>();
            }

            renderer().Extensions.Each(ext =>
            {
                if (!_renderers.ContainsKey(ext))
                {
                    _renderers.Add(ext, renderer);
                }
                else
                {
                    IWebRenderer alreadySet = _renderers[ext]();
                    Logger.AddEntry("Renderer of type ({0}) for extension ({1}) has already been added, Renderer of type ({2}) will not be added",
                        LogEventType.Warning,                        
                        alreadySet.GetType().Name,
                        ext,
                        renderer.GetType().Name
                    );
                }
            });
        }

    }
}
