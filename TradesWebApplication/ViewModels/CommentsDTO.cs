using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradesWebApplication.ViewModels
{
    public class CommentsDTO
    {
        public int trade_id { get; set; }

        public int comment_id { get; set; }

        public string comments { get; set; }

    }
}