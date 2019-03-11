using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Bam.Net.Server
{
    public partial class Includes
    {
        public Includes()
        {
            this._scripts = new HashSet<string>();
            this._css = new List<string>();
            FoundCommonPaths = new HashSet<string>();
            FoundAppPaths = new HashSet<string>();
        }

        [JsonIgnore]
        public HashSet<string> FoundCommonPaths { get; set; }
        
        [JsonIgnore]
        public HashSet<string> FoundAppPaths { get; set; }
        
        HashSet<string> _scripts;
        public string[] Scripts
        {
            get
            {
                return _scripts.ToArray();
            }
            set
            {
                _scripts = new HashSet<string>();
                value.Each(s => _scripts.Add(s));
            }
        }

        List<string> _css;
        public string[] Css
        {
            get
            {
                return _css.ToArray();
            }
            set
            {
                _css = new List<string>();
                _css.AddRange(value);
            }
        }

        public void AddScript(string path)
        {
            _scripts.Add(path);
        }

        public void AddCss(string path)
        {
            _css.Add(path);
        }

        public Includes Combine(Includes includes)
        {
            return Combine(this, includes);
        }

        public static Includes Combine(Includes includes1, Includes includes2)
        {
            Includes result = new Includes();
            HashSet<string> css = new HashSet<string>();
            includes1.Css.Each(c =>
            {
                css.Add(c);
            });
            includes2.Css.Each(c =>
            {
                css.Add(c);
            });
            HashSet<string> scripts = new HashSet<string>();
            includes1.Scripts.Each(scr =>
            {
                scripts.Add(scr);
            });
            includes2.Scripts.Each(scr =>
            {
                scripts.Add(scr);
            });

            result.Css = css.ToArray();
            result.Scripts = scripts.ToArray();
            
            return result;
        }
    }
}
