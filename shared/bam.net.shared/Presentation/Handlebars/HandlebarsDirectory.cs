using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Presentation.Handlebars
{
    public class HandlebarsDirectory
    {
        public static implicit operator DirectoryInfo(HandlebarsDirectory dir)
        {
            return dir.Directory;
        }

        public HandlebarsDirectory(DirectoryInfo directory)
        {
            FileExtension = "hbs";
            Directory = directory;
        }

        public HandlebarsDirectory(string directoryPath): this(new DirectoryInfo(directoryPath))
        {
        }

        public Dictionary<string, Func<object, string>> Templates { get; private set; }

        public HandlebarsDirectory CombineWith(params HandlebarsDirectory[] dirs)
        {
            Reload();
            HandlebarsDirectory combined = new HandlebarsDirectory(Directory);
            combined.CopyProperties(this);
            foreach(HandlebarsDirectory dir in dirs)
            { 
                dir.Reload();
                foreach (DirectoryInfo partialDir in dir.PartialsDirectories)
                {
                    combined.PartialsDirectories.Add(partialDir);
                }
                foreach(string key in dir.Templates.Keys)
                {
                    combined.Templates.AddMissing(key, dir.Templates[key]);
                }
            }
            combined.Reload();
            return combined;
        }

        public void AddTemplate(string templateName, string source, bool reload = false)
        {
            string filePath = Path.Combine(Directory.FullName, $"{templateName}.{FileExtension}");
            source.SafeWriteToFile(filePath, true);
            if (reload)
            {
                Reload();
            }
            else
            {
                Templates.AddMissing(templateName, HandlebarsDotNet.Handlebars.Compile(source));
            }
        }

        public void AddPartial(string templateName, string source, bool reload = false)
        {
            if(PartialsDirectories == null)
            {
                AddPartialsDirectory(Path.Combine(Directory.FullName, "Partials"));
            }
            string filePath = Path.Combine(Directory.FullName, $"{templateName}.{FileExtension}");
            source.SafeWriteToFile(filePath, true);
            if (reload)
            {
                Reload();
            }
            else
            {
                HandlebarsDotNet.Handlebars.RegisterTemplate(templateName, source);
            }
        }

        public string Render(string templateName, object data)
        {
            if (!_loaded)
            {
                Reload();
            }
            if (Templates.ContainsKey(templateName))
            {
                return Templates[templateName](data);
            }
            return string.Empty;
        }
        DirectoryInfo _directory;
        public DirectoryInfo Directory
        {
            get
            {
                return _directory;
            }
            set
            {
                SetDirectory(value);
            }
        }
        public void AddPartialsDirectory(string partialsDirectory)
        {
            if(PartialsDirectories == null)
            {
                PartialsDirectories = new HashSet<DirectoryInfo>
                {
                    new DirectoryInfo(partialsDirectory)
                };
            }
            else
            {
                PartialsDirectories.Add(new DirectoryInfo(partialsDirectory));
            }
            Reload();
        }
        public string FileExtension { get; set; }
        public HashSet<DirectoryInfo> PartialsDirectories { get; set; }
        object _reloadLock = new object();
        bool _loaded = false;
        public void Reload()
        {
            Load(true);
        }

        public void Load(bool reload)
        {
            if(!_loaded || reload)
            {
                lock (_reloadLock)
                {
                    Templates = new Dictionary<string, Func<object, string>>();
                    if (PartialsDirectories != null)
                    {
                        foreach (DirectoryInfo partialsDirectory in PartialsDirectories)
                        {
                            if (partialsDirectory.Exists)
                            {
                                foreach (FileInfo partial in partialsDirectory.GetFiles($"*.{FileExtension}"))
                                {
                                    string shortName = Path.GetFileNameWithoutExtension(partial.FullName);
                                    string longName = partial.FullName.Truncate($".{FileExtension}".Length);
                                    string content = partial.ReadAllText();
                                    HandlebarsDotNet.Handlebars.RegisterTemplate(shortName, content);
                                    HandlebarsDotNet.Handlebars.RegisterTemplate(longName, content);
                                }
                            }
                        }
                    }
                    if (Directory != null && Directory.Exists)
                    {
                        foreach (FileInfo file in Directory?.GetFiles($"*.{FileExtension}"))
                        {
                            string shortName = Path.GetFileNameWithoutExtension(file.FullName);
                            string longName = file.FullName.Truncate($".{FileExtension}".Length);
                            string content = file.ReadAllText();
                            Func<object, string> template = HandlebarsDotNet.Handlebars.Compile(content);
                            Templates.AddMissing(shortName, template);
                            Templates.AddMissing(longName, template);
                        }
                    }
                    _loaded = true;
                }
            }
        }

        private void SetDirectory(DirectoryInfo directory)
        {
            _directory = directory;
            if (PartialsDirectories == null)
            {
                AddPartialsDirectory(directory.FullName);
                if (!_directory.Exists)
                {
                    _directory.Create();
                }
                DirectoryInfo partials = _directory.GetDirectories("Partials").FirstOrDefault();
                if (partials != null)
                {
                    AddPartialsDirectory(partials.FullName);
                }
            }
            Reload();
        }
    }
}
