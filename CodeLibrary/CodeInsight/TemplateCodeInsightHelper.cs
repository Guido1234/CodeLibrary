using CodeLibrary.Core;
using FastColoredTextBoxNS;
using GK.Template.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class TemplateCodeInsightHandler : BaseInsightHandler
    {
        public override void Initialize()
        {
            _ListBoxInsight.Items.Clear();

            List<Type> methodtTypes = Utils.GetObjectsWithBaseType(typeof(GK.Template.Methods.MethodBase), true);
            List<FormatMethodAttribute> attribs = new List<FormatMethodAttribute>();
            foreach (Type t in methodtTypes)
            {
                FormatMethodAttribute att = FormatMethodAttribute.GetAttribute(t);
                if (!string.IsNullOrEmpty(att.Name))
                {
                    attribs.Add(att);
                }
            }
            attribs = (from t in attribs
                       orderby t.Name ascending
                       select t).ToList();

            foreach (FormatMethodAttribute att in attribs)
            {
                _ListBoxInsight.Items.Add(att);
            }
        }

        public override void ListBoxInsight_Click(object sender, EventArgs e)
        {
            _ListBoxInsight.Visible = false;
            if (_ListBoxInsight.SelectedIndex > -1)
            {
                FormatMethodAttribute att = (FormatMethodAttribute)_ListBoxInsight.Items[_ListBoxInsight.SelectedIndex];
                _TbCode.SelectedText = att.Name;
            }
            _TbCode.Focus();
        }

        public override void ListBoxInsight_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 || e.KeyValue == 8)
            {
                _ListBoxInsight.Visible = false;
                e.Handled = true;
            }
            if (e.KeyValue == 13 || e.KeyValue == 32)
            {
                _ListBoxInsight.Visible = false;
                FormatMethodAttribute att = (FormatMethodAttribute)_ListBoxInsight.Items[_ListBoxInsight.SelectedIndex];
                _TbCode.SelectedText = att.Name;
                e.Handled = true;
            }
        }

        public override void ListBoxInsight_Leave(object sender, EventArgs e)
        {
            _ListBoxInsight.Visible = false;
            _TbCode.Focus();
        }

        public override void TbCode_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ':')
            {
                Range r = _TbCode.Selection;
                Range line = _TbCode.GetLine(r.Start.iLine);
                char[] chars = line.Text.ToCharArray();
                for (int ii = r.Start.iChar - 1; ii >= 0; ii--)
                {
                    if (chars[ii] == '}')
                        return;

                    if (chars[ii] == '{')
                    {
                        Point p = _TbCode.PlaceToPoint(r.Start);
                        p.X += 10;
                        p.Y += 50;
                        _ListBoxInsight.Location = p;

                        _ListBoxInsight.Visible = true;

                        _ListBoxInsight.BringToFront();
                        _ListBoxInsight.Focus();
                    }
                }
            }
        }
    }
}