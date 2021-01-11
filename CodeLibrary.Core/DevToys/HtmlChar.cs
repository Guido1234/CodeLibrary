using System;
using System.Collections.Generic;
using System.Text;

namespace DevToys
{
    /// <summary>
    /// Represents a HTML Character.
    /// </summary>
    public struct HtmlChar : IComparable, IComparable<HtmlChar>, IComparable<char>, IComparable<string>, IEquatable<HtmlChar>, IEquatable<char>
    {
        public enum HtmlEncoding { HtmlNameEncoding = 0, HtmlNumberEncoding = 1, AsciiEncoding = 2 }

        public int Decimal { get; private set; }

        public string Html { get; private set; }

        public char[] HtmlCharArray { get; private set; }

        public string HtmlNumber { get; private set; }

        public char[] HtmlNumberCharArray { get; private set; }

        public bool IsEmpty => string.IsNullOrEmpty(Html);

        public bool Required { get; private set; }

        public char Symbol { get; private set; }

        /// <summary>Smart Encode HTML Text</summary>
        /// <param name="strict">If true all encode-able characters are encoded also non required characters.</param>
        public static string HtmlSmartEncode(string htmltext, HtmlEncoding htmlencoding, bool strict = false)
        {
            if (string.IsNullOrEmpty(htmltext))
                return string.Empty;

            var _text = htmltext.ToCharArray();
            var _sb = new StringBuilder();

            for (int index = 0; index < _text.Length; index++)
            {
                if (IsEncodedHtmlCharAtPosition(ref _text, ref index, out HtmlChar _hc))
                    AppendChar(_sb, _hc, htmlencoding, true);
                else
                    AppendChar(_sb, New(_text[index]), htmlencoding, strict);
            }
            return _sb.ToString();
        }

        public static implicit operator char(HtmlChar value) => (char)value.Decimal;

        public static implicit operator HtmlChar(char value) => New(value);

        public static bool IsSpecialChar(char c) => SpecialCharacters.Characters.ContainsKey(c);

        public static HtmlChar New(char c) => SpecialCharacters.Characters.ContainsKey(c) ? SpecialCharacters.Characters[c] : New(c, c, c.ToString(), string.Format("&#{0};", (int)c), false);

        public static HtmlChar NewEmpty() => new HtmlChar() { Decimal = 0, Symbol = ' ', Html = string.Empty, HtmlNumber = "", HtmlCharArray = new char[0], HtmlNumberCharArray = new char[0], Required = false };

        public static bool operator !=(HtmlChar c1, HtmlChar c2) => c1.CompareTo(c2) != 0;

        public static bool operator !=(HtmlChar c1, char c2) => c1.CompareTo(c2) != 0;

        public static bool operator !=(char c1, HtmlChar c2) => c1.CompareTo(c2) != 0;

        public static bool operator !=(HtmlChar c1, string c2) => c1.CompareTo(c2) != 0;

        public static bool operator !=(string c1, HtmlChar c2) => c1.CompareTo(c2) != 0;

        public static bool operator <(HtmlChar c1, HtmlChar c2) => c1.CompareTo(c2) < 0;

        public static bool operator <(HtmlChar c1, char c2) => c1.CompareTo(c2) < 0;

        public static bool operator <(char c1, HtmlChar c2) => c1.CompareTo(c2) < 0;

        public static bool operator <(HtmlChar c1, string c2) => c1.CompareTo(c2) < 0;

        public static bool operator <(string c1, HtmlChar c2) => c1.CompareTo(c2) < 0;

        public static bool operator ==(HtmlChar c1, HtmlChar c2) => c1.CompareTo(c2) == 0;

        public static bool operator ==(HtmlChar c1, char c2) => c1.CompareTo(c2) == 0;

        public static bool operator ==(char c1, HtmlChar c2) => c1.CompareTo(c2) == 0;

        public static bool operator ==(HtmlChar c1, string c2) => c1.CompareTo(c2) == 0;

        public static bool operator ==(string c1, HtmlChar c2) => c1.CompareTo(c2) == 0;

        public static bool operator >(HtmlChar c1, HtmlChar c2) => c1.CompareTo(c2) > 0;

        public static bool operator >(HtmlChar c1, char c2) => c1.CompareTo(c2) > 0;

        public static bool operator >(char c1, HtmlChar c2) => c1.CompareTo(c2) > 0;

        public static bool operator >(HtmlChar c1, string c2) => c1.CompareTo(c2) > 0;

