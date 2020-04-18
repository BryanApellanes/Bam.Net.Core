/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bam.Net;
using System.Collections.Concurrent;
using Bam.Net.CommandLine;

namespace Bam.Net.Caching.File
{
	/// <summary>
	/// A caching mechanism for files. 
	/// </summary>
	public abstract class FileCache: IFileCache
	{
        static readonly object _lock = new object();
        static ConcurrentDictionary<string, CachedFile> _cachedFiles;
        static ConcurrentDictionary<string, string> _hashes;

        public FileCache()
        {
            _lock.DoubleCheckLock(ref _cachedFiles, () => new ConcurrentDictionary<string, CachedFile>());
            _lock.DoubleCheckLock(ref _hashes, () => new ConcurrentDictionary<string, string>());
        }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        public string FileExtension { get; protected set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public abstract byte[] GetContent(string filePath);

        /// <summary>
        /// Gets the content of the zipped file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public abstract byte[] GetZippedContent(string filePath);

        /// <summary>
        /// The result of zipping ReadAllBytes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual byte[] GetZippedBytes(FileInfo file)
        {
            if (EnsureFileIsLoaded(file))
            {
                return _cachedFiles[file.FullName].GetZippedBytes();
            }

            return System.IO.File.ReadAllBytes(file.FullName).GZip();
        }

        /// <summary>
        /// The result of ReadAllBytes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual byte[] GetBytes(FileInfo file)
        {
            if (EnsureFileIsLoaded(file))
            {
                return _cachedFiles[file.FullName].GetBytes();
            }

            return System.IO.File.ReadAllBytes(file.FullName);
        }

        /// <summary>
        /// The result of ReadAllText
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual string GetText(FileInfo file)
        {
            if (EnsureFileIsLoaded(file))
            {
                return _cachedFiles[file.FullName].GetText();
            }
            return System.IO.File.ReadAllText(file.FullName);
        }
        
        /// <summary>
        /// The result of zipping ReadAllText
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual byte[] GetZippedText(FileInfo file)
        {
            EnsureFileIsLoaded(file);
            return _cachedFiles[file.FullName].GetZippedText();
        }

        /// <summary>
        /// Ensures the file is loaded.
        /// </summary>
        /// <param name="file">The file.</param>
        public bool EnsureFileIsLoaded(FileInfo file)
        {
            if (!_cachedFiles.ContainsKey(file.FullName))
            {
                Task.Run(() => Load(file));
                return false;
            }

            Task.Run(() =>
            {
                if (HashChanged(file))
                {
                    Task.Run(() => Message.Log("FileCache: {0} hash changed, reloading file.", file.FullName));
                    Reload(file);
                }
            });
            
            return true;
        }

        /// <summary>
        /// Determines if the hash of the specified file has changed.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        protected bool HashChanged(FileInfo file)
        {
            if (_hashes.TryGetValue(file.FullName, out string hash) && 
                _cachedFiles.TryGetValue(file.FullName, out CachedFile cachedFile))
            {
                return !string.IsNullOrEmpty(hash) && !cachedFile.ContentHash.Equals(hash);
            }
            return false;
        }

        /// <summary>
        /// Removes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public bool Remove(FileInfo file)
        {
            return _cachedFiles.TryRemove(file.FullName, out CachedFile value);
        }

        /// <summary>
        /// Reloads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public virtual CachedFile Reload(FileInfo file)
        {
            return Load(file);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public virtual CachedFile Load(FileInfo file)
        {
            string cacheKey = file.FullName;
            CachedFile newValue = new CachedFile(file);
            _cachedFiles.AddOrUpdate(cacheKey, (key)=> newValue, (key, oldValue) => newValue);
            _hashes.AddOrUpdate(cacheKey, (key) => newValue.ContentHash, (key, oldValue) => newValue.ContentHash);
            return newValue;
        }      
    }
}
