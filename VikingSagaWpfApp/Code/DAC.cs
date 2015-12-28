using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace VikingSaga.Code
{
    public static class DAC
    {
        private static Object m_lock = new Object();

        public static void Main(String[] args)
        {
            CreateTestProfile();
        }

        public static void CreateTestProfile()
        {
            Debug.WriteLine("**** Started *****");

            var cardImageURL = @"heroes\warrior-hero.png";
            var map = MapFactory.CreateMap(MapFactory.MAP1);
            Hero hero = new Warrior { Name = "Ragnar", HP = 10, Mana = 5, Level = 1, XP = 0, Gold = 0, CardImageURL = cardImageURL, Map = map, CampaignType = Campaign.CampaignFactory.CampaignEnum.TheBloodWolf };
            var profile = new VikingSagaUserProfile { Name = "ethlore", Password = "viking", Gold = 0, Deck = CardFactory.CreateCampaignStarterDeck(), SelectedHero = hero };
            profile.Heroes[0] = hero;

            StoreUser(ConfigurationManager.AppSettings["BasePath"] + @"xml\" + profile.Name.ToLower() + ".xml", profile);
            var restoredUser = RestoreUser(ConfigurationManager.AppSettings["BasePath"] + @"xml\" + profile.Name.ToLower() + ".xml");

            Debug.WriteLine("**** Ended *****");
        }

        private static VikingSagaUserProfile RestoreUser(string filePath)
        {
            lock (m_lock)
            {
                if (!(File.Exists(filePath)))
                    return null;

                var xmlSerializer = new XmlSerializer(typeof(VikingSagaUserProfile));
                var fileStream = new FileStream(filePath, FileMode.Open);
                VikingSagaUserProfile user = (VikingSagaUserProfile)xmlSerializer.Deserialize(fileStream);
                fileStream.Close();

                return user;
            }            
        }

        private static void StoreUser(String filePath, VikingSagaUserProfile user)
        {
            lock (m_lock)
            {
                if (File.Exists(filePath))
                {
                    var xmlSerializer = new XmlSerializer(typeof(VikingSagaUserProfile));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    xmlSerializer.Serialize(fileStream, user);
                    fileStream.Close();
                }
                else
                    throw new Exception("File [" + filePath + "] does not exist!");
            }
        }

        public static void StoreProfile(VikingSagaUserProfile profile)
        {
            StoreUser(ConfigurationManager.AppSettings["BasePath"] +@"xml\" + profile.Name.ToLower() + ".xml", profile);
        }

        public static VikingSagaUserProfile RestoreProfile(String uniqueProfileName)
        {
            return RestoreUser(ConfigurationManager.AppSettings["BasePath"] + @"xml\" + uniqueProfileName.ToLower() + ".xml");
        }
    }
}