        public static bool operator >(string c1, HtmlChar c2) => c1.CompareTo(c2) > 0;

        public static HtmlChar[] ToHtmlCharArray(string s)
        {
            var chars = HtmlSmartEncode(s, HtmlEncoding.AsciiEncoding).ToCharArray();
            var htmlchars = new HtmlChar[chars.Length];
            for (int ii = 0; ii < chars.Length; ii++)
                htmlchars[ii] = New(chars[ii]);

            return htmlchars;
        }

        public static HtmlChar ToLower(HtmlChar c) => SpecialCharacters.Characters.ContainsKey(char.ToLower(c.Symbol)) ? SpecialCharacters.Characters[char.ToLower(c.Symbol)] : New(char.ToLower(c.Symbol));

        public static string ToString(HtmlChar[] chars, HtmlEncoding encoding = HtmlEncoding.HtmlNameEncoding, bool strict = false)
        {
            var _sb = new StringBuilder();
            for (int ii = 0; ii < chars.Length; ii += ((chars[ii] == null) ? 2 : 1))
                AppendChar(_sb, chars[ii], encoding, strict);

            return _sb.ToString();
        }

        public static HtmlChar ToUpper(HtmlChar c) => SpecialCharacters.Characters.ContainsKey(char.ToUpper(c.Symbol)) ? SpecialCharacters.Characters[char.ToUpper(c.Symbol)] : New(char.ToUpper(c.Symbol));

        public int CompareTo(HtmlChar character) => (Decimal < character.Decimal) ? -1 : (Decimal > character.Decimal) ? 1 : 0;

        public int CompareTo(char character) => (Decimal < character) ? -1 : (Decimal > character) ? 1 : 0;

        public int CompareTo(string character)
        {
            if (string.IsNullOrEmpty(character))
                return -1;

            char[] _text = character.ToCharArray();

            int _index = 0;
            if (!IsEncodedHtmlCharAtPosition(ref _text, ref _index, out HtmlChar _hc))
                _hc = New(_text[_index]);

            if (Decimal < _hc.Decimal)
                return -1;

            return (Decimal > _hc.Decimal) ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            if (obj.GetType() == typeof(char))
                return CompareTo((char)obj);

            if (obj.GetType() == typeof(string))
                return CompareTo((string)obj);

            if (obj.GetType() == typeof(HtmlChar))
                return CompareTo((HtmlChar)obj);

            return CompareTo(obj.ToString());
        }

        public bool Equals(HtmlChar character) => (character.Decimal == Decimal);

        public bool Equals(char character) => (character == Decimal);

        public bool Equals(string character) => (CompareTo(character) == 0);

        public override bool Equals(object obj) => (CompareTo(obj) == 0);

        public override int GetHashCode() => Decimal.GetHashCode();

        public override string ToString() => Html;

        private static void AppendChar(StringBuilder sb, HtmlChar htmlchar, HtmlEncoding encoding, bool strict)
        {
            if (htmlchar.Required || strict)
            {
                if (encoding == HtmlEncoding.AsciiEncoding)
                    sb.Append(htmlchar.Symbol);
                if (encoding == HtmlEncoding.HtmlNameEncoding)
                    sb.Append(htmlchar.Html);
                if (encoding == HtmlEncoding.HtmlNumberEncoding)
                    sb.Append(htmlchar.HtmlNumber);

                return;
            }
            sb.Append(htmlchar.Symbol);
        }

        private static bool IsEncodedHtmlCharAtPosition(ref char[] htmltext, ref int index, out HtmlChar charFound)
        {
            charFound = SpecialCharacters.Empty;

            if (!htmltext[index].Equals('&'))
                return false; // HTML character always starts with an amp. no need to look further.

            var _sb = new StringBuilder();
            var _c = htmltext[index];
            int _max = 0;
            while (_c != ';' && index < htmltext.Length)
            {
                _c = htmltext[index];
                _sb.Append(_c);
                index++;
                _max++;
                if (_max > 6)
                {
                    index -= _max; // return to original position.
                    return false;
                }
            }

            string _key = _sb.ToString();
            if (!SpecialCharacters.Indexes.ContainsKey(_key))
            {
                index -= _max; // return to original position.
                return false;
            }

            charFound = SpecialCharacters.Characters[SpecialCharacters.Indexes[_key]];
            index--;
            return true;
        }

