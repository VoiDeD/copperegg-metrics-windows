using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoppereggMetrics
{
    static class Settings
    {
        const string SETTINGS_FILE = "settings.xml";

        public static SettingsXml Current { get; private set; }


        static Settings()
        {
            Current = new SettingsXml();
        }


        public static void Load()
        {
            string settingsPath = Path.Combine( Application.StartupPath, SETTINGS_FILE );

            Current = SettingsXml.Load( settingsPath );
        }
    }

    public class SettingsXml : XmlSerializable<SettingsXml>
    {
        public string Server { get; set; }

        public string CopperEggAPIKey { get; set; }

        public string MySQLConnectionString { get; set; }
    }
}
