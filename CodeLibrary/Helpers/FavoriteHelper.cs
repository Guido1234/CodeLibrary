using CodeLibrary.Core;
using System;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class FavoriteHelper
    { 
        private readonly ToolStripMenuItem _mnuFavoriteLibraries;
        private readonly FileHelper _fileHelper;

        public FavoriteHelper(FormCodeLibrary mainform, FileHelper fileHelper)
        {
            _mnuFavoriteLibraries = mainform.mnuFavoriteLibraries;
            _fileHelper = fileHelper;
        }

        public void AddCurrentToFavorite()
        {
            string _file = Config.LastOpenedFile;

            if (!File.Exists(_file))
                return;

            foreach (string _f in Config.FavoriteLibs)
            {
                if (_f.Equals(_file, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            Config.FavoriteLibs.Add(_file);

            FileInfo _fileInfo = new FileInfo(_file);

            ToolStripItem _item = _mnuFavoriteLibraries.DropDownItems.Add(_fileInfo.Name);
            _item.Tag = _file;
            _item.Image = global::CodeLibrary.Properties.Resources.star_32x32;
            _item.ImageScaling = ToolStripItemImageScaling.None;
            _item.Click += Item_Click;
        }

        public void BuildMenu()
        {
            _mnuFavoriteLibraries.DropDownItems.Clear();

            int index = 1;
            foreach (string _file in Config.FavoriteLibs)
            {
                if (!File.Exists(_file))
                    continue;

                FileInfo _fileInfo = new FileInfo(_file);

                ToolStripMenuItem _item = (ToolStripMenuItem)_mnuFavoriteLibraries.DropDownItems.Add(_fileInfo.Name);
                _item.Tag = _file;
                _item.Image = global::CodeLibrary.Properties.Resources.star_32x32;
                _item.ImageScaling = ToolStripItemImageScaling.None;
                if (index > 0 && index < 13)
                    _item.ShortcutKeys = Keys.Control | GetShortCutKey(index);

                _item.Click += Item_Click;
                index++;
            }
        }

        public void RemoveCurrentFromFavorite()
        {
            string _file = Config.LastOpenedFile;

            if (!File.Exists(_file))
                return;

            Config.FavoriteLibs.Remove(_file);

            ToolStripItem _removeItem = null;
            foreach (ToolStripItem _item in _mnuFavoriteLibraries.DropDownItems)
            {
                if (_item.Tag.Equals(_file))
                {
                    _removeItem = _item;
                    break;
                }
            }
            if (_removeItem == null)
                return;

            _mnuFavoriteLibraries.DropDownItems.Remove(_removeItem);
        }

        private Keys GetShortCutKey(int index)
        {
            switch (index)
            {
                case 1:
                    return Keys.F1;

                case 2:
                    return Keys.F2;

                case 3:
                    return Keys.F3;

                case 4:
                    return Keys.F4;

                case 5:
                    return Keys.F5;

                case 6:
                    return Keys.F6;

                case 7:
                    return Keys.F7;

                case 8:
                    return Keys.F8;

                case 9:
                    return Keys.F9;

                case 10:
                    return Keys.F10;

                case 11:
                    return Keys.F11;

                case 12:
                    return Keys.F12;
            }
            return Keys.None;
        }

        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripItem _item = (ToolStripItem)sender;
            string _filename = (string)_item.Tag;

            if (!File.Exists(_filename))
            {
                MessageBox.Show($"File '{_filename}' does not exists!.");
                return;
            }

            _fileHelper.OpenFile(_filename);
        }


        public void OpenDemo()
        {
            string _filename = Path.Combine(Application.StartupPath, @"Libraries\Demo.json");
            
 
            _fileHelper.SaveFile(false);

            if (!File.Exists(_filename))
            {
                MessageBox.Show($"File '{_filename}' does not exists!.");
                return;
            }

            _fileHelper.OpenFile(_filename);
        }

        public void OpenCSharpLibrary()
        {
            string _filename = Path.Combine(Application.StartupPath, @"Libraries\CSharp Library.json");


            _fileHelper.SaveFile(false);

            if (!File.Exists(_filename))
            {
                MessageBox.Show($"File '{_filename}' does not exists!.");
                return;
            }

            _fileHelper.OpenFile(_filename);
        }
    }
}