        private static HtmlChar New(int _decimal, char symbol, string html, string htmlnumber, bool required = true)
        {
            var _hc = new HtmlChar { Decimal = _decimal, Symbol = symbol, Html = !string.IsNullOrEmpty(html) ? html : htmlnumber, HtmlNumber = htmlnumber, Required = required };
            _hc.HtmlCharArray = _hc.Html.ToCharArray();
            _hc.HtmlNumberCharArray = _hc.HtmlNumber.ToCharArray();
            return _hc;
        }

        public class SpecialCharacters
        {
            public static readonly HtmlChar aacute = New(225, 'á', "&aacute;", "&#225;");
            public static readonly HtmlChar Aacute = New(193, 'Á', "&Aacute;", "&#193;");
            public static readonly HtmlChar acirc = New(226, 'â', "&acirc;", "&#226;");
            public static readonly HtmlChar Acirc = New(194, 'Â', "&Acirc;", "&#194;");
            public static readonly HtmlChar acute = New(180, '´', "&acute;", "&#180;");
            public static readonly HtmlChar aelig = New(230, 'æ', "&aelig;", "&#230;");
            public static readonly HtmlChar AElig = New(198, 'Æ', "&AElig;", "&#198;");
            public static readonly HtmlChar agrave = New(224, 'à', "&agrave;", "&#224;");
            public static readonly HtmlChar Agrave = New(192, 'À', "&Agrave;", "&#192;");
            public static readonly HtmlChar amp = New(38, '&', "&amp;", "&#38;");
            public static readonly HtmlChar aring = New(229, 'å', "&aring;", "&#229;");
            public static readonly HtmlChar Aring = New(197, 'Å', "&Aring;", "&#197;");
            public static readonly HtmlChar Asterisk = New(42, '*', string.Empty, "&#42;", false);
            public static readonly HtmlChar atilde = New(227, 'ã', "&atilde;", "&#227;");
            public static readonly HtmlChar Atilde = New(195, 'Ã', "&Atilde;", "&#195;");
            public static readonly HtmlChar AtSymbol = New(64, '@', string.Empty, "&#64;", false);
            public static readonly HtmlChar auml = New(228, 'ä', "&auml;", "&#228;");
            public static readonly HtmlChar Auml = New(196, 'Ä', "&Auml;", "&#196;");
            public static readonly HtmlChar Backslash = New(92, '\\', string.Empty, "&#92;", false);
            public static readonly HtmlChar brvbar = New(166, '¦', "&brvbar;", "&#166;");
            public static readonly HtmlChar Bullet = New(8226, '•', string.Empty, "&#8226;");
            public static readonly HtmlChar CaretCircumflex = New(94, '^', string.Empty, "&#94;", false);
            public static readonly HtmlChar ccedil = New(231, 'ç', "&ccedil;", "&#231;");
            public static readonly HtmlChar Ccedil = New(199, 'Ç', "&Ccedil;", "&#199;");
            public static readonly HtmlChar cedil = New(184, '¸', "&cedil;", "&#184;");
            public static readonly HtmlChar cent = New(162, '¢', "&cent;", "&#162;");
            public static readonly HtmlChar ClosingBrace = New(125, '}', string.Empty, "&#125;", false);
            public static readonly HtmlChar ClosingBracket = New(93, ']', string.Empty, "&#93;", false);
            public static readonly HtmlChar ClosingParenthesis = New(41, ')', string.Empty, "&#41;", false);
            public static readonly HtmlChar Colon = New(58, ':', string.Empty, "&#58;", false);
            public static readonly HtmlChar Comma = New(44, ',', string.Empty, "&#44;", false);
            public static readonly HtmlChar copy = New(169, '©', "&copy;", "&#169;");
            public static readonly HtmlChar curren = New(164, '¤', "&curren;", "&#164;");
            public static readonly HtmlChar Dagger = New(8224, '†', string.Empty, "&#8224;");
            public static readonly HtmlChar deg = New(176, '°', "&deg;", "&#176;");
            public static readonly HtmlChar divide = New(247, '÷', "&divide;", "&#247;");
            public static readonly HtmlChar DollarSign = New(36, '$', string.Empty, "&#36;", false);
            public static readonly HtmlChar DoubleDagger = New(8225, '‡', string.Empty, "&#8225;");
            public static readonly HtmlChar DoubleLowQuotationMark = New(8222, '„', string.Empty, "&#8222;");
            public static readonly HtmlChar eacute = New(233, 'é', "&eacute;", "&#233;");
            public static readonly HtmlChar Eacute = New(201, 'É', "&Eacute;", "&#201;");
            public static readonly HtmlChar ecirc = New(234, 'ê', "&ecirc;", "&#234;");
            public static readonly HtmlChar Ecirc = New(202, 'Ê', "&Ecirc;", "&#202;");
            public static readonly HtmlChar egrave = New(232, 'è', "&egrave;", "&#232;");
            public static readonly HtmlChar Egrave = New(200, 'È', "&Egrave;", "&#200;");
            public static readonly HtmlChar EmDash = New(8212, '—', string.Empty, "&#8212;");
            public static readonly HtmlChar Empty = New(0, ' ', string.Empty, string.Empty);
            public static readonly HtmlChar EnDash = New(8211, '–', string.Empty, "&#8211;");
            public static readonly HtmlChar EqualSign = New(61, '=', string.Empty, "&#61;", false);
            public static readonly HtmlChar EquivalencySignTilde = New(126, '~', string.Empty, "&#126;", false);
            public static readonly HtmlChar eth = New(240, 'ð', "&eth;", "&#240;");
            public static readonly HtmlChar ETH = New(208, 'Ð', "&ETH;", "&#208;");
            public static readonly HtmlChar euml = New(235, 'ë', "&euml;", "&#235;");
            public static readonly HtmlChar Euml = New(203, 'Ë', "&Euml;", "&#203;");
            public static readonly HtmlChar euro = New(8364, '€', "&euro;", "&#8364;");
            public static readonly HtmlChar ExclamationPoint = New(33, '!', string.Empty, "&#33;", false);
            public static readonly HtmlChar frac12 = New(189, '½', "&frac12;", "&#189;");
            public static readonly HtmlChar frac14 = New(188, '¼', "&frac14;", "&#188;");
            public static readonly HtmlChar frac34 = New(190, '¾', "&frac34;", "&#190;");
            public static readonly HtmlChar GraveAccent = New(96, '`', string.Empty, "&#96;", false);
            public static readonly HtmlChar gt = New(62, '>', "&gt;", "&#62;");
            public static readonly HtmlChar HorizontalEllipsis = New(8230, '…', string.Empty, "&#8230;");
            public static readonly HtmlChar iacute = New(237, 'í', "&iacute;", "&#237;");
            public static readonly HtmlChar Iacute = New(205, 'Í', "&Iacute;", "&#205;");
            public static readonly HtmlChar icirc = New(238, 'î', "&icirc;", "&#238;");
            public static readonly HtmlChar Icirc = New(206, 'Î', "&Icirc;", "&#206;");
            public static readonly HtmlChar iexcl = New(161, '¡', "&iexcl;", "&#161;");
            public static readonly HtmlChar igrave = New(236, 'ì', "&igrave;", "&#236;");
            public static readonly HtmlChar Igrave = New(204, 'Ì', "&Igrave;", "&#204;");
            public static readonly HtmlChar iquest = New(191, '¿', "&iquest;", "&#191;");
            public static readonly HtmlChar iuml = New(239, 'ï', "&iuml;", "&#239;");
            public static readonly HtmlChar Iuml = New(207, 'Ï', "&Iuml;", "&#207;");
            public static readonly HtmlChar laquo = New(171, '«', "&laquo;", "&#171;");
            public static readonly HtmlChar LeftDoubleQuotationMark = New(8220, '“', string.Empty, "&#8220;");
            public static readonly HtmlChar LeftSingleQuotationMark = New(8216, '‘', string.Empty, "&#8216;");
            public static readonly HtmlChar lt = New(60, '<', "&lt;", "&#60;");
            public static readonly HtmlChar macr = New(175, '¯', "&macr;", "&#175;");
            public static readonly HtmlChar micro = New(181, 'µ', "&micro;", "&#181;");
            public static readonly HtmlChar middot = New(183, '·', "&middot;", "&#183;");
            public static readonly HtmlChar MinusSignHyphen = New(45, '-', string.Empty, "&#45;", false);
            public static readonly HtmlChar nbsp = New(160, (char)160, "&nbsp;", "&#160;", true);
            public static readonly HtmlChar not = New(172, '¬', "&not;", "&#172;");
            public static readonly HtmlChar ntilde = New(241, 'ñ', "&ntilde;", "&#241;");
            public static readonly HtmlChar Ntilde = New(209, 'Ñ', "&Ntilde;", "&#209;");
            public static readonly HtmlChar NumberSign = New(35, '#', string.Empty, "&#35;", false);
            public static readonly HtmlChar oacute = New(243, 'ó', "&oacute;", "&#243;");
            public static readonly HtmlChar Oacute = New(211, 'Ó', "&Oacute;", "&#211;");
            public static readonly HtmlChar ocirc = New(244, 'ô', "&ocirc;", "&#244;");
            public static readonly HtmlChar Ocirc = New(212, 'Ô', "&Ocirc;", "&#212;");
            public static readonly HtmlChar ograve = New(242, 'ò', "&ograve;", "&#242;");
            public static readonly HtmlChar Ograve = New(210, 'Ò', "&Ograve;", "&#210;");
            public static readonly HtmlChar OpeningBrace = New(123, '{', string.Empty, "&#123;", false);
            public static readonly HtmlChar OpeningBracket = New(91, '[', string.Empty, "&#91;", false);
            public static readonly HtmlChar OpeningParenthesis = New(40, '(', string.Empty, "&#40;", false);
            public static readonly HtmlChar ordf = New(170, 'ª', "&ordf;", "&#170;");
            public static readonly HtmlChar ordm = New(186, 'º', "&ordm;", "&#186;");
            public static readonly HtmlChar oslash = New(248, 'ø', "&oslash;", "&#248;");
            public static readonly HtmlChar Oslash = New(216, 'Ø', "&Oslash;", "&#216;");
            public static readonly HtmlChar otilde = New(245, 'õ', "&otilde;", "&#245;");
            public static readonly HtmlChar Otilde = New(213, 'Õ', "&Otilde;", "&#213;");
            public static readonly HtmlChar ouml = New(246, 'ö', "&ouml;", "&#246;");
            public static readonly HtmlChar Ouml = New(214, 'Ö', "&Ouml;", "&#214;");
            public static readonly HtmlChar para = New(182, '¶', "&para;", "&#182;");
            public static readonly HtmlChar PercentSign = New(37, '%', string.Empty, "&#37;", false);
            public static readonly HtmlChar Period = New(46, '.', string.Empty, "&#46;", false);
            public static readonly HtmlChar PerThousandSign = New(8240, '‰', string.Empty, "&#8240;");
            public static readonly HtmlChar plusmn = New(177, '±', "&plusmn;", "&#177;");
            public static readonly HtmlChar PlusSign = New(43, '+', string.Empty, "&#43;", false);
            public static readonly HtmlChar pound = New(163, '£', "&pound;", "&#163;");
            public static readonly HtmlChar QuestionMark = New(63, '?', string.Empty, "&#63;", false);
            public static readonly HtmlChar quot = New(34, '\"', "&quot;", "&#34;");
            public static readonly HtmlChar raquo = New(187, '»', "&raquo;", "&#187;");
            public static readonly HtmlChar reg = New(174, '®', "&reg;", "&#174;");
            public static readonly HtmlChar RightDoubleQuotationMark = New(8221, '”', string.Empty, "&#8221;");
            public static readonly HtmlChar RightSingleQuotationMark = New(8217, '’', string.Empty, "&#8217;");
            public static readonly HtmlChar sect = New(167, '§', "&sect;", "&#167;");
            public static readonly HtmlChar Semicolon = New(59, ';', string.Empty, "&#59;", false);
            public static readonly HtmlChar shy = New(173, '­', "&shy;", "&#173;");
            public static readonly HtmlChar SingleLowQuotationMark = New(8218, '‚', string.Empty, "&#8218;");
            public static readonly HtmlChar SingleQuote = New(39, '\'', string.Empty, "&#39;", false);
            public static readonly HtmlChar Slash = New(47, '/', string.Empty, "&#47;", false);
            public static readonly HtmlChar sup1 = New(185, '¹', "&sup1;", "&#185;");
            public static readonly HtmlChar sup2 = New(178, '²', "&sup2;", "&#178;");
            public static readonly HtmlChar sup3 = New(179, '³', "&sup3;", "&#179;");
            public static readonly HtmlChar szlig = New(223, 'ß', "&szlig;", "&#223;");
            public static readonly HtmlChar thorn = New(254, 'þ', "&thorn;", "&#254;");
            public static readonly HtmlChar THORN = New(222, 'Þ', "&THORN;", "&#222;");
            public static readonly HtmlChar times = New(215, '×', "&times;", "&#215;");
            public static readonly HtmlChar TradeMarkSign = New(8482, '™', string.Empty, "&#8482;");
            public static readonly HtmlChar uacute = New(250, 'ú', "&uacute;", "&#250;");
            public static readonly HtmlChar Uacute = New(218, 'Ú', "&Uacute;", "&#218;");
            public static readonly HtmlChar ucirc = New(251, 'û', "&ucirc;", "&#251;");
            public static readonly HtmlChar Ucirc = New(219, 'Û', "&Ucirc;", "&#219;");
            public static readonly HtmlChar ugrave = New(249, 'ù', "&ugrave;", "&#249;");
            public static readonly HtmlChar Ugrave = New(217, 'Ù', "&Ugrave;", "&#217;");
            public static readonly HtmlChar uml = New(168, '¨', "&uml;", "&#168;");
            public static readonly HtmlChar Underscore = New(95, '_', string.Empty, "&#95;", false);
            public static readonly HtmlChar uuml = New(252, 'ü', "&uuml;", "&#252;");
            public static readonly HtmlChar Uuml = New(220, 'Ü', "&Uuml;", "&#220;");
            public static readonly HtmlChar yacute = New(253, 'ý', "&yacute;", "&#253;");
            public static readonly HtmlChar Yacute = New(221, 'Ý', "&Yacute;", "&#221;");
            public static readonly HtmlChar yen = New(165, '¥', "&yen;", "&#165;");
            public static readonly HtmlChar yuml = New(255, 'ÿ', "&yuml;", "&#255;");

