﻿namespace ImageImporter.Services.Quartz
{
    public class JobKeyAttribute : Attribute
    {
        public string Key { get; set; }

        public JobKeyAttribute(string key)
        {
            Key = key;
        }
    }

}
