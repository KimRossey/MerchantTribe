using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeaserWeb.Models
{
    public class EmailStorage
    {
        public static bool SaveEmail(string email)
        {
            Data.TeaserDataEntities context = new Data.TeaserDataEntities();
            Data.TeaserEmail e = new Data.TeaserEmail();
            e.Email = email;
            e.TimeStampUtc = DateTime.UtcNow;
            context.TeaserEmails.AddObject(e);
            context.SaveChanges();
            return true;
        }
    }
}