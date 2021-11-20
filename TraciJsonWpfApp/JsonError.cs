using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraciJsonWpfApp
{
    internal class JsonError
    {
        public string jsonErrorMessage;
        public string jsonFilePath;
        public int errorLineNumber;

        public JsonError(string jsonErrorMessage)
        {
            this.jsonErrorMessage = jsonErrorMessage;
        }


    }
}
