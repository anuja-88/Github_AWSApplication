using System;
using System.Collections.Generic;
using System.Text;

namespace AWSLambdaFunction
{
    public class ResponeModel
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string EventSource { get; set; }
    }
}
