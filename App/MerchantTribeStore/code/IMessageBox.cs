using System;

namespace BVCommerce
{
    public interface IMessageBox
    {
        void ClearMessage();

        void ShowOk(string msg);
        void ShowError(string msg);
        void ShowInformation(string msg);
        void ShowQuestion(string msg);
        void ShowWarning(string msg);
        void ShowException(Exception ex);
        void ShowMessage(string msg, BVSoftware.Commerce.Content.DisplayMessageType msgType);

    }
}