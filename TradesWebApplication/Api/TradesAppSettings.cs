using System.Configuration;

namespace TradesWebApplication.Api
{
    public class TradesAppSettings : ConfigurationSection
    {

        private static TradesAppSettings settings
                    = ConfigurationManager.GetSection("TradesAppSettings") as TradesAppSettings;

        public static TradesAppSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("FlakeServiceURI", IsRequired = false)]
        public string FlakeServiceURI
        {
            get
            {
                return this["FlakeServiceURI"] as string;
            }
        }

        [ConfigurationProperty("TradeSemanticURIPrefix", IsRequired = false)]
        public string TradeSemanticURIPrefix
        {
            get
            {
                return this["TradeSemanticURIPrefix"] as string;
            }
        }

        [ConfigurationProperty("TradeSemanticURISuffix", IsRequired = false)]
        public string TradeSemanticURISuffix
        {
            get
            {
                return this["TradeSemanticURISuffix"] as string;
            }
        }

        [ConfigurationProperty("TradeLineGroupSemanticURIPrefix", IsRequired = false)]
        public string TradeLineGroupSemanticURIPrefix
        {
            get
            {
                return this["TradeLineGroupSemanticURIPrefix"] as string;
            }
        }

        [ConfigurationProperty("TradeLineGroupSemanticURISuffix", IsRequired = false)]
        public string TradeLineGroupSemanticURISuffix
        {
            get
            {
                return this["TradeLineGroupSemanticURISuffix"] as string;
            }
        }

        [ConfigurationProperty("TradeLineSemanticURIPrefix", IsRequired = false)]
        public string TradeLineSemanticURIPrefix
        {
            get
            {
                return this["TradeLineSemanticURIPrefix"] as string;
            }
        }

        [ConfigurationProperty("TradeLineSemanticURISuffix", IsRequired = false)]
        public string TradeLineSemanticURISuffix
        {
            get
            {
                return this["TradeLineSemanticURISuffix"] as string;
            }
        }

        [ConfigurationProperty("ConsumerId", IsRequired = false)]
        public string ConsumerId
        {
            get
            {
                return this["ConsumerId"] as string;
            }
        }

        [ConfigurationProperty("SharedSecret", IsRequired = false)]
        public string SharedSecret
        {
            get
            {
                return this["SharedSecret"] as string;
            }
        }

        [ConfigurationProperty("IsisTradesEndpoint", IsRequired = false)]
        public string IsisTradesEndpoint
        {
            get
            {
                return this["IsisTradesEndpoint"] as string;
            }
        }

        [ConfigurationProperty("IsisTradesStatusEndpoint", IsRequired = false)]
        public string IsisTradesStatusEndpoint
        {
            get
            {
                return this["IsisTradesStatusEndpoint"] as string;
            }
        }
        
    }
}