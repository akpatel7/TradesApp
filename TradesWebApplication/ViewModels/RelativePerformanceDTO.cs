using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradesWebApplication.ViewModels
{
    public class RelativePerformanceDTO
    {
        public int? trade_performance_id
        {
            get;
            set;
        }

        public int trade_id
        {
            get;
            set;
        }

        public int measure_type_id
        {
            get;
            set;
        }

        public int? return_currency_id
        {
            get;
            set;
        }

        public string return_value
        {
            get;
            set;
        }

        public int return_benchmark_id
        {
            get;
            set;
        }

        public DateTime last_updated
        {
            get;
            set;
        }

        public DateTime? return_date { get; set; }
    }
}