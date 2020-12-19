﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeLibrary.Core
{
    public static class Utils
    {
        public const string REG_USRSETTING = @"Software\{0}\Settings";

        public enum FileOrDirectory
        {
            File = 0,
            Directory = 1,
            DoesNotExist = 2
        }

        public enum TextEncoding
        {
            Default = 0,
            ASCII = 1,
            BigEndianUnicode = 2,
            Unicode = 3,
            UTF32 = 4,
            UTF7 = 5,
            UTF8 = 6
        }

        public static string ByteArrayToString(byte[] bytes) => ByteArrayToString(bytes, TextEncoding.UTF8);

        public static string ByteArrayToString(byte[] bytes, TextEncoding encoding) => GetEncoder(encoding).GetString(bytes);

        public static string CombinePath(string path1, string path2)
        {
            path1 = path1.Trim(new char[] { '\\' });
            path2 = path2.Trim(new char[] { '\\' });
            return $"{path1}\\{path2}";
        }

        public static bool DerivesFromBaseType(Type type, Type basetype)
        {
            if (type == basetype)
                return true;

            if (type == null)
                return false;

            if (type.BaseType != null)
            {
                if (type.BaseType == basetype)
                    return true;

                return DerivesFromBaseType(type.BaseType, basetype);
            }
            return false;
        }

        public static string FromBase64(string text) => ByteArrayToString(Convert.FromBase64String(text));

        public static T FromJson<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (T)serializer.ReadObject(memoryStream);
        }

        public static List<T> FromJsonToList<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (List<T>)serializer.ReadObject(memoryStream);
        }

        public static string GetCurrentUserRegisterKey(string regpath, string key, string defaultvalue)
        {
            if (string.IsNullOrEmpty(regpath))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                return string.Empty;

            string keyValue = defaultvalue;
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(regpath);

            if (regKey != null)
                keyValue = regKey.GetValue(key) as string;

            return keyValue;
        }

        public static string GetCurrentUserRegisterKey(string regpath, string key)
        {
            return GetCurrentUserRegisterKey(regpath, key, string.Empty);
        }

        public static Encoding GetEncoder(TextEncoding encoding)
        {
            switch (encoding)
            {
                case TextEncoding.ASCII:
                    return Encoding.ASCII;

                case TextEncoding.BigEndianUnicode:
                    return Encoding.BigEndianUnicode;

                case TextEncoding.Default:
                    return Encoding.Default;

                case TextEncoding.Unicode:
                    return Encoding.Unicode;

                case TextEncoding.UTF32:
                    return Encoding.UTF32;

                case TextEncoding.UTF7:
                    return Encoding.UTF7;

                case TextEncoding.UTF8:
                    return Encoding.UTF8;
            }
            return Encoding.Default;
        }

        public static List<Type> GetObjectsWithBaseType(Type basetype, bool skipBaseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
                foreach (Type t in assembly.GetTypes())
                    if (DerivesFromBaseType(t, basetype))
                        if (!(t == basetype && skipBaseType))
                            types.Add(t);

            return types;
        }

        public static FileOrDirectory IsFileOrDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return FileOrDirectory.DoesNotExist;

            if (File.Exists(path))
                return FileOrDirectory.File;
            else if (Directory.Exists(path))
                return FileOrDirectory.Directory;

            return FileOrDirectory.DoesNotExist;
        }

        public static bool MatchPattern(string s, string pattern)
        {
            if (pattern == null || s == null) return false;

            string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpattern in patterns)
            {
                string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                if (regex.IsMatch(s))
                    return true;
            }
            return false;
        }

        public static string ParentPath(string path)
        {
            return ParentPath(path, '\\');
        }

        public static string ParentPath(string path, char separator)
        {
            string[] paths = SplitPath(path, separator);
            if (paths.Length <= 1)
                return string.Empty;
            return paths[paths.Length - 2];
        }

        public static string PathCombine(params string[] paths)
        {
            StringBuilder sb = new StringBuilder();
            string _path;
            for (int ii = 0; ii < paths.Length; ii++)
            {
                if (ii > 0)
                    _path = $"\\{paths[ii].Trim(new char[] { ' ', '\\' })}";
                else
                    _path = $"{paths[ii].Trim(new char[] { ' ', '\\' })}";
                sb.Append(_path);
            }
            _path = sb.ToString();
            if (_path.Contains("\\\\"))
                _path = _path.Replace("\\\\", "\\");

            return _path;
        }

        public static string PathName(string path) => PathName(path, '\\');

        public static string PathName(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            string[] paths = path.Split(separator);
            if (paths.Length == 1)
                return paths[0];
            return paths[paths.Length - 1];
        }

        public static void SetCurrentUserRegisterKey(string regpath, string key, string value)
        {
            if (string.IsNullOrEmpty(regpath))
                return;

            if (string.IsNullOrEmpty(key))
                return;

            string keyValue = string.Empty;
            string[] keytree = SplitPath(regpath, '\\');

            // Check whether each leave in the tree exists, if not create the leave.
            foreach (string s in keytree)
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(s, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
                if (regKey == null)
                    Registry.CurrentUser.CreateSubKey(s);
            }

            RegistryKey editKey = Registry.CurrentUser.OpenSubKey(regpath, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            if (editKey != null)
                editKey.SetValue(key, value);
        }

        public static List<string> Split(string text, string splitter, bool skipEmpty)
        {
            int _splitterLen = splitter.Length;
            List<string> _items = new List<string>();
            int _start = 0;
            int _end = text.IndexOf(splitter);
            while (_end > -1)
            {
                string _item = text.Substring(_start, _end - _start);
                if (_item.Length == 0 && skipEmpty)
                    _items.Add(_item);
                else
                    _items.Add(_item);

                _start = _end + _splitterLen;
                _end = text.IndexOf(splitter, _start);
            }
            if (_start < text.Length)
            {
                string _item = text.Substring(_start, text.Length - _start);
                if (_item.Length == 0 && skipEmpty)
                    _items.Add(_item);
                else
                    _items.Add(_item);
            }

            return _items;
        }

        public static string[] SplitPath(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
                return new string[0];

            string[] items = path.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 0)
                return new string[0];

            if (items.Length == 1)
                return items;

            string[] items2 = new string[items.Length];
            items2[0] = items[0];
            for (int ii = 1; ii < items.Length; ii++)
                items2[ii] = string.Format("{0}{1}{2}", items2[ii - 1], separator, items[ii]);

            for (int ii = 0; ii < items.Length; ii++)
                items[ii] = items2[(items.Length - 1) - ii];

            return items2;
        }

        public static string[] SplitPath(string path) => SplitPath(path, Path.DirectorySeparatorChar);

        public static byte[] StringToByteArray(string s) => StringToByteArray(s, TextEncoding.UTF8);

        public static byte[] StringToByteArray(string s, TextEncoding encoding) => GetEncoder(encoding).GetBytes(s);

        public static string ToBase64(string text) => Convert.ToBase64String(StringToByteArray(text));

        public static string ToJson<T>(IEnumerable<T> items)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
                serializer.WriteObject(stream, items);
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        public static string ToJson<T>(T items)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, items);
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }


        public static string[] SplitLines(string text)
        {
            var _result = new List<string>();
            var _partBuilder = new StringBuilder();
            var _textCharArray = text.ToCharArray();
            var _prevChar = (char)0;

            for (int ii = 0; ii < _textCharArray.Length; ii++)
            {
                char _currChar = _textCharArray[ii];

                if (_currChar == '\n' && _prevChar == '\r')
                {
                    _partBuilder.Length--;
                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    _prevChar = (char)0;
                    continue;
                }
                if (_currChar == '\n' && _prevChar != '\r')
                {
                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    _prevChar = (char)0;
                    continue;
                }
                if (_prevChar == '\n' || _prevChar == '\r')
                {
                    if (_currChar != '\r' && _currChar != '\n')
                        _partBuilder.Append(_currChar);

                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    _prevChar = _textCharArray[ii];
                    continue;
                }
                _partBuilder.Append(_currChar);
                _prevChar = _textCharArray[ii];
            }
            return _result.ToArray();
        }
    }
}