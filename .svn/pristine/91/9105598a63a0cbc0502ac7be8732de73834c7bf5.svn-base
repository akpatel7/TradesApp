using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradesWebApplication.Helpers
{
    public class ModelStateHelper
    {
        /// <summary>
        /// Update Model State
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="validationErrors"></param>
        public static void UpdateModelState(ModelStateDictionary modelState, Hashtable validationErrors)
        {
            foreach (string property in validationErrors.Keys)
            {
                modelState.AddModelError(property, "");
            }

        }
    }
}