            internal static Dictionary<int, HtmlChar> Characters { get; } = new Dictionary<int, HtmlChar>()
            {
                { 225, aacute }, { 193, Aacute }, { 226, acirc }, { 194, Acirc }, { 180, acute }, { 230, aelig }, { 198, AElig }, { 224, agrave }, { 192, Agrave }, { 38, amp }, { 229, aring }, { 197, Aring }, { 42, Asterisk }, { 227, atilde }, { 195, Atilde }, { 64, AtSymbol }, { 228, auml }, { 196, Auml }, { 92, Backslash }, { 166, brvbar }, { 8226, Bullet }, { 94, CaretCircumflex }, { 231, ccedil }, { 199, Ccedil }, { 184, cedil }, { 162, cent }, { 125, ClosingBrace }, { 93, ClosingBracket }, { 41, ClosingParenthesis }, { 58, Colon }, { 44, Comma }, { 169, copy }, { 164, curren }, { 8224, Dagger }, { 176, deg }, { 247, divide }, { 36, DollarSign }, { 8225, DoubleDagger }, { 8222, DoubleLowQuotationMark }, { 233, eacute }, { 201, Eacute }, { 234, ecirc }, { 202, Ecirc }, { 232, egrave }, { 200, Egrave }, { 8212, EmDash }, { 0, Empty }, { 8211, EnDash }, { 61, EqualSign }, { 126, EquivalencySignTilde }, { 240, eth }, { 208, ETH }, { 235, euml }, { 203, Euml }, { 8364, euro }, { 33, ExclamationPoint }, { 189, frac12 }, { 188, frac14 }, { 190, frac34 }, { 96, GraveAccent }, { 62, gt }, { 8230, HorizontalEllipsis }, { 237, iacute }, { 205, Iacute }, { 238, icirc }, { 206, Icirc }, { 161, iexcl }, { 236, igrave }, { 204, Igrave }, { 191, iquest }, { 239, iuml }, { 207, Iuml }, { 171, laquo }, { 8220, LeftDoubleQuotationMark }, { 8216, LeftSingleQuotationMark }, { 60, lt }, { 175, macr }, { 181, micro }, { 183, middot }, { 45, MinusSignHyphen }, { 160, nbsp }, { 172, not }, { 241, ntilde }, { 209, Ntilde }, { 35, NumberSign }, { 243, oacute }, { 211, Oacute }, { 244, ocirc }, { 212, Ocirc }, { 242, ograve }, { 210, Ograve }, { 123, OpeningBrace }, { 91, OpeningBracket }, { 40, OpeningParenthesis }, { 170, ordf }, { 186, ordm }, { 248, oslash }, { 216, Oslash }, { 245, otilde }, { 213, Otilde }, { 246, ouml }, { 214, Ouml }, { 182, para }, { 37, PercentSign }, { 46, Period }, { 8240, PerThousandSign }, { 177, plusmn }, { 43, PlusSign }, { 163, pound }, { 63, QuestionMark }, { 34, quot }, { 187, raquo }, { 174, reg }, { 8221, RightDoubleQuotationMark }, { 8217, RightSingleQuotationMark }, { 167, sect }, { 59, Semicolon }, { 173, shy }, { 8218, SingleLowQuotationMark }, { 39, SingleQuote }, { 47, Slash }, { 185, sup1 }, { 178, sup2 }, { 179, sup3 }, { 223, szlig }, { 254, thorn }, { 222, THORN }, { 215, times }, { 8482, TradeMarkSign }, { 250, uacute }, { 218, Uacute }, { 251, ucirc }, { 219, Ucirc }, { 249, ugrave }, { 217, Ugrave }, { 168, uml }, { 95, Underscore }, { 252, uuml }, { 220, Uuml }, { 253, yacute }, { 221, Yacute }, { 165, yen }, { 255, yuml }
            };

