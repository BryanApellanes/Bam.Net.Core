using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using Bam.Net.Logging;

namespace Bam.Net
{
    public static class FileSystemWatchingExtensions
    {
        static ConcurrentDictionary<string, FileSystemWatcher> _watchers;
        static FileSystemWatchingExtensions()
        {
            _watchers = new ConcurrentDictionary<string, FileSystemWatcher>();
        }

        public static FileSystemWatcher OnChange(this FileSystemInfo fs, FileSystemEventHandler changeHandler)
        {
            FileSystemWatcher watcher = Get(fs.FullName);
            return OnChange(watcher, changeHandler);
        }

        public static FileSystemWatcher OnChange(this FileSystemWatcher watcher, FileSystemEventHandler changeHandler)
        {
            if (watcher == null) return watcher;
            watcher.Changed -= changeHandler;
            watcher.Changed += changeHandler;
            return watcher;
        }

        public static FileSystemWatcher OnCreated(this FileSystemInfo fs, FileSystemEventHandler createdHandler)
        {
            FileSystemWatcher watcher = Get(fs.FullName);
            return OnCreated(watcher, createdHandler);
        }

        public static FileSystemWatcher OnCreated(this FileSystemWatcher watcher, FileSystemEventHandler createdHandler)
        {
            if (watcher == null) return watcher;
            watcher.Created -= createdHandler;
            watcher.Created += createdHandler;
            return watcher;
        }

        public static FileSystemWatcher OnDeleted(this FileSystemInfo fs, FileSystemEventHandler deletedHandler)
        {
            FileSystemWatcher watcher = Get(fs.FullName);
            return OnDeleted(watcher, deletedHandler);
        }

        public static FileSystemWatcher OnDeleted(this FileSystemWatcher watcher, FileSystemEventHandler deletedHandler)
        {
            if (watcher == null) return watcher;
            watcher.Deleted -= deletedHandler;
            watcher.Deleted += deletedHandler;
            return watcher;
        }

        public static FileSystemWatcher OnError(this FileSystemInfo fs, ErrorEventHandler errorHandler)
        {
            FileSystemWatcher watcher = Get(fs.FullName);
            return OnError(watcher, errorHandler);
        }

        public static FileSystemWatcher OnError(this FileSystemWatcher watcher, ErrorEventHandler errorHandler)
        {
            if (watcher == null) return watcher;
            watcher.Error -= errorHandler;
            watcher.Error += errorHandler;
            return watcher;
        }

        public static FileSystemWatcher OnRenamed(this FileSystemInfo fs, RenamedEventHandler renamedHandler)
        {
            FileSystemWatcher watcher = Get(fs.FullName);
            return OnRenamed(watcher, renamedHandler);
        }

        public static FileSystemWatcher OnRenamed(this FileSystemWatcher watcher, RenamedEventHandler renamedHandler)
        {
            if (watcher == null) return watcher;
            watcher.Renamed -= renamedHandler;
            watcher.Renamed += renamedHandler;
            return watcher;
        }

        private static FileSystemWatcher Get(string directoryPath)
        {
            if (File.Exists(directoryPath))
            {
                directoryPath = new FileInfo(directoryPath).Directory.FullName;
            }
            if (!_watchers.ContainsKey(directoryPath))
            {
                try
                {
                    _watchers.TryAdd(directoryPath, new FileSystemWatcher { Path = directoryPath, EnableRaisingEvents = true, IncludeSubdirectories = true });
                }
                catch (Exception ex)
                {
                    Log.Trace("Exception occurred trying to watch config directory ({0}): {1}", directoryPath, ex.Message);
                }
            }

            if (_watchers.TryGetValue(directoryPath, out FileSystemWatcher fsWatcher))
            {
                return fsWatcher;
            }

            return null;
        }
    }
}
