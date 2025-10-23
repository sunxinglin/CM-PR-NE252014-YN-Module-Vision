using System.ComponentModel;
using System.Globalization;
using System.Resources;
using VisDummy.Lang.Resources;

namespace VisDummy.Lang
{
    public class LanguageManager : INotifyPropertyChanged
    {
        /// <summary>
        /// 资源
        /// </summary>
        private readonly ResourceManager _resourceManager;
        public LanguageManager()
        {
            _resourceManager = new ResourceManager("VisDummy.Lang.Resources.Language", typeof(LanguageManager).Assembly);
        }

        private static readonly Lazy<LanguageManager> _lazy = new(() => new LanguageManager());
        public static LanguageManager Instance => _lazy.Value;
        public event PropertyChangedEventHandler PropertyChanged;


        public string this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                return _resourceManager.GetString(name);
            }
        }

        public void ChangeLanguge(CultureInfo cultureInfo)
        {
            Language.Culture = cultureInfo;
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("item[]"));
        }
        public void ChangeLanguge(LanguageEnum languages)
        {
            switch (languages)
            {
                case LanguageEnum.中文:
                    LanguageManager.Instance.ChangeLanguge(new CultureInfo("zh-CN"));
                    break;
                case LanguageEnum.English:
                    LanguageManager.Instance.ChangeLanguge(new CultureInfo("en-GB"));
                    break;
                case LanguageEnum.Indonesia:
                    LanguageManager.Instance.ChangeLanguge(new CultureInfo("id"));
                    break;
                default: break;
            }
        }
    }
}
