﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVCommerce.api.rest
{
    public class BaseRestHandler: IRestHandler
    {

        public BVSoftware.Commerce.BVApplication BVApp {get;set;}
        public BaseRestHandler(BVSoftware.Commerce.BVApplication app)
        {
            this.BVApp = app;
        }

        public string FirstParameter(string allParams)
        {
            return GetParameterByIndex(0, allParams);
        }
        public string GetParameterByIndex(int index, string allParams)
        {            
            string result = string.Empty;
            if (allParams == null) return result;

            if (allParams.Trim().Length > 0)
            {
                if (index < 0) index = 0;

                string[] parts = allParams.Split('/');
                if (parts.Length - 1 >= index)
                {                                        
                    result = parts[index];
                }
            }
            return result;
        }
        
        public virtual string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            throw new NotImplementedException();
        }
        public virtual string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            throw new NotImplementedException();
        }
        public virtual string PutAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            throw new NotImplementedException();
        }
        public virtual string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            throw new NotImplementedException();
        }
    }
}