            internal static Dictionary<string, int> Indexes { get; } = new Dictionary<string, int>()
            {
                { "&#33;", 33 }, { "&#34;", 34 }, { "&#35;", 35 }, { "&#36;", 36 }, { "&#37;", 37 }, { "&#38;", 38 }, { "&#39;", 39 }, { "&#40;", 40 }, { "&#41;", 41 }, { "&#42;", 42 }, { "&#43;", 43 }, { "&#44;", 44 }, { "&#45;", 45 }, { "&#46;", 46 }, { "&#47;", 47 }, { "&#58;", 58 }, { "&#59;", 59 }, { "&#60;", 60 }, { "&#61;", 61 }, { "&#62;", 62 }, { "&#63;", 63 }, { "&#64;", 64 }, { "&#91;", 91 }, { "&#92;", 92 }, { "&#93;", 93 }, { "&#94;", 94 }, { "&#95;", 95 }, { "&#96;", 96 }, { "&#123;", 123 }, { "&#125;", 125 }, { "&#126;", 126 }, { "&#160;", 160 }, { "&#161;", 161 }, { "&#162;", 162 }, { "&#163;", 163 }, { "&#164;", 164 }, { "&#165;", 165 }, { "&#166;", 166 }, { "&#167;", 167 }, { "&#168;", 168 }, { "&#169;", 169 }, { "&#170;", 170 }, { "&#171;", 171 }, { "&#172;", 172 }, { "&#173;", 173 }, { "&#174;", 174 }, { "&#175;", 175 }, { "&#176;", 176 }, { "&#177;", 177 }, { "&#178;", 178 }, { "&#179;", 179 }, { "&#180;", 180 }, { "&#181;", 181 }, { "&#182;", 182 }, { "&#183;", 183 }, { "&#184;", 184 }, { "&#185;", 185 }, { "&#186;", 186 }, { "&#187;", 187 }, { "&#188;", 188 }, { "&#189;", 189 }, { "&#190;", 190 }, { "&#191;", 191 }, { "&#192;", 192 }, { "&#193;", 193 }, { "&#194;", 194 }, { "&#195;", 195 }, { "&#196;", 196 }, { "&#197;", 197 }, { "&#198;", 198 }, { "&#199;", 199 }, { "&#200;", 200 }, { "&#201;", 201 }, { "&#202;", 202 }, { "&#203;", 203 }, { "&#204;", 204 }, { "&#205;", 205 }, { "&#206;", 206 }, { "&#207;", 207 }, { "&#208;", 208 }, { "&#209;", 209 }, { "&#210;", 210 }, { "&#211;", 211 }, { "&#212;", 212 }, { "&#213;", 213 }, { "&#214;", 214 }, { "&#215;", 215 }, { "&#216;", 216 }, { "&#217;", 217 }, { "&#218;", 218 }, { "&#219;", 219 }, { "&#220;", 220 }, { "&#221;", 221 }, { "&#222;", 222 }, { "&#223;", 223 }, { "&#224;", 224 }, { "&#225;", 225 }, { "&#226;", 226 }, { "&#227;", 227 }, { "&#228;", 228 }, { "&#229;", 229 }, { "&#230;", 230 }, { "&#231;", 231 }, { "&#232;", 232 }, { "&#233;", 233 }, { "&#234;", 234 }, { "&#235;", 235 }, { "&#236;", 236 }, { "&#237;", 237 }, { "&#238;", 238 }, { "&#239;", 239 }, { "&#240;", 240 }, { "&#241;", 241 }, { "&#242;", 242 }, { "&#243;", 243 }, { "&#244;", 244 }, { "&#245;", 245 }, { "&#246;", 246 }, { "&#247;", 247 }, { "&#248;", 248 }, { "&#249;", 249 }, { "&#250;", 250 }, { "&#251;", 251 }, { "&#252;", 252 }, { "&#253;", 253 }, { "&#254;", 254 }, { "&#255;", 255 }, { "&#8211;", 8211 }, { "&#8212;", 8212 }, { "&#8216;", 8216 }, { "&#8217;", 8217 }, { "&#8218;", 8218 }, { "&#8220;", 8220 }, { "&#8221;", 8221 }, { "&#8222;", 8222 }, { "&#8224;", 8224 }, { "&#8225;", 8225 }, { "&#8226;", 8226 }, { "&#8230;", 8230 }, { "&#8240;", 8240 }, { "&#8364;", 8364 }, { "&#8482;", 8482 },
                { "&quot;", 34 }, { "&amp;", 38 }, { "&lt;", 60 }, { "&gt;", 62 }, { "&nbsp;", 160 }, { "&iexcl;", 161 }, { "&cent;", 162 }, { "&pound;", 163 }, { "&curren;", 164 }, { "&yen;", 165 }, { "&brvbar;", 166 }, { "&sect;", 167 }, { "&uml;", 168 }, { "&copy;", 169 }, { "&ordf;", 170 }, { "&laquo;", 171 }, { "&not;", 172 }, { "&shy;", 173 }, { "&reg;", 174 }, { "&macr;", 175 }, { "&deg;", 176 }, { "&plusmn;", 177 }, { "&sup2;", 178 }, { "&sup3;", 179 }, { "&acute;", 180 }, { "&micro;", 181 }, { "&para;", 182 }, { "&middot;", 183 }, { "&cedil;", 184 }, { "&sup1;", 185 }, { "&ordm;", 186 }, { "&raquo;", 187 }, { "&frac14;", 188 }, { "&frac12;", 189 }, { "&frac34;", 190 }, { "&iquest;", 191 }, { "&Agrave;", 192 }, { "&Aacute;", 193 }, { "&Acirc;", 194 }, { "&Atilde;", 195 }, { "&Auml;", 196 }, { "&Aring;", 197 }, { "&AElig;", 198 }, { "&Ccedil;", 199 }, { "&Egrave;", 200 }, { "&Eacute;", 201 }, { "&Ecirc;", 202 }, { "&Euml;", 203 }, { "&Igrave;", 204 }, { "&Iacute;", 205 }, { "&Icirc;", 206 }, { "&Iuml;", 207 }, { "&ETH;", 208 }, { "&Ntilde;", 209 }, { "&Ograve;", 210 }, { "&Oacute;", 211 }, { "&Ocirc;", 212 }, { "&Otilde;", 213 }, { "&Ouml;", 214 }, { "&times;", 215 }, { "&Oslash;", 216 }, { "&Ugrave;", 217 }, { "&Uacute;", 218 }, { "&Ucirc;", 219 }, { "&Uuml;", 220 }, { "&Yacute;", 221 }, { "&THORN;", 222 }, { "&szlig;", 223 }, { "&agrave;", 224 }, { "&aacute;", 225 }, { "&acirc;", 226 }, { "&atilde;", 227 }, { "&auml;", 228 }, { "&aring;", 229 }, { "&aelig;", 230 }, { "&ccedil;", 231 }, { "&egrave;", 232 }, { "&eacute;", 233 }, { "&ecirc;", 234 }, { "&euml;", 235 }, { "&igrave;", 236 }, { "&iacute;", 237 }, { "&icirc;", 238 }, { "&iuml;", 239 }, { "&eth;", 240 }, { "&ntilde;", 241 }, { "&ograve;", 242 }, { "&oacute;", 243 }, { "&ocirc;", 244 }, { "&otilde;", 245 }, { "&ouml;", 246 }, { "&divide;", 247 }, { "&oslash;", 248 }, { "&ugrave;", 249 }, { "&uacute;", 250 }, { "&ucirc;", 251 }, { "&uuml;", 252 }, { "&yacute;", 253 }, { "&thorn;", 254 }, { "&yuml;", 255 }, { "&euro;", 8364 }
            };
        }
    }
}