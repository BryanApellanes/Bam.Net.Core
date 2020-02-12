/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Bam.Net.Server;
using Newtonsoft.Json;
using Bam.Net.Presentation;
using Bam.Net.Presentation.Html;
using Bam.Net.Configuration;

namespace Bam.Net.Server
{
    public partial class LayoutConf
    {
        static string ContentRootKey = "ContentRoot";
        static string DefaultContentRoot = BamPaths.Content;
        /// <summary>
        /// Required for deserialization
        /// </summary>
        public LayoutConf() { }

        public LayoutConf(AppConf conf)
        {
            IncludeCommon = true;
			SetConf(conf);
        }

        internal AppConf AppConf
        {
            get;
            set;
        }
        public string Extras { get; set; }
        public string QueryString { get; set; }
        public string LayoutName { get; set; }

        public bool IncludeCommon { get; set; }

		public bool RenderBody { get; set; }

		public void SetConf(AppConf appConf)
		{
			RenderBody = appConf.RenderLayoutBody;
			LayoutName = appConf.DefaultLayout;
			AppConf = appConf;
		}

        public LayoutModel CreateLayoutModel(string[] htmlPathSegments = null)
        {
            LayoutModel model = new LayoutModel()
            {
                ApplicationName = AppConf.Name,
                QueryString = QueryString,
                Extras = string.IsNullOrEmpty(Extras) ? null : JsonConvert.DeserializeObject(Extras),
                LayoutName = LayoutName,
                ApplicationDisplayName = AppConf.DisplayName
            };
            SetIncludes(AppConf, model);

            if (htmlPathSegments != null && RenderBody) 
            {
                SetContent(model, htmlPathSegments);
            }

            return model;
        }

        protected string ContentRoot
        {
            get
            {
                return AppConf?.BamConf?.ContentRoot ?? DefaultConfiguration.GetAppSetting(ContentRootKey, DefaultContentRoot);
            }
        }

        protected internal void SetContent(LayoutModel layout, string[] pathSegments) 
        {
            Fs appRoot = AppConf.AppRoot;
            if (appRoot.FileExists(pathSegments)) 
            {
                string html = appRoot.ReadAllText(pathSegments);
                CQ dollarSign = CQ.Create(html);
                string body = dollarSign["body"].Html();
                layout.PageContent = body;

                StringBuilder headLinks = new StringBuilder();
                dollarSign["link", dollarSign["head"]].Each(el =>
                {
                    headLinks.AppendLine(el.OuterHTML);
                });
                StringBuilder links = new StringBuilder(layout.StyleSheetLinkTags);
                links.Append(headLinks.ToString());              
                layout.StyleSheetLinkTags = links.ToString();

                StringBuilder scriptTags = new StringBuilder();
                dollarSign["script", dollarSign["head"]].Each(el =>
                {
                    scriptTags.AppendLine(el.OuterHTML);
                });
                StringBuilder scripts = new StringBuilder(layout.ScriptTags);
                scripts.Append(scriptTags.ToString());
                layout.ScriptTags = scripts.ToString();
            }
        }
    }
}
