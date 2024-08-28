using CommunityToolkit.Mvvm.Input;
using Key2Btn.Base.Helper.ExClass;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CH = ColorHelper;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    public struct ColorStruct
    {
        public string Name { get; set; }
        public string Kana { get; set; }
        public string ARGB { get; set; }
        public HSVA HSVA { get; set; }

        public ColorStruct(string name, string argb)
        {
            Name = name;
            Kana = string.Empty;
            ARGB = argb;
            HSVA = ConvertRgbToHSVA(ARGB);
        }

        public ColorStruct(string name, string kana, string rgb)
        {
            Name = name;
            Kana = kana;
            ARGB = ConvertRgbToHex(rgb);
            HSVA = ConvertRgbToHSVA(ARGB);
        }

        static HSVA ConvertRgbToHSVA(string argb)
        {
            var temp = (Color)ColorConverter.ConvertFromString($"{argb}");
            var temp2 = CH.ColorConverter.RgbToHsv(new CH.RGB(temp.R, temp.G, temp.B));
            var alpha = Math.Clamp(temp.A / 255.0, 0d, 1d);
            return new HSVA(temp2.H, temp2.S, temp2.V, alpha);
        }
        static string ConvertRgbToHex(string rgb)
        {
            // 分割输入字符串并转换为整数
            string[] parts = rgb.Split(',');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid RGB format");
            }

            // 将每个部分解析为整数
            int r = int.Parse(parts[0]);
            int g = int.Parse(parts[1]);
            int b = int.Parse(parts[2]);

            // 将整数转换为带透明度的十六进制格式字符串
            string hex = $"#FF{r:x2}{g:x2}{b:x2}".ToUpper();

            return hex;
        }
    }

    public partial class cColorPicker : TextBox
    {
        static cColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cColorPicker), new FrameworkPropertyMetadata(typeof(cColorPicker)));
        }
    }

    public partial class cColorPicker
    {
        static ObservableCollection<ColorStruct> ColorCollection0 = ((Func<ObservableCollection<ColorStruct>>)(() =>
        {
            var ColorInfoList = new ObservableCollection<ColorStruct>();
            var colorsType = typeof(System.Windows.Media.Colors);
            var colorsTypePropertyInfos = colorsType.GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (PropertyInfo temp in colorsTypePropertyInfos)
            {
                ColorInfoList.Add(new ColorStruct(temp.Name, $"{temp.GetValue(temp)}"));
            }

            return ColorInfoList;
        })).Invoke();
        static ObservableCollection<ColorStruct> ColorCollection1 { get; set; } = new()
        {
            new("薔薇色", "ばらいろ", "233,84,107"),
            new("韓紅", "からくれない", "233,84,100"),
            new("今様色", "いまよういろ", "208,87,107"),
            new("中紅", "なかべに", "200,81,121"),
            new("銀朱", "ぎんしゅ", "200,85,84"),
            new("紅樺色", "べにかばいろ", "187,85,72"),
            new("赤茶", "あかちゃ", "187,85,53"),
            new("煉瓦色", "れんがいろ", "181,82,51"),
            new("雀茶", "すずめちゃ", "170,79,55"),
            new("団十郎茶", "だんじゅうろうちゃ", "159,86,58"),
            new("柿渋色", "かきしぶいろ", "159,86,58"),
            new("檜皮色", "ひわだいろ", "150,80,54"),
            new("茶色", "ちゃいろ", "150,80,66"),
            new("小豆色", "あずきいろ", "150,81,77"),
            new("鳶色", "とびいろ", "149,72,63"),
            new("紅鳶", "べにとび", "154,73,63"),
            new("蘇芳", "すおう", "158,61,63"),
            new("紅海老茶", "べにえびちゃ", "167,56,54"),
            new("朱、緋", "あけ", "186,38,54"),
            new("茜色", "あかねいろ", "183,40,46"),
            new("真紅", "しんく", "162,32,65"),
            new("濃紅", "こいくれない", "162,32,65"),
            new("臙脂", "えんじ", "185,64,71"),
            new("赤紅", "あかべに", "197,61,67"),
            new("赤丹", "あかに", "206,82,66"),
            new("樺色", "かばいろ", "205,94,60"),
            new("黄櫨染", "こうろぜん", "214,106,53"),
            new("丹色", "にいろ", "228,94,50"),
            new("照柿", "てりがき", "235,98,56"),
            new("柿色", "かきいろ", "237,109,61"),
            new("鉛丹色", "えんたんいろ", "236,109,81"),
            new("黄丹", "おうに", "238,121,72"),
            new("赤朽葉色", "あかくちばいろ", "219,132,73"),
            new("紅鬱金", "べにうこん", "203,131,71"),
            new("狐色", "きつねいろ", "195,135,67"),
            new("黄土色", "おうどいろ", "195,145,67"),
            new("黄唐茶", "きがらちゃ", "185,140,70"),
            new("黄橡", "きつるばみ", "182,141,76"),
            new("丁字染", "ちょうじぞめ", "173,125,76"),
            new("香染", "こうぞめ", "173,125,76"),
            new("芝翫茶", "しかんちゃ", "173,126,78"),
            new("枇杷茶", "びわちゃ", "174,124,79"),
            new("焦香", "こがれこう", "174,124,88"),
            new("櫨色", "はじいろ", "183,123,87"),
            new("土器色", "かわらけいろ", "195,120,84"),
            new("駱駝色", "らくだいろ", "191,121,78"),
            new("琥珀色", "こはくいろ", "191,120,58"),
            new("土色", "つちいろ", "188,118,60"),
            new("胡桃色", "くるみいろ", "168,111,76"),
            new("赭", "そほ", "171,105,83"),
            new("砺茶", "とのちゃ", "159,111,85"),
            new("宗伝唐茶", "そうでんからちゃ", "161,109,93"),
            new("蘇芳香", "すおうこう", "168,105,101"),
            new("浅蘇芳", "あさすおう", "162,87,104"),
            new("京紫", "きょうむらさき", "157,91,139"),
            new("二藍", "ふたあい", "145,92,139"),
            new("古代紫", "こだいむらさき", "137,91,138"),
            new("紫", "むらさき", "136,72,152"),
            new("茄子紺", "なすこん", "130,72,128"),
            new("蒲葡", "えびぞめ", "122,65,113"),
            new("暗紅色", "あんこうしょく", "116,50,92"),
            new("葡萄色", "ぶどういろ", "82,47,96"),
            new("桑の実色", "くわのみいろ", "85,41,91"),
            new("深紫", "ふかむらさき", "73,55,89"),
            new("濃鼠", "こいねず", "79,69,92"),
            new("褐色", "かちいろ", "77,76,97"),
            new("錆鼠", "さびねず", "71,88,92"),
            new("錆鉄御納戸", "さびてつおなんど", "72,88,89"),
            new("革色", "かわいろ", "71,89,80"),
            new("虫襖", "むしあお", "58,91,82"),
            new("天鵞絨", "びろうど", "47,93,80"),
            new("高麗納戸", "こうらいなんど", "44,79,84"),
            new("青褐", "あおかち", "57,62,79"),
            new("藍鉄", "あいてつ", "57,63,76"),
            new("羊羹色", "ようかんいろ", "56,60,60"),
            new("檳榔子染", "びんろうじぞめ", "67,61,60"),
            new("千歳茶", "せんさいちゃ", "73,74,65"),
            new("仙斎茶", "せんさいちゃ", "71,75,66"),
            new("藍墨茶", "あいすみちゃ", "71,74,77"),
            new("消炭色", "けしずみいろ", "82,78,77"),
            new("紅消鼠", "べにけしねずみ", "82,71,72"),
            new("黒橡", "くろつるばみ", "84,74,71"),
            new("藍媚茶", "あいこびちゃ", "85,86,71"),
            new("藍海松茶", "あいみるちゃ", "86,86,75"),
            new("海松茶", "みるちゃ", "90,84,75"),
            new("丼鼠", "どぶねずみ", "89,84,85"),
            new("藤煤竹", "ふじすすたけ", "90,83,89"),
            new("墨", "すみ", "89,88,87"),
            new("柳煤竹", "やなぎすすたけ", "91,99,86"),
            new("樺茶色", "かばちゃいろ", "114,98,80"),
            new("媚茶", "こびちゃ", "113,98,70"),
            new("黄枯茶", "きがらちゃ", "118,92,71"),
            new("煤竹色", "すすたけいろ", "111,81,76"),
            new("焦茶", "こげちゃ", "111,75,62"),
            new("紅檜皮", "べにひはだ", "123,71,65"),
            new("海老茶", "えびちゃ", "119,60,48"),
            new("栗皮茶", "くりかわちゃ", "109,60,50"),
            new("茶褐色", "ちゃかっしょく", "102,64,50"),
            new("赤褐色", "せっかっしょく", "104,63,54"),
            new("憲法色", "けんぽういろ", "84,63,50"),
            new("涅色", "くりいろ", "85,71,56"),
            new("似せ紫", "にせむらさき", "81,55,67"),
            new("紫鳶", "むらさきとび", "95,65,75"),
            new("濃色", "こきいろ", "99,73,80"),
            new("滅紫", "けしむらさき", "89,66,85"),
            new("鉄御納戸", "てつおなんど", "69,87,101"),
            new("御召茶", "おめしちゃ", "67,103,107"),
            new("熨斗目花色", "のしめはないろ", "66,101,121"),
            new("紺鼠", "こんねず", "68,97,123"),
            new("御召御納戸", "おめしおなんど", "76,100,115"),
            new("錆御納戸", "さびおなんど", "83,114,125"),
            new("青碧", "せいへき", "71,131,132"),
            new("舛花色", "ますはないろ", "91,126,145"),
            new("錆浅葱", "さびあさぎ", "92,146,145"),
            new("藍鼠", "あいねず", "108,132,141"),
            new("薄花色", "うすはないろ", "105,138,171"),
            new("藤納戸", "ふじなんど", "112,108,170"),
            new("紅掛花色", "べにかけはないろ", "104,105,155"),
            new("菫色", "すみれいろ", "112,88,163"),
            new("江戸紫", "えどむらさき", "116,83,153"),
            new("青紫", "あおむらさき", "103,69,152"),
            new("菖蒲色", "しょうぶいろ", "103,65,150"),
            new("本紫", "ほんむらさき", "101,49,142"),
            new("紺藍", "こんあい", "74,72,142"),
            new("紅桔梗", "べにききょう", "77,67,152"),
            new("桔梗色", "ききょういろ", "86,84,162"),
            new("紺桔梗", "こんききょう", "77,90,175"),
            new("花色", "はないろ", "77,90,175"),
            new("群青色", "ぐんじょういろ", "76,108,179"),
            new("杜若色", "かきつばたいろ", "62,98,173"),
            new("薄縹", "うすはなだ", "80,126,164"),
            new("薄花桜", "うすはなざくら", "90,121,186"),
            new("薄群青", "うすぐんじょう", "83,131,195"),
            new("縹色", "はなだいろ", "39,146,195"),
            new("薄藍", "うすあい", "0,148,200"),
            new("青", "あお", "0,149,217"),
            new("浅葱色", "あさぎいろ", "0,163,175"),
            new("青緑", "あおみどり", "0,164,151"),
            new("花緑青", "はなろくしょう", "0,163,129"),
            new("納戸色", "なんどいろ", "0,136,153"),
            new("紺碧", "こんぺき", "0,123,187"),
            new("花浅葱", "はなあさぎ", "42,131,162"),
            new("瑠璃色", "るりいろ", "30,80,162"),
            new("瑠璃紺", "るりこん", "25,68,142"),
            new("紺瑠璃", "こんるり", "22,74,132"),
            new("藍色", "あいいろ", "22,94,131"),
            new("青藍", "せいらん", "39,74,120"),
            new("深縹", "こきはなだ", "42,64,115"),
            new("紺色", "こんいろ", "34,58,112"),
            new("紺青", "こんじょう", "25,47,96"),
            new("留紺", "とめこん", "28,48,92"),
            new("褐返", "かちかえし", "32,55,68"),
            new("百入茶", "ももしおちゃ", "31,49,52"),
            new("黒紅", "くろべに", "48,40,51"),
            new("紫黒", "しこく", "46,41,48"),
            new("蝋色", "ろういろ", "43,43,43"),
            new("黒", "くろ", "43,43,43"),
            new("黒緑", "くろみどり", "51,54,49"),
            new("赤墨", "あかすみ", "63,49,43"),
            new("黒鳶", "くろとび", "67,47,47"),
            new("黒茶", "くろちゃ", "88,56,34"),
            new("錆色", "さびいろ", "108,53,36"),
            new("葡萄茶", "えびちゃ", "108,44,47"),
            new("唐茶", "からちゃ", "120,60,29"),
            new("赤錆色", "あかさびいろ", "138,51,25"),
            new("栗梅", "くりうめ", "133,46,25"),
            new("弁柄色", "べんがらいろ", "143,46,20"),
            new("褐色", "かっしょく", "138,59,0"),
            new("栗色", "くりいろ", "118,47,7"),
            new("赤銅色", "しゃくどういろ", "117,33,0"),
            new("葡萄色", "えびいろ", "100,1,37"),
            new("紫紺", "しこん", "70,14,68"),
            new("鉄紺", "てつこん", "23,24,75"),
            new("濃藍", "こいあい", "15,35,80"),
            new("鉄色", "てついろ", "0,82,67"),
            new("深緑", "ふかみどり", "0,85,46"),
            new("常磐色", "ときわいろ", "0,123,67"),
            new("萌葱色", "もえぎいろ", "0,110,84"),
            new("常磐緑", "ときわみどり", "2,135,96"),
            new("木賊色", "とくさいろ", "59,121,96"),
            new("緑青色", "ろくしょういろ", "71,136,94"),
            new("老竹色", "おいたけいろ", "118,145,100"),
            new("山鳩色", "やまばといろ", "118,124,107"),
            new("青鈍", "あおにび", "107,123,110"),
            new("鈍色", "にびいろ", "114,113,113"),
            new("紫鼠", "むらさきねず", "113,104,108"),
            new("葡萄鼠", "ぶどうねずみ", "112,91,103"),
            new("岩井茶", "いわいちゃ", "107,111,89"),
            new("麹塵", "きくじん", "110,121,85"),
            new("肥後煤竹", "ひごすすたけ", "137,120,88"),
            new("銀煤竹", "ぎんすすだけ", "133,104,89"),
            new("煎茶色", "せんちゃいろ", "140,100,80"),
            new("枯茶", "からちゃ", "141,100,73"),
            new("渋紙色", "しぶかみいろ", "148,98,67"),
            new("灰茶", "はいちゃ", "152,98,60"),
            new("路考茶", "ろこうちゃ", "140,112,66"),
            new("朽葉色", "くちばいろ", "145,115,71"),
            new("梅幸茶", "ばいこうちゃ", "136,121,56"),
            new("鶯色", "うぐいすいろ", "146,140,54"),
            new("根岸色", "ねぎしいろ", "147,139,75"),
            new("黄海松茶", "きみるちゃ", "145,135,84"),
            new("鶸茶", "ひわちゃ", "140,136,97"),
            new("利休色", "りきゅういろ", "143,134,103"),
            new("生壁色", "なまかべいろ", "148,132,106"),
            new("空五倍子色", "うつぶしいろ", "157,137,108"),
            new("灰汁色", "あくいろ", "158,148,120"),
            new("胡桃染", "くるみぞめ", "165,143,134"),
            new("鳩羽鼠", "はとばねずみ", "158,139,142"),
            new("鼠色", "ねずみいろ", "148,148,149"),
            new("桔梗鼠", "ききょうねず", "149,148,154"),
            new("薄鼠", "うすねず", "151,144,164"),
            new("鳩羽色", "はとばいろ", "149,133,156"),
            new("竜胆色", "りんどういろ", "144,121,173"),
            new("紫苑色", "しおんいろ", "134,123,169"),
            new("源氏鼠", "げんじねず", "136,128,132"),
            new("煤色", "すすいろ", "136,127,122"),
            new("江戸鼠", "えどねず", "146,129,120"),
            new("利休鼠", "りきゅうねずみ", "136,142,126"),
            new("豆がら茶", "まめがらちゃ", "139,150,141"),
            new("湊鼠", "みなとねずみ", "128,152,155"),
            new("水浅葱", "みずあさぎ", "128,171,169"),
            new("青竹色", "あおたけいろ", "126,190,171"),
            new("青磁色", "せいじいろ", "126,190,165"),
            new("千草色", "ちぐさいろ", "146,181,169"),
            new("薄青", "うすあお", "147,182,156"),
            new("柳染", "やなぎぞめ", "147,184,129"),
            new("淡萌黄", "うすもえぎ", "147,202,118"),
            new("浅緑", "あさみどり", "136,203,127"),
            new("柳色", "やなぎいろ", "168,201,127"),
            new("苗色", "なえいろ", "176,202,113"),
            new("抹茶色", "まっちゃいろ", "197,197,106"),
            new("木蘭色", "もくらんじき", "199,179,112"),
            new("榛色", "はしばみいろ", "191,164,111"),
            new("伽羅色", "きゃらいろ", "216,163,115"),
            new("飴色", "あめいろ", "222,176,104"),
            new("小麦色", "こむぎいろ", "228,158,97"),
            new("深支子", "こきくちなし", "235,155,111"),
            new("東雲色", "しののめいろ", "241,144,114"),
            new("曙色", "あけぼのいろ", "241,144,114"),
            new("珊瑚朱色", "さんごしゅいろ", "238,131,111"),
            new("甚三紅", "じんざもみ", "238,130,124"),
            new("薄紅", "うすべに", "240,144,141"),
            new("桃色", "ももいろ", "240,145,153"),
            new("紅梅色", "こうばいいろ", "242,160,161"),
            new("一斤染", "いっこんぞめ", "245,177,153"),
            new("赤香", "あかこう", "246,184,148"),
            new("洒落柿", "しゃれがき", "247,189,143"),
            new("淡香", "うすこう", "243,191,136"),
            new("肉色", "にくいろ", "241,191,153"),
            new("人色", "ひといろ", "241,191,153"),
            new("丁子色", "ちょうじいろ", "239,205,154"),
            new("香色", "こういろ", "239,205,154"),
            new("薄香", "うすこう", "240,207,160"),
            new("浅黄", "うすき", "237,211,161"),
            new("砥粉色", "とのこいろ", "244,221,165"),
            new("蜂蜜色", "はちみついろ", "253,222,165"),
            new("蒸栗色", "むしぐりいろ", "235,225,169"),
            new("若芽色", "わかめいろ", "224,235,175"),
            new("夏虫色", "なつむしいろ", "206,228,174"),
            new("裏葉柳", "うらはやなぎ", "193,216,172"),
            new("薄萌葱", "うすもえぎ", "186,220,173"),
            new("柳鼠", "やなぎねず", "200,213,187"),
            new("青磁鼠", "せいじねず", "190,210,195"),
            new("千草鼠", "ちぐさねず", "190,211,202"),
            new("灰青", "はいあお", "192,198,201"),
            new("霞色", "かすみいろ", "200,194,198"),
            new("潤色", "うるみいろ", "200,194,190"),
            new("枯野色", "かれのいろ", "211,203,198"),
            new("牡丹鼠", "ぼたんねず", "211,204,214"),
            new("暁鼠", "あかつきねず", "211,207,217"),
            new("薄雲鼠", "うすくもねず", "212,220,218"),
            new("蕎麦切色", "そばきりいろ", "212,220,214"),
            new("絹鼠", "きぬねず", "221,220,214"),
            new("白鼠", "しろねず", "220,221,221"),
            new("薄梅鼠", "うすうめねず", "220,214,217"),
            new("鴇鼠", "ときねず", "228,210,216"),
            new("灰桜", "はいざくら", "232,211,209"),
            new("灰梅", "はいうめ", "232,211,199"),
            new("練色", "ねりいろ", "237,228,205"),
            new("灰白色", "かいはくしょく", "233,228,212"),
            new("素色", "そしょく", "234,229,227"),
            new("桜鼠", "さくらねず", "233,223,229"),
            new("白梅鼠", "しらうめねず", "229,228,230"),
            new("紫水晶", "むらさきすいしょう", "231,231,235"),
            new("白花色", "しらはないろ", "232,236,239"),
            new("白菫色", "しろすみれいろ", "234,237,247"),
            new("藍白", "あいじろ", "235,246,247"),
            new("月白", "げっぱく", "234,244,252"),
            new("乳白色", "にゅうはくしょく", "243,243,243"),
            new("白練", "しろねり", "243,243,242"),
            new("桜色", "さくらいろ", "254,244,244"),
            new("薄桜", "うすざくら", "253,239,242"),
            new("生成り色", "きなりいろ", "251,250,245"),
            new("白磁", "はくじ", "248,251,248"),
            new("卯の花色", "うのはないろ", "247,252,254"),
            new("白", "しろ", "255,255,255"),
            new("胡粉色", "ごふんいろ", "255,255,252"),
            new("象牙色", "ぞうげいろ", "248,244,230"),
            new("灰黄緑", "はいきみどり", "230,234,227"),
            new("淡紅藤", "あわべにふじ", "230,205,227"),
            new("白藤色", "しらふじいろ", "219,208,230"),
            new("淡藤色", "あわふじいろ", "187,200,230"),
            new("藤色", "ふじいろ", "187,188,222"),
            new("秘色色", "ひそくいろ", "171,206,216"),
            new("瓶覗", "かめのぞき", "162,215,221"),
            new("空色", "そらいろ", "160,216,239"),
            new("水色", "みずいろ", "188,226,232"),
            new("白藍", "しらあい", "193,228,233"),
            new("白緑", "びゃくろく", "214,233,202"),
            new("薄卵色", "うすたまごいろ", "253,232,208"),
            new("鳥の子色", "とりのこいろ", "255,241,207"),
            new("肌色", "はだいろ", "252,226,196"),
            new("女郎花", "おみなえし", "242,242,176"),
            new("洗柿", "あらいがき", "242,201,172"),
            new("雄黄", "ゆうおう", "249,200,155"),
            new("珊瑚色", "さんごいろ", "245,177,170"),
            new("鴇色", "ときいろ", "244,179,194"),
            new("虹色", "にじいろ", "246,191,188"),
            new("撫子色", "なでしこいろ", "238,187,203"),
            new("石竹色", "せきちくいろ", "229,171,190"),
            new("紅藤色", "べにふじいろ", "204,166,191"),
            new("浅紫", "あさむらさき", "196,163,191"),
            new("薄葡萄", "うすぶどう", "192,162,199"),
            new("藤鼠", "ふじねず", "166,165,196"),
            new("半色", "はしたいろ", "166,154,189"),
            new("藤紫", "ふじむらさき", "165,154,202"),
            new("薄色", "うすいろ", "168,157,172"),
            new("薄墨色", "うすずみいろ", "163,163,162"),
            new("錫色", "すずいろ", "158,161,163"),
            new("素鼠", "すねずみ", "159,160,160"),
            new("茶鼠", "ちゃねずみ", "169,158,147"),
            new("深川鼠", "ふかがわねずみ", "151,167,145"),
            new("青白橡", "あおしろつるばみ", "155,168,141"),
            new("柳茶", "やなぎちゃ", "161,164,109"),
            new("利休茶", "りきゅうちゃ", "165,149,100"),
            new("油色", "あぶらいろ", "161,147,97"),
            new("桑染", "くわぞめ", "183,155,91"),
            new("青朽葉", "あおくちば", "173,162,80"),
            new("青丹", "あおに", "153,171,78"),
            new("鶸萌黄", "ひわもえぎ", "130,174,70"),
            new("松葉色", "まつばいろ", "131,155,92"),
            new("草色", "くさいろ", "123,141,66"),
            new("国防色", "こくぼうしょく", "123,108,62"),
            new("海松色", "みるいろ", "114,109,64"),
            new("鶯茶", "うぐいすちゃ", "113,92,31"),
            new("璃寛茶", "りかんちゃ", "106,93,33"),
            new("苔色", "こけいろ", "105,130,27"),
            new("桑茶", "くわちゃ", "149,111,41"),
            new("柿茶", "かきちゃ", "149,78,42"),
            new("代赭", "たいしゃ", "187,85,32"),
            new("緋色", "ひいろ", "211,56,28"),
            new("紅緋", "べにひ", "232,57,41"),
            new("紅赤", "べにあか", "217,51,63"),
            new("紅", "くれない", "215,0,58"),
            new("赤", "あか", "230,0,51"),
            new("猩々緋", "しょうじょうひ", "226,4,27"),
            new("深緋", "こきひ", "201,23,30"),
            new("赤橙", "あかだいだい", "234,85,6"),
            new("金赤", "きんあか", "234,85,6"),
            new("朱色", "しゅいろ", "235,97,1"),
            new("黄赤", "きあか", "236,104,0"),
            new("人参色", "にんじんいろ", "236,104,0"),
            new("橙色", "だいだいいろ", "238,120,0"),
            new("蜜柑色", "みかんいろ", "240,131,0"),
            new("金茶", "きんちゃ", "243,152,0"),
            new("山吹色", "やまぶきいろ", "248,181,0"),
            new("向日葵色", "ひまわりいろ", "252,200,0"),
            new("蒲公英色", "たんぽぽいろ", "255,217,0"),
            new("黄色", "きいろ", "255,217,0"),
            new("中黄", "ちゅうき", "255,234,0"),
            new("鬱金色", "うこんいろ", "250,191,20"),
            new("藤黄", "とうおう", "247,193,20"),
            new("緑黄色", "りょくおうしょく", "220,203,24"),
            new("黄金", "こがね", "230,180,34"),
            new("金色", "こんじき", "230,180,34"),
            new("櫨染", "はじぞめ", "217,166,46"),
            new("黄朽葉色", "きくちばいろ", "211,162,67"),
            new("芥子色", "からしいろ", "208,175,76"),
            new("柑子色", "こうじいろ", "246,173,73"),
            new("萱草色", "かんぞういろ", "248,184,98"),
            new("玉蜀黍色", "とうもろこしいろ", "238,195,98"),
            new("花葉色", "はなばいろ", "251,210,107"),
            new("卵色", "たまごいろ", "252,213,117"),
            new("刈安色", "かりやすいろ", "245,229,107"),
            new("黄檗色", "きはだいろ", "254,242,99"),
            new("菜の花色", "なのはないろ", "255,236,71"),
            new("黄支子色", "きくちなしいろ", "255,219,79"),
            new("支子色", "くちなしいろ", "251,202,77"),
            new("金糸雀色", "かなりあいろ", "235,216,66"),
            new("鶸色", "ひわいろ", "215,207,58"),
            new("若草色", "わかくさいろ", "195,216,37"),
            new("黄緑", "きみどり", "184,210,0"),
            new("萌黄", "もえぎ", "170,207,83"),
            new("若苗色", "わかなえいろ", "199,220,104"),
            new("若葉色", "わかばいろ", "185,208,139"),
            new("山葵色", "わさびいろ", "168,191,147"),
            new("白橡", "しろつるばみ", "203,185,148"),
            new("白茶", "しらちゃ", "221,187,153"),
            new("枯色", "かれいろ", "224,195,140"),
            new("枯草色", "かれくさいろ", "228,220,138"),
            new("淡黄", "たんこう", "248,229,140"),
            new("若菜色", "わかないろ", "216,230,152"),
            new("砂色", "すないろ", "220,211,178"),
            new("亜麻色", "あまいろ", "214,198,175"),
            new("薄柿", "うすがき", "212,172,173"),
            new("水柿", "みずがき", "228,171,155"),
            new("宍色", "ししいろ", "239,171,147"),
            new("赤白橡", "あかしろつるばみ", "215,169,140"),
            new("ときがら茶", "ときがらちゃ", "224,158,135"),
            new("退紅", "あらぞめ", "214,144,144"),
            new("梅鼠", "うめねず", "192,153,160"),
            new("利休白茶", "りきゅうしろちゃ", "179,173,160"),
            new("薄鈍", "うすにび", "173,173,173"),
            new("銀鼠", "ぎんねず", "175,175,176"),
            new("錆青磁", "さびせいじ", "166,200,178"),
            new("若緑", "わかみどり", "152,217,142"),
            new("若竹色", "わかたけいろ", "104,190,141"),
            new("薄緑", "うすみどり", "105,176,118"),
            new("緑", "みどり", "62,179,112"),
            new("翡翠色", "ひすいいろ", "56,180,139"),
            new("新橋色", "しんばしいろ", "89,185,198"),
            new("浅縹", "あさはなだ", "132,185,203"),
            new("白群", "びゃくぐん", "131,204,210"),
            new("勿忘草色", "わすれなぐさいろ", "137,195,235"),
            new("青藤色", "あおふじいろ", "132,162,212"),
            new("紅掛空色", "べにかけそらいろ", "132,145,195"),
            new("紅碧", "べにみどり", "132,145,195"),
            new("灰色", "はいいろ", "125,125,125"),
            new("鉛色", "なまりいろ", "123,124,125"),
            new("梅染", "うめぞめ", "180,138,118"),
            new("柴染", "ふしぞめ", "178,140,110"),
            new("丁子茶", "ちょうじちゃ", "180,134,107"),
            new("遠州茶", "えんしゅうちゃ", "202,130,105"),
            new("洗朱", "あらいしゅ", "208,130,108"),
            new("真赭", "まそほ", "213,124,107"),
            new("纁", "そひ", "224,129,94"),
            new("肉桂色", "にっけいいろ", "221,122,86"),
            new("浅緋", "うすきひ", "223,113,99"),
            new("真朱", "まそお", "236,109,113"),
            new("赤紫", "あかむらさき", "235,110,165"),
            new("牡丹色", "ぼたんいろ", "231,96,158"),
            new("躑躅色", "つつじいろ", "233,82,149"),
            new("紅紫", "べにむらさき", "180,76,151"),
            new("梅紫", "うめむらさき", "170,76,143"),
            new("若紫", "わかむらさき", "188,100,164"),
            new("菖蒲色", "あやめいろ", "204,126,177"),
            new("桃花色", "ももはないろ", "225,152,180"),
            new("薄紅梅", "うすこうばい", "229,151,178"),
            new("長春色", "ちょうしゅんいろ", "201,117,134"),
            new("鴇浅葱", "ときあさぎ", "184,136,132"),
            new("江戸茶", "えどちゃ", "205,140,92"),
            new("山吹茶", "やまぶきちゃ", "200,153,50"),
            new("菜種油色", "なたねゆいろ", "166,148,37"),
            new("黄茶", "きちゃ", "225,123,52"),
            new("杏色", "あんずいろ", "247,185,119"),
            new("露草色", "つゆくさいろ", "56,161,219"),
            new("天色", "あまいろ", "44,169,225"),
            new("千歳緑", "ちとせみどり", "49,103,69"),
            new("鉄黒", "てつぐろ", "40,26,20"),
            new("憲法黒茶", "けんぽうくろちゃ", "36,26,8"),
            new("黒檀", "こくたん", "37,13,0"),
            new("暗黒色", "あんこくしょく", "22,22,14"),
            new("烏羽色", "からすばいろ", "24,6,20"),
            new("漆黒", "しっこく", "13,0,21"),
            new("濡羽色", "ぬればいろ", "0,11,0"),
        };

        private CH.HSV? tempColorHSV;
        private bool isPopupOpening;
        private bool isHueChanging;
        private bool isSaturationChanging;
        private bool isValueChanging;

        [RelayCommand]
        private void OnPopupOpend(object para)
        {
            isPopupOpening = true;
            if (IsValidColorCode(this.Text))
            {
                var temp = (Color)ColorConverter.ConvertFromString($"{this.Text}");
                var temp2 = CH.ColorConverter.RgbToHsv(new CH.RGB(temp.R, temp.G, temp.B));
                ColorAlpha = Math.Clamp(temp.A / 255.0, 0d, 1d);
                ColorHSVA = new HSVA(temp2.H, temp2.S, temp2.V, ColorAlpha);

                Debug.WriteLine($"OnPopupOpend: {this.Text} (ARGB:{temp.A},{temp.R},{temp.G},{temp.B})");
            }
            isPopupOpening = false;
        }

        [RelayCommand(CanExecute = nameof(IsNotPopupOpening))]
        private void OnListBoxSelectionChanged(object para)
        {
            if (para is ColorStruct color)
            {
                var temp = (Color)ColorConverter.ConvertFromString($"{color.ARGB}");
                var temp2 = CH.ColorConverter.RgbToHsv(new CH.RGB(temp.R, temp.G, temp.B));
                ColorAlpha = Math.Clamp(temp.A / 255.0, 0d, 1d);
                ColorHSVA = new HSVA(temp2.H, temp2.S, temp2.V, ColorAlpha);

                Debug.WriteLine($"OnListBoxSelectionChanged: {this.Text} (ARGB:{temp.A},{temp.R},{temp.G},{temp.B})");
            }
        }
        private bool IsNotPopupOpening => !isPopupOpening && !isHueChanging && !isSaturationChanging && !isValueChanging;

        [RelayCommand(CanExecute = nameof(CanChangeHue))]
        private void OnHueValueChanged(object para)
        {
            if (para is RoutedPropertyChangedEventArgs<double> e && tempColorHSV is not null)
            {
                isHueChanging = true;
                ColorHSVA = new HSVA((int)e.NewValue, tempColorHSV.S, tempColorHSV.V, ColorAlpha);
                Debug.WriteLine($"(Hue) new ColorHSVA: {ColorHSVA}     base:{tempColorHSV}");
                isHueChanging = false;
            }
        }
        private bool CanChangeHue => !isSaturationChanging && !isValueChanging;

        [RelayCommand(CanExecute = nameof(CanChangeSaturation))]
        private void OnSaturationValueChanged(object para)
        {
            if (para is RoutedPropertyChangedEventArgs<double> e && tempColorHSV is not null)
            {
                isSaturationChanging = true;
                ColorHSVA = new HSVA(tempColorHSV.H, (byte)e.NewValue, tempColorHSV.V, ColorAlpha);
                {
                    if ((byte)e.NewValue == 0)
                    {
                        var hsva = new HSVA(tempColorHSV.H, (byte)e.NewValue, tempColorHSV.V, ColorAlpha);
                        hsva.H = tempColorHSV.H;
                        hsva.V = tempColorHSV.V;
                        ColorHSVA = hsva;
                    }
                }
                Debug.WriteLine($"(Saturation) new ColorHSVA: {ColorHSVA}     base:{tempColorHSV}");
                isSaturationChanging = false;
            }
        }
        private bool CanChangeSaturation => !isHueChanging && !isValueChanging;

        [RelayCommand(CanExecute = nameof(CanChangeValue))]
        private void OnValueValueChanged(object para)
        {
            if (para is RoutedPropertyChangedEventArgs<double> e && tempColorHSV is not null)
            {
                isValueChanging = true;
                ColorHSVA = new HSVA(tempColorHSV.H, tempColorHSV.S, (byte)e.NewValue, ColorAlpha);
                {
                    if ((byte)e.NewValue == 0)
                    {
                        var hsva = new HSVA(tempColorHSV.H, tempColorHSV.S, (byte)e.NewValue, ColorAlpha);
                        hsva.H = tempColorHSV.H;
                        hsva.S = tempColorHSV.S;
                        ColorHSVA = hsva;
                    }
                }

                Debug.WriteLine($"(Value) new ColorHSVA: {ColorHSVA}     base:{tempColorHSV}");
                isValueChanging = false;
            }
        }
        private bool CanChangeValue => !isHueChanging && !isSaturationChanging;

        [RelayCommand(CanExecute = nameof(CanChangeAlpha))]
        private void OnAlphaValueChanged(object para)
        {
            if (para is RoutedPropertyChangedEventArgs<double> e && tempColorHSV is not null)
            {
                ColorAlpha = Math.Clamp(e.NewValue / 100.0, 0d, 1d);
                var temp = new HSVA(tempColorHSV.H, tempColorHSV.S, tempColorHSV.V, ColorAlpha);
                {
                    temp.H = tempColorHSV.H;
                    temp.S = tempColorHSV.S;
                    temp.V = tempColorHSV.V;
                    temp.Alpha = ColorAlpha;
                }
                ColorHSVA = temp;
            }
        }
        private bool CanChangeAlpha => !isHueChanging && !isSaturationChanging && !isValueChanging;


        [RelayCommand]
        private void OnSliderPreviewMouseDown(object para)
        {
            if (!(para is System.Windows.Input.MouseButtonEventArgs e)) { return; }
            tempColorHSV = ColorHSVA;
        }
        [RelayCommand]
        private void OnSliderPreviewMouseUp(object para)
        {
            tempColorHSV = null;
        }

        /// <summary>
        /// 颜色检查
        /// </summary>
        public static bool IsValidColorCode(string input)
        {
            string pattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }

    public partial class cColorPicker
    {
        public double ColorAlpha
        {
            get { return (double)GetValue(ColorAlphaProperty); }
            set { SetValue(ColorAlphaProperty, value); }
        }
        public static readonly DependencyProperty ColorAlphaProperty = DependencyProperty.Register(
            name: "ColorAlpha",
            propertyType: typeof(double),
            ownerType: typeof(cColorPicker),
            typeMetadata: new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public HSVA ColorHSVA
        {
            get { return (HSVA)GetValue(ColorHSVProperty); }
            set { SetValue(ColorHSVProperty, value); }
        }
        public static readonly DependencyProperty ColorHSVProperty = DependencyProperty.Register(
            name: "ColorHSVA",
            propertyType: typeof(HSVA),
            ownerType: typeof(cColorPicker),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ObservableCollection<ColorStruct> PredefinedColors
        {
            get { return (ObservableCollection<ColorStruct>)GetValue(PredefinedColorsProperty); }
            set { SetValue(PredefinedColorsProperty, value); }
        }
        public static readonly DependencyProperty PredefinedColorsProperty = DependencyProperty.Register(
            name: "PredefinedColors",
            propertyType: typeof(ObservableCollection<ColorStruct>),
            ownerType: typeof(cColorPicker),
            typeMetadata: new FrameworkPropertyMetadata(ColorCollection0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
