/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bam.Net.Server;
using Bam.Net.Logging;
using Newtonsoft.Json;

namespace Bam.Net.Server
{
    public class ContentLocator
    {
        const string FileName = "contentLocator.json";
        private ContentLocator()
        {
            this._searchRules = new List<SearchRule>();
        }

        private ContentLocator(Fs rootToCheck)
            : this()
        {
            this.ContentRoot = rootToCheck;
        }

        public bool Locate(string relativePath, out string outAbsolutePath, out string[] checkedPaths)
        {
			if (!relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
			{
				relativePath = "{0}{1}"._Format(Path.DirectorySeparatorChar.ToString(), relativePath);
			}
            string ext = Path.GetExtension(relativePath).ToLowerInvariant();
            string foundPath = string.Empty;
			string checkNext = ContentRoot.CleanAppPath("~" + relativePath);
            Fs fs = ContentRoot;
            List<string> pathsChecked = new List<string>
            {
                checkNext
            };

            if (!fs.FileExists(relativePath, out foundPath))
            {
				foundPath = string.Empty;
                SearchRule[] extRules = _searchRules.Where(sr => sr.Ext.ToLowerInvariant().Equals(ext)).ToArray();
                extRules.Each(rule =>
                {
                    rule.SearchDirectories.Each(dir =>
                    {
                        List<string> segments = new List<string>();
                        segments.AddRange(dir.RelativePath.DelimitSplit("/", "\\"));
                        segments.AddRange(relativePath.DelimitSplit("/", "\\"));
                        string subPath = Path.Combine(segments.ToArray());
                        checkNext = ContentRoot.CleanAppPath(subPath);
                        pathsChecked.Add(checkNext);

                        if (fs.FileExists(subPath, out foundPath))
                        {                         
                            return false; // stop the each loop
						}
						else
						{
							foundPath = string.Empty;
						}

                        return true; // continue the each loop
                    });

                    if (!string.IsNullOrEmpty(foundPath))
                    {
                        return false; // stop the each loop
                    }

                    return true; // continue the each loop
                });
            }
            
            checkedPaths = pathsChecked.ToArray();
            outAbsolutePath = foundPath;
            return !string.IsNullOrEmpty(foundPath);
        }

        [JsonIgnore]
        public Fs ContentRoot
        {
            get;
            set;
        }

        public void Save()
        {
            string json = this.ToJson(true);
            ContentRoot.WriteFile(FileName, json);
        }

        public static ContentLocator Load(AppContentResponder appContent)
        {
            ContentLocator locator = Load(appContent.AppRoot);
            locator.AppName = appContent.AppConf.Name;
            return locator;
        }

		public static ContentLocator Load(string rootToCheck)
		{
			return Load(new Fs(new DirectoryInfo(rootToCheck)));
		}

        public static ContentLocator Load(Fs rootToCheck)
        {            
            ContentLocator locator = null;
            if (rootToCheck.FileExists(FileName))
            {
                FileInfo file = rootToCheck.GetFile(FileName);
                locator = file.FromJsonFile<ContentLocator>();
            }
            else
            {
                locator = new ContentLocator(rootToCheck);
                string[] imageDirs = new string[] { "~/img", "~/image" };
                locator.AddSearchRule(".png", imageDirs);
                locator.AddSearchRule(".gif", imageDirs);
                locator.AddSearchRule(".jpg", imageDirs);

                string[] pageDirs = new string[] { "~/pages", "~/views", "~/html" };
                locator.AddSearchRule(".htm", pageDirs);
                locator.AddSearchRule(".html", pageDirs);

                string[] cssDirs = new string[] { "~/css" };
                locator.AddSearchRule(".css", cssDirs);

				string[] jsDirs = new string[] { "~/js", "~/lib", "~/common" };
				locator.AddSearchRule(".js", jsDirs);

                locator.Save();
            }

            locator.ContentRoot = rootToCheck;
            return locator;
        }

        public string AppName
        {
            get;
            private set;
        }

        public void AddSearchRule(string ext, params string[] relativeRootDirsToSearch)
        {
            SearchDirectory[] searchDirs = new SearchDirectory[relativeRootDirsToSearch.Length];
            relativeRootDirsToSearch.Each((dir, i) =>
            {
                searchDirs[i] = new SearchDirectory(i + 1, dir);
            });

            AddSearchRule(new SearchRule(ext, searchDirs));
        }

        public void AddSearchRule(SearchRule rule)
        {
            _searchRules.Add(rule);
            Save();
        }

        List<SearchRule> _searchRules;
        public SearchRule[] SearchRules
        {
            get => _searchRules.ToArray();
            set => _searchRules = new List<SearchRule>(value);
        }
    }
}
