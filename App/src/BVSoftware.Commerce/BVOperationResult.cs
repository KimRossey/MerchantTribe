
using System;

namespace BVSoftware.Commerce
{

    public class BVOperationResult
    {

        private bool _Success = false;
        private string _Message = string.Empty;

        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public BVOperationResult()
        {

        }

        public BVOperationResult(bool resultSuccess, string resultMessage)
        {
            _Success = resultSuccess;
            _Message = resultMessage;
        }

    }
}