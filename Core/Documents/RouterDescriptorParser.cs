using System;
using System.Collections.Generic;

namespace Core.Documents
{
    class RouterDescriptorParser
    {
        public IEnumerable<RouterDescriptor> GetDescriptors(string textData)
        {
            if(string.IsNullOrEmpty(textData))
            {
                throw new ArgumentException("'textData' should be defined");
            }

            if(!textData.StartsWith("router"))
            {
                throw new ArgumentException("Bad document");
            }

            var routersList = new List<RouterDescriptor>();
            var splittedByrouter = textData.Split(new[] {"router"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var routerDescription in splittedByrouter)
            {
                
            }


            return routersList;
        }
    }